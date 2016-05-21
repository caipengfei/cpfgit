using qch.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace web.filters
{
    public class AdminAuthorization : ActionFilterAttribute
    {
        UserService userService = new UserService();
        UserRoleService userRoleService = new UserRoleService();
        /// <summary>
        /// 指定允许访问的用户角色
        /// </summary>
        public string Roles { get; set; }
        /// <summary>
        /// 待返回的URL
        /// </summary>
        public string ReturnUrl { get; set; }
        public override void OnActionExecuting( ActionExecutingContext filterContext )
        {
            var user = filterContext.HttpContext.User;
            if (user == null || !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {

                filterContext.Result = new RedirectResult( "/Admin/User/Login" );
            }
            else
            {
                var userInfo = userService.GetLoginUser();//当前登录用户信息
                var userRole = userRoleService.GetAll(userInfo.Guid);//当前用户角色

                bool IsEnabled = false;//初始化不允许访问
                if (!string.IsNullOrEmpty( Roles ))
                { //如果指定了允许访问的角色,遍历当前登录用户角色是否包含指定的角色
                    string[] roles = Roles.Split( ',' );
                    foreach (var item in roles)
                    {
                        //如果当前用户角色包含允许的角色
                        foreach (var tt in userRole)
                        {
                            if (tt.RoleName.Contains(item))
                            {
                                IsEnabled = true;//允许角色访问
                                break;
                            }
                        }                        
                    }
                    if (IsEnabled == false)
                    {
                        filterContext.Result = new JsonResult() { Data = new qch.core.Msg { type = "error", Data = "没有管理权限无法访问！" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
            }
            base.OnActionExecuting( filterContext );
        }


    }
}