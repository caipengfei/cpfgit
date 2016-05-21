using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace qch.Infrastructure
{
    /// <summary>
    /// 短信平台服务  无效
    /// </summary>
    public class SMS
    {
        /// <summary>
        /// 向手机发送短信
        /// </summary>
        /// <param name="DstMobile">以逗号分割的手机号码列表</param>
        /// <param name="SmsMsg">短信内容</param>
        /// <returns>成功时返回true</returns>
        public bool SendSms(string DstMobile, string SmsMsg, string ToUrl)
        {
            string mToUrl = "";	//即将引用的url   			
            string mRtv = "";		//引用的返回字符串
            //string name = ConfigurationManager.AppSettings["smsName"];
            //string password = ConfigurationManager.AppSettings["smsPassword"];
            //password = qch.Infrastructure.DESEncrypt.Decrypt(password);
            //编码
            SmsMsg = System.Web.HttpUtility.UrlEncode(SmsMsg, System.Text.Encoding.GetEncoding("gb2312"));

            // 备用IP地址为203.81.21.13

            //mToUrl = string.Format("http://203.81.21.34/send/gsend.asp?name={0}&pwd={1}&dst={2}&msg={3}", name, password, DstMobile, SmsMsg);
            mToUrl = ToUrl;
            try
            {
                System.Net.HttpWebResponse rs = (System.Net.HttpWebResponse)System.Net.HttpWebRequest.Create(mToUrl).GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(rs.GetResponseStream(), System.Text.Encoding.Default);
                mRtv = sr.ReadToEnd();
            }
            catch
            {
                return false;	//对 url http 请求的时候发生的错误  比如页面不存在 或者页面本身执行发生错误
            }

            if (mRtv.Substring(0, 5) != "num=0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
