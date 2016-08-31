using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Senparc.Weixin.MP.CommonAPIs;
using qch.Models;
using qch.Repositories;

namespace qch.core
{
    public class JsapiService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static object obj = new object();
        JsapiRepository jsapiRp = new JsapiRepository();
        private string GetSignature(string jsapi_ticket, string noncestr, string timestamp, string url)
        {

            string tmpStr = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url;

            return FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1").ToLower();

        }
        //创建随机字符串  
        private string createNonceStr()
        {
            int length = 16;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string str = "";
            Random rad = new Random();
            for (int i = 0; i < length; i++)
            {
                str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
            }
            return str;
        }

        //发起一个http请球，返回值  
        public string httpGet(string url)
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据  
                Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据  
                string pageHtml = System.Text.Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句              

                return pageHtml;
            }
            catch (WebException webEx)
            {
                log.Error(webEx.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// 获取JsSDK签名对象
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public JsapiModel GetSign(string ShareUrl, string PageName, string appId)
        {
            lock (obj)
            {
                try
                {
                    log.Info("GetSign方法-----------------------------");
                    string timestamp = qch.Infrastructure.TimeHelper.ConvertDateTime(DateTime.Now);
                    string noncestr = createNonceStr();
                    string access_token = "";
                    string url = "";
                    string jsapi_ticket = "";
                    log.Info("timestamp=" + timestamp);
                    log.Info("noncestr=" + noncestr);
                    access_token = AccessTokenContainer.GetToken(appId);

                    url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=" + access_token + "";
                    jsapi_ticket = httpGet(url);

                    SignModel sign = new SignModel();
                    sign = (SignModel)JsonConvert.DeserializeObject(jsapi_ticket, typeof(SignModel));
                    if (sign.Errcode != 0)
                    {
                        log.Info("JsapiService重新获取了新的token");
                        access_token = AccessTokenContainer.GetToken(appId, true);
                        url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=" + access_token + "";
                        jsapi_ticket = httpGet(url);
                    }
                    log.Info("access_token=" + access_token);
                    log.Info("jsapi_ticket=" + jsapi_ticket);
                    log.Info("sign=" + sign.Ticket);

                    JsapiModel jsmodel = new JsapiModel();
                    jsmodel.Noncestr = noncestr;
                    jsmodel.PageName = PageName;
                    jsmodel.Timestamp = timestamp;
                    jsmodel.AppId = appId;
                    jsmodel.access_token = access_token;
                    jsmodel.Jsapi_ticket = sign.Ticket;
                    jsmodel.Signature = GetSignature(sign.Ticket, noncestr, timestamp, ShareUrl);
                    jsmodel.CreateDate = DateTime.Now;
                    //jsapiRp.Add(jsmodel);
                    return jsmodel;
                    #region 已取消的逻辑 （错误的）
                    //var model = jsapiRp.Get(PageName);
                    //if (model == null)
                    //{


                    //}
                    //else
                    //{
                    //    //获取写入缓存的时间
                    //    DateTime addTime = qch.Infrastructure.TimeHelper.GetTime(model.Timestamp);
                    //    //如果当前时间在 写入缓存时间+110分钟之内
                    //    if (addTime.AddMinutes(110) > DateTime.Now)
                    //    {
                    //        log.Info("未到期");
                    //        return model;
                    //    }
                    //    else
                    //    {
                    //        log.Info("到期，重新生成");
                    //        //if (!AccessTokenContainer.CheckRegistered(appId))
                    //        //{
                    //        //    AccessTokenContainer.Register(appId, appSecret);
                    //        //}
                    //        access_token = AccessTokenContainer.GetToken(appId);
                    //        url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=" + access_token + "";
                    //        jsapi_ticket = httpGet(url);
                    //        SignModel sign = new SignModel();
                    //        sign = (SignModel)JsonConvert.DeserializeObject(jsapi_ticket, typeof(SignModel));
                    //        if (sign.Errcode == 40001)
                    //        {
                    //            access_token = AccessTokenContainer.GetToken(appId, true);
                    //            url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=" + access_token + "";
                    //            jsapi_ticket = httpGet(url);
                    //        }
                    //        log.Info("到期重新生成的sign=" + sign);
                    //        jsapiRp.Del(model);
                    //        JsapiModel jsmodel = new JsapiModel();
                    //        jsmodel.Noncestr = noncestr;
                    //        jsmodel.PageName = PageName;
                    //        jsmodel.Timestamp = timestamp;
                    //        jsmodel.AppId = appId;
                    //        jsmodel.access_token = access_token;
                    //        jsmodel.Jsapi_ticket = sign.Ticket;
                    //        jsmodel.Signature = GetSignature(sign.Ticket, noncestr, timestamp, ShareUrl);
                    //        jsmodel.CreateDate = DateTime.Now;
                    //        jsapiRp.Add(jsmodel);
                    //        return jsmodel;
                    //    }
                    //}
                    #endregion
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return null;
                }
            }
        }
    }
}