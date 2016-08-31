using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web.filters;

namespace web.Areas.Admin.Controllers
{
    [AdminAuthorization(Roles = "admin,financial")]
    public class NewsController : Controller
    {
        NewsService service = new NewsService();
        AreaService areaService = new AreaService();
        UserService userService = new UserService();
        public UserModel LoginUser { get { return userService.GetLoginUser(); } }
        //
        // GET: /Admin/News/

        public ActionResult Index(string name)
        {
            
            //var c = Maticsoft.Common.AES.Decrypt("EsKBzg9E46ic7BgSDS6Xwg==");
            //var d = Maticsoft.Common.AES.Decrypt("JhHl0DCUKi3sOHWMYxG+xg==");
            //var a = Maticsoft.Common.DEncrypt.DESEncrypt.Decrypt("4E2C6CF3289C2EAD");
            //var b = Maticsoft.Common.DEncrypt.DESEncrypt.Decrypt("1D4D2BC318872FEB");
            //var e = Maticsoft.Common.AES.Encrypt("13213807082");
            ViewBag.name = name;
            return View();
        }
        [HttpPost]
        public ActionResult Index(int page, int pagesize, string name)
        {
            var model = service.GetAll(page, pagesize, name);

            if (model != null)
            {
                return Json(new { Rows = model.Items, Total = model.TotalItems });
            }
            else
            {
                return Json(new { Rows = 0, Total = 0 });
            }
        }
        //保存
        public ActionResult Save(string Guid)
        {
            var list = areaService.GetAllProvince();
            ViewData["Province"] = new SelectList(list, "ProvinceID", "ProvinceName");
            if (list != null && list.ToList().Count > 0)
            {
                var list2 = areaService.GetCityByProvince(list.ToList()[0].ProvinceID
                    );
                ViewData["City"] = new SelectList(list2, "CityId", "CityName");
                if (list2 != null && list2.ToList().Count > 0)
                {
                    var list3 = areaService.GetDistrictByCity(list2.ToList()[0].CityID);
                    ViewData["Distrct"] = new SelectList(list3, "DistrictID", "DistrictName");

                }
            }
            var model = service.GetById(Guid);
            if (model == null)
            {
                model = new NewsModel();
                if (LoginUser != null)
                { model.t_News_Author = LoginUser.t_User_RealName; }
                model.t_News_Date = DateTime.Now;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(NewsModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "上传失败！";
            if (model.Recommand)
                model.t_News_Recommand = 1;
            else
                model.t_News_Recommand = 0;
            try
            {
                //获取上传图片                
                HttpPostedFileBase file1 = Request.Files["file1"];
                qch.Infrastructure.ImgModel img = null;
                if (file1 != null && file1.ContentLength > 0)
                {
                    qch.Infrastructure.FileUpService fileUpService = new qch.Infrastructure.FileUpService();
                    //上传图片的保存路径
                    img = fileUpService.SaveImageFile(file1, fileUpService.ImageUrl, false);
                }
                if (img != null)
                {
                    model.t_News_Pic = img.OriginalImg;
                }
                msg = service.Save(model);
            }
            catch (Exception ex)
            {
                msg.Data = ex.Message.ToString();
                return Json(msg);
            }
            return Json(msg);
        }
        //根据省获取市
        [HttpPost]
        public ActionResult GetCity(long Id)
        {
            var model = areaService.GetCityByProvince(Id);
            return Json(model);
        }
        //根据市获取区县
        [HttpPost]
        public ActionResult GetDis(long Id)
        {
            var model = areaService.GetDistrictByCity(Id);
            return Json(model);
        }

    }
}
