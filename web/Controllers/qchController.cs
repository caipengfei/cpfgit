using qch.core;
using qch.Models;
using Senparc.Weixin.MP.Sample.CommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace web.Controllers
{
    public class qchController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserService userService = new UserService();
        //
        // GET: /qch/

        public ActionResult Index()
        {
            return View();
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
        //用户注册
        public ActionResult Reg()
        {
            return View();
        }
        //重置密码
        public ActionResult RePassword()
        {
            return View();
        }
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
            return Json(msg);
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

    }
}
