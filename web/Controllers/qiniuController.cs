using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class qiniuController : Controller
    {
        //
        // GET: /qiniu/
        string Access_Key = "X2s2-Q7qFqhEr8mpCz4k_ZBlh7SX0Jp3jgubdd_V";
        string Secret_Key = "-pCRmWeh5oAYi9dqfSwWJK0uxVQaIm3CyvzdAPvi";
        public const string UTF8 = "UTF-8";
        public ActionResult Index()
        {
            string RTMPPublishDomain = "";  //直播空间绑定的 RTMP 推流域名
            string Hub = "";                //直播空间名
            string StreamKey = "";          //流名
            string ExpireAt = "";           //Unix 时间戳，表示推流地址的过期时间。
            string Token = "";              //推流凭证,把sk先哈希然后转成url编码的base64，ak:sk

            RTMPPublishDomain = "pili-publish.www.cn-qch.com/";
            Hub = "qch-lvie/";
            StreamKey = "qch-test002";
            ExpireAt = qch.Infrastructure.TimeHelper.ConvertDateTime(DateTime.Now);
            string path = "rtmp://" + RTMPPublishDomain + Hub + StreamKey + "?e=" + ExpireAt;
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes("UTF-8");
            byte[] dataBuffer = Encoding.UTF8.GetBytes(path + Secret_Key);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            Token = Convert.ToBase64String(hashBytes);
            Token = Access_Key + ":" + Token;
            string url = "rtmp://" + RTMPPublishDomain + Hub + StreamKey + "?e=" + ExpireAt + "&token=" + Token;
            //HMACSHA1 mSkSpec = new HMACSHA1(UTF8);

            return Content(url);
            return View();
        }
        public ActionResult Text()
        {
            return View();
        }

    }
}
