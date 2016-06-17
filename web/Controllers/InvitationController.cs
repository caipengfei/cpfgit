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
    /// <summary>
    /// 用户推荐关系
    /// 必须登录才能访问该控制器
    /// </summary>
    [UserAuthorization]
    public class InvitationController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserService userService = new UserService();
        IntegralService integralService = new IntegralService();

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
        JsapiService jsapiService = new JsapiService();
        string appId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
            }
        }
        //
        // GET: /Invitation/
        //用户推荐信息页面
        public ActionResult Index()
        {
            ViewBag.UserGuid = "";
            if (LoginUser != null)
            {
                ViewBag.Phone = LoginUser.t_User_LoginId;
                ViewBag.Integral = 0;
                int zhijie = integralService.GetIntegral(LoginUser.Guid, "yonghuzhuce");
                int jianjie = integralService.GetIntegral(LoginUser.Guid, "jianjietuijian");
                ViewBag.tj1 = userService.GetReferral1(LoginUser.Guid);
                ViewBag.tj2 = userService.GetReferral2(LoginUser.Guid);
                ViewBag.Integral = zhijie + jianjie;
                ViewBag.UserGuid = LoginUser.Guid;
            }
            #region jsapi
            string shareurl = Request.Url.ToString();
            log.Info("邀请好友分享链接" + shareurl);
            JsapiModel apiModel = new JsapiModel();
            apiModel = jsapiService.GetSign(shareurl, shareurl, appId);
            if (apiModel == null)
            {
                apiModel = new JsapiModel();
            }
            ViewBag.AppId = apiModel.AppId;
            ViewBag.Timestamp = apiModel.Timestamp;
            ViewBag.Noncestr = apiModel.Noncestr;
            ViewBag.Signature = apiModel.Signature;
            #endregion
            return View();
        }
        //一级推荐人列表
        public ActionResult List()
        {
            if (LoginUser != null)
            {
                var list = userService.GetReferral1(1, 20, LoginUser.Guid);
                return View(list);
            }
            return View();
        }
        public ActionResult List1(int? page)
        {
            int p = page ?? 2;
            if (LoginUser != null)
            {
                var list = userService.GetReferral1(p, 20, LoginUser.Guid);
                return View(list);
            }
            return View();
        }
        //一级推荐人明细页面
        public ActionResult Info(string UserGuid)
        {
            var model = userService.GetById(UserGuid);
            int x = 0;
            if (model != null)
            {
                if (!string.IsNullOrWhiteSpace(model.t_ReommUser))
                {
                    x = integralService.GetTJIntegral(model.t_ReommUser, model.t_User_Date);
                }
            }
            else
                model = new UserModel();
            ViewBag.TjIntegral = x;
            ViewBag.tuijian = userService.GetReferral1(UserGuid);
            return View(model);
        }

        public ActionResult Qiuyu(int? x,int? y)
        {
            var xy = x % y;
            ViewBag.XY2 = xy;
            return View();
        }

    }
}
