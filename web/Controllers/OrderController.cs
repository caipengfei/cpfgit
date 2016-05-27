using qch.core;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class OrderController : Controller
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        OrderService orderService = new OrderService();
        ActivityApplyService applyService = new ActivityApplyService();
        ActivityService activityService = new ActivityService();
        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View();
        }
        //确认订单
        public ActionResult CreateOrder(string orderNo)
        {
            ViewBag.OrderNo = orderNo;
            var order = orderService.GetByOrderNo(orderNo);
            if (order != null)
                ViewBag.PayMoney = order.t_Order_Money;
            string payNonce = TenPayUtil.GetNoncestr();
            Session["Nonce"] = payNonce;
            //获取验证授权地址
            string payUrl = OAuth.GetAuthorizeUrl("wxcb0a85c19532ab3e", "http://test.cn-qch.com/TenPayV3/ApplyPay?order_no=" + orderNo, payNonce, OAuthScope.snsapi_base);
            ViewBag.PayUrl = payUrl;
            return View();
        }
        //支付结果
        public ActionResult PayRequest(string orderNo)
        {
            log.Info("Order=" + orderNo);
            ViewBag.OrderNo = orderNo;
            
            var order = orderService.GetByOrderNo(orderNo);
            if (order != null)
            {
                ViewBag.ApplyGuid = order.t_Order_Remark;
                log.Info("ApplyGuid=" + order.t_Order_Remark);
            }
            return View();
        }

    }
}
