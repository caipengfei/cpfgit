using qch.core;
using qch.Models;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Sample.CommonService;
using Senparc.Weixin.MP.Sample.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class wxUserController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string appId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
            }
        }

        string appSecret { get { return System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppSecret"].ToString(); } }
        public static readonly string WeixinUrl = System.Configuration.ConfigurationManager.AppSettings["WeixinUrl"];
        WXUserService wxservice = new WXUserService();
        UserService userService = new UserService();
        ActivityApplyService service = new ActivityApplyService();
        /// <summary>
        /// 图片保存路径
        /// </summary>        
        public static string ImageUrl { get { return string.Format("/images/{0}/{1}/{2}/", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); } }

        //
        // GET: /wxUser/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Apply(string code, string state)
        {
            #region 微信验证
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            //通过，用code换取access_token
            var result = OAuth.GetAccessToken(appId, appSecret, code);

            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }
            //获取用户绑定信息

            string Nonce = qch.Infrastructure.CookieHelper.GetCookieValue("Nonce");

            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                ViewBag.OpenId = "";
                ViewBag.UserLogo = "";
                ViewBag.Name = "";
                var userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                if (userInfo != null)
                {
                    log.Info("微信用户信息");
                    log.Info("sex:" + userInfo.sex);
                    log.Info("city:" + userInfo.city);
                    log.Info("c:" + userInfo.country);
                    //下载微信头像到指定文件夹
                    string avatorFileName = Senparc.Weixin.MP.Sample.CommonService.ImageHelper.DownloadFile(100, 100, userInfo.headimgurl);
                    log.Info("avatorFileName=" + avatorFileName);
                    string avatorFilePath = System.Web.HttpContext.Current.Server.MapPath("~/content" + ImageUrl + avatorFileName);
                    bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                    System.IO.File.Copy(avatorFilePath, "D:\\QCH2.0\\Attach\\User\\" + avatorFileName, isrewrite);
                    ViewBag.Name = userInfo.nickname;
                    ViewBag.UserLogo = userInfo.headimgurl;
                    ViewBag.OpenId = userInfo.openid;
                    Session["OpenId"] = userInfo.openid;
                    Session["NickName"] = userInfo.nickname;
                    Session["Avator"] = avatorFileName;
                    Session["Sex"] = userInfo.sex;
                    Session["Area"] = userInfo.city;
                    var msg = wxservice.Save(new WXUserModel
                    {
                        Guid = Guid.NewGuid().ToString(),
                        OpenId = userInfo.openid,
                        Nonce = Nonce,
                        UserGuid = "",
                        WxTgUserGuid = "",
                        MediaId = "",
                        QrCode = ""
                    });
                    if (msg.type == "success") { log.Info("插入成功"); } else { log.Error("插入失败"); }
                }

                return View();
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
            #endregion
        }
        //活动报名
        [HttpPost]
        public ActionResult ActivityApply(string phone, string code)
        {
            string guid = qch.Infrastructure.CookieHelper.GetCookieValue("ActivityGuid");
            string name = "";
            string avator = "";
            string sex = "";
            if (Session["NickName"] != null && Session["NickName"].ToString() != "")
            {
                name = Session["NickName"].ToString();
            }
            if (Session["Avator"] != null && Session["Avator"].ToString() != "")
            {
                avator = Session["Avator"].ToString();
            }
            if (Session["Sex"] != null && Session["Sex"].ToString() != "")
            {
                sex = Session["Sex"].ToString() == "1" ? "男" : "女";
            }
            log.Info("post请求报名信息--------------------");
            log.Info("guid:" + guid);
            log.Info("phone:" + phone);
            log.Info("code:" + code);
            log.Info("name:" + name);
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "操作失败";
            var ret = qch.Infrastructure.CookieHelper.GetCookieValue(phone);
            log.Info("服务器生成的短信验证码：" + ret);
            if (code != ret)
            {
                msg.type = "success";
                msg.Data = "验证码错误";
                return Json(msg);
            }
            qch.Infrastructure.CookieHelper.ClearCookie(phone);
            msg = service.Apply(guid, phone, avator, name, sex);
            return Json(msg);
        }
        public ActionResult UserInfo(string guid)
        {
            if (!string.IsNullOrWhiteSpace(guid))
            {
                qch.Infrastructure.CookieHelper.SetCookie("ActivityGuid", guid);
                log.Info("这是userinfo");
                string Nonce = ToolHelper.createNonceStr();//随机数
                qch.Infrastructure.CookieHelper.SetCookie("Nonce", Nonce);
                var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/apply", Nonce, OAuthScope.snsapi_userinfo);
                log.Info(weixinAuthUrl);
                return Redirect(weixinAuthUrl);
            }
            else
            {
                return Content("活动Id缺失");
            }
            //string a = qch.Infrastructure.DESEncrypt.Encrypt("123456");
            //var user = userService.GetById("9536cd5f-89b2-458a-8651-9fd688e3eecc");
            //if (user != null)
            //{
            //    ViewBag.UserName = user.t_User_RealName;
            //}
            //return View();
        }

        //[WxAuthorization]
        public ActionResult Bind(string code, string state, int? TgUId, string ReturnUrl)
        {
            #region 微信验证
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            //通过，用code换取access_token
            var result = OAuth.GetAccessToken(appId, appSecret, code);

            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            //获取用户绑定信息
            log.Info("当前请求授权的OpenId=" + result.openid);
            Session["OpenId"] = result.openid;


            // var bindInfo = cardweixinService.Get(result.openid);
            //if (bindInfo == null)
            //{
            //    return Content("请关注消费通商城公众号！");
            //}
            string Nonce1 = qch.Infrastructure.CookieHelper.GetCookieValue("Nonce");
            //  string Nonce2 = bindInfo.Nonce;
            log.Info("state：" + state);
            log.Info("Cookie中微信回传的随机数：" + Nonce1);
            // log.Info("数据库中微信回传的随机数：" + Nonce2);
            //if (state != Nonce1 && state!=xftwl.Infrastructure.CookieHelper.GetCookieValue("Nonce"))
            //{
            //    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
            //    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
            //    return Content("验证失败或该链接已过期！");
            //}



            Session["OAuthAccessTokenStartTime"] = DateTime.Now;
            Session["OAuthAccessToken"] = result;

            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                ViewBag.OpenId = "";
                ViewBag.UserLogo = "";
                ViewBag.Name = "";
                var userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                if (userInfo != null)
                {
                    ViewBag.Name = userInfo.nickname;
                    ViewBag.UserLogo = userInfo.headimgurl;
                    ViewBag.OpenId = userInfo.openid;
                    Session["OpenId"] = userInfo.openid;
                    Session["NickName"] = userInfo.nickname;
                    Session["OpenId"] = userInfo.openid;
                    Session["Avator"] = userInfo.headimgurl;
                    //var msg = wxservice.Save(new WXUserModel
                    //{
                    //    Guid = Guid.NewGuid().ToString(),
                    //    OpenId = userInfo.openid,
                    //    Nonce = Nonce1,
                    //    UserGuid = "123456",
                    //    WxTgUserGuid = "",
                    //    MediaId = "",
                    //    QrCode = ""
                    //});
                    //if (msg.type == "success") { log.Info("插入成功"); } else { log.Error("插入失败"); }
                }

                return View();
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
            #endregion


        }
    }
}
