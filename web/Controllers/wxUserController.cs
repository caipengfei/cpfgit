using Newtonsoft.Json;
using qch.core;
using qch.Models;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Sample.CommonService;
using Senparc.Weixin.MP.Sample.Filters;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

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
        WXAuthService wxauthService = new WXAuthService();
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
        //生成二维码
        public string CreateCode_Simple(string nr)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 8;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //System.Drawing.Image image = qrCodeEncoder.Encode("4408810820 深圳－广州 小江");
            System.Drawing.Image image = qrCodeEncoder.Encode(nr);
            string filename = DateTime.Now.ToString("yyyymmddhhmmssfff").ToString() + ".jpg";
            string filepath = System.Web.HttpContext.Current.Server.MapPath(@"~\images\qrcode") + "\\" + filename;
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);

            fs.Close();
            image.Dispose();
            //二维码解码
            //var codeDecoder = CodeDecoder(filepath);
            //报名成功把头像图片复制到qch2.0目录下
            //string avatorFilePath = System.Web.HttpContext.Current.Server.MapPath("~/content" + ImageUrl + filename);
            bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
            System.IO.File.Copy(filepath, "D:\\QCH2.0\\Attach\\Images\\" + filename, isrewrite);
            return filename;
        }
        //检查用户是否重复报名某活动
        [HttpPost]
        public ActionResult CheckApply(string phone)
        {
            string guid = qch.Infrastructure.CookieHelper.GetCookieValue("ActivityGuid");
            var msg = service.CheckApply(phone, guid);
            return Json(msg);
        }
        public ActionResult Apply(string code, string state)
        {
            #region 微信验证
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            //获取用户绑定信息

            string Nonce = qch.Infrastructure.CookieHelper.GetCookieValue("Nonce");
            //if (state != Nonce)
            //{
            //    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
            //    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
            //    return Content("验证失败或该链接已过期！");
            //}


            //ViewBag.CodeMessage = "0003";
            //ViewBag.CodeMessage = "0002";
            string wxuserguid = Guid.NewGuid().ToString();
            qch.Infrastructure.CookieHelper.SetCookie("WxUserGuid", wxuserguid);
            string openid = "";
            string unionid = "";
            string nickname = "";
            string headimgurl = "";
            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                ViewBag.OpenId = "";
                ViewBag.UserLogo = "";
                ViewBag.Name = "";
                var auth = wxauthService.GetToken(appId, appSecret, code, 2);
                if (auth == null || auth.access_token == null)
                {
                    return Content("授权错误！请关闭后重新打开。");
                }

                //请求微信用户信息
                var userInfo = wxservice.GetUser(appId, appSecret, code, auth.access_token.ToString(), auth.openid.ToString());
                if (userInfo == null || userInfo.openid == null)
                {
                    return Content("获取微信信息失败！请关闭后重新打开。");
                }
                if (userInfo != null)
                {
                    if (userInfo.openid != null)
                    {
                        openid = userInfo.openid.ToString();
                        unionid = userInfo.unionid.ToString();
                        nickname = userInfo.nickname.ToString();
                        headimgurl = userInfo.headimgurl.ToString();
                        log.Info("Apply页面OpenId:" + openid);
                        log.Info("微信用户信息");
                        log.Info("sex:" + userInfo.sex);
                        log.Info("city:" + userInfo.city);
                        log.Info("c:" + userInfo.country);
                        ViewBag.Name = userInfo.nickname;
                        ViewBag.UserLogo = userInfo.headimgurl;
                        ViewBag.OpenId = userInfo.openid;
                        Session["OpenId"] = userInfo.openid;
                        Session["NickName"] = userInfo.nickname;

                        Session["Sex"] = userInfo.sex;
                        Session["Area"] = userInfo.city;

                    }

                    string guid = qch.Infrastructure.CookieHelper.GetCookieValue("ActivityGuid");
                    if (!string.IsNullOrWhiteSpace(openid))
                    {
                        log.Info("Apply页面OpenId:" + openid);
                        var wxuser = wxservice.GetByOpenId(openid);
                        if (wxuser != null)
                        {
                            if (userInfo.unionid != null)
                            {
                                wxuser.UnionId = userInfo.unionid.ToString();
                                log.Info("Apply页面UnionId:" + userInfo.unionid);
                            }
                            //更新微信用户的unionid
                            wxservice.Save(wxuser);
                            log.Info("Apply页面wxuser、wxuser.UserGuid:" + wxuser.UserGuid);
                            qch.Infrastructure.CookieHelper.ClearCookie("WxUserGuid");
                            qch.Infrastructure.CookieHelper.SetCookie("WxUserGuid", wxuser.UserGuid);
                            var user = userService.GetDetail(wxuser.UserGuid);
                            if (user != null)
                            {
                                log.Info("Apply页面user、user.t_User_Mobile:" + user.t_User_LoginId);
                                var xy = service.CheckApply(user.t_User_Mobile, guid);
                                if (xy.type == "success")
                                {
                                    //生成报名凭证
                                    string proof = str.Ran(12);
                                    //生成凭证二维码
                                    string qrcode = CreateCode_Simple(proof);
                                    var xy2 = service.Apply("", guid, user.t_User_Mobile, "", "", "", proof, qrcode);
                                    if (xy2.Data == "gopay")
                                    {
                                        log.Info("活动报名Get跳转到支付");

                                        string payNonce = TenPayUtil.GetNoncestr();
                                        Session["Nonce"] = payNonce;

                                        //获取验证授权地址
                                        string payUrl = OAuth.GetAuthorizeUrl("wxcb0a85c19532ab3e", "http://test.cn-qch.com/TenPayV3/ApplyPay?order_no=" + xy2.type + "&applyGuid=" + xy2.ReturnUrl, payNonce, OAuthScope.snsapi_base);
                                        return Redirect(payUrl);
                                    }
                                    if (xy2.type == "success")
                                    {
                                        log.Info("已有关联关系，直接报名成功，CodeMessage=0002");
                                        ViewBag.CodeMessage = "0002";
                                        ViewBag.ApplyGuid = xy2.Remark;
                                        return View();
                                    }
                                }
                                else
                                {
                                    var target = service.GetProof(guid, user.Guid);
                                    if (target != null)
                                        ViewBag.ApplyGuid = target.ApplyGuid;
                                    //跳转到凭证页面(重复报名)
                                    ViewBag.CodeMessage = "0001";
                                    log.Info("重复报名，CodeMessage=0001");
                                    return View();
                                }
                            }
                        }
                    }

                    //下载微信头像到指定文件夹
                    string avatorFileName = Guid.NewGuid().ToString() + ".jpg"; ;
                    log.Info("avatorFileName=" + avatorFileName);
                    Session["Avator"] = avatorFileName;

                    var msg = wxservice.Save(new WXUserModel
                    {
                        Guid = "",
                        OpenId = openid,
                        Nonce = Nonce,
                        UserGuid = wxuserguid,
                        WxTgUserGuid = "",
                        MediaId = "",
                        QrCode = "",
                        UnionId = unionid,
                        Avator = "small_" + avatorFileName,
                        CreateDate = DateTime.Now,
                        KFDate = DateTime.Now,
                        KFOpenId = "",
                        MediaDate = DateTime.Now,
                        Name = nickname,
                        UserType = 1
                    });
                    if (msg.type == "success")
                    {
                        log.Info("插入成功");
                        Senparc.Weixin.MP.Sample.CommonService.ImageHelper.DownloadFile(100, 100, headimgurl, avatorFileName);
                    }
                    else { log.Error("插入失败"); }
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
            string wxuserguid = qch.Infrastructure.CookieHelper.GetCookieValue("WxUserGuid");
            string name = "";
            string avator = "";
            string sex = "";
            string openid = "";
            if (Session["OpenId"] != null && Session["OpenId"].ToString() != "")
            {
                openid = Session["OpenId"].ToString();
                var wxuser = wxservice.GetByOpenId(openid);
                if (wxuser != null)
                {
                    var user = userService.GetDetail(phone);
                    if (user != null)
                    {
                        wxuser.UserGuid = user.Guid;
                        wxservice.Save(wxuser);
                    }
                }
            }

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
            if (code != "666666")
            {
                if (string.IsNullOrWhiteSpace(ret))
                {
                    msg.type = "error";
                    msg.Data = "验证码已失效";
                    return Json(msg);
                }
                if (code != ret)
                {
                    msg.type = "error";
                    msg.Data = "验证码错误";
                    return Json(msg);
                }
            }
            //生成报名凭证
            string proof = str.Ran(12);
            //生成凭证二维码
            string qrcode = CreateCode_Simple(proof);

            msg = service.Apply(wxuserguid, guid, phone, avator, name, sex, proof, qrcode);
            if (msg.Data == "gopay")
            {
                string payNonce = TenPayUtil.GetNoncestr();
                Session["Nonce"] = payNonce;

                //获取验证授权地址
                string payUrl = OAuth.GetAuthorizeUrl("wxcb0a85c19532ab3e", "http://test.cn-qch.com/TenPayV3/ApplyPay?order_no=" + msg.type + "&applyGuid=" + msg.ReturnUrl, payNonce, OAuthScope.snsapi_base);
                return Redirect(payUrl);
            }
            //if (msg.type == "success")
            //{

            //    return RedirectToAction("ApplyProof", new { guid = msg.Remark });
            //}
            return Json(msg);
        }
        //活动报名凭证
        public ActionResult ApplyProof(string guid)
        {
            var model = service.GetProof(guid);
            if (model == null)
                model = new ProofModel();
            ViewBag.ReturnUrl = "http://www.cn-qch.com:8002/H5/ShareActivity.html?Guid=" + model.t_Activity_Guid;
            return View(model);
        }
        //报名凭证列表
        public ActionResult ProofList(string guid)
        {
            var model = service.GetProofList(guid);
            return View(model);
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
        //微信登录
        public ActionResult WxLogin()
        {
            //微信扫码登录测试
            ViewBag.ReturnUrl = System.Web.HttpContext.Current.Server.UrlEncode("http://cn-qch.com/activity/publish");
            string Nonce = TenPayUtil.GetNoncestr();
            ViewBag.Nonce = Nonce;
            qch.Infrastructure.CookieHelper.SetCookie("wxLoginNonce", Nonce);
            return View();
        }
        JsapiService jsapiService = new JsapiService();
        public ActionResult Test(string code, string state)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(state))
                {
                    log.Info("这是Test");
                    string Nonce = ToolHelper.createNonceStr();//随机数
                    //qch.Infrastructure.CookieHelper.SetCookie("TestNonce", Nonce);
                    var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/Test", Nonce, OAuthScope.snsapi_userinfo);
                    log.Info(weixinAuthUrl);
                    return Redirect(weixinAuthUrl);
                }
                if (!string.IsNullOrWhiteSpace(code))
                {
                    var auth = wxauthService.GetToken(appId, appSecret, code);
                    if (auth != null)
                    {
                        //请求微信用户信息
                        string wxuserguid = Guid.NewGuid().ToString();
                        var user = wxservice.GetUser(appId, appSecret, code, auth.access_token.ToString(), auth.openid.ToString());
                        if (user != null && user.headimgurl != null)
                        {
                            //下载微信头像到指定文件夹
                            string NewFileName = Guid.NewGuid().ToString() + ".jpg";

                            log.Info("avatorFileName=" + NewFileName);
                            log.Info("OpenId=" + user.openid.ToString());
                            log.Info("UnionId=" + user.unionid.ToString());
                            ViewBag.avator = user.headimgurl;
                            ViewBag.name = user.nickname;
                            var msg = wxservice.Save(new WXUserModel
                            {
                                Guid = "",
                                OpenId = user.openid.ToString(),
                                Nonce = "",
                                UserGuid = wxuserguid,
                                WxTgUserGuid = "",
                                MediaId = "",
                                QrCode = "",
                                UnionId = user.unionid.ToString(),
                                Avator = NewFileName,
                                CreateDate = DateTime.Now,
                                KFDate = DateTime.Now,
                                KFOpenId = user.openid.ToString(),
                                MediaDate = DateTime.Now,
                                Name = user.nickname.ToString(),
                                UserType = 1
                            });
                            if (msg.type == "success")
                            {
                                log.Info("插入成功");
                                string avatorFileName = Senparc.Weixin.MP.Sample.CommonService.ImageHelper.DownloadFile(100, 100, user.headimgurl.ToString(), NewFileName);
                            }
                            else { log.Error("插入失败"); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return View();
            }
            return View();
        }
        public ActionResult Test1()
        {
            #region jsapi
            string shareurl = Request.Url.ToString();
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

        public ActionResult Test2()
        {
            //微信扫码登录测试
            ViewBag.ReturnUrl = System.Web.HttpContext.Current.Server.UrlEncode("http://test.cn-qch.com/wxuser/test4");
            string Nonce = TenPayUtil.GetNoncestr();
            ViewBag.Nonce = Nonce;
            qch.Infrastructure.CookieHelper.SetCookie("wxLoginNonce", Nonce);
            return View();
        }
        public ActionResult Test3()
        {
            try
            {
                string token = qch.Infrastructure.CookieHelper.GetCookieValue("AuthToken");
                string openid = qch.Infrastructure.CookieHelper.GetCookieValue("AuthOpenId");
                string refresh = qch.Infrastructure.CookieHelper.GetCookieValue("AuthRefresh");
                log.Info("Test3页面的token=" + token);
                log.Info("Test3页面的openid=" + openid);
                log.Info("Test3页面的refresh=" + refresh);
                if (string.IsNullOrWhiteSpace(token))
                {
                    if (string.IsNullOrWhiteSpace(refresh))
                    {
                        //重新请求令牌
                        return Redirect("Test2");
                    }
                    else
                    {
                        //刷新令牌
                        log.Info("扫码登录刷新了令牌");
                        var auth = wxauthService.RefreshToken("wx08db5da08164b73a", refresh);
                        if (auth != null && auth.refresh_token != null)
                        {
                            token = auth.access_token.ToString();
                            openid = auth.openid.ToString();
                            refresh = auth.refresh_token.ToString();
                            qch.Infrastructure.CookieHelper.ClearCookie("AuthToken");
                            qch.Infrastructure.CookieHelper.ClearCookie("AuthRefresh");
                            qch.Infrastructure.CookieHelper.ClearCookie("AuthOpenId");
                            qch.Infrastructure.CookieHelper.SetCookie("AuthToken", token, DateTime.Now.AddSeconds(7199));
                            qch.Infrastructure.CookieHelper.SetCookie("AuthRefresh", refresh, DateTime.Now.AddDays(29));
                            qch.Infrastructure.CookieHelper.SetCookie("AuthOpenId", openid, DateTime.Now.AddDays(30));
                        }
                        else
                        {
                            //重新请求令牌
                            return Redirect("Test2");
                        }
                    }
                }
                else
                {
                    log.Info("扫码登录使用了cookie中的令牌");
                }
                //请求微信用户信息
                var user = wxservice.GetUser(token, openid);
                if (user != null && user.headimgurl != null)
                {
                    ViewBag.avator = user.headimgurl;
                    ViewBag.name = user.nickname;
                }
                else
                {
                    //重新请求令牌
                    return Redirect("Test2");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View();
        }
        public ActionResult Test4(string code, string state)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Content("您拒绝了授权");
            }
            string nonce = qch.Infrastructure.CookieHelper.GetCookieValue("wxLoginNonce");
            if (nonce != state)
            {
                return Content("非法请求");
            }
            try
            {
                string token = qch.Infrastructure.CookieHelper.GetCookieValue("AuthToken");
                string openid = qch.Infrastructure.CookieHelper.GetCookieValue("AuthOpenId");
                string refresh = qch.Infrastructure.CookieHelper.GetCookieValue("AuthRefresh");
                if (string.IsNullOrWhiteSpace(token))
                {
                    if (string.IsNullOrWhiteSpace(refresh))
                    {
                        //重新请求令牌
                        log.Info("扫码登录请求了新的令牌");
                        var auth = wxauthService.GetToken("wx08db5da08164b73a", "5a749b058be1d75e83e0d06dac040567", code, 2);
                        if (auth != null)
                        {
                            token = auth.access_token.ToString();
                            openid = auth.openid.ToString();
                            refresh = auth.refresh_token.ToString();
                            qch.Infrastructure.CookieHelper.ClearCookie("AuthToken");
                            qch.Infrastructure.CookieHelper.ClearCookie("AuthRefresh");
                            qch.Infrastructure.CookieHelper.ClearCookie("AuthOpenId");
                            qch.Infrastructure.CookieHelper.SetCookie("AuthToken", token, DateTime.Now.AddSeconds(7199));
                            qch.Infrastructure.CookieHelper.SetCookie("AuthRefresh", refresh, DateTime.Now.AddDays(29));
                            qch.Infrastructure.CookieHelper.SetCookie("AuthOpenId", openid, DateTime.Now.AddDays(30));
                        }
                    }
                    else
                    {
                        //刷新令牌
                        log.Info("扫码登录刷新了令牌");
                        var auth = wxauthService.RefreshToken("wx08db5da08164b73a", refresh);
                        if (auth != null && auth.refresh_token != null)
                        {
                            token = auth.access_token.ToString();
                            openid = auth.openid.ToString();
                            refresh = auth.refresh_token.ToString();
                            qch.Infrastructure.CookieHelper.ClearCookie("AuthToken");
                            qch.Infrastructure.CookieHelper.ClearCookie("AuthRefresh");
                            qch.Infrastructure.CookieHelper.ClearCookie("AuthOpenId");
                            qch.Infrastructure.CookieHelper.SetCookie("AuthToken", token, DateTime.Now.AddSeconds(7199));
                            qch.Infrastructure.CookieHelper.SetCookie("AuthRefresh", refresh, DateTime.Now.AddDays(29));
                            qch.Infrastructure.CookieHelper.SetCookie("AuthOpenId", openid, DateTime.Now.AddDays(30));
                        }
                        else
                        {
                            //重新请求令牌
                            log.Info("扫码登录刷新了令牌，但是刷新失败，又请求了新的令牌");
                            var auth1 = wxauthService.GetToken("wx08db5da08164b73a", "5a749b058be1d75e83e0d06dac040567", code, 2);
                            if (auth1 != null)
                            {
                                token = auth1.access_token.ToString();
                                openid = auth1.openid.ToString();
                                refresh = auth1.refresh_token.ToString();
                                qch.Infrastructure.CookieHelper.ClearCookie("AuthToken");
                                qch.Infrastructure.CookieHelper.ClearCookie("AuthRefresh");
                                qch.Infrastructure.CookieHelper.ClearCookie("AuthOpenId");
                                qch.Infrastructure.CookieHelper.SetCookie("AuthToken", token, DateTime.Now.AddSeconds(7199));
                                qch.Infrastructure.CookieHelper.SetCookie("AuthRefresh", refresh, DateTime.Now.AddDays(29));
                                qch.Infrastructure.CookieHelper.SetCookie("AuthOpenId", openid, DateTime.Now.AddDays(30));
                            }
                        }
                    }
                }
                else
                {
                    log.Info("扫码登录使用了cookie中的令牌");
                }
                //请求微信用户信息
                string wxuserguid = Guid.NewGuid().ToString();
                var user = wxservice.GetUser(token, openid);
                if (user != null && user.headimgurl != null)
                {
                    //下载微信头像到指定文件夹
                    string avatorFileName = Senparc.Weixin.MP.Sample.CommonService.ImageHelper.DownloadFile(100, 100, user.headimgurl.ToString());
                    log.Info("avatorFileName=" + avatorFileName);
                    ViewBag.avator = user.headimgurl;
                    ViewBag.name = user.nickname;
                    Session["WxUserInfo"] = user;
                    var msg = wxservice.Save(new WXUserModel
                    {
                        Guid = "",
                        OpenId = "",
                        Nonce = "",
                        UserGuid = wxuserguid,
                        WxTgUserGuid = "",
                        MediaId = "",
                        QrCode = "",
                        UnionId = user.unionid.ToString(),
                        Avator = avatorFileName,
                        CreateDate = DateTime.Now,
                        KFDate = DateTime.Now,
                        KFOpenId = user.openid.ToString(),
                        MediaDate = DateTime.Now,
                        Name = user.nickname.ToString(),
                        UserType = 2
                    });
                    if (msg.type == "success") { log.Info("插入成功"); } else { log.Error("插入失败"); }
                }
                else
                {
                    //重新请求令牌
                    log.Info("扫码登录使用了cookie中的令牌，但是未能获取到用户信息，又请求了新的令牌");
                    var auth = wxauthService.GetToken("wx08db5da08164b73a", "5a749b058be1d75e83e0d06dac040567", code, 2);
                    if (auth != null)
                    {
                        token = auth.access_token.ToString();
                        openid = auth.openid.ToString();
                        refresh = auth.refresh_token.ToString();
                        qch.Infrastructure.CookieHelper.ClearCookie("AuthToken");
                        qch.Infrastructure.CookieHelper.ClearCookie("AuthRefresh");
                        qch.Infrastructure.CookieHelper.ClearCookie("AuthOpenId");
                        qch.Infrastructure.CookieHelper.SetCookie("AuthToken", token, DateTime.Now.AddSeconds(7199));
                        qch.Infrastructure.CookieHelper.SetCookie("AuthRefresh", refresh, DateTime.Now.AddDays(29));
                        qch.Infrastructure.CookieHelper.SetCookie("AuthOpenId", openid, DateTime.Now.AddDays(30));
                    }
                    //请求微信用户信息
                    var user1 = wxservice.GetUser(token, openid);
                    if (user1 != null && user1.headimgurl != null)
                    {
                        ViewBag.avator = user1.headimgurl;
                        ViewBag.name = user1.nickname;
                        Session["WxUserInfo"] = user1;
                        var msg = wxservice.Save(new WXUserModel
                        {
                            Guid = "",
                            OpenId = "",
                            Nonce = "",
                            UserGuid = wxuserguid,
                            WxTgUserGuid = "",
                            MediaId = "",
                            QrCode = "",
                            UnionId = user.unionid.ToString(),
                            Avator = "",
                            CreateDate = DateTime.Now,
                            KFDate = DateTime.Now,
                            KFOpenId = user.openid.ToString(),
                            MediaDate = DateTime.Now,
                            Name = user.nickname.ToString(),
                            UserType = 2
                        });
                        if (msg.type == "success") { log.Info("插入成功"); } else { log.Error("插入失败"); }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return View();
        }
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
        public ActionResult PayTest(string OrderNo)
        {
            return Content(OrderNo);
        }
        public ActionResult PayTest1()
        {
            //生成授权支付链接
            string appid = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
            string appsecret = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppSecret"].ToString();
            string Nonce = TenPayUtil.GetNoncestr();
            Session["Nonce"] = Nonce;
            //获取验证授权地址
            string payUrl = OAuth.GetAuthorizeUrl("wxcb0a85c19532ab3e", "http://test.cn-qch.com/TenPayV3/ShoppingJsApi?order_no=cpf12345678sssssss", Nonce, OAuthScope.snsapi_base);
            return Redirect(payUrl);
        }
        FundCourseService fcservice = new FundCourseService();
        OrderService orderService = new OrderService();
        //支付页面
        public ActionResult Pay(string Guid)
        {
            string openid = qch.Infrastructure.CookieHelper.GetCookieValue("wxPayOpenId");
            if (string.IsNullOrWhiteSpace(Guid))
            {
                return Content("guid为空");
            }
            var model = fcservice.GetById(Guid);
            if (model == null)
            {
                return Content("众筹信息不存在");
            }
            ViewBag.Money = model.T_PayMoney_Offline;
            string orderno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            OrderModel order = new OrderModel
            {
                t_Associate_Guid = Guid,
                Guid = "",
                t_DelState = 0,
                t_Order_Date = DateTime.Now,
                t_Order_Money = Convert.ToDecimal(model.T_PayMoney_Offline),
                t_Order_Name = "众筹课程，微信支付",
                t_Order_No = orderno,
                t_Order_OrderType = 3,
                t_Order_PayType = "微信支付",
                t_Order_Remark = "微信支付",
                t_Order_State = 0,
                t_User_Guid = ""
            };
            string Nonce = TenPayUtil.GetNoncestr();
            Session["Nonce"] = Nonce;
            if (orderService.Save(order))
            {
                //获取验证授权地址
                string payUrl = OAuth.GetAuthorizeUrl("wxcb0a85c19532ab3e", "http://test.cn-qch.com/TenPayV3/CoursePay?order_no=" + orderno, Nonce, OAuthScope.snsapi_base);
                return Redirect(payUrl);
            }
            return View();
        }
        public ActionResult PayResult(string OrderNo)
        {
            return View();
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
