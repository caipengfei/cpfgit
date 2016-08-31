using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web.Areas.Admin.Models;
using web.filters;

namespace web.Areas.Admin.Controllers
{    
    public class UserController : Controller
    {
        //
        // GET: /Admin/User/
        UserService userService = new UserService();
        UserRoleService userRoleService = new UserRoleService();

        [AdminAuthorization(Roles = "admin,financial")]
        public ActionResult Index(string phone)
        {
            ViewBag.phone = phone;
            return View();
        }
        [HttpPost]
        [AdminAuthorization(Roles = "admin,financial")]
        public ActionResult Index(int page, int pagesize, string phone)
        {
            var model = userService.GetAll(page, pagesize, phone);

            if (model != null)
            {
                return Json(new { Rows = model.Items, Total = model.TotalItems });
            }
            else
            {
                return Json(new { Rows = 0, Total = 0 });
            }
        }
        [AdminAuthorization(Roles = "admin,financial")]
        public ActionResult Edit()
        {
            return View();
        }
        
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            //string s = qch.Infrastructure.DESEncrypt.Encrypt("123456");
            Msg msg = new Msg();
            //if (!userService.ValidateSafeCode(model.SafeCode))
            //{
            //    msg.type = "error";
            //    msg.Data = "验证码错误";
            //    return Json(msg);
            //}
            msg = userService.Login(model);
            if (msg.type == "success")
            {
                msg.ReturnUrl = "/admin/center";
            }
            return Json(msg);
        }
        #region 查看用户角色
        [AdminAuthorization(Roles = "admin")]
        public ActionResult SelectRole(string id)
        {

            //查询所有角色并遍历
            UserRoleViewModel model = new UserRoleViewModel();
            model.UserGuid = id;
            model.UserName = userService.GetDetail(id).t_User_RealName;
            model.Roles = userRoleService.GetAllRole();
            model.UserRoles = userRoleService.GetUserRole(id);
            return View(model);
        }
        [HttpPost]
        [AdminAuthorization(Roles = "admin")]
        public ActionResult SelectRole(FormCollection form)
        {
            Msg msg = new Msg();
            try
            {
                var roles = userRoleService.GetAllRole();
                string UserGuid = form["UserGuid"];
                if (roles != null)
                {
                    //清除现有角色
                    userRoleService.ClearRole(UserGuid);
                    foreach (var item in roles)
                    {

                        if (form[item.RoleName] != null && form[item.RoleName].ToString() == "on")
                        {
                            //添加用户角色
                            userRoleService.Save(new UserRoleModel { Id = 0, RoleName = item.RoleName, UserGuid = UserGuid });
                        }
                    }
                }
                return Json(new Msg { type = "success", Data = "操作成功" });
            }
            catch (Exception)
            {
                return Json(new Msg { type = "error", Data = "操作失败" });
            }


        }
        #endregion
    }
}
