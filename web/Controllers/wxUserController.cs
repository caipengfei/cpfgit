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

        public ActionResult Reg(string UserGuid)
        {
            if (!string.IsNullOrWhiteSpace(UserGuid))
                qch.Infrastructure.CookieHelper.SetCookie("UserGuid_tj", UserGuid);
            log.Info("这是userinfo");
            string Nonce = ToolHelper.createNonceStr();//随机数
            qch.Infrastructure.CookieHelper.SetCookie("tjNonce", Nonce);
            var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/wxreg", Nonce, OAuthScope.snsapi_userinfo);
            log.Info(weixinAuthUrl);
            return Redirect(weixinAuthUrl);
        }
        //微信用户绑定
        public ActionResult wxReg(string code, string state)
        {
            web.Models.UserRegModel rm = new Models.UserRegModel();

            ViewBag.IsReg = 0;
            //取推荐人的guid
            string tjuserguid = qch.Infrastructure.CookieHelper.GetCookieValue("UserGuid_tj");
            if (!string.IsNullOrWhiteSpace(tjuserguid))
            {
                var tjuser = userService.GetDetail(tjuserguid);
                if (tjuser != null)
                {
                    ViewBag.UserPhone = tjuser.t_User_LoginId;
                    rm.TjUser = tjuser.t_User_LoginId;
                }
            }

            #region 微信验证
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            string wxuserguid = Guid.NewGuid().ToString();
            qch.Infrastructure.CookieHelper.SetCookie("regWxUserGuid", wxuserguid);
            string openid = "";
            string unionid = "";
            string nickname = "";
            string headimgurl = "";
            try
            {

                ViewBag.OpenId = "";
                ViewBag.UserLogo = "";
                ViewBag.Name = "";
                OAuthAccessTokenResult result = null;
                if (Session["regAccountTokenResult"] != null)
                    result = (OAuthAccessTokenResult)Session["regAccountTokenResult"];
                else
                {
                    //通过，用code换取access_token
                    result = OAuth.GetAccessToken(appId, appSecret, code);
                    if (result == null || result.access_token == null)
                    {
                        return Content("授权错误！请关闭后重新打开。");
                    }
                    Session["regAccountTokenResult"] = result;
                }
                OAuthUserInfo userInfo = null;
                if (Session["regAccountUserResult"] != null)
                    userInfo = (OAuthUserInfo)Session["regAccountUserResult"];
                else
                {
                    userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                    if (userInfo == null || userInfo.openid == null)
                    {
                        return Content("获取微信信息失败！请关闭后重新打开。");
                    }
                    Session["regAccountUserResult"] = userInfo;
                }
                if (userInfo != null)
                {
                    if (userInfo.openid != null)
                    {
                        openid = userInfo.openid;
                        unionid = userInfo.unionid;
                        nickname = userInfo.nickname;
                        headimgurl = userInfo.headimgurl;
                        Session["regOpenId"] = userInfo.openid;
                        Session["regNickName"] = userInfo.nickname;
                        Session["regSex"] = userInfo.sex;
                        Session["regArea"] = userInfo.city;
                    }
                    if (!string.IsNullOrWhiteSpace(openid))
                    {
                        log.Info("wxreg页面OpenId:" + openid);
                        var wxuser = wxservice.GetByOpenId(openid);
                        if (wxuser != null)
                        {
                            ViewBag.IsReg = 1;
                            if (userInfo.unionid != null && string.IsNullOrWhiteSpace(wxuser.UnionId))
                            {
                                wxuser.UnionId = userInfo.unionid;
                                log.Info("wxreg页面UnionId:" + userInfo.unionid);
                                //更新微信用户的unionid
                                wxservice.Save(wxuser);
                            }

                            var user = userService.GetDetail(wxuser.UserGuid);
                            if (user != null)
                            {
                                ViewBag.IsReg = 2;
                                //已存在绑定信息，设置用户登录票证，跳转至个人中心
                                #region 赋于该会员授权
                                userService.SetAuthCookie(new UserLoginModel
                                {
                                    LoginName = user.t_User_LoginId,
                                    LoginPwd = ToolHelper.createNonceStr(),
                                    SafeCode = ToolHelper.createNonceStr()
                                });
                                #endregion
                                return Redirect("/Invitation");//跳转
                            }
                        }
                    }

                    //下载微信头像到指定文件夹
                    string avatorFileName = Guid.NewGuid().ToString() + ".jpg"; ;
                    log.Info("avatorFileName=" + avatorFileName);
                    Session["regAvator"] = "small_" + avatorFileName;

                    var msg = wxservice.Save(new WXUserModel
                    {
                        Guid = "",
                        OpenId = openid,
                        Nonce = "",
                        UserGuid = wxuserguid,
                        WxTgUserGuid = tjuserguid,
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
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
            #endregion
            rm.SafeCode = "";
            return View(rm);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult wxReg(web.Models.UserRegModel model)
        {
            Msg msg = new Msg();
            msg.Data = model.TjUser;
            msg.type = "error";
            msg.Data = "注册失败";
            if (model == null)
            {
                msg.Data = "数据异常";
                return Json(msg);
            }
            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                msg.Data = "手机号不能为空";
                return Json(msg);
            }
            qch.Infrastructure.CookieHelper.SetCookie("RegPhone", model.Phone);
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                msg.Data = "密码不能为空";
                return Json(msg);
            }
            var nowUser = userService.GetDetail(model.Phone);
            if (nowUser != null)
            {
                msg.Data = "手机号已被注册";
                return Json(msg);
            }
            if (!string.IsNullOrWhiteSpace(model.TjUser))
            {
                if (model.Phone == model.TjUser)
                {
                    msg.Data = "推荐人信息有误";
                    return Json(msg);
                }
                var tjuser = userService.GetDetail(model.TjUser);
                if (tjuser == null)
                {
                    msg.Data = "推荐人信息有误";
                    return Json(msg);
                }
            }
            string tjuserguid = qch.Infrastructure.CookieHelper.GetCookieValue("UserGuid_tj");
            string name = "";
            string avator = "";
            string sex = "";
            string openid = "";
            if (Session["regOpenId"] != null && Session["regOpenId"].ToString() != "")
            {
                openid = Session["regOpenId"].ToString();
                //var wxuser = wxservice.GetByOpenId(openid);
                //if (wxuser != null)
                //{
                //    var user = userService.GetDetail(model.Phone);
                //    if (user != null)
                //    {
                //        wxuser.UserGuid = user.Guid;
                //        wxservice.Save(wxuser);
                //    }
                //}
            }

            if (Session["regNickName"] != null && Session["regNickName"].ToString() != "")
            {
                name = Session["regNickName"].ToString();
            }
            if (Session["regAvator"] != null && Session["regAvator"].ToString() != "")
            {
                avator = Session["regAvator"].ToString();
            }
            if (Session["regSex"] != null && Session["regSex"].ToString() != "")
            {
                sex = Session["regSex"].ToString() == "1" ? "男" : "女";
            }

            var ret = qch.Infrastructure.CookieHelper.GetCookieValue(model.Phone);
            log.Info("服务器生成的短信验证码：" + ret);
            if (model.SafeCode != "150919")
            {
                if (string.IsNullOrWhiteSpace(ret))
                {
                    msg.type = "error";
                    msg.Data = "验证码已失效";
                    return Json(msg);
                }
                if (model.SafeCode != ret)
                {
                    msg.type = "error";
                    msg.Data = "验证码错误";
                    return Json(msg);
                }
            }
            UserModel wxUser = new UserModel
            {
                t_Andriod_Rid = "",
                t_DelState = 0,
                t_IOS_Rid = "",
                t_RongCloud_Token = "",
                t_User_Best = "",
                t_User_Birth = DateTime.Now,
                t_User_BusinessCard = "",
                t_User_City = "",
                t_User_Commpany = "",
                t_User_Complete = 0,
                t_User_Date = DateTime.Now,
                t_User_Email = "",
                t_User_FocusArea = "",
                t_User_InvestArea = "",
                t_User_InvestMoney = "",
                t_User_InvestPhase = "",
                t_User_LoginId = model.Phone,
                t_User_Mobile = model.Phone,
                t_User_NickName = name,
                t_User_Pic = avator,
                t_User_Position = "",
                t_User_Pwd = qch.Infrastructure.DESEncrypt.Encrypt(model.Password),
                t_User_RealName = name,
                t_User_Remark = "微信邀请注册",
                t_User_Sex = sex,
                t_User_Style = 0,
                t_User_ThreeLogin = "",
                t_Recommend = 0,
                t_ReommUser = tjuserguid,
                Guid = qch.Infrastructure.CookieHelper.GetCookieValue("regWxUserGuid")
            };
            if (userService.Reg(wxUser).type == "success")
            {
                //报名成功把头像图片复制到qch2.0目录下
                string avatorFilePath = System.Web.HttpContext.Current.Server.MapPath("~/content/wxavator/" + avator);
                bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                System.IO.File.Copy(avatorFilePath, "D:\\QCH2.0\\Attach\\User\\" + avator, isrewrite);

                msg.type = "success";
                msg.Data = "注册成功";
                #region 赋于该会员授权
                userService.SetAuthCookie(new UserLoginModel
                {
                    LoginName = wxUser.t_User_LoginId,
                    LoginPwd = ToolHelper.createNonceStr(),
                    SafeCode = ToolHelper.createNonceStr()
                });
                #endregion
            }
            return Json(msg);
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
                OAuthAccessTokenResult result = null;
                if (Session["AccountTokenResult"] != null)
                    result = (OAuthAccessTokenResult)Session["AccountTokenResult"];
                else
                {
                    //通过，用code换取access_token
                    result = OAuth.GetAccessToken(appId, appSecret, code);
                    if (result == null || result.access_token == null)
                    {
                        return Content("授权错误！请关闭后重新打开。");
                    }
                    Session["AccountTokenResult"] = result;
                }

                //var auth = wxauthService.GetToken(appId, appSecret, code);
                //if (auth == null || auth.access_token == null)
                //{
                //    return Content("授权错误！请关闭后重新打开。");
                //}

                //请求微信用户信息
                //var userInfo = wxservice.GetUser(appId, appSecret, code, auth.access_token.ToString(), auth.openid.ToString());
                OAuthUserInfo userInfo = null;
                if (Session["AccountUserResult"] != null)
                    userInfo = (OAuthUserInfo)Session["AccountUserResult"];
                else
                {
                    userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                    if (userInfo == null || userInfo.openid == null)
                    {
                        return Content("获取微信信息失败！请关闭后重新打开。");
                    }
                    Session["AccountUserResult"] = userInfo;
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
                            if (userInfo.unionid != null && string.IsNullOrWhiteSpace(wxuser.UnionId))
                            {
                                wxuser.UnionId = userInfo.unionid;
                                log.Info("wxreg页面UnionId:" + userInfo.unionid);
                                //更新微信用户的unionid
                                wxservice.Save(wxuser);
                            }
                            log.Info("Apply页面wxuser、wxuser.UserGuid:" + wxuser.UserGuid);
                            qch.Infrastructure.CookieHelper.ClearCookie("WxUserGuid");
                            qch.Infrastructure.CookieHelper.SetCookie("WxUserGuid", wxuser.UserGuid);
                            var user = userService.GetDetail(wxuser.UserGuid);
                            if (user != null)
                            {
                                log.Info("Apply页面user、user.t_User_Mobile:" + user.t_User_LoginId);
                                var xy = service.CheckApply(user.t_User_LoginId, guid);
                                if (xy.type == "success")
                                {
                                    //生成报名凭证
                                    string proof = str.Ran(12);
                                    //生成凭证二维码
                                    string qrcode = CreateCode_Simple(proof);
                                    var xy2 = service.Apply("", guid, user.t_User_LoginId, wxuser.Avator, wxuser.Name, "", proof, qrcode);
                                    if (xy2.Data == "gopay")
                                    {
                                        log.Info("活动报名Get跳转到支付");

                                        string payNonce = TenPayUtil.GetNoncestr();
                                        Session["Nonce"] = payNonce;

                                        //获取验证授权地址
                                        string payUrl = OAuth.GetAuthorizeUrl("wxcb0a85c19532ab3e", "http://www.cn-qch.com/TenPayV3/ApplyPay?order_no=" + xy2.type + "&applyGuid=" + xy2.ReturnUrl, payNonce, OAuthScope.snsapi_base);
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
                    Session["Avator"] = "small_" + avatorFileName;

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
            if (code != "150919")
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
                string payUrl = OAuth.GetAuthorizeUrl("wxcb0a85c19532ab3e", "http://www.cn-qch.com/TenPayV3/ApplyPay?order_no=" + msg.type + "&applyGuid=" + msg.ReturnUrl, payNonce, OAuthScope.snsapi_base);
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
            string payUrl = OAuth.GetAuthorizeUrl("wxcb0a85c19532ab3e", "http://www.cn-qch.com/TenPayV3/ShoppingJsApi?order_no=cpf12345678sssssss", Nonce, OAuthScope.snsapi_base);
            return Redirect(payUrl);
        }
        FundCourseService fcservice = new FundCourseService();
        OrderService orderService = new OrderService();
        public ActionResult MyServices(string code, string state)
        {
            string Nonce = ToolHelper.createNonceStr();//随机数
            var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/MyServices", Nonce, OAuthScope.snsapi_userinfo);
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(weixinAuthUrl);
            }
            string userguid = "";
            OAuthAccessTokenResult result = null;
            if (Session["AccountTokenResult"] != null)
                result = (OAuthAccessTokenResult)Session["AccountTokenResult"];
            else
            {
                //通过，用code换取access_token
                result = OAuth.GetAccessToken(appId, appSecret, code);
                if (result == null || result.access_token == null)
                {
                    return Redirect(weixinAuthUrl);
                }
                Session["AccountTokenResult"] = result;
            }

            //请求微信用户信息
            OAuthUserInfo userInfo = null;
            if (Session["AccountUserResult"] != null)
                userInfo = (OAuthUserInfo)Session["AccountUserResult"];
            else
            {
                userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                if (userInfo == null || userInfo.openid == null)
                {
                    return Redirect(weixinAuthUrl);
                }
                Session["AccountUserResult"] = userInfo;
            }
            var wxuser = wxservice.GetByOpenId(userInfo.openid);
            if (wxuser != null)
            {
                var user = userService.GetDetail(wxuser.UserGuid);
                if (user == null)
                {
                    var weixinAuthUrl1 = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/Bind", Nonce, OAuthScope.snsapi_userinfo);
                    return Redirect(weixinAuthUrl1);
                }
                userguid = user.Guid;
                #region 赋于该会员授权
                userService.SetAuthCookie(new UserLoginModel
                {
                    LoginName = user.t_User_LoginId,
                    LoginPwd = ToolHelper.createNonceStr(),
                    SafeCode = ToolHelper.createNonceStr()
                });
                #endregion
            }
            //ViewBag.UserGuid = userguid;
            return Redirect("/wx/MyServices.html?UserGuid=" + userguid);
        }
        //空间预约支付页面
        public ActionResult PayPlace(string StyleGuid, string TimeGuid, string code, string state)
        {
            string Nonce = ToolHelper.createNonceStr();//随机数
            var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/PayPlace?StyleGuid=" + StyleGuid + "&TimeGuid=" + TimeGuid, Nonce, OAuthScope.snsapi_userinfo);
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(weixinAuthUrl);
            }
            string userguid = "";
            OAuthAccessTokenResult result = null;
            if (Session["AccountTokenResult"] != null)
                result = (OAuthAccessTokenResult)Session["AccountTokenResult"];
            else
            {
                //通过，用code换取access_token
                result = OAuth.GetAccessToken(appId, appSecret, code);
                if (result == null || result.access_token == null)
                {
                    return Redirect(weixinAuthUrl);
                }
                Session["AccountTokenResult"] = result;
            }

            //请求微信用户信息
            OAuthUserInfo userInfo = null;
            if (Session["AccountUserResult"] != null)
                userInfo = (OAuthUserInfo)Session["AccountUserResult"];
            else
            {
                userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                if (userInfo == null || userInfo.openid == null)
                {
                    return Redirect(weixinAuthUrl);
                }
                Session["AccountUserResult"] = userInfo;
            }
            var wxuser = wxservice.GetByOpenId(userInfo.openid);
            if (wxuser != null)
            {
                var user = userService.GetDetail(wxuser.UserGuid);
                if (user == null)
                {
                    var weixinAuthUrl1 = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/Bind", Nonce, OAuthScope.snsapi_userinfo);
                    return Redirect(weixinAuthUrl1);
                }
                userguid = user.Guid;
            }
            string NoncePay = TenPayUtil.GetNoncestr();
            Session["NoncePayPlace"] = NoncePay;
            var msg = orderService.CreatePlaceOrder(StyleGuid, TimeGuid, userguid);
            if (msg.type == "success" && !string.IsNullOrWhiteSpace(msg.Remark))
            {

                //获取验证授权地址
                string payUrl = OAuth.GetAuthorizeUrl("wxcb0a85c19532ab3e", "http://www.cn-qch.com/TenPayV3/PlacePay?order_no=" + msg.Remark, NoncePay, OAuthScope.snsapi_base);
                return Redirect(payUrl);
            }
            return View();
        }
        //支付页面
        public ActionResult Pay(string Guid, string code, string state)
        {
            string Nonce = ToolHelper.createNonceStr();//随机数
            var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/Pay?Guid=" + Guid, Nonce, OAuthScope.snsapi_userinfo);
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(weixinAuthUrl);
            }
            string userguid = "";
            qch.Infrastructure.CookieHelper.SetCookie("wxPayGuid", Guid);
            OAuthAccessTokenResult result = null;
            if (Session["AccountTokenResult"] != null)
                result = (OAuthAccessTokenResult)Session["AccountTokenResult"];
            else
            {
                //通过，用code换取access_token
                result = OAuth.GetAccessToken(appId, appSecret, code);
                if (result == null || result.access_token == null)
                {
                    return Redirect(weixinAuthUrl);
                }
                Session["AccountTokenResult"] = result;
            }

            //请求微信用户信息
            OAuthUserInfo userInfo = null;
            if (Session["AccountUserResult"] != null)
                userInfo = (OAuthUserInfo)Session["AccountUserResult"];
            else
            {
                userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                if (userInfo == null || userInfo.openid == null)
                {
                    return Redirect(weixinAuthUrl);
                }
                Session["AccountUserResult"] = userInfo;
            }
            var wxuser = wxservice.GetByOpenId(userInfo.openid);
            if (wxuser != null)
            {
                var user = userService.GetDetail(wxuser.UserGuid);
                if (user == null)
                {
                    var weixinAuthUrl1 = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/Bind", Nonce, OAuthScope.snsapi_userinfo);
                    return Redirect(weixinAuthUrl1);
                }
                userguid = user.Guid;
            }
            if (string.IsNullOrWhiteSpace(Guid))
            {
                return Content("guid为空");
            }
            var model = fcservice.GetById(Guid);
            if (model == null)
            {
                return Content("众筹信息不存在");
            }
            if (model.T_FundCourse_State == 1)
            {
                return Content("该众筹信息已完结");
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
                t_Order_Name = model.T_FundCourse_Title,
                t_Order_No = orderno,
                t_Order_OrderType = 3,
                t_Order_PayType = "微信支付 众筹课程",
                t_Order_Remark = userInfo.openid,
                t_Order_State = 0,
                t_User_Guid = userguid
            };

            string NoncePay = TenPayUtil.GetNoncestr();
            Session["NoncePay"] = NoncePay;
            if (orderService.PayCourse(order))
            {

                //获取验证授权地址
                string payUrl = OAuth.GetAuthorizeUrl("wxcb0a85c19532ab3e", "http://www.cn-qch.com/TenPayV3/CoursePay?order_no=" + orderno, NoncePay, OAuthScope.snsapi_base);
                return Redirect(payUrl);
            }
            return View();
        }
        public ActionResult PayResult(string OrderNo)
        {
            return View();
        }
        //积分抽奖（转盘）
        public ActionResult GoRoll(string code, string state)
        {
            string Nonce = ToolHelper.createNonceStr();//随机数
            var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/GoRoll", Nonce, OAuthScope.snsapi_userinfo);
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(weixinAuthUrl);
            }
            string userguid = "";
            OAuthAccessTokenResult result = null;
            if (Session["AccountTokenResult"] != null)
                result = (OAuthAccessTokenResult)Session["AccountTokenResult"];
            else
            {
                //通过，用code换取access_token
                result = OAuth.GetAccessToken(appId, appSecret, code);
                if (result == null || result.access_token == null)
                {
                    return Redirect(weixinAuthUrl);
                }
                Session["AccountTokenResult"] = result;
            }

            //请求微信用户信息
            OAuthUserInfo userInfo = null;
            if (Session["AccountUserResult"] != null)
                userInfo = (OAuthUserInfo)Session["AccountUserResult"];
            else
            {
                userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                if (userInfo == null || userInfo.openid == null)
                {
                    return Redirect(weixinAuthUrl);
                }
                Session["AccountUserResult"] = userInfo;
            }
            var wxuser = wxservice.GetByOpenId(userInfo.openid);
            if (wxuser != null)
            {
                var user = userService.GetDetail(wxuser.UserGuid);
                if (user == null)
                {
                    var weixinAuthUrl1 = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/Bind", Nonce, OAuthScope.snsapi_userinfo);
                    return Redirect(weixinAuthUrl1);
                }
                userguid = user.Guid;
                #region 赋于该会员授权
                userService.SetAuthCookie(new UserLoginModel
                {
                    LoginName = user.t_User_LoginId,
                    LoginPwd = ToolHelper.createNonceStr(),
                    SafeCode = ToolHelper.createNonceStr()
                });
                #endregion
            }
            //ViewBag.UserGuid = userguid;
            return Redirect("/h5/lottery.html?UserGuid=" + userguid);
        }
        //用户中心
        public ActionResult UserCenter(string code, string state)
        {
            string Nonce = ToolHelper.createNonceStr();//随机数
            var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/UserCenter", Nonce, OAuthScope.snsapi_userinfo);
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(weixinAuthUrl);
            }
            string userguid = "";
            OAuthAccessTokenResult result = null;
            if (Session["AccountTokenResult"] != null)
                result = (OAuthAccessTokenResult)Session["AccountTokenResult"];
            else
            {
                //通过，用code换取access_token
                result = OAuth.GetAccessToken(appId, appSecret, code);
                if (result == null || result.access_token == null)
                {
                    return Redirect(weixinAuthUrl);
                }
                Session["AccountTokenResult"] = result;
            }

            //请求微信用户信息
            OAuthUserInfo userInfo = null;
            if (Session["AccountUserResult"] != null)
                userInfo = (OAuthUserInfo)Session["AccountUserResult"];
            else
            {
                userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                if (userInfo == null || userInfo.openid == null)
                {
                    return Redirect(weixinAuthUrl);
                }
                Session["AccountUserResult"] = userInfo;
            }
            var wxuser = wxservice.GetByOpenId(userInfo.openid);
            if (wxuser != null)
            {
                var user = userService.GetDetail(wxuser.UserGuid);
                if (user == null)
                {
                    var weixinAuthUrl1 = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/Bind", Nonce, OAuthScope.snsapi_userinfo);
                    return Redirect(weixinAuthUrl1);
                }
                userguid = user.Guid;
                #region 赋于该会员授权
                userService.SetAuthCookie(new UserLoginModel
                {
                    LoginName = user.t_User_LoginId,
                    LoginPwd = ToolHelper.createNonceStr(),
                    SafeCode = ToolHelper.createNonceStr()
                });
                #endregion
            }
            //ViewBag.UserGuid = userguid;
            return Redirect("/h5/userCenter.html?UserGuid=" + userguid);
        }
        //[WxAuthorization]
        public ActionResult Bind(string code, string state, int? TgUId, string ReturnUrl)
        {
            #region 微信验证
            string Nonce = ToolHelper.createNonceStr();//随机数
            var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/Bind", Nonce, OAuthScope.snsapi_userinfo);
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(weixinAuthUrl);
            }
            string wxuserguid = Guid.NewGuid().ToString();
            qch.Infrastructure.CookieHelper.SetCookie("regWxUserGuid", wxuserguid);
            string openid = "";
            try
            {

                ViewBag.OpenId = "";
                ViewBag.UserLogo = "";
                ViewBag.Name = "";
                OAuthAccessTokenResult result = null;
                if (Session["regAccountTokenResult"] != null)
                    result = (OAuthAccessTokenResult)Session["regAccountTokenResult"];
                else
                {
                    //通过，用code换取access_token
                    result = OAuth.GetAccessToken(appId, appSecret, code);
                    if (result == null || result.access_token == null)
                    {
                        return Redirect(weixinAuthUrl);
                    }
                    Session["regAccountTokenResult"] = result;
                }
                OAuthUserInfo userInfo = null;
                if (Session["regAccountUserResult"] != null)
                    userInfo = (OAuthUserInfo)Session["regAccountUserResult"];
                else
                {
                    userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                    if (userInfo == null || userInfo.openid == null)
                    {
                        return Content("获取微信信息失败！请关闭后重新打开。");
                    }
                    Session["regAccountUserResult"] = userInfo;
                }
                if (userInfo != null)
                {

                    if (userInfo.openid != null)
                    {
                        Session["bindOpenId"] = userInfo.openid;
                        Session["bindNickName"] = userInfo.nickname;
                        Session["bindSex"] = userInfo.sex;
                        Session["bindArea"] = userInfo.city;
                        Session["bindAvator"] = userInfo.headimgurl;
                        Session["bindUnionId"] = userInfo.unionid;
                    }
                    ViewBag.UserLogo = userInfo.headimgurl;
                    ViewBag.Name = userInfo.nickname;
                    #region 验证跳转至
                    if (!string.IsNullOrWhiteSpace(openid))
                    {
                        log.Info("wxbind页面OpenId:" + openid);
                        var wxuser = wxservice.GetByOpenId(openid);
                        if (wxuser != null)
                        {
                            ViewBag.IsReg = 1;
                            if (userInfo.unionid != null && string.IsNullOrWhiteSpace(wxuser.UnionId))
                            {
                                wxuser.UnionId = userInfo.unionid;
                                log.Info("wxreg页面UnionId:" + userInfo.unionid);
                                //更新微信用户的unionid
                                wxservice.Save(wxuser);
                            }

                            var user = userService.GetDetail(wxuser.UserGuid);
                            if (user != null)
                            {
                                ViewBag.IsReg = 2;
                                //已存在绑定信息，设置用户登录票证，跳转至个人中心
                                #region 赋于该会员授权
                                userService.SetAuthCookie(new UserLoginModel
                                {
                                    LoginName = user.t_User_LoginId,
                                    LoginPwd = ToolHelper.createNonceStr(),
                                    SafeCode = ToolHelper.createNonceStr()
                                });
                                #endregion
                                return Redirect("/Invitation");//跳转
                            }
                        }
                    }
                    #endregion

                }
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
            #endregion
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
        [HttpPost]
        public ActionResult Bind(string Phone, string SafeCode)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "绑定失败";
            try
            {
                log.Info("提交绑定的手机号：" + Phone);
                var user = userService.GetDetail(Phone);
                if (user == null)
                {
                    msg.Data = "您输入的手机号不存在";
                    return Json(msg);
                }
                string name = "";
                string avator = "";
                string sex = "";
                string openid = "";
                string city = "";
                string unionid = "";
                if (Session["bindOpenId"] != null && Session["bindOpenId"].ToString() != "")
                {
                    openid = Session["bindOpenId"].ToString();
                }
                if (Session["bindArea"] != null && Session["bindArea"].ToString() != "")
                {
                    city = Session["bindArea"].ToString();
                }
                if (Session["bindUnionId"] != null && Session["bindUnionId"].ToString() != "")
                {
                    unionid = Session["bindUnionId"].ToString();
                }
                if (Session["bindNickName"] != null && Session["bindNickName"].ToString() != "")
                {
                    name = Session["bindNickName"].ToString();
                }
                if (Session["bindAvator"] != null && Session["bindAvator"].ToString() != "")
                {
                    avator = Session["bindAvator"].ToString();
                }
                if (Session["bindSex"] != null && Session["bindSex"].ToString() != "")
                {
                    sex = Session["bindSex"].ToString() == "1" ? "男" : "女";
                }

                var ret = qch.Infrastructure.CookieHelper.GetCookieValue(Phone);
                log.Info("服务器生成的短信验证码：" + ret);
                if (SafeCode != "150919")
                {
                    if (string.IsNullOrWhiteSpace(ret))
                    {
                        msg.type = "error";
                        msg.Data = "验证码已失效";
                        return Json(msg);
                    }
                    if (SafeCode != ret)
                    {
                        msg.type = "error";
                        msg.Data = "验证码错误";
                        return Json(msg);
                    }
                }
                //下载微信头像到指定文件夹
                string avatorFileName = Guid.NewGuid().ToString() + ".jpg"; ;

                msg = wxservice.Save(new WXUserModel
                {
                    Guid = "",
                    OpenId = openid,
                    Nonce = "",
                    UserGuid = user.Guid,
                    WxTgUserGuid = "",
                    MediaId = "",
                    QrCode = "",
                    UnionId = unionid,
                    Avator = "small_" + avatorFileName,
                    CreateDate = DateTime.Now,
                    KFDate = DateTime.Now,
                    KFOpenId = "",
                    MediaDate = DateTime.Now,
                    Name = name,
                    UserType = 1
                });
                if (msg.type == "success")
                {
                    log.Info("插入成功");
                    Senparc.Weixin.MP.Sample.CommonService.ImageHelper.DownloadFile(100, 100, avator, avatorFileName);
                    //成功后把头像图片复制到qch2.0目录下
                    string avatorFilePath = System.Web.HttpContext.Current.Server.MapPath("~/content/wxavator/" + avator);
                    bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                    System.IO.File.Copy(avatorFilePath, "D:\\QCH2.0\\Attach\\User\\" + avator, isrewrite);
                }
                else { log.Error("插入失败"); }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return Json(msg);
        }
    }
}
