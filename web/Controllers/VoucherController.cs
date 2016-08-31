using qch.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class VoucherController : Controller
    {
        VoucherService service = new VoucherService();
        //
        // GET: /Voucher/

        public ActionResult Index()
        {
            string phone = qch.Infrastructure.CookieHelper.GetCookieValue("RegPhone");
            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (phone.Length > 7)
                {
                    phone = phone.Substring(0, 3) + "****" + phone.Substring(7);
                    ViewBag.RegPhone = phone;
                }
            }
            var voucher = service.GetByAction("yonghuzhuce");
            if (voucher == null)
                voucher = new qch.Models.VoucherModel();
            return View(voucher);
        }

    }
}
