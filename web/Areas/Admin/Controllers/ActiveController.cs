using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web.filters;

namespace web.Areas.Admin.Controllers
{
    [AdminAuthorization(Roles = "admin,financial")]
    public class ActiveController : Controller
    {
        //
        // GET: /Admin/Active/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult GetAll()
        {
            return View();
        }
        public ActionResult AddGoods()
        {
            return View();
        }
        public ActionResult GetGoods()
        {
            return View();
        }
        
    }
}
