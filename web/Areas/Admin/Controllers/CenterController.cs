using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web.filters;

namespace web.Areas.Admin.Controllers
{
    [AdminAuthorization(Roles = "admin,financial,service")]
    public class CenterController : Controller
    {
        //
        // GET: /Admin/Center/

        public ActionResult Index()
        {
            return View();
        }

    }
}
