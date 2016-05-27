using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class AppController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        JsapiService jsapiService = new JsapiService();
        string appId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
            }
        }
        //
        // GET: /App/

        public ActionResult Index()
        {
            #region jsapi
            string shareurl = Request.Url.ToString();
            log.Info("app下载分享链接" + shareurl);
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

    }
}
