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
    /// 
    /// </summary>
    public class WXUserService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        WXUserRepository rp = new WXUserRepository();

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
        /// 获取微信用户信息（请求微信服务器）
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        public KFUserModel GetUser(string Token, string OpenId)
        {
            try
            {
                string userUrl = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", Token, OpenId);
                string user_json = httpGet(userUrl);
                log.Info("user_json=" + user_json);
                KFUserModel model = new KFUserModel();
                model = (KFUserModel)JsonConvert.DeserializeObject(user_json, typeof(KFUserModel));
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 请求微信用户信息，如果取不到，则重新请求token，然后再去发送请求微信用户信息
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <param name="Code"></param>
        /// <param name="Token"></param>
        /// <param name="OpenId"></param>
        /// <returns></returns>
        public KFUserModel GetUser(string AppID, string AppSecret, string Code, string Token, string OpenId)
        {
            try
            {
                string userUrl = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", Token, OpenId);
                string user_json = httpGet(userUrl);
                log.Info("user_json=" + user_json);
                KFUserModel model = null;
                model = (KFUserModel)JsonConvert.DeserializeObject(user_json, typeof(KFUserModel));
                if (model == null || model.nickname == null)
                {
                    //重新请求token
                    log.Info("WXUserService，GetUser方法获取微信用户信息异常，重新请求了token");
                    string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", AppID, AppSecret, Code);
                    string json = httpGet(url);
                    log.Info("json=" + json);
                    KFSignModel sm = null;
                    sm = (KFSignModel)JsonConvert.DeserializeObject(json, typeof(KFSignModel));
                    if (sm != null && sm.access_token != null)
                    {
                        Token = sm.access_token.ToString();
                        OpenId = sm.openid.ToString();
                        string userUrl2 = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", Token, OpenId);
                        string jj = httpGet(userUrl2);
                        model = (KFUserModel)JsonConvert.DeserializeObject(jj, typeof(KFUserModel));
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// GetByUserId
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public WXUserModel GetByUserId(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                return rp.GetByUserId(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// GetByOpenId
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public WXUserModel GetByOpenId(string OpenId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(OpenId))
                    return null;
                return rp.GetByOpenId(OpenId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// GetByUnionId
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public WXUserModel GetByUnionId(string UnionId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UnionId))
                    return null;
                return rp.GetByUnionId(UnionId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// getbyid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public WXUserModel GetById(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                return rp.GetById(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存微信用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public bool SaveUser(WXUserModel model)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        return false;
        //    }
        //}
        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Msg Save(WXUserModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "操作失败";
            try
            {
                if (model == null)
                    return msg;
                var tt = GetById(model.Guid);
                if (tt != null)
                {
                    //model.CreateDate = DateTime.Now;
                    model.MediaDate = DateTime.Now;
                    if (rp.Edit(model))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
                else
                {
                    var x = rp.GetByUnionId(model.UnionId);
                    if (x != null)
                    {
                        msg.Data = "UnionId已存在";
                        log.Info("UnionId已存在");
                        if (!string.IsNullOrWhiteSpace(model.KFOpenId))
                        {
                            x.KFOpenId = model.KFOpenId;
                            x.Name = model.Name;
                            x.Avator = model.Avator;
                            rp.Edit(x);
                        }
                        return msg;
                    }
                    var a = rp.GetByOpenId(model.OpenId);
                    if (a != null)
                    {
                        msg.Data = "openid已存在";
                        log.Info("openid已存在");
                        return msg;
                    }
                    var b = rp.GetByUserId(model.UserGuid);
                    if (b != null)
                    {
                        msg.Data = "已存在";
                        log.Info("已存在");
                        return msg;
                    }
                    model.Guid = Guid.NewGuid().ToString();
                    model.CreateDate = DateTime.Now;
                    model.MediaDate = DateTime.Now;
                    if (rp.Add(model))
                    {
                        msg.type = "success";
                        msg.Data = "新增成功";
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public bool Del(string Guid)
        {
            try
            {
                var model = GetById(Guid);
                if (model == null)
                    return false;
                return rp.Del(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
    }
}
