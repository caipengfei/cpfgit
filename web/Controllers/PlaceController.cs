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
        public ActionResult Upload()
        {
            return View();
        }
        #endregion
        #region 场地管理
        //用户某个空间的所有场地
        [UserAuthorization]
        public ActionResult ucRoom(string PlaceGuid)
        {
            if (LoginUser == null)
                return RedirectToAction("/qch/login");

            var model = service.GetPlaceStyle(PlaceGuid);

            return View(model);
        }
        //上传空间场地
        public ActionResult UploadRoom()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadRoom(PlaceStyleModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "保存失败";

            return Json(msg);
        }
        #endregion

    }
}
