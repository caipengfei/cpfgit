using qch.core;
using qch.Infrastructure;
using qch.Models;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Sample.CommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using xftwl.Infrastructure;

namespace Senparc.Weixin.MP.Sample.Filters
{
    /// <summary>
    /// 微信授权（扫码登录）
    /// </summary>
    public class WxAuthorization : ActionFilterAttribute
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserService userService = new UserService();
        WXUserService wxService = new WXUserService();

        UserModel _loginUser;
        UserModel LoginUser
        {
            get
            {
                if (_loginUser == null)
                {
                    try
                    {
                        HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie 
                        if (authCookie == null)
                            return null;
                        FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密 
                        var loginUser = SerializeHelper.Instance.JsonDeserialize<UserLoginModel>(Ticket.UserData);//反序列化  
                        return userService.GetDetail(loginUser.LoginName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        return null;
                    }
                }
                return _loginUser;
            }
            set
            {
                this._loginUser = value;
            }
        }
        public static readonly string WeixinUrl = System.Configuration.ConfigurationManager.AppSettings["WeixinUrl"];
        /// <summary>
        /// 指定允许访问的用户角色
        /// </summary>
        public string Roles { get; set; }
        /// <summary>
        /// 待返回的URL
        /// </summary>
        public string ReturnUrl { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region 微信授权与会员授权的结合
            /*
               如果用户未登录
               1生成将获取openid的页面生成微信授权链接并跳转
               2微信授权通过，通过openid获取绑定人信息，并赋于登录状态
               3微信授权不通过，跳转到绑定页面
             */

            try
            {
                if (LoginUser == null)
                {
                    string url = filterContext.HttpContext.Request.Url.ToString();
                    HttpContext.Current.Session["ReturnUrl"] = url;
                    log.Info("授权请求url=" + url);
                    string openid = CookieHelper.GetCookieValue("OpenId");
                    log.Info("从cookie中取到的openid=" + openid);
                    if (openid == "")
                    {
                        #region  没有经过授权页面的访问，如果没有登录，就去请求微信授权
                        string Nonce = ToolHelper.createNonceStr();//随机数
                        CookieHelper.SetCookie("Nonce", Nonce);
                        log.Info("请求微信授权时生成的随机数为：" + Nonce);
                        string appId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
                        log.Info("appid=" + appId);
                        log.Info("WeixinUrl=" + WeixinUrl);
                        var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/auth/index", Nonce, OAuthScope.snsapi_userinfo);
                        log.Info("生成的授权链接为=" + weixinAuthUrl);
                        #endregion

                        filterContext.Result = new RedirectResult(weixinAuthUrl);

                    }
                    else
                    {
                        //通过授权页的访问，用openid获取绑定人信息
                        //如果己绑定，赋于该会员的会员授权
                        //如果未绑定，跳转到会员绑定页面
                        string DeOpenId = qch.Infrastructure.DESEncrypt.Decrypt(openid);
                        var bindInfo = wxService.GetByOpenId(DeOpenId);
                        log.Info("解密后的openid=" + DeOpenId);
                        string wxtguid = "";
                        if (bindInfo != null)
                            wxtguid = bindInfo.WxTgUserGuid;

                        if (bindInfo == null || string.IsNullOrWhiteSpace(bindInfo.UserGuid))
                        {
                            //找不到绑定信息，需要重新关注公众号
                            #region 没有绑定，现在去绑定
                            log.Info("当前微信用户还没有与会员绑定，跳转到绑定页面");
                            string Nonce = ToolHelper.createNonceStr();//随机数

                            var msg = wxService.Save(new qch.Models.WXUserModel
                            {
                                CreateDate = DateTime.Now,
                                MediaDate = DateTime.Now,
                                MediaId = "",
                                Nonce = Nonce, //每个用户随机数与openid在一起存放数据库中
                                OpenId = DeOpenId,
                                QrCode = "",
                                UserGuid = "",
                                WxTgUserGuid = wxtguid,
                                Guid = Guid.NewGuid().ToString()
                            });
                            if (msg.type == "success")
                                log.Info("微信用户添加成功，OpenId=" + DeOpenId);
                            else
                                log.Error("微信用户添加失败，OpenId=" + DeOpenId);
                            string appId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
                            log.Info("ReturnUrl=" + ReturnUrl);
                            var weixinAuthUrl = OAuth.GetAuthorizeUrl(appId, WeixinUrl + "/wxuser/reg", Nonce, OAuthScope.snsapi_userinfo);
                            #endregion
                            filterContext.Result = new RedirectResult(weixinAuthUrl);
                            log.Info("当前微信用户还没有与会员绑定，跳转到绑定页面" + weixinAuthUrl);
                        }
                        else
                        {
                            #region 赋于该会员授权

                            var user = userService.GetDetail(bindInfo.UserGuid);
                            if (user != null)
                            {
                                userService.SetAuthCookie(new UserLoginModel
                                {
                                    LoginName = user.t_User_LoginId,
                                    LoginPwd = ToolHelper.createNonceStr(),
                                    SafeCode = ToolHelper.createNonceStr()
                                });
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("WxAuthorization过滤器出现了异常：" + ex.Message);
            }
            #endregion

            base.OnActionExecuting(filterContext);
        }


    }
}