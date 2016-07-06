using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class GoodsController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Goods/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List(string UserGuid, string Sign)
        {
            if (string.IsNullOrWhiteSpace(UserGuid))
            {
                return Content("请从正规渠道进入");
            }
            string s = qch.Infrastructure.Encrypt.MD5Encrypt(UserGuid + "150919", true);
            if (Sign == s)
            {
                //签名正确
                return Redirect("/wx/mall.html?UserGuid=" + UserGuid);
            }
            else
            {
                //签名错误
                return Content("错误：请从正规渠道进入！");
            }
        }

    }
}
