using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using qch.Infrastructure;
using qch.core;
using qch.Models;

namespace web.Filters
{

    public class UserAuthorization : ActionFilterAttribute
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserService userService = new UserService();
        UserRoleService userInRoleService = new UserRoleService();

        /// <summary>
        /// 指定允许访问的用户角色
        /// </summary>
        public string Roles { get; set; }
        /// <summary>
        /// 待返回的URL
        /// </summary>
        public string ReturnUrl { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserModel loginUser = new UserModel();
            try
            {
                HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie 
                if (authCookie != null)
                {
                    FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密 
                    var user = SerializeHelper.Instance.JsonDeserialize<UserLoginModel>(Ticket.UserData);//反序列化  
                    if (user != null)
                        loginUser = userService.GetDetail(user.LoginName);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            if (loginUser == null || string.IsNullOrWhiteSpace(loginUser.Guid))
            {
                string url = "/qch/login";
                if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    url = url + "?ReturnUrl=" + ReturnUrl;
                filterContext.Result = new RedirectResult(url);
            }

            base.OnActionExecuting(filterContext);
        }


    }
}