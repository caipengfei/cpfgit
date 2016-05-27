using qch.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using System.Net.Http;
using web.Models;
using Newtonsoft.Json;


namespace web.Controllers
{
    public class HomeController : Controller
    {
        //首页
        public ActionResult Index()
        {
            return View();
        }
        //头部
        public ActionResult Top()
        {
            return View();
        }
        //底部
        public ActionResult Footer()
        {
            return View();
        }
        //下载app
        public ActionResult DownLoad()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult TestApi()
        {
            string miyao = "life4u4me";

            string username = "CN12189";
            miyao = qch.Infrastructure.Encrypt.MD5Encrypt(username + miyao, true);
            //string url1 = string.Format("http://www.memvip.net/api/userApi?cmd=modifyUserInfo&PassportName={0}&PowerKey={1}&SpecialEMoney=22", username, miyao);
            //var user1 = EditUsers(url1);
            string url = string.Format("http://www.memvip.net/api/userApi?cmd=getUserInfo&PassportName={0}&PowerKey={1}", username, miyao);
            var user = GetUsers(url);
            return View();
        }


        public static object GetUsers(string Url)
        {
            GetUserModel user = new GetUserModel();
            //var resourceServerUri = new Uri(Url);
            HttpClient client = new HttpClient();
            var response = client.GetStringAsync(Url).Result;
            //HttpResponseMessage response = client.GetAsync(Url).Result;
            //response.EnsureSuccessStatusCode();
            //string responseBody = response.Content.ReadAsStringAsync().Result;
            if (response != null)
            {
                
                user = (GetUserModel)JsonConvert.DeserializeObject(response, typeof(GetUserModel));
                //var u = response.Content.ReadAsAsync<GetUserModel>().Result;
            }
            return user;
        }
        public static object EditUsers(string Url)
        {
            GetUserModel user = new GetUserModel();
            //var resourceServerUri = new Uri(Url);
            HttpClient client = new HttpClient();
            var response = client.GetStringAsync(Url).Result;
            //HttpResponseMessage response = client.GetAsync(Url).Result;
            //response.EnsureSuccessStatusCode();
            //string responseBody = response.Content.ReadAsStringAsync().Result;
            if (response != null)
            {

                user = (GetUserModel)JsonConvert.DeserializeObject(response, typeof(GetUserModel));
                //var u = response.Content.ReadAsAsync<GetUserModel>().Result;
            }
            return user;
        }

        #region 生成二维码
        //生成二维码方法一
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
            string filepath = Server.MapPath(@"~\images\qrcode") + "\\" + filename;
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);

            fs.Close();
            image.Dispose();
            //二维码解码
            //var codeDecoder = CodeDecoder(filepath);
            return filepath;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="strData">要生成的文字或者数字，支持中文。如： "4408810820 深圳－广州" 或者：4444444444</param>
        /// <param name="qrEncoding">三种尺寸：BYTE ，ALPHA_NUMERIC，NUMERIC</param>
        /// <param name="level">大小：L M Q H</param>
        /// <param name="version">版本：如 8</param>
        /// <param name="scale">比例：如 4</param>
        /// <returns></returns>
        public ActionResult CreateCode_Choose(string strData, string qrEncoding, string level, int version, int scale)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            string encoding = qrEncoding;
            switch (encoding)
            {
                case "Byte":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case "AlphaNumeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case "Numeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            qrCodeEncoder.QRCodeScale = scale;
            qrCodeEncoder.QRCodeVersion = version;
            switch (level)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }
            //文字生成图片
            Image image = qrCodeEncoder.Encode(strData);
            string filename = DateTime.Now.ToString("yyyymmddhhmmssfff").ToString() + ".jpg";
            string filepath = Server.MapPath(@"~\images\qrcode") + "\\" + filename;
            //如果文件夹不存在，则创建
            //if (!Directory.Exists(filepath))
            //    Directory.CreateDirectory(filepath);
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();
            image.Dispose();
            return Content(@"/images/qrcode/" + filename);
        }

        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <returns></returns>
        public string CodeDecoder(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            Bitmap myBitmap = new Bitmap(Image.FromFile(filePath));
            QRCodeDecoder decoder = new QRCodeDecoder();
            string decodedString = decoder.decode(new QRCodeBitmapImage(myBitmap));
            return decodedString;
        }
        #endregion
        #region 短信验证码
        [HttpPost]
        public ActionResult SendSMS(string phone)
        {
            SMS sms = new SMS();
            var msg = sms.GetSMS(phone);
            return Json(msg);
        }
        [HttpPost]
        public ActionResult CheckSMS(string phone, string code)
        {
            SMS sms = new SMS();
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "验证码错误";
            var ret = qch.Infrastructure.CookieHelper.GetCookieValue(phone);
            if (code == ret)
            {
                msg.type = "success";
                msg.Data = "验证成功";
                qch.Infrastructure.CookieHelper.ClearCookie(phone);
            }
            return Json(msg);
        }
        #endregion
        #region 验证码
        //验证码
        public ActionResult GetValidateCode()
        {
            qch.Infrastructure.MvcCaptcha vCode = new qch.Infrastructure.MvcCaptcha();
            string code = vCode.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
        #endregion
    }
}
