using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.TenPayLibV3;
using System.Web.Security;
using qch.Infrastructure;
using ThoughtWorks.QRCode.Codec;
using System.Web.Http.Filters;
using Senparc.Weixin.MP.Sample.CommonService;
using qch.Models;
using qch.core;

namespace Senparc.Weixin.MP.Sample.Controllers
{
    /// <summary>
    /// 根据官方的Webforms Demo改写，所以可以看到直接Response.Write()之类的用法，实际项目中不提倡这么做。
    /// </summary>
    public class TenPayV3Controller : Controller
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserService userService = new UserService();
        OrderService orderService = new OrderService();
        WXUserService wxservice = new WXUserService();
        ActivityApplyService aaservice = new ActivityApplyService();
        /// <summary>
        /// 当前服务器Url
        /// </summary>
        /// <value>The server URL.</value>
        string ServerUrl { get { return System.Web.Configuration.WebConfigurationManager.AppSettings["WeixinUrl"].ToString(); } }

        UserModel _loginUser;
        /// <summary>
        /// 登录用户
        /// </summary>
        UserModel LoginUser
        {
            get
            {

                try
                {
                    HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie 
                    FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密 
                    var loginUser = SerializeHelper.Instance.JsonDeserialize<UserLoginModel>(Ticket.UserData);//反序列化  
                    return userService.GetDetail(loginUser.LoginName);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return null;
                }
            }
            set
            {
                this._loginUser = value;
            }

        }
        string appId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
            }
        }

        string appSecret { get { return System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppSecret"].ToString(); } }
        OAuthUserInfo WxUser { get; set; }

        private static TenPayV3Info _tenPayV3Info;

        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"]];
                }
                return _tenPayV3Info;
            }
        }

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 统一下单
        /// </summary>
        /// <returns></returns>
        public ActionResult QR_Code()
        {
            decimal payMoney = 0.01m;//支付总金额
            string timeStamp = "";
            string nonceStr = "";
            string paySign = "";
            string sp_billno = "cpf1662";
            ViewData["money"] = payMoney;
            ViewData["orderno"] = sp_billno;
            ViewData["date"] = DateTime.Now;

            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            log.Info("创建支付应答对象");
            //初始化
            packageReqHandler.Init();
            log.Info("己完成初始化");

            timeStamp = TenPayUtil.GetTimestamp();
            nonceStr = TenPayUtil.GetNoncestr();

            #region 设置package订单参数
            string totalfee = "0";//价格
            totalfee = Convert.ToString(payMoney * 100);
            if (totalfee.Contains("."))
                totalfee = totalfee.Substring(0, totalfee.IndexOf("."));
            log.Info("支付上送的必要参数列表------------------------------------------------begin");
            log.Info("TenPayV3Info.AppId=" + TenPayV3Info.AppId);
            log.Info("TenPayV3Info.MchId=" + TenPayV3Info.MchId);
            log.Info("nonceStr=" + nonceStr);
            log.Info("sp_billno=" + sp_billno);
            log.Info("totalfee=" + totalfee);
            log.Info("TenPayV3Info.TenPayV3Notify=" + TenPayV3Info.TenPayV3Notify);
            log.Info("TenPayV3Type.JSAPI.ToString()=" + TenPayV3Type.JSAPI.ToString());
            log.Info("TenPayV3Info.Key=" + TenPayV3Info.Key);

            packageReqHandler.SetParameter("appid", "wxcb0a85c19532ab3e");	 //公众账号ID
            packageReqHandler.SetParameter("mch_id", "1284975001");	 //商户号
            packageReqHandler.SetParameter("device_info", "WEB");	 //设备号
            packageReqHandler.SetParameter("nonce_str", nonceStr);           //随机字符串
            packageReqHandler.SetParameter("body", "测试商品body");//商品信息 商品或支付单简要描述
            packageReqHandler.SetParameter("detail", "测试商品detail");//商品详情 商品名称明细列表
            packageReqHandler.SetParameter("out_trade_no", sp_billno);	//商家订单号
            packageReqHandler.SetParameter("total_fee", "1");			//商品金额,以分为单位(money * 100).ToString()
            packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
            packageReqHandler.SetParameter("notify_url", "http://test.cn-qch.com/TenPayV3/PayNotifyUrl");	//接收财付通通知的URL
            packageReqHandler.SetParameter("trade_type", "NATIVE");	//交易类型
            //packageReqHandler.SetParameter("openid", openIdResult.openid);	           //用户的openId
            packageReqHandler.SetParameter("attach", "0");  //设置交易类型:购物
            string sign = packageReqHandler.CreateMd5Sign("key", "qingchuanghuiqingchuanghui123456");
            packageReqHandler.SetParameter("sign", sign);
            log.Info("sign=" + sign);
            log.Info("支付上送的必要参数列表------------------------------------------------end");
            #endregion
            log.Info("设置订单金额" + totalfee);
            log.Info("己生成签名");
            string data = packageReqHandler.ParseXML();
            log.Info("转成xml的结果：" + data);
            var result1 = TenPayV3.Unifiedorder(data);
            log.Info("预支付订单信息" + result1);
            var res = XDocument.Parse(result1);
            string prepayId = res.Element("xml").Element("prepay_id").Value;//微信返回的预支付ID
            string result_code = res.Element("xml").Element("result_code").Value;
            if (result_code == "FAIL")
            {
                string err_code_des = res.Element("xml").Element("err_code_des").Value;
                return Content(err_code_des);
            }
            string code_url = res.Element("xml").Element("code_url").Value;//微信返回的code_url
            log.Info("微信返回的code_url=" + code_url);
            if (!string.IsNullOrWhiteSpace(code_url))
            {
                //生成凭证二维码
                string qrcode = qch.Infrastructure.QRcode.CreateCode_Simple(code_url, "D:\\QCH2.0\\Attach\\Images\\");
                ViewBag.QRCode = "http://120.25.106.244:8002/Attach/Images/" + qrcode;
            }
            return View();
        }
        /// <summary>
        /// 购物支付
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult ShoppingJsApi(string code, string state)
        {
            try
            {
                log.Info("url参数中的code=" + code);
                log.Info("url参数中的state=" + state);

                #region 微信支付验证
                if (string.IsNullOrEmpty(code))
                {
                    return Content("您拒绝了授权！");
                }

                if (state != Session["Nonce"].ToString())
                {
                    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                    return Content("验证失败！请从正规途径进入！");
                }

                //通过，用code换取access_token

                var openIdResult = OAuth.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);
                if (openIdResult.errcode != ReturnCode.请求成功)
                {
                    log.Error(openIdResult.errmsg);
                    return Content("错误：" + openIdResult.errmsg);
                }

                #endregion



                //ViewData["openid"] = openIdResult.openid;
                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";

                #region 订单验证
                string sp_billno = "";//订单号
                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");
                if (Request["order_no"] != null && Request["order_no"].ToString() != "")
                {
                    sp_billno = Request["order_no"].ToString();
                }

                #endregion


                decimal payMoney = 0.01m;//支付总金额

                ViewData["money"] = payMoney;
                ViewData["orderno"] = sp_billno;
                ViewData["date"] = DateTime.Now;

                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                log.Info("创建支付应答对象");
                //初始化
                packageReqHandler.Init();
                log.Info("己完成初始化");
                //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

                timeStamp = TenPayUtil.GetTimestamp();
                nonceStr = TenPayUtil.GetNoncestr();

                #region 设置package订单参数
                string totalfee = "0";//价格
                totalfee = Convert.ToString(payMoney * 100);
                if (totalfee.Contains("."))
                    totalfee = totalfee.Substring(0, totalfee.IndexOf("."));
                log.Info("支付上送的必要参数列表------------------------------------------------begin");
                log.Info("TenPayV3Info.AppId=" + TenPayV3Info.AppId);
                log.Info("TenPayV3Info.MchId=" + TenPayV3Info.MchId);
                log.Info("nonceStr=" + nonceStr);
                log.Info("sp_billno=" + sp_billno);
                log.Info("totalfee=" + totalfee);
                log.Info("TenPayV3Info.TenPayV3Notify=" + TenPayV3Info.TenPayV3Notify);
                log.Info("TenPayV3Type.JSAPI.ToString()=" + TenPayV3Type.JSAPI.ToString());
                log.Info("openIdResult.openid=" + openIdResult.openid);
                log.Info("TenPayV3Info.Key=" + TenPayV3Info.Key);

                packageReqHandler.SetParameter("appid", "wxcb0a85c19532ab3e");	 //公众账号ID
                packageReqHandler.SetParameter("mch_id", "1284975001");	 //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);           //随机字符串
                packageReqHandler.SetParameter("body", "测试商品1");//商品信息 127字符
                packageReqHandler.SetParameter("out_trade_no", sp_billno);	//商家订单号
                packageReqHandler.SetParameter("total_fee", "1");			//商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", "http://test.cn-qch.com/TenPayV3/PayNotifyUrl");	//接收财付通通知的URL
                packageReqHandler.SetParameter("trade_type", "JSAPI");	//交易类型
                packageReqHandler.SetParameter("openid", openIdResult.openid);	           //用户的openId
                packageReqHandler.SetParameter("attach", "0");  //设置交易类型:购物
                string sign = packageReqHandler.CreateMd5Sign("key", "qingchuanghuiqingchuanghui123456");
                packageReqHandler.SetParameter("sign", sign);
                log.Info("sign=" + sign);
                log.Info("支付上送的必要参数列表------------------------------------------------end");
                #endregion
                log.Info("设置订单金额" + totalfee);
                log.Info("己生成签名");
                string data = packageReqHandler.ParseXML();
                log.Info("转成xml的结果：" + data);
                var result1 = TenPayV3.Unifiedorder(data);
                log.Info("预支付订单信息" + result1);
                var res = XDocument.Parse(result1);
                string prepayId = res.Element("xml").Element("prepay_id").Value;//微信返回的预支付ID
                log.Info("微信返回的预支付ID" + prepayId);

                #region 设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                log.Info("设置支付参数");
                paySignReqHandler.SetParameter("appId", "wxcb0a85c19532ab3e");
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", "qingchuanghuiqingchuanghui123456");
                #endregion

                log.Info("支付签名为" + paySign);
                ViewData["appId"] = TenPayV3Info.AppId;
                ViewData["timeStamp"] = timeStamp;
                ViewData["nonceStr"] = nonceStr;
                ViewData["package"] = string.Format("prepay_id={0}", prepayId);
                ViewData["paySign"] = paySign;
                ViewBag.ReturnUrl = "http://test.cn-qch.com/xft/appleshared?OrderNo=cpf123456";

                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return View();
            }


        }
        public ActionResult PlacePay(string code, string state)
        {
            try
            {
                log.Info("url参数中的code=" + code);
                log.Info("url参数中的state=" + state);

                #region 微信支付验证
                if (string.IsNullOrEmpty(code))
                {
                    return Content("您拒绝了授权！");
                }

                if (state != Session["NoncePayPlace"].ToString())
                {
                    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                    return Content("验证失败！请从正规途径进入！");
                }

                //通过，用code换取access_token

                var openIdResult = OAuth.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);
                if (openIdResult.errcode != ReturnCode.请求成功)
                {
                    log.Error(openIdResult.errmsg);
                    return Content("错误：" + openIdResult.errmsg);
                }

                #endregion



                //ViewData["openid"] = openIdResult.openid;
                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";

                #region 订单验证
                string sp_billno = "";//订单号
                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");
                if (Request["order_no"] != null && Request["order_no"].ToString() != "")
                {
                    sp_billno = Request["order_no"].ToString();
                }
                var orderInfo = orderService.GetByOrderNo(sp_billno);
                if (orderInfo == null)
                {
                    log.Error("订单信息不存在，订单号：" + sp_billno);
                    return Content("错误：订单信息不存在");
                }
                #endregion

                ViewBag.PayMoney = orderInfo.t_Order_Money;
                ViewBag.OrderName = orderInfo.t_Order_Name;
                decimal payMoney = orderInfo.t_Order_Money;//支付总金额
                if (openIdResult.openid == "oU0q_uOO4dSfO4m3Ekpc1GrHhHhw")
                {
                    payMoney = 0.01m;
                }
                ViewBag.OrderNo = sp_billno;

                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                log.Info("创建支付应答对象");
                //初始化
                packageReqHandler.Init();
                log.Info("己完成初始化");
                //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

                timeStamp = TenPayUtil.GetTimestamp();
                nonceStr = TenPayUtil.GetNoncestr();

                #region 设置package订单参数
                string totalfee = "0";//价格
                totalfee = Convert.ToString(payMoney * 100);
                if (totalfee.Contains("."))
                    totalfee = totalfee.Substring(0, totalfee.IndexOf("."));
                log.Info("支付上送的必要参数列表------------------------------------------------begin");
                log.Info("TenPayV3Info.AppId=" + TenPayV3Info.AppId);
                log.Info("TenPayV3Info.MchId=" + TenPayV3Info.MchId);
                log.Info("nonceStr=" + nonceStr);
                log.Info("sp_billno=" + sp_billno);
                log.Info("totalfee=" + totalfee);
                log.Info("TenPayV3Info.TenPayV3Notify=" + TenPayV3Info.TenPayV3Notify);
                log.Info("TenPayV3Type.JSAPI.ToString()=" + TenPayV3Type.JSAPI.ToString());
                log.Info("openIdResult.openid=" + openIdResult.openid);
                log.Info("TenPayV3Info.Key=" + TenPayV3Info.Key);

                packageReqHandler.SetParameter("appid", "wxcb0a85c19532ab3e");	 //公众账号ID
                packageReqHandler.SetParameter("mch_id", "1284975001");	 //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);           //随机字符串
                packageReqHandler.SetParameter("body", orderInfo.t_Order_Name);//商品信息 127字符
                packageReqHandler.SetParameter("out_trade_no", sp_billno);	//商家订单号
                packageReqHandler.SetParameter("total_fee", totalfee);			//商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", "http://test.cn-qch.com/TenPayV3/PayNotifyUrl");	//接收财付通通知的URL
                packageReqHandler.SetParameter("trade_type", "JSAPI");	//交易类型
                packageReqHandler.SetParameter("openid", openIdResult.openid);	           //用户的openId
                packageReqHandler.SetParameter("attach", "0");  //设置交易类型:购物
                string sign = packageReqHandler.CreateMd5Sign("key", "qingchuanghuiqingchuanghui123456");
                packageReqHandler.SetParameter("sign", sign);
                log.Info("sign=" + sign);
                log.Info("支付上送的必要参数列表------------------------------------------------end");
                #endregion
                log.Info("设置订单金额" + totalfee);
                log.Info("己生成签名");
                string data = packageReqHandler.ParseXML();
                log.Info("转成xml的结果：" + data);
                var result1 = TenPayV3.Unifiedorder(data);
                log.Info("预支付订单信息" + result1);
                var res = XDocument.Parse(result1);
                string prepayId = res.Element("xml").Element("prepay_id").Value;//微信返回的预支付ID
                log.Info("微信返回的预支付ID" + prepayId);

                #region 设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                log.Info("设置支付参数");
                paySignReqHandler.SetParameter("appId", "wxcb0a85c19532ab3e");
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", "qingchuanghuiqingchuanghui123456");
                #endregion

                log.Info("支付签名为" + paySign);
                ViewData["appId"] = TenPayV3Info.AppId;
                ViewData["timeStamp"] = timeStamp;
                ViewData["nonceStr"] = nonceStr;
                ViewData["package"] = string.Format("prepay_id={0}", prepayId);
                ViewData["paySign"] = paySign;
                ViewBag.ReturnUrl = "http://test.cn-qch.com/wx/paySuccess.html?orderNo=" + sp_billno;

                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return View();
            }
        }
        public ActionResult CoursePay(string code, string state)
        {
            try
            {
                log.Info("url参数中的code=" + code);
                log.Info("url参数中的state=" + state);

                #region 微信支付验证
                if (string.IsNullOrEmpty(code))
                {
                    return Content("您拒绝了授权！");
                }

                if (state != Session["NoncePay"].ToString())
                {
                    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                    return Content("验证失败！请从正规途径进入！");
                }

                //通过，用code换取access_token

                var openIdResult = OAuth.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);
                if (openIdResult.errcode != ReturnCode.请求成功)
                {
                    log.Error(openIdResult.errmsg);
                    return Content("错误：" + openIdResult.errmsg);
                }

                #endregion



                //ViewData["openid"] = openIdResult.openid;
                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";

                #region 订单验证
                string sp_billno = "";//订单号
                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");
                if (Request["order_no"] != null && Request["order_no"].ToString() != "")
                {
                    sp_billno = Request["order_no"].ToString();
                }
                var orderInfo = orderService.GetByOrderNo(sp_billno);
                if (orderInfo == null)
                {
                    log.Error("订单信息不存在，订单号：" + sp_billno);
                    return Content("错误：订单信息不存在");
                }
                orderInfo.t_User_Guid = openIdResult.openid;
                //var wxuser = wxservice.GetByOpenId(openIdResult.openid);
                //if (wxuser != null && wxuser.UserGuid != null)
                //{
                //    orderInfo.t_User_Guid = wxuser.UserGuid;
                //}
                ////更新订单的用户GUID
                //orderService.Save(orderInfo);
                #endregion

                ViewBag.PayMoney = orderInfo.t_Order_Money;
                decimal payMoney = orderInfo.t_Order_Money;//支付总金额
                if (openIdResult.openid == "oU0q_uOO4dSfO4m3Ekpc1GrHhHhw")
                {
                    payMoney = 0.01m;
                }
                ViewData["money"] = payMoney;
                ViewData["orderno"] = sp_billno;
                ViewData["date"] = DateTime.Now;

                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                log.Info("创建支付应答对象");
                //初始化
                packageReqHandler.Init();
                log.Info("己完成初始化");
                //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

                timeStamp = TenPayUtil.GetTimestamp();
                nonceStr = TenPayUtil.GetNoncestr();

                #region 设置package订单参数
                string totalfee = "0";//价格
                totalfee = Convert.ToString(payMoney * 100);
                if (totalfee.Contains("."))
                    totalfee = totalfee.Substring(0, totalfee.IndexOf("."));
                log.Info("支付上送的必要参数列表------------------------------------------------begin");
                log.Info("TenPayV3Info.AppId=" + TenPayV3Info.AppId);
                log.Info("TenPayV3Info.MchId=" + TenPayV3Info.MchId);
                log.Info("nonceStr=" + nonceStr);
                log.Info("sp_billno=" + sp_billno);
                log.Info("totalfee=" + totalfee);
                log.Info("TenPayV3Info.TenPayV3Notify=" + TenPayV3Info.TenPayV3Notify);
                log.Info("TenPayV3Type.JSAPI.ToString()=" + TenPayV3Type.JSAPI.ToString());
                log.Info("openIdResult.openid=" + openIdResult.openid);
                log.Info("TenPayV3Info.Key=" + TenPayV3Info.Key);

                packageReqHandler.SetParameter("appid", "wxcb0a85c19532ab3e");	 //公众账号ID
                packageReqHandler.SetParameter("mch_id", "1284975001");	 //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);           //随机字符串
                packageReqHandler.SetParameter("body", "众筹支付");//商品信息 127字符
                packageReqHandler.SetParameter("out_trade_no", sp_billno);	//商家订单号
                packageReqHandler.SetParameter("total_fee", totalfee);			//商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", "http://test.cn-qch.com/TenPayV3/PayNotifyUrl");	//接收财付通通知的URL
                packageReqHandler.SetParameter("trade_type", "JSAPI");	//交易类型
                packageReqHandler.SetParameter("openid", openIdResult.openid);	           //用户的openId
                packageReqHandler.SetParameter("attach", "0");  //设置交易类型:购物
                string sign = packageReqHandler.CreateMd5Sign("key", "qingchuanghuiqingchuanghui123456");
                packageReqHandler.SetParameter("sign", sign);
                log.Info("sign=" + sign);
                log.Info("支付上送的必要参数列表------------------------------------------------end");
                #endregion
                log.Info("设置订单金额" + totalfee);
                log.Info("己生成签名");
                string data = packageReqHandler.ParseXML();
                log.Info("转成xml的结果：" + data);
                var result1 = TenPayV3.Unifiedorder(data);
                log.Info("预支付订单信息" + result1);
                var res = XDocument.Parse(result1);
                string prepayId = res.Element("xml").Element("prepay_id").Value;//微信返回的预支付ID
                log.Info("微信返回的预支付ID" + prepayId);

                #region 设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                log.Info("设置支付参数");
                paySignReqHandler.SetParameter("appId", "wxcb0a85c19532ab3e");
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", "qingchuanghuiqingchuanghui123456");
                #endregion

                log.Info("支付签名为" + paySign);
                ViewData["appId"] = TenPayV3Info.AppId;
                ViewData["timeStamp"] = timeStamp;
                ViewData["nonceStr"] = nonceStr;
                ViewData["package"] = string.Format("prepay_id={0}", prepayId);
                ViewData["paySign"] = paySign;
                ViewBag.ReturnUrl = "http://test.cn-qch.com/wxuser/payResult?orderNo=" + sp_billno;

                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return View();
            }


        }
        /// <summary>
        /// 报名支付
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult ApplyPay(string code, string state)
        {
            try
            {
                log.Info("url参数中的code=" + code);
                log.Info("url参数中的state=" + state);

                #region 微信支付验证
                if (string.IsNullOrEmpty(code))
                {
                    return Content("您拒绝了授权！");
                }

                if (state != Session["Nonce"].ToString())
                {
                    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                    return Content("验证失败！请从正规途径进入！");
                }

                //通过，用code换取access_token

                var openIdResult = OAuth.GetAccessToken(TenPayV3Info.AppId, TenPayV3Info.AppSecret, code);
                if (openIdResult.errcode != ReturnCode.请求成功)
                {
                    log.Error(openIdResult.errmsg);
                    return Content("错误：" + openIdResult.errmsg);
                }

                #endregion

                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";
                //string applyGuid = "";
                //if (Request["applyGuid"] != null && Request["applyGuid"].ToString() != "")
                //{
                //    applyGuid = Request["applyGuid"].ToString();
                //}
                #region 订单验证
                string sp_billno = "";//订单号
                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");
                if (Request["order_no"] != null && Request["order_no"].ToString() != "")
                {
                    sp_billno = Request["order_no"].ToString();
                }
                var orderInfo = orderService.GetByOrderNo(sp_billno);
                if (orderInfo == null)
                {
                    log.Error("订单信息不存在，订单号：" + sp_billno);
                    return Content("错误：订单信息不存在");
                }
                //orderInfo.t_User_Guid = openIdResult.openid;
                //var wxuser = wxservice.GetByOpenId(openIdResult.openid);
                //if (wxuser != null && wxuser.UserGuid != null)
                //{
                //    orderInfo.t_User_Guid = wxuser.UserGuid;
                //}
                ////更新订单的用户GUID
                //orderService.Save(orderInfo);
                #endregion

                ViewBag.PayMoney = orderInfo.t_Order_Money;
                decimal payMoney = orderInfo.t_Order_Money;//支付总金额
                if (openIdResult.openid == "oU0q_uOO4dSfO4m3Ekpc1GrHhHhw")
                {
                    payMoney = 0.01m;
                }
                ViewData["money"] = payMoney;
                ViewData["orderno"] = sp_billno;
                ViewData["date"] = DateTime.Now;

                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                log.Info("创建支付应答对象");
                //初始化
                packageReqHandler.Init();
                log.Info("己完成初始化");
                //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

                timeStamp = TenPayUtil.GetTimestamp();
                nonceStr = TenPayUtil.GetNoncestr();

                #region 设置package订单参数
                string totalfee = "0";//价格
                totalfee = Convert.ToString(payMoney * 100);
                if (totalfee.Contains("."))
                    totalfee = totalfee.Substring(0, totalfee.IndexOf("."));
                log.Info("支付上送的必要参数列表------------------------------------------------begin");
                log.Info("TenPayV3Info.AppId=" + TenPayV3Info.AppId);
                log.Info("TenPayV3Info.MchId=" + TenPayV3Info.MchId);
                log.Info("nonceStr=" + nonceStr);
                log.Info("sp_billno=" + sp_billno);
                log.Info("totalfee=" + totalfee);
                log.Info("TenPayV3Info.TenPayV3Notify=" + TenPayV3Info.TenPayV3Notify);
                log.Info("TenPayV3Type.JSAPI.ToString()=" + TenPayV3Type.JSAPI.ToString());
                log.Info("openIdResult.openid=" + openIdResult.openid);
                log.Info("TenPayV3Info.Key=" + TenPayV3Info.Key);

                packageReqHandler.SetParameter("appid", "wxcb0a85c19532ab3e");	 //公众账号ID
                packageReqHandler.SetParameter("mch_id", "1284975001");	 //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);           //随机字符串
                packageReqHandler.SetParameter("body", "青创汇活动报名费用");//商品信息 127字符
                packageReqHandler.SetParameter("out_trade_no", sp_billno);	//商家订单号
                packageReqHandler.SetParameter("total_fee", totalfee);			//商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", "http://test.cn-qch.com/TenPayV3/PayNotifyUrl");	//接收财付通通知的URL
                packageReqHandler.SetParameter("trade_type", "JSAPI");	//交易类型
                packageReqHandler.SetParameter("openid", openIdResult.openid);	           //用户的openId
                packageReqHandler.SetParameter("attach", "1");  //设置交易类型:购物
                string sign = packageReqHandler.CreateMd5Sign("key", "qingchuanghuiqingchuanghui123456");
                packageReqHandler.SetParameter("sign", sign);
                log.Info("sign=" + sign);
                log.Info("支付上送的必要参数列表------------------------------------------------end");
                #endregion
                log.Info("设置订单金额" + totalfee);
                log.Info("己生成签名");
                string data = packageReqHandler.ParseXML();
                log.Info("转成xml的结果：" + data);
                var result1 = TenPayV3.Unifiedorder(data);
                log.Info("预支付订单信息" + result1);
                var res = XDocument.Parse(result1);
                string prepayId = res.Element("xml").Element("prepay_id").Value;//微信返回的预支付ID
                log.Info("微信返回的预支付ID" + prepayId);

                #region 设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                log.Info("设置支付参数");
                paySignReqHandler.SetParameter("appId", "wxcb0a85c19532ab3e");
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", "qingchuanghuiqingchuanghui123456");
                #endregion

                log.Info("支付签名为" + paySign);
                ViewData["appId"] = TenPayV3Info.AppId;
                ViewData["timeStamp"] = timeStamp;
                ViewData["nonceStr"] = nonceStr;
                ViewData["package"] = string.Format("prepay_id={0}", prepayId);
                ViewData["paySign"] = paySign;
                ViewBag.ReturnUrl = "http://test.cn-qch.com/order/PayRequest?orderNo=" + sp_billno;

                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return View();
            }


        }

        /// <summary>
        /// 微信支付通知
        /// </summary>
        /// <returns></returns>
        public ActionResult PayNotifyUrl()
        {
            try
            {
                log.Info("微信支付回传通知------------------------------");
                TenPayLibV3.ResponseHandler resHandler = new TenPayLibV3.ResponseHandler(null);
                string result_code = resHandler.GetParameter("result_code");//业务结果：SUCCESS/FAIL
                log.Info("result_code=" + result_code);
                string appid = resHandler.GetParameter("appid");
                log.Info("appid=" + appid);
                string mch_id = resHandler.GetParameter("mch_id");
                string device_info = resHandler.GetParameter("device_info");
                string nonce_str = resHandler.GetParameter("nonce_str");
                string sign = resHandler.GetParameter("sign");
                string err_code = resHandler.GetParameter("err_code");
                string err_code_des = resHandler.GetParameter("err_code_des");
                string openid = resHandler.GetParameter("openid");
                string is_subscribe = resHandler.GetParameter("is_subscribe");
                string trade_type = resHandler.GetParameter("trade_type");
                string bank_type = resHandler.GetParameter("bank_type");
                string total_fee = resHandler.GetParameter("total_fee");//总金额
                string coupon_fee = resHandler.GetParameter("coupon_fee");
                string fee_type = resHandler.GetParameter("fee_type");
                string transaction_id = resHandler.GetParameter("transaction_id");//微信支付订单号
                string out_trade_no = resHandler.GetParameter("out_trade_no");//商户订单号
                string attach = resHandler.GetParameter("attach");
                string time_end = resHandler.GetParameter("time_end");//支付完成时间
                log.Info("out_trade_no=" + out_trade_no);
                //string filename = string.Format("pay{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));
                //string file = Server.MapPath("~/App_Data/" + filename);
                //var fileStream = System.IO.File.OpenWrite(Server.MapPath("~/App_Data/" + filename));
                //fileStream.Write(Encoding.Default.GetBytes(result_code), 0, Encoding.Default.GetByteCount(result_code));
                //fileStream.Close();

                //正确的订单处理  
                if (result_code == "SUCCESS")//业务结果
                {
                    /***********************充值成功**********************/
                    //处理数据库逻辑
                    //注意交易单不要重复处理
                    //注意判断返回金额
                    decimal money = Convert.ToDecimal(total_fee) / 100;
                    log.Info(string.Format("订单号{0},金额：{1},己成功支付", out_trade_no, money));
                    //调用处理支付成功后的业务
                    var msg = orderService.Pay(out_trade_no, money);
                    log.Info("订单支付成功后，系统处理结果：" + msg.Data);
                    //处理报名
                    if (attach == "1")
                    {
                        log.Info("活动报名支付后：" + msg.Data);
                    }
                    //return Content(string.Format("订单号{0},金额：{1},己成功支付", out_trade_no, money));
                    //attach 0表示充值，1表示活动支付
                    //报名时只更新订单状态
                    //充值的时候更新订单状态
                    //if (attach == "0")
                    //{
                    //    //要防止重复处理


                    //}
                    //------------------------------
                    //处理业务完毕
                    //------------------------------
                }
                else
                {
                    log.Error(string.Format("错误：订单号{0}", out_trade_no));
                    //return Content(result_code);
                }

                return Content(result_code);//返回给微信的结果
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }




        }
        /// <summary>
        /// 微信支付通知App使用
        /// </summary>
        /// <returns></returns>
        public ActionResult PayNotifyUrl2()
        {
            try
            {
                log.Info("微信支付回传通知（APP）------------------------------");
                TenPayLibV3.ResponseHandler resHandler = new TenPayLibV3.ResponseHandler(null);
                string result_code = resHandler.GetParameter("result_code");//业务结果：SUCCESS/FAIL
                log.Info("result_code=" + result_code);
                string appid = resHandler.GetParameter("appid");
                log.Info("appid=" + appid);
                string mch_id = resHandler.GetParameter("mch_id");
                string device_info = resHandler.GetParameter("device_info");
                string nonce_str = resHandler.GetParameter("nonce_str");
                string sign = resHandler.GetParameter("sign");
                string err_code = resHandler.GetParameter("err_code");
                string err_code_des = resHandler.GetParameter("err_code_des");
                string openid = resHandler.GetParameter("openid");
                string is_subscribe = resHandler.GetParameter("is_subscribe");
                string trade_type = resHandler.GetParameter("trade_type");
                string bank_type = resHandler.GetParameter("bank_type");
                string total_fee = resHandler.GetParameter("total_fee");//总金额
                string coupon_fee = resHandler.GetParameter("coupon_fee");
                string fee_type = resHandler.GetParameter("fee_type");
                string transaction_id = resHandler.GetParameter("transaction_id");//微信支付订单号
                string out_trade_no = resHandler.GetParameter("out_trade_no");//商户订单号
                string attach = resHandler.GetParameter("attach");
                string time_end = resHandler.GetParameter("time_end");//支付完成时间
                log.Info("out_trade_no=" + out_trade_no);
                //string filename = string.Format("pay{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));
                //string file = Server.MapPath("~/App_Data/" + filename);
                //var fileStream = System.IO.File.OpenWrite(Server.MapPath("~/App_Data/" + filename));
                //fileStream.Write(Encoding.Default.GetBytes(result_code), 0, Encoding.Default.GetByteCount(result_code));
                //fileStream.Close();
                //decimal money = Convert.ToDecimal(total_fee) / 100;
                //正确的订单处理  
                if (result_code == "SUCCESS")//业务结果
                {
                    /***********************充值成功**********************/
                    //处理数据库逻辑
                    //注意交易单不要重复处理
                    //注意判断返回金额
                    decimal money = Convert.ToDecimal(total_fee) / 100;
                    log.Info(string.Format("订单号{0},金额：{1},己成功支付", out_trade_no, money));
                    //调用处理支付成功后的业务
                    var msg = orderService.Pay(out_trade_no, money);
                    log.Info("订单支付成功后，系统处理结果：" + msg.Data);
                    //return Content(string.Format("订单号{0},金额：{1},己成功支付", out_trade_no, money));
                    //attach 0表示充值，1表示活动支付
                    //报名时只更新订单状态
                    //充值的时候更新订单状态
                    //if (attach == "0")
                    //{
                    //    //要防止重复处理


                    //}
                    //------------------------------
                    //处理业务完毕
                    //------------------------------
                }
                else
                {
                    log.Error(string.Format("错误：订单号{0}", out_trade_no));
                    //return Content(result_code);
                }

                return Content(result_code);//返回给微信的结果
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }




        }

        /// <summary>
        /// 扫码支付通知
        /// </summary>
        /// <returns></returns>
        public ActionResult PayNotifyUrl3()
        {
            try
            {
                log.Info("微信支付回传通知（扫码支付）------------------------------");
                TenPayLibV3.ResponseHandler resHandler = new TenPayLibV3.ResponseHandler(null);
                string result_code = resHandler.GetParameter("result_code");//业务结果：SUCCESS/FAIL
                log.Info("result_code=" + result_code);
                string appid = resHandler.GetParameter("appid");
                log.Info("appid=" + appid);
                string mch_id = resHandler.GetParameter("mch_id");
                string device_info = resHandler.GetParameter("device_info");
                string nonce_str = resHandler.GetParameter("nonce_str");
                string sign = resHandler.GetParameter("sign");
                string err_code = resHandler.GetParameter("err_code");
                string err_code_des = resHandler.GetParameter("err_code_des");
                string openid = resHandler.GetParameter("openid");
                string is_subscribe = resHandler.GetParameter("is_subscribe");
                string trade_type = resHandler.GetParameter("trade_type");
                string bank_type = resHandler.GetParameter("bank_type");
                string total_fee = resHandler.GetParameter("total_fee");//总金额
                string coupon_fee = resHandler.GetParameter("coupon_fee");
                string fee_type = resHandler.GetParameter("fee_type");
                string transaction_id = resHandler.GetParameter("transaction_id");//微信支付订单号
                string out_trade_no = resHandler.GetParameter("out_trade_no");//商户订单号
                string attach = resHandler.GetParameter("attach");
                string time_end = resHandler.GetParameter("time_end");//支付完成时间
                log.Info("out_trade_no=" + out_trade_no);
                //string filename = string.Format("pay{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));
                //string file = Server.MapPath("~/App_Data/" + filename);
                //var fileStream = System.IO.File.OpenWrite(Server.MapPath("~/App_Data/" + filename));
                //fileStream.Write(Encoding.Default.GetBytes(result_code), 0, Encoding.Default.GetByteCount(result_code));
                //fileStream.Close();

                //正确的订单处理  
                if (result_code == "SUCCESS")//业务结果
                {
                    /***********************充值成功**********************/
                    //处理数据库逻辑
                    //注意交易单不要重复处理
                    //注意判断返回金额
                    decimal money = Convert.ToDecimal(total_fee) / 100;
                    log.Info(string.Format("订单号{0},金额：{1},己成功支付", out_trade_no, money));
                    //调用处理支付成功后的业务
                    var msg = orderService.Pay(out_trade_no, money);
                    log.Info("订单支付成功后，系统处理结果：" + msg.Data);
                    //处理报名
                    if (attach == "1")
                    {
                        log.Info("活动报名支付后：" + msg.Data);
                    }
                    //return Content(string.Format("订单号{0},金额：{1},己成功支付", out_trade_no, money));
                    //attach 0表示充值，1表示活动支付
                    //报名时只更新订单状态
                    //充值的时候更新订单状态
                    //if (attach == "0")
                    //{
                    //    //要防止重复处理


                    //}
                    //------------------------------
                    //处理业务完毕
                    //------------------------------
                }
                else
                {
                    log.Error(string.Format("错误：订单号{0}", out_trade_no));
                    //return Content(result_code);
                }

                return Content(result_code);//返回给微信的结果
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }




        }
    }

    /// <summary>
    /// 微信支付返回结果
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// 商品或支付单简要描述
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 支付总金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 订单类型：0购物订单,1充值订单
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// 充值时的会员卡信息：卡号|充值天数 ，用|分隔
        /// </summary>
        public string CardInfo { get; set; }



    }


}
