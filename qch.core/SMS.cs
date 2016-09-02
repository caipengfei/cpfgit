using Maticsoft.Common.CCPRestSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 短信业务
    /// </summary>
    public class SMS
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region 短信验证码
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="userMobile"></param>
        /// <returns></returns>
        public Msg GetSMS(string userMobile, string IpAddr)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "短信发送失败";
            log.Info("userMobile=" + userMobile);
            log.Info("IpAddr=" + IpAddr);
            if (string.IsNullOrWhiteSpace(userMobile))
            {
                msg.Data = "手机号码有误";
                return msg;
            }
            //System.Web.HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[userMobile];
            //if (cookie != null)
            //{
            //    log.Info("cookie:" + cookie.Expires);
            //}
            string ret = null;
            CCPRestSDK api = new CCPRestSDK();
            //ip格式如下，不带https://
            bool isInit = api.init("sandboxapp.cloopen.com", "8883");
            //主账号，主账号令牌
            api.setAccount("8a48b5514f73ea32014f8bdff18e2f96", "0019e7e2b07a419fbdef0acc43b24d39");
            //应用ID
            api.setAppId("aaf98f894f73de3f014f8be1e7bf106e");
            try
            {
                if (isInit)
                {
                    //取6位随机码
                    string str = Ran();
                    //获取内容,str:验证码，3：参数三分钟
                    string[] arrStr = { str, "3" };
                    //短信接收号码, 短信模板id, 内容数据
                    Dictionary<string, object> retData = api.SendTemplateSMS(userMobile, "36063", arrStr);
                    ret = new Maticsoft.Common.SMS().getDictionaryData(retData);
                    if (!string.IsNullOrWhiteSpace(ret))
                    {
                        msg.type = "success";
                        msg.Data = "发送成功";
                        qch.Infrastructure.CookieHelper.SetCookie(userMobile, str, DateTime.Now.AddMinutes(3));
                    }
                    return msg;
                }
                else
                {
                    return msg;
                }
            }
            catch (Exception exc)
            {
                log.Error(exc.Message);
                return msg;
            }
        }
        #endregion
        /// <summary>
        /// 产生随机码
        /// </summary>
        private string Ran()
        {
            ArrayList MyArray = new ArrayList();
            Random random = new Random();
            string str = null;
            //循环的次数     
            int Nums = 4;
            while (Nums > 0)
            {
                int i = random.Next(1, 9);
                if (MyArray.Count < 4)
                {
                    MyArray.Add(i);
                }
                Nums -= 1;
            }
            for (int j = 0; j <= MyArray.Count - 1; j++)
            {
                str += MyArray[j].ToString();
            }
            return str;
        }
    }
}
