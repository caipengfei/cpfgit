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
        //
        // GET: /Invitation/
        //用户推荐信息页面
        public ActionResult Index()
        {
            if (LoginUser != null)
            {
                ViewBag.Phone = LoginUser.t_User_LoginId;
                ViewBag.Integral = 0;
                int zhijie = integralService.GetIntegral(LoginUser.Guid, "zhijietuijian");
                int jianjie = integralService.GetIntegral(LoginUser.Guid, "jianjietuijian");
                ViewBag.tj1 = userService.GetReferral1(LoginUser.Guid);
                ViewBag.tj2 = userService.GetReferral2(LoginUser.Guid);
                ViewBag.Integral = zhijie + jianjie;
            }
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
        public ActionResult Inof(string UserGuid)
        {
            var model = userService.GetById(UserGuid);
            ViewBag.tuijian = userService.GetReferral1(UserGuid);
            return View(model);
        }

    }
}
