using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class UserController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserInfo()
        {
            log.Info("这是userinfo");
            return View();
        }
    }
}
