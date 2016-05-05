using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using qch.core;
using qch.Models;
using Senparc.Weixin.MP.TenPayLibV3;

namespace api.Controllers
{
    /// <summary>
    /// weixin_JSSDK
    /// </summary>
    public class JsApiController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        JsapiService jsapiService = new JsapiService();
        string appId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
            }
        }
        string appSecret { get { return System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppSecret"].ToString(); } }
        private static TenPayV3Info _tenPayV3Info;

        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"]];
                }
                return _tenPayV3Info;
            }
        }
        
        /// <summary>
        /// 获取jsapi对象
        /// </summary>
        /// <param name="ShareUrl"></param>
        /// <returns></returns>
        public object GetJsApi(string ShareUrl)
        {
            try
            {
                Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.Register(appId, appSecret);
                log.Info("appid=" + appId);
                log.Info("ShareUrl=" + ShareUrl);
                JsapiModel apiModel = new JsapiModel();
                apiModel = jsapiService.GetSign(ShareUrl, ShareUrl, appId);
                if (apiModel == null)
                {
                    apiModel = new JsapiModel();
                }
                var target = new
                {
                    AppId = apiModel.AppId,
                    Timestamp = apiModel.Timestamp,
                    Noncestr = apiModel.Noncestr,
                    Signature = apiModel.Signature
                };
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
