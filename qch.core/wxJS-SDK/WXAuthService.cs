using Newtonsoft.Json;
using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 微信授权业务层
    /// </summary>
    public class WXAuthService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        WXAuthRepository rp = new WXAuthRepository();


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
        /// 网页授权获取token
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public KFSignModel GetToken(string AppID, string AppSecret, string code)
        {
            try
            {
                KFSignModel model = new KFSignModel();
                string token = qch.Infrastructure.CookieHelper.GetCookieValue("AuthToken");
                string openid = qch.Infrastructure.CookieHelper.GetCookieValue("AuthOpenId");
                string refresh = qch.Infrastructure.CookieHelper.GetCookieValue("AuthRefresh");
                if (string.IsNullOrWhiteSpace(token))
                {
                    if (string.IsNullOrWhiteSpace(refresh))
                    {
                        //重新请求令牌
                        log.Info("网页授权请求了新的令牌");
                        var auth = GetToken(AppID, AppSecret, code, 2);
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
                        log.Info("网页授权刷新了令牌");
                        var auth = RefreshToken(AppID, refresh);
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
                            log.Info("网页授权刷新了令牌，但是刷新失败，又请求了新的令牌");
                            var auth1 = GetToken(AppID, AppSecret, code, 2);
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
                    log.Info("网页授权使用了cookie中的令牌");
                }
                model.access_token = token;
                model.openid = openid;
                model.refresh_token = refresh;
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 请求令牌
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <param name="Code"></param>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public KFSignModel GetToken(string AppID, string AppSecret, string Code, int TypeId)
        {
            try
            {
                string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", AppID, AppSecret, Code);
                string json = httpGet(url);
                log.Info("json=" + json);
                KFSignModel sm = new KFSignModel();
                sm = (KFSignModel)JsonConvert.DeserializeObject(json, typeof(KFSignModel));
                return sm;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public KFSignModel RefreshToken(string AppID, string refresh_token)
        {
            try
            {
                string url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", AppID, refresh_token);
                string json = httpGet(url);
                log.Info("json=" + json);
                KFSignModel sm = new KFSignModel();
                sm = (KFSignModel)JsonConvert.DeserializeObject(json, typeof(KFSignModel));
                return sm;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
