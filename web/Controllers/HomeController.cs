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
using System.Drawing.Imaging;
using ZXing;
using ZXing.QrCode.Internal;
using ZXing.Common;


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
        public ActionResult Test1()
        {
            NewsService ns = new NewsService();
            var model = ns.GetAll(1, 300);
            return View(model);
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
        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="text"></param>
        public string Generate3(string text)
        {
            string filename = DateTime.Now.ToString("yyyymmddhhmmssfff").ToString() + ".jpg";
            string filepath = Server.MapPath(@"~\images\qrcode") + "\\" + filename;
            System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            //Logo 图片
            Bitmap logo = new Bitmap(Server.MapPath(@"~\content\image\index\qchlogo.png"));
            //构造二维码写码器
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
            hint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hint.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

            //生成二维码 
            BitMatrix bm = writer.encode("https://www.baidu.com/", BarcodeFormat.QR_CODE, 300, 300, hint);
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            Bitmap map = barcodeWriter.Write(bm);


            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bm.getEnclosingRectangle();

            //计算插入图片的大小和位置
            int middleW = Math.Min((int)(rectangle[2] / 3.5), logo.Width);
            int middleH = Math.Min((int)(rectangle[3] / 3.5), logo.Height);
            int middleL = (map.Width - middleW) / 2;
            int middleT = (map.Height - middleH) / 2;

            //将img转换成bmp格式，否则后面无法创建Graphics对象
            Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmpimg))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(map, 0, 0);
            }
            //将二维码插入图片
            Graphics myGraphic = Graphics.FromImage(bmpimg);
            //白底
            myGraphic.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
            myGraphic.DrawImage(logo, middleL, middleT, middleW, middleH);

            //保存成图片
            bmpimg.Save(fs, ImageFormat.Png);
            fs.Close();
            bmpimg.Dispose();
            return "asdfasdfasdf";
        }
        #endregion
        #region 短信验证码
        [HttpPost]
        public ActionResult SendSMS(string phone)
        {
            SMS sms = new SMS();
            string ip = Request.UserHostAddress;
            var msg = sms.GetSMS(phone, ip);
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
