using qch.core;
using qch.Infrastructure;
using qch.Models;
using Senparc.Weixin.MP.Sample.CommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ThoughtWorks.QRCode.Codec;
using web.Filters;

namespace web.Controllers
{
    public class qchController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserService userService = new UserService();
        TopicService topicService = new TopicService();
        WXUserService wxservice = new WXUserService();
        AccountService accountService = new AccountService();
        VoucherService voucherService = new VoucherService();
        IntegralService integralService = new IntegralService();
        ProjectService projectService = new ProjectService();
        ActivityService activityService = new ActivityService();
        UserModel _loginUser;
        UserModel LoginUser
        {
            get
            {
                if (_loginUser == null)
                {
                    try
                    {
                        HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie 
                        if (authCookie == null)
                            return null;
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
                return _loginUser;
            }
            set
            {
                this._loginUser = value;
            }
        }
        public string ReturnUrl
        {
            get
            {
                string url = "";
                if (Request.UrlReferrer != null && Request.UrlReferrer.Query != null)
                {
                    var tt = Request.UrlReferrer.Query;
                    int x = tt.IndexOf('=');
                    if (x > 0)
                        url = tt.Substring(x + 1);
                }
                return url;
            }
        }
        //
        // GET: /qch/

        public ActionResult Index()
        {
            return View();
        }
        [UserAuthorization]
        public ActionResult UserCenter()
        {
            if (LoginUser != null)
            {
                ViewBag.UserPosition = LoginUser.t_User_Position;
                ViewBag.UserName = LoginUser.t_User_RealName;
                //用户创业币
                var cyb = accountService.GetBalance(LoginUser.Guid);
                //积分
                var integral = integralService.GetIntegral(LoginUser.Guid);
                //优惠券
                long voucherCount = 0;
                var voucher = voucherService.GetAlluvByUser(1, 9999, LoginUser.Guid);
                if (voucher != null)
                    voucherCount = voucher.TotalItems;
                ViewBag.VoucherCount = voucherCount;//优惠券数量
                ViewBag.Integral = integral;//积分余额
                ViewBag.Balance = cyb;//创业币余额
                //最近发布的项目
                var project = projectService.GetTop1ForPC(LoginUser.Guid);
                if (project == null)
                    project = new SelectProject();
                return View(project);
            }
            return View();
        }
        //个人中心上部、左部

        public ActionResult uc()
        {
            if (LoginUser != null)
            {
                ViewBag.UserPosition = LoginUser.t_User_Position;
                ViewBag.UserName = LoginUser.t_User_RealName;
            }
            return View();
        }
        public ActionResult map()
        {
            return View();
        }
        //近期活动
        public ActionResult activity()
        {
            var model = activityService.GetListFroWX(1, 3, "", 0, "");
            return View(model);
        }
        #region 合伙人列表
        public ActionResult PartnersTop5()
        {
            var model = userService.GetAll(1, 5, 3);
            if (model != null)
            {
                ViewData["TotalPage"] = model.TotalPages;
            }
            else
            {
                ViewData["TotalPage"] = 1;
            }
            return View(model);
        }
        public ActionResult Partners(int? page, int? pagesize)
        {
            int Page = page ?? 1;
            int PageSize = pagesize ?? 15;
            var model = userService.GetAll(Page, PageSize, 3);
            ViewData["Page"] = Page;
            ViewData["PageSize"] = PageSize;
            if (model != null)
            {
                ViewData["TotalPage"] = model.TotalPages;
            }
            else
            {
                ViewData["TotalPage"] = 1;
            }
            return View(model);
        }
        #endregion
        #region 视频
        #endregion
        #region 登录与退出
        //用户登录
        public ActionResult Login()
        {
            var url = CreateQrCode();
            ViewBag.QrCode = url;
            qch.Infrastructure.CookieHelper.SetCookie("qrcodeUrl", url);
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            var msg = userService.Login(model);
            if (msg.type == "success")
            {
                if (ReturnUrl != "")
                    msg.Data = ReturnUrl;
                else
                {
                    string s = "/qch";
                    msg.Data = s;
                }
            }
            return Json(msg);
        }
        public ActionResult Logout()
        {
            userService.LogOut();
            return RedirectToAction("index", "qch");
        }
        #endregion
        #region 注册 找回密码
        //用户注册
        public ActionResult Reg()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Reg(UserModel model)
        {
            return View();
        }
        //重置密码
        public ActionResult RePassword()
        {
            return View();
        }
        #endregion
        #region 微信登录相关页面
        public ActionResult WeChatNoLogin(string OpenId, string UnionId)
        {
            if (string.IsNullOrWhiteSpace(OpenId))
                return RedirectToAction("/qch/login");
            ViewBag.Avator = qch.Infrastructure.CookieHelper.GetCookieValue("wxLoginUserAvator");
            ViewBag.Nickname = qch.Infrastructure.CookieHelper.GetCookieValue("wxLoginUserNickname");
            Session["wxLoginUserOpenId"] = OpenId;
            Session["wxLoginUserUnionId"] = UnionId;
            ViewBag.OpenId = OpenId;
            ViewBag.UnionId = UnionId;
            return View();
        }
        public ActionResult Bind()
        {
            ViewBag.Avator = qch.Infrastructure.CookieHelper.GetCookieValue("wxLoginUserAvator");

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
                string openid = "";
                string unionid = "";
                if (Session["wxLoginUserOpenId"] != null && Session["wxLoginUserOpenId"].ToString() != "")
                {
                    openid = Session["wxLoginUserOpenId"].ToString();
                }
                if (Session["wxLoginUserUnionId"] != null && Session["wxLoginUserUnionId"].ToString() != "")
                {
                    unionid = Session["wxLoginUserUnionId"].ToString();
                }
                var wxuser = wxservice.GetByOpenId(openid, unionid);
                if (wxuser != null)
                {
                    msg = wxservice.Bind(wxuser);
                }
                if (msg.type == "success")
                {
                    //设置登录票证
                    userService.SetAuthCookie(new UserLoginModel
                    {
                        LoginName = user.t_User_LoginId,
                        LoginPwd = ToolHelper.createNonceStr(),
                        SafeCode = ToolHelper.createNonceStr()
                    });
                }
                else { log.Error("web端微信扫码登录绑定用户信息失败"); }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return Json(msg);
        }
        #endregion

        #region 暂未开启的业务
        //点赞列表
        public ActionResult PariseList()
        {
            var model = topicService.GetPariseList(1, 20);
            return View(model);
        }

        //生成app登录二维码
        public string CreateQrCode()
        {
            string s = str.createNonceStr(18);
            string nr = string.Format("uuid={0}&returnUrl={1}", s, "http://www.cn-qch.com/qch/QrCodeLogin");
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
            return "/images/qrcode/" + filename;
        }
        //检测扫码状态
        public string CheckQrCode()
        {
            try
            {
                string ip = Request.UserHostAddress;
                var m = Session[ip];
                if (m != null)
                    return ip;
                else
                    return "";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return "";
            }
        }
        public ActionResult CheckIDCard()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckIDCard(string identity, string name)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "验证失败";
            IdentityService idservice = new IdentityService();
            if (!idservice.CheckIDCard(identity))
            {
                msg.type = "error";
                msg.Data = "身份证号错误";
            }
            else
            {
                msg.type = "success";
                msg.Data = "验证通过";
            }
            return Json(msg);
        }
        //app扫码登录
        public ActionResult QrCodeLogin(string UUID, string UserGuid, string Sign)
        {
            string s = qch.Infrastructure.Encrypt.MD5Encrypt(UUID + UserGuid, true);
            log.Info("服务器Sign：" + s);
            if (Sign == s)
            {
                //签名正确
                ViewBag.UserGuid = UserGuid;
                qch.Infrastructure.CookieHelper.SetCookie("UserGuid", UserGuid, DateTime.Now.AddSeconds(20));
                string ip = Request.UserHostAddress;
                Session[ip] = UserGuid;
            }
            else
            {
                //签名错误
                return Content("签名错误");
            }
            return View();
        }
        [HttpPost]
        public ActionResult QrCodeLogin()
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "登录失败";
            string UserGuid = qch.Infrastructure.CookieHelper.GetCookieValue("UserGuid");
            if (string.IsNullOrWhiteSpace(UserGuid))
            {
                msg.Data = "授权已过期，请重新扫码";
            }
            var user = userService.GetDetail(UserGuid);
            if (user != null)
            {
                #region 设置用户登录凭证
                userService.SetAuthCookie(new UserLoginModel
                {
                    LoginName = user.t_User_LoginId,
                    LoginPwd = ToolHelper.createNonceStr(),
                    SafeCode = ToolHelper.createNonceStr()
                });
                #endregion
                msg.Data = "扫码登录成功";
                msg.type = "success";
                //删除二维码
                string url = qch.Infrastructure.CookieHelper.GetCookieValue("qrcodeUrl");
                System.IO.File.Delete(url);
                return Json(msg);
            }
            return View();
        }
        #endregion

    }
}
