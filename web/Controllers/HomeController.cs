using qch.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "修改此模板以快速启动你的 ASP.NET MVC 应用程序。";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";

            return View();
        }
        
        #region 短信验证码
        [HttpPost]
        public ActionResult SendSMS(string phone)
        {
            SMS sms = new SMS();
            var msg = sms.GetSMS(phone);
            return Json(msg);
        }
        [HttpPost]
        public ActionResult CheckSMS(string phone,string code)
        {
            SMS sms = new SMS();
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "验证码错误";
            var ret = qch.Infrastructure.CookieHelper.GetCookieValue(phone);
            if (code == ret)
            {
                msg.type = "success";
                msg.Data = "验证成功";
                qch.Infrastructure.CookieHelper.ClearCookie(phone);
            }
            return Json(msg);
        }
        #endregion
        #region 验证码
        //验证码
        public ActionResult GetValidateCode()
        {
            qch.Infrastructure.MvcCaptcha vCode = new qch.Infrastructure.MvcCaptcha();
            string code = vCode.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
        #endregion
    }
}
