using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Senparc.Weixin.MP.Sample.CommonService
{

    public class ImageHelper
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        xftwl.Common.Card.CardWeixinService cardweixinService = new xftwl.Common.Card.CardWeixinService();

        /// <summary>
        /// 图片保存路径
        /// </summary>        
        public static string ImageUrl { get { return string.Format("/images/{0}/{1}/{2}/", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); } }

        /// <summary>
        /// 生成微信用户专用推广二维码
        /// </summary>
        /// <param name="wxavator">微信用户头像地址</param>
        /// <param name="nickname">微信用户昵称</param>
        /// <param name="UserId">绑定的用户ID</param>
        public string CreateUserQrCode(string wxavator, string nickname, int UserId)
        {
            /*
               1下载微信图片到公众平台服务器
               2.用指定背景生成带水印的图片
               3.上传图片
            */
            try
            {
                string appId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
                string apptoken = CommonAPIs.AccessTokenContainer.GetToken(appId);

                //下载微信头像到指定文件夹
                string avatorFileName = DownloadFile(100, 100, wxavator);
                string avatorFilePath = System.Web.HttpContext.Current.Server.MapPath("~/content" + ImageUrl + avatorFileName);
                //创建临时二维码 mediaid有效期3天

                var qrcode = QrCode.Create(apptoken, 604800, UserId);
                if (qrcode != null)
                {
                    //2生成带水印的图片
                    //2.1下载二维码
                    var qrcodeUrl = QrCode.GetShowQrCodeUrl(qrcode.ticket);
                    string qrcodeFileName = DownloadFile(380, 380, qrcodeUrl);
                    string qrcodeFilePath = System.Web.HttpContext.Current.Server.MapPath("~/content" + ImageUrl + qrcodeFileName);

                    //2.2复制背景图到指定文件夹                    
                    if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/content" + ImageUrl)))
                    {
                        System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/content" + ImageUrl));
                    }
                    String bgImgFilePath = System.Web.HttpContext.Current.Server.MapPath("~/content/images/bg.jpg");
                    string UserFileName = DateTime.Now.ToString("yyMMddhhmmss") + "_" + UserId + ".jpg";
                    String targetFilePath = System.Web.HttpContext.Current.Server.MapPath("~/content" + ImageUrl + UserFileName);
                    bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                    System.IO.File.Copy(bgImgFilePath, targetFilePath, isrewrite);

                    //2.3生成带水印的图片
                    //贴微信昵称
                    LetterWatermark(targetFilePath, 18, "我是:" + nickname, System.Drawing.Color.Black, "WxUserName");
                    //贴二维码过期时间
                    LetterWatermark(targetFilePath, 14, "该二维码将在" + DateTime.Now.AddSeconds(2591000).ToString("yyyy-MM-dd hh:mm:ss") + "后过期", System.Drawing.Color.DarkGray, "WxExpiryDate");
                    //贴二维码
                    ImageWatermark(targetFilePath, qrcodeFilePath, "WxQrCode");
                    //贴微信头像
                    ImageWatermark(targetFilePath, avatorFilePath, "WxUserLogo");

                    //3上传生成的水印图到微信服务器
                    var media = Senparc.Weixin.MP.AdvancedAPIs.Media.Upload(apptoken, UploadMediaFileType.image, targetFilePath);
                    if (media != null && !string.IsNullOrWhiteSpace(media.media_id))
                    {
                        //保存用户二维码信息到数据库
                        var bindInfo = cardweixinService.Get(UserId);
                        if (bindInfo != null)
                        {
                            bindInfo.MediaDate = DateTime.Now;
                            bindInfo.MediaId = media.media_id;
                            bindInfo.QrCode = targetFilePath;
                            if (!cardweixinService.Save(bindInfo))
                            {
                            }
                        }
                        else
                        {
                            log.Error("上传生成的水印图到微信服务器成功，但是获取用户绑定信息失败");
                            return "";
                        }
                        //删除下载的头像和二维码
                        System.IO.File.Delete(qrcodeFilePath);
                        System.IO.File.Delete(avatorFilePath);
                        return media.media_id;
                    }
                    else
                    {
                        log.Error("上传生成的水印图到微信服务器失败或没有接受到media_id");
                        return "";
                    }
                }
                else
                {
                    log.Error("创建临时二维码失败，UserId=" + UserId);
                }
                return "";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return "";
            }
        }


        /// <summary>  
        /// 下载图片
        /// 返回图片名称
        /// </summary>  
        /// <param name="fileNameFullPath">要下载的图片全路径</param>  
        /// <returns></returns>  
        public static string DownloadFile(double width, double height, string fileNameFullPath)
        {
            //得到要下载的文件文件名  
            string fileName = fileNameFullPath.Substring(fileNameFullPath.LastIndexOf("\\") + 1);
            //新文件名由年月日时分秒及毫秒组成  
            //string NewFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond.ToString()
            //    + fileNameFullPath.Substring(fileNameFullPath.LastIndexOf("."));
            string NewFileName = Guid.NewGuid().ToString() + ".jpg";
            // 创建WebClient实例  
            WebClient myWebClient = new WebClient();
            //string path = System.Web.HttpContext.Current.Server.MapPath("~/content" + ImageUrl);
            string path = System.Web.HttpContext.Current.Server.MapPath("~/content" + ImageUrl);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            myWebClient.DownloadFile(fileNameFullPath, path + NewFileName);
            string img = ImageUrl + NewFileName;
            string img2 = ImageUrl + "small_" + NewFileName;
            //生成原图的缩略图
            souImg(width, height, System.Web.HttpContext.Current.Server.MapPath("~/content" + img), System.Web.HttpContext.Current.Server.MapPath("~/content" + img2));
            //删除原图
            System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath("~/content" + img));
            return "small_" + NewFileName;
        }
        /// <summary>
        /// 缩略图
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns>bool</returns>
        public static bool souImg(double width1, double height1, string path, string newpath)
        {
            bool suc = false;
            double smallwidth = width1; //范围宽
            double smallheight = height1; //范围高
            double width; //原图宽
            double height; //原图高
            double newwidth; //缩略图宽
            double newheight; //缩略图高
            double n_scale; //压缩比例
            //string newpath; //新文件名;
            //newpath = path.Replace(".jpg", "x.jpg");
            //从 原图片 创建 Image 对象
            Image image = Image.FromFile(path);
            //获取缩略比例
            width = image.Width;
            height = image.Height;
            if (width > height)
            {
                if (width > smallwidth)
                {
                    n_scale = width / smallwidth;
                    newwidth = smallwidth;
                }
                else
                {
                    n_scale = 1;

                    newwidth = width;
                }
                newheight = height / n_scale;

                if (newheight > smallheight)
                {
                    n_scale = newheight / smallheight;
                    newheight = smallheight;
                    newwidth = smallwidth / n_scale;
                }
            }
            else
            {
                if (height > smallheight)
                {
                    n_scale = height / smallheight;
                    newheight = smallheight;
                }
                else
                {
                    n_scale = 1;
                    newheight = height;
                }
                newwidth = width / n_scale;

                if (newwidth > smallwidth)
                {
                    n_scale = newwidth / smallwidth;
                    newwidth = smallwidth;
                    newheight = smallheight / n_scale;
                }
            }

            //用指定的大小和格式初始化 Bitmap 类的新实例
            Bitmap bitmap = new Bitmap(Convert.ToInt32(newwidth), Convert.ToInt32(newheight), PixelFormat.Format32bppArgb);
            //从指定的 Image 对象创建新 Graphics 对象
            Graphics graphics = Graphics.FromImage(bitmap);
            //清除整个绘图面并以透明背景色填充
            graphics.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制 原图片 对象
            graphics.DrawImage(image, new Rectangle(0, 0, Convert.ToInt32(newwidth), Convert.ToInt32(newheight)));
            image.Dispose();
            graphics.Dispose();
            try
            {
                //将此 原图片 以指定格式并用指定的编解码参数保存到指定文件
                bitmap.Save(newpath, ImageFormat.Jpeg);
                suc = true;
            }
            catch
            {
                suc = false;
            }

            bitmap.Dispose();

            return suc;
        }


        #region 文字水印
        /// <summary>
        /// 文字水印处理方法
        /// </summary>
        /// <param name="path">图片路径（绝对路径）</param>
        /// <param name="size">字体大小</param>
        /// <param name="letter">水印文字</param>
        /// <param name="color">颜色</param>
        /// <param name="location">水印位置</param>
        public static string LetterWatermark(string path, int size, string letter, Color color, string location)
        {
            #region

            string kz_name = Path.GetExtension(path);
            if (kz_name == ".jpg" || kz_name == ".bmp" || kz_name == ".jpeg")
            {
                DateTime time = DateTime.Now;
                string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
                Image img = Bitmap.FromFile(path);
                Graphics gs = Graphics.FromImage(img);
                ArrayList loca = GetLocation(location, img, size, letter.Length);
                Font font = new Font("宋体", size);
                Brush br = new SolidBrush(color);
                gs.DrawString(letter, font, br, float.Parse(loca[0].ToString()), float.Parse(loca[1].ToString()));
                gs.Dispose();
                string newpath = Path.GetDirectoryName(path) + filename + kz_name;
                img.Save(newpath);
                img.Dispose();
                File.Copy(newpath, path, true);
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
            }
            return path;

            #endregion
        }

        /// <summary>
        /// 文字水印位置的方法
        /// </summary>
        /// <param name="location">位置代码</param>
        /// <param name="img">图片对象</param>
        /// <param name="width">宽(当水印类型为文字时,传过来的就是字体的大小)</param>
        /// <param name="height">高(当水印类型为文字时,传过来的就是字符的长度)</param>
        private static ArrayList GetLocation(string location, Image img, int width, int height)
        {
            #region

            ArrayList loca = new ArrayList();  //定义数组存储位置
            float x = 10;
            float y = 10;

            if (location == "LT")
            {
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "WxUserName")
            {//微信称昵位置

                loca.Add(250);
                loca.Add(173);
            }
            else if (location == "WxExpiryDate")
            {//微信二维码过期位置
                loca.Add(150);
                loca.Add(692);
            }
            else if (location == "T")
            {
                x = img.Width / 2 - (width * height) / 2;
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "RT")
            {
                x = img.Width - width * height;
            }
            else if (location == "LC")
            {
                y = img.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - height;
                y = img.Height / 2;
            }
            else if (location == "LB")
            {
                y = img.Height - width - 5;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height - width - 5;
            }
            else
            {
                x = img.Width - width * height;
                y = img.Height - width - 5;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;

            #endregion
        }
        #endregion


        #region 图片水印
        /// <summary>
        /// 图片水印处理方法
        /// </summary>
        /// <param name="path">需要加载水印的图片路径（绝对路径）</param>
        /// <param name="waterpath">水印图片（绝对路径）</param>
        /// <param name="location">水印位置（传送正确的代码）</param>
        public static string ImageWatermark(string path, string waterpath, string location)
        {
            string kz_name = Path.GetExtension(path);
            if (kz_name == ".jpg" || kz_name == ".bmp" || kz_name == ".jpeg")
            {
                DateTime time = DateTime.Now;
                string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
                Image img = Bitmap.FromFile(path);
                Image waterimg = Image.FromFile(waterpath);
                Graphics g = Graphics.FromImage(img);
                ArrayList loca = GetLocation(location, img, waterimg);
                g.DrawImage(waterimg, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterimg.Width, waterimg.Height));
                waterimg.Dispose();
                g.Dispose();
                string newpath = Path.GetDirectoryName(path) + filename + kz_name;
                img.Save(newpath);
                img.Dispose();
                File.Copy(newpath, path, true);
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
            }
            return path;
        }

        /// <summary>
        /// 图片水印位置处理方法
        /// </summary>
        /// <param name="location">水印位置</param>
        /// <param name="img">需要添加水印的图片</param>
        /// <param name="waterimg">水印图片</param>
        private static ArrayList GetLocation(string location, Image img, Image waterimg)
        {
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;

            if (location == "LT")
            {
                x = 10;
                y = 10;
            }
            else if (location == "WxUserLogo")
            {//微信用户头像
                //x = img.Width / 2 - waterimg.Width / 2;
                //y = img.Height - waterimg.Height;
                x = 277;
                y = 54;
            }
            else if (location == "WxQrCode")
            {//微信二维码
                x = 136;
                y = 260;
            }
            else if (location == "T")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else if (location == "RT")
            {
                x = img.Width - waterimg.Width;
                y = 10;
            }
            else if (location == "LC")
            {
                x = 10;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - waterimg.Width;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "LB")
            {
                x = 10;
                y = img.Height - waterimg.Height;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else
            {
                x = img.Width - waterimg.Width;
                y = img.Height - waterimg.Height;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;
        }
        #endregion

    }
}