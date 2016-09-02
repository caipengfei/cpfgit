using qch.core;
using qch.Infrastructure;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using web.Filters;

namespace web.Controllers
{
    public class PlaceController : Controller
    {
        PlaceService service = new PlaceService();
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserService userService = new UserService();
        IntegralService integralService = new IntegralService();
        AreaService areaService = new AreaService();
        PicService picService = new PicService();
        UserModel _loginUser;
        UserModel LoginUser
        {
            get
            {
                if (_loginUser == null)
                {
                    try
                    {
                        HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie 
                        if (authCookie == null)
                            return null;
                        FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密 
                        var loginUser = SerializeHelper.Instance.JsonDeserialize<UserLoginModel>(Ticket.UserData);//反序列化  
                        return userService.GetDetail(loginUser.LoginName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        return null;
                    }
                }
                return _loginUser;
            }
            set
            {
                this._loginUser = value;
            }
        }
        //
        // GET: /Place/

        //空间列表
        public ActionResult Index(int? page, int? pagesize)
        {
            int Page = page ?? 1;
            int PageSize = pagesize ?? 10;
            var model = service.GetAllPlace(Page, PageSize);
            ViewData["Page"] = Page;
            ViewData["PageSize"] = PageSize;
            if (model != null)
            {
                ViewData["TotalPage"] = model.TotalPages;
            }
            else
            {
                ViewData["TotalPage"] = 1;
            }
            return View(model);
        }
        //空间详情
        public ActionResult Info(string Guid)
        {
            var model = service.GetPlaceInfo(Guid);
            if (model == null)
                model = new PlaceModel();
            var pics = picService.GetByGuid(model.Guid);
            if (pics == null)
                pics = new List<PicModel>();
            model.Pics = pics;
            return View(model);
        }
        #region 空间管理
        //用户空间列表
        [UserAuthorization]
        public ActionResult ucPlace(int? page, int? pagesize)
        {
            if (LoginUser == null)
                return RedirectToAction("/qch/login");
            int Page = page ?? 1;
            int PageSize = pagesize ?? 10;
            var model = service.GetAllPlace(Page, PageSize, LoginUser.Guid, 1);
            ViewData["Page"] = Page;
            ViewData["PageSize"] = PageSize;
            if (model != null)
            {
                ViewData["TotalPage"] = model.TotalPages;
            }
            else
            {
                ViewData["TotalPage"] = 1;
            }
            return View(model);
        }
        //上传空间
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        [UserAuthorization]
        public ActionResult Upload(PlaceModel model)
        {
            if (LoginUser == null)
                return RedirectToAction("/qch/login");
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "保存失败";
            var a = service.GetPlaceInfo(LoginUser.Guid, model.t_Place_Name);
            if (a != null)
            {
                msg.Data = "请勿重复上传！";
                return Json(msg);
            }
            model.t_Place_GroupID = 0;
            model.t_AddBy = LoginUser.t_User_RealName;
            model.t_User_Guid = LoginUser.Guid;
            model.t_DelState = 0;
            model.t_Place_Recommand = 0;
            model.t_Place_Audit = 0;
            model.t_Place_Tips = "";
            if (service.SavePlace(model))
            {
                msg.type = "success";
                msg.Data = "保存成功";
            }
            return Json(msg);
        }
        public ActionResult Area()
        {
            var list = areaService.GetAllProvince();
            return View(list);
        }
        #endregion
        #region 场地管理
        //用户某个空间的所有场地
        [UserAuthorization]
        public ActionResult ucRoom(string PlaceGuid)
        {
            if (LoginUser == null)
                return RedirectToAction("/qch/login");
            ViewBag.PlaceGuid = PlaceGuid;
            var model = service.GetPlaceStyle(PlaceGuid);

            return View(model);
        }
        //上传空间场地
        public ActionResult UploadRoom()
        {
            return View();
        }
        [HttpPost]
        [UserAuthorization]
        public ActionResult UploadRoom(PlaceStyleModel model)
        {
            if (LoginUser == null)
                return RedirectToAction("/qch/login");
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "保存失败";
            var a = service.GetPlaceStyleByName(model.t_Place_StyleName);
            if (a != null)
            {
                msg.Data = "请勿重复上传";
                return Json(msg);
            }
            //获取上传图片                
            HttpPostedFileBase file1 = Request.Files["file1"];
            ImgModel img = null;
            if (file1 != null && file1.ContentLength > 0)
            {
                FileUpService fileUpService = new FileUpService();
                //上传图片的保存路径
                img = fileUpService.SaveImageFile(file1, "/Images/PlaceImgs/", false);
            }
            model.t_AddBy = LoginUser.t_User_RealName;
            if (img != null)
            {
                string x = img.OriginalImg;
                if (!string.IsNullOrWhiteSpace(img.OriginalImg) && img.OriginalImg.Length > 5)
                    x = img.OriginalImg.Substring(img.OriginalImg.LastIndexOf('/') + 1);
                model.t_Place_Pic = x;
                if (service.SaveRoom(model))
                {
                    msg.type = "success";
                    msg.Data = "保存成功";
                    string avatorFilePath = System.Web.HttpContext.Current.Server.MapPath("~" + img.OriginalImg);
                    bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                    System.IO.File.Copy(avatorFilePath, "D:\\QCH2.0\\Attach\\Images\\" + x, isrewrite);
                }
            }
            return Json(msg);
        }
        #endregion

    }
}
