using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Admin/User/
        UserService userService = new UserService();
        public ActionResult Index()
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
            return Json(msg);
        }
    }
}
