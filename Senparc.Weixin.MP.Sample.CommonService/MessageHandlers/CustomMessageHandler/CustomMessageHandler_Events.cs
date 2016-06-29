using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using System.Configuration;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Net;
using System.IO;
using System.Text;
using qch.core;
using qch.Models;

namespace Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        UserService userService = new UserService();
        WXUserService wxservice = new WXUserService();
        AccountService accountService = new AccountService();
        IntegralService integralService = new IntegralService();
        VoucherService voucherService = new VoucherService();
        SignInService signInService = new SignInService();
        /// <summary>
        /// 图片保存路径
        /// </summary>        
        public string ImageUrl { get { return string.Format("/images/{0}/{1}/{2}/", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); } }
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string Nonce = "";
        string wxappId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();

        //当用户关注公众号后，获取欢迎信息
        private ResponseMessageBase GetWelcomeInfo()
        {

            string url = OAuth.GetAuthorizeUrl(wxappId, "https://wx.xftka.com/user/bind.html", Nonce, OAuthScope.snsapi_userinfo);

            var responseMessage = CreateResponseMessage<ResponseMessageNews>();

            //responseMessage.Articles.Add(new Article
            //{
            //    Title = "消费通商城",
            //    Description = "欢迎关注 让您睡觉都赚钱的平台 ----- 消费通商城 ！",
            //    PicUrl = "https://mmbiz.qlogo.cn/mmbiz/tEhdqg7SO3l8CnMQDzPk5ia2kUowXyltVz5C4Ug75GNCkWlDaIZckMKnVicibHmJhbcst5aMicTEhj3ILXfY3GLqTg/0?wx_fmt=jpeg",
            //    Url = "https://wx.xftka.com/ahome"
            //});

            ////增加图文消息
            //responseMessage.Articles.Add(new Article
            //{
            //    Title = "用户注册",
            //    Description = "开启“消费能省钱，不消费也能赚钱”的全新体验",
            //    PicUrl = "https://mmbiz.qlogo.cn/mmbiz/tEhdqg7SO3nb8rW3PoDrTQ2PTX6L5jy0xFJCiaia0Yib2lfLwmicx7gOdqb7pq1GkBo2h4kFmb5jIxhctehUbvZJPQ/0?wx_fmt=png",
            //    Url = url
            //});

            //responseMessage.Articles.Add(new Article
            //{
            //    Title = "分享赚钱",
            //    Description = "点击“分享赚钱”了解如何帮您赚钱",
            //    PicUrl = "https://mmbiz.qlogo.cn/mmbiz/tEhdqg7SO3nb8rW3PoDrTQ2PTX6L5jy0gOqYatqjpYDMiaAe0Rh25dmaw9wXTSzz6v8bZ5P1rr2vqiadylH03wtA/0?wx_fmt=png",
            //    Url = "https://wx.xftka.com/relay/index"
            //});

            //responseMessage.Articles.Add(new Article
            //{
            //    Title = "服务中心",
            //    Description = "点击“服务中心”让您更了解消费通网络公司",
            //    PicUrl = "https://mmbiz.qlogo.cn/mmbiz/tEhdqg7SO3nb8rW3PoDrTQ2PTX6L5jy0RxlSzP2M4Uk0vxdcGVwiaw6CCIqXDBqEB2m2o01AYBGyCiagVhS7GV8Q/0?wx_fmt=png",
            //    Url = "https://wx.xftka.com/ahome/kf"
            //});

            return responseMessage;

            /*
       要做的事情：1获取关注者的用户信息
                   2保存关注者用户信息至数据库
       */


        }

        public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        {
            // 预处理文字或事件类型请求。
            // 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
            // 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
            // 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
            // 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
            // 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey


            try
            {
                if (requestMessage.Content.ToLower() == "xft")
                {
                    //#region test
                    //System.Collections.Generic.Dictionary<string, string> dir = new System.Collections.Generic.Dictionary<string, string>();
                    //dir.Add("AppId", ConfigurationManager.AppSettings["xftAppId"].ToString());
                    //dir.Add("AppSecret",xftwl.Infrastructure.DESEncrypt.MD5Encrypt(ConfigurationManager.AppSettings["xftAppSecret"].ToString(), false));
                    //dir.Add("OpenId", "oa5e5uFH25GOUXr8OrwS0f8qTL2A");
                    //dir.Add("Content", "向微信用户发消息的测试");

                    //log.Info(xftwl.Infrastructure.HttpHelper.HttpHelper.CreateAutoSubmitForm("https://wx.xftka.com/msg/push", dir, Encoding.UTF8));
                    //    #endregion

                    var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                    strongResponseMessage.Content = "<a href='http://service.vip-wifi.com/Portal/Wx/login?weixin=xfthmk'>欢迎来到，点击此处开始免费上网</a>";
                    return strongResponseMessage;



                }
                return null;//返回null，则继续执行OnTextRequest或OnEventRequest
            }
            catch (Exception ex)
            {
                var responseMessage = CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = ex.Message;
                return responseMessage;
            }




        }

        //自定义菜单点击事件
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            //UserModel userInfo = new UserModel();
            IResponseMessageBase reponseMessage = null;
            string appId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
            //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定
            string OpenId = requestMessage.FromUserName;
            string Nonce = ToolHelper.createNonceStr();//随机数
            log.Info("****服务器生成的随机数：" + Nonce);
            log.Info("****服务器段的OpenId：" + OpenId);
            var bindInfo = wxservice.GetByOpenId(OpenId);
            //菜单点击，需要跟创建菜单时的Key匹配
            switch (requestMessage.EventKey)
            {
                #region 我的账户
                case "myMessage": //查询我的帐户
                    {

                        //未绑定会员帐户
                        if (bindInfo == null || string.IsNullOrWhiteSpace(bindInfo.UserGuid))
                        {
                            var responseMessage = CreateResponseMessage<ResponseMessageText>();
                            string urlReg = OAuth.GetAuthorizeUrl(appId, "http://www.cn-qch.com/wxuser/reg", Nonce, OAuthScope.snsapi_userinfo);
                            string urlBind = OAuth.GetAuthorizeUrl(appId, "http://www.cn-qch.com/wxuser/bind", Nonce, OAuthScope.snsapi_userinfo);
                            responseMessage.Content = string.Format("亲，您尚未绑定青创汇服务号！\r\n");
                            responseMessage.Content += string.Format("已有青创汇账号？\r\n");
                            responseMessage.Content += "<a href='" + urlBind + "'>请点击这里绑定</a>\r\n";
                            responseMessage.Content += "What？你还没注册青创汇？\r\n";
                            responseMessage.Content += "<a href='" + urlReg + "'>点击这里注册并绑定</a>\r\n";
                            return responseMessage;
                        }
                        else
                        {
                            log.Info("查询己绑定用户信息");
                            //如果己绑定查询该用户会员卡信息
                            var responseMessage = CreateResponseMessage<ResponseMessageText>();

                            //xftwl.OAuth.Client.Signature signSerivce = new xftwl.OAuth.Client.Signature();
                            //signSerivce.SetParameter("OpenId", OpenId);
                            //var sign = signSerivce.CreateSign("AppSecret", System.Web.Configuration.WebConfigurationManager.AppSettings["OauthClientSecret"].ToString());
                            //var package = signSerivce.GetContent();

                            //xftwl.OAuth.Client.GetResult<Models.CardInfoModel> client = new xftwl.OAuth.Client.GetResult<Models.CardInfoModel>();
                            //client.ApiServerUrl = "https://cardapi.xftka.com";
                            //System.Collections.Generic.Dictionary<string, string> dir = new System.Collections.Generic.Dictionary<string, string>();
                            //dir.Add("AppId", System.Web.Configuration.WebConfigurationManager.AppSettings["OauthClient"].ToString());
                            //dir.Add("PackageContent", package);
                            //dir.Add("Sign", sign);
                            //var target = client.PostData("/api/card/getinfo", dir);

                            var target = userService.GetDetail(bindInfo.UserGuid);

                            if (target == null)
                            {
                                responseMessage.Content = "什么也没有取到，请稍后再试";
                                return responseMessage;
                            }


                            if (target != null)
                            {
                                log.Info("己取到这个微信用户数据");
                                //设置登录票证
                                userService.SetAuthCookie(new UserLoginModel
                                {
                                    LoginName = target.t_User_LoginId,
                                    LoginPwd = ToolHelper.createNonceStr(),
                                    SafeCode = ToolHelper.createNonceStr()
                                });
                                var account = accountService.GetBalance(target.Guid);
                                var integral = integralService.GetIntegral(target.Guid);
                                var voucher = voucherService.GetAlluvByUser(1, 9999, target.Guid);
                                long vouchers = 0;
                                if (voucher != null && voucher.Items != null)
                                {
                                    vouchers = voucher.TotalItems;
                                }
                                //var voucher1 = voucherService.GetVoucherByUser(target.Guid, 1);
                                //var voucher2 = voucherService.GetVoucherByUser(target.Guid, 2);
                                //var voucher3 = voucherService.GetVoucherByUser(target.Guid, 3);
                                responseMessage.Content = string.Format("日期:{0}\r\n姓名:{1}\r\n手机号:{2}\r\n", DateTime.Now, target.t_User_RealName, target.t_User_LoginId);

                                responseMessage.Content += string.Format("您当前的创业币:{0}\r\n", account);
                                responseMessage.Content += string.Format("可用优惠券:{0}\r\n", vouchers);
                                //responseMessage.Content += string.Format("{0}:{1}\r\n", voucher1.VoucherType, voucher1.VoucherCount);
                                //responseMessage.Content += string.Format("{0}:{1}\r\n", voucher2.VoucherType, voucher2.VoucherCount);
                                //responseMessage.Content += string.Format("{0}:{1}\r\n", voucher3.VoucherType, voucher3.VoucherCount);
                                responseMessage.Content += string.Format("累计积分:{0} \r\n", integral);
                                string url = string.Format("<a href='http://www.cn-qch.com/wx/userCenter.html?UserGuid={0}'>查看个人中心</a>", target.Guid);

                                responseMessage.Content += url;
                                return responseMessage;
                            }

                            responseMessage.Content = "什么也没有取到，请稍后再试";
                            return responseMessage;
                        }

                    }
                #endregion
                #region 绑定账户
                case "bind":
                    {
                        log.Info("用户点击账户绑定按钮事件");
                        if (bindInfo == null || string.IsNullOrWhiteSpace(bindInfo.UserGuid))
                        {
                            //不存在绑定信息
                            var responseMessage = CreateResponseMessage<ResponseMessageText>();
                            string urlReg = OAuth.GetAuthorizeUrl(appId, "http://www.cn-qch.com/wxuser/reg", Nonce, OAuthScope.snsapi_userinfo);
                            string urlBind = OAuth.GetAuthorizeUrl(appId, "http://www.cn-qch.com/wxuser/bind", Nonce, OAuthScope.snsapi_userinfo);
                            responseMessage.Content = string.Format("亲，您尚未绑定青创汇服务号！\r\n");
                            responseMessage.Content += string.Format("已有青创汇账号？\r\n");
                            responseMessage.Content += "<a href='" + urlBind + "'>请点击这里绑定</a>\r\n";
                            responseMessage.Content += "What？你还没注册青创汇？\r\n";
                            responseMessage.Content += "<a href='" + urlReg + "'>点击这里注册并绑定</a>\r\n";
                            log.Info(responseMessage.Content);
                            return responseMessage;
                        }
                        else
                        {
                            //存在绑定信息
                            var responseMessage = CreateResponseMessage<ResponseMessageText>();
                            responseMessage.Content = "您已经绑定过青创汇\r\n";
                            responseMessage.Content += "如需解除绑定，请回复 jcbd";
                            return responseMessage;
                        }
                    }
                #endregion
                #region 创客社区
                case "chuangke":
                    {
                        var responseMessage = CreateResponseMessage<ResponseMessageText>();
                        responseMessage.Content = "功能开发中敬请期待！";
                        return responseMessage;
                    }
                #endregion
                #region 积分抽奖
                    //已改为链接跳转
                case "integral":
                    {
                        var responseMessage = CreateResponseMessage<ResponseMessageText>();
                        responseMessage.Content = "功能开发中敬请期待！";
                        return responseMessage;
                    }
                #endregion
                #region 签到送积分
                case "signUp":
                    {
                        //未绑定会员帐户
                        if (bindInfo == null || string.IsNullOrWhiteSpace(bindInfo.UserGuid))
                        {
                            var responseMessage = CreateResponseMessage<ResponseMessageText>();
                            string url = OAuth.GetAuthorizeUrl(appId, "http://www.cn-qch.com/wxuser/bind", Nonce, OAuthScope.snsapi_userinfo);
                            responseMessage.Content = "<a href='" + url + "'>请先绑定青创汇帐户</a>";
                            return responseMessage;
                        }
                        else
                        {
                            //如果己绑定查询该用户信息
                            var responseMessage = CreateResponseMessage<ResponseMessageText>();
                            var target = userService.GetDetail(bindInfo.UserGuid);
                            if (target != null)
                            {
                                log.Info("用户签到，己取到这个微信用户数据");
                                var msg = signInService.SignIn(target.Guid);
                                responseMessage.Content = msg.Data;
                                if (msg.type == "success")
                                {
                                    responseMessage.Content += "\r\n";
                                    responseMessage.Content += msg.Remark;
                                    responseMessage.Content += "\r\n去APP签到多送5积分哦~";
                                }
                                return responseMessage;
                            }
                            responseMessage.Content = "签到失败，请稍后再试";
                            return responseMessage;
                        }

                    }
                #endregion
                #region 优惠券领取
                case "voucher":
                    {
                        var responseMessage = CreateResponseMessage<ResponseMessageText>();
                        responseMessage.Content = "运营喵策划中，敬请期待！！！";
                        return responseMessage;
                    }
                #endregion
                #region 预约报名查询
                    //已经改为链接跳转
                case "myserver":
                    {
                        var responseMessage = CreateResponseMessage<ResponseMessageText>();
                        responseMessage.Content = "功能开发中......";
                        return responseMessage;
                    }
                #endregion
                #region 邀请好友结果
                case "yqRequest":
                    {
                        var responseMessage = CreateResponseMessage<ResponseMessageText>();
                        if (bindInfo != null)
                        {
                            var uservoucher = voucherService.GetVoucherByUser(bindInfo.UserGuid, "zhijietuijian");
                            int xy1 = userService.GetReferral1(bindInfo.UserGuid);
                            int xy2 = userService.GetReferral2(bindInfo.UserGuid);
                            responseMessage.Content += string.Format("您当前的推荐信息:\r\n");
                            responseMessage.Content += string.Format("直接邀请注册:{0}人\r\n", xy1);
                            responseMessage.Content += string.Format("间接邀请注册:{0}人\r\n", xy2);
                            responseMessage.Content += string.Format("获得奖励: \r\n");
                            if (uservoucher != null)
                                responseMessage.Content += string.Format("优惠券: {0}张{1}元{2}\r\n", uservoucher.VoucherCount, uservoucher.VoucherMoney, uservoucher.VoucherTypeText);
                            else
                                responseMessage.Content += string.Format("暂无优惠券奖励！ \r\n");
                            responseMessage.Content += string.Format("创业币:{0} \r\n", 0);
                            return responseMessage;
                        }
                        else
                        {
                            string urlReg = OAuth.GetAuthorizeUrl(appId, "http://www.cn-qch.com/wxuser/reg", Nonce, OAuthScope.snsapi_userinfo);
                            string urlBind = OAuth.GetAuthorizeUrl(appId, "http://www.cn-qch.com/wxuser/bind", Nonce, OAuthScope.snsapi_userinfo);
                            responseMessage.Content = string.Format("亲，您尚未绑定青创汇服务号！\r\n");
                            responseMessage.Content += string.Format("已有青创汇账号？\r\n");
                            responseMessage.Content += "<a href='" + urlBind + "'>请点击这里绑定</a>\r\n";
                            responseMessage.Content += "What？你还没注册青创汇？\r\n";
                            responseMessage.Content += "<a href='" + urlReg + "'>点击这里注册并绑定</a>\r\n";
                            return responseMessage;
                        }
                    }
                #endregion
                case "kf"://联系客服
                    {

                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "客服转接中，请稍侯......\r\n 客服电话：400-1690-999";//显示欢迎信息
                        reponseMessage = strongResponseMessage;

                        //多客服响应
                        return this.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();

                    }
                //break;

                default:
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您点击了按钮，EventKey：" + requestMessage.EventKey;
                        reponseMessage = strongResponseMessage;
                    }
                    break;
            }

            return reponseMessage;
        }

        public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "您刚才发送了ENTER事件请求。";
            return responseMessage;
        }


        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
            //var responseMessage = CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = "这里写什么都无所谓，比如：上帝爱你！";
            //return responseMessage;//这里也可以返回null（需要注意写日志时候null的问题）
            return null;
        }

        /// <summary>
        /// 扫描带场景二维码关注
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            try
            {
                log.Info("扫描带场景二维码关注事件");



                //通过扫描关注
                var responseMessage = CreateResponseMessage<ResponseMessageText>();

                responseMessage.Content = "您己关注消费通";
                return responseMessage;
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                //通过扫描关注
                var responseMessage = CreateResponseMessage<ResponseMessageText>();

                responseMessage.Content = "通过扫描关注。";
                return responseMessage;
            }

        }

        /// <summary>
        /// 事件之扫码推事件(scancode_push)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodePushRequest(RequestMessageEvent_Scancode_Push requestMessage)
        {
            string appId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
            string openId = requestMessage.FromUserName;
            string token = CommonAPIs.AccessTokenContainer.GetToken(appId);
            var wxuser = CommonAPIs.CommonApi.GetUserInfo(token, openId);

            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之扫码推事件";
            return responseMessage;
        }

        /// <summary>
        /// 事件之扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodeWaitmsgRequest(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之扫码推事件且弹出“消息接收中”提示框";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        {
            //说明：这条消息只作为接收，下面的responseMessage到达不了客户端，类似OnEvent_UnsubscribeRequest
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您点击了view按钮，将打开网页：" + requestMessage.EventKey;
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_MassSendJobFinishRequest(RequestMessageEvent_MassSendJobFinish requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "接收到了群发完成的信息。";
            return responseMessage;
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            log.Info("订阅关注公众号事件");
            log.Info(string.Format("Encrypt={0},EventKey={1}", requestMessage.Encrypt, requestMessage.EventKey));

            #region 扫码后的操作
            try
            {
                //保存推荐人

                if (!string.IsNullOrWhiteSpace(requestMessage.EventKey))
                {

                    Nonce = ToolHelper.createNonceStr();//随机数
                    //扫码关注后，得到扫码人的微信会员信息
                    string appId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
                    string openId = requestMessage.FromUserName;
                    log.Info("订阅公众号openid=" + openId);
                    string token = CommonAPIs.AccessTokenContainer.GetToken(appId);
                    var wxuser = CommonAPIs.CommonApi.GetUserInfo(token, openId);
                    //推广人ID

                    int TgUserId = Convert.ToInt32(requestMessage.EventKey.Split('_')[1]);
                    log.Info("扫码二维码得到的推荐人id：" + TgUserId);
                    qch.Infrastructure.CookieHelper.ClearCookie("TgUId");
                    qch.Infrastructure.CookieHelper.SetCookie("TgUId", TgUserId.ToString());

                    //获取推广用户OpenId
                    string txOpenId = "";
                    var tguser = wxservice.GetByOpenId("");
                    if (tguser != null)
                    {
                        txOpenId = tguser.OpenId;
                    }
                    else { log.Error("通过扫描关注后未能在xft_card_weixin表中获取到推广人的用户绑定信息"); }
                    //保存微信用户信息
                    var user = wxservice.GetByOpenId("");
                    //如果不存在，直接新增 
                    if (user == null)
                    {



                        string content = string.Format("您推荐的一级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                        //推送消息给推广人
                        Custom.SendText(token, tguser.OpenId, content);

                        #region 给推广人的上级发消息
                        //if (tguser.WxTgUserId > 0)
                        //{
                        //    //上上级用户
                        //    var tg2 = cardweixinService.Get(tguser.WxTgUserId);

                        //    if (tg2 != null)
                        //    {
                        //        log.Info("上级的上级用户UserId=" + tg2.UserId + "  openid=" + tg2.OpenId);
                        //        string content2 = string.Format("您推荐的二级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                        //        Custom.SendText(token, tg2.OpenId, content2);
                        //        #region 给3级发消息
                        //        //获取上上上级用户
                        //        var tg3 = cardweixinService.Get(tg2.WxTgUserId);
                        //        if (tg3 != null)
                        //        {
                        //            log.Info("上级的上级的上级用户UserId=" + tg3.UserId + "  openid=" + tg3.OpenId);
                        //            string content3 = string.Format("您推荐的三级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                        //            Custom.SendText(token, tg3.OpenId, content3);
                        //        }
                        //        #endregion
                        //    }
                        //}

                        #endregion
                    }
                    else
                    {
                        //如果存在，查看该微信用户是否有会员卡信息，如果没有会员信息，或者推荐人为空，那么更新xft_card_weixin WxTgUserId字段，其它情况不处理
                        //var userinfo = userService.GetDetail(user.UserId);
                        //if (userinfo == null || string.IsNullOrEmpty(userinfo.ReferrerCardNo))
                        //{
                        //    user.WxTgUserId = tguser.UserId;
                        //    cardweixinService.Save(user);
                        //}
                        //if (user.WxTgUserId != tguser.UserId)
                        //{
                        //    string content = string.Format("您推荐的一级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                        //    //推送消息给推广人
                        //    Custom.SendText(token, tguser.OpenId, content);

                        //    #region 给推广人的上级发消息
                        //    if (tguser.WxTgUserId > 0)
                        //    {
                        //        var tg2 = cardweixinService.Get(tguser.WxTgUserId);
                        //        if (tg2 != null)
                        //        {
                        //            string content2 = string.Format("您推荐的二级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                        //            Custom.SendText(token, tg2.OpenId, content2);
                        //            #region 给3级发消息
                        //            //获取上上上级用户
                        //            var tg3 = cardweixinService.Get(tg2.WxTgUserId);
                        //            if (tg3 != null)
                        //            {
                        //                log.Info("上级的上级的上级用户UserId=" + tg3.UserId + "  openid=" + tg3.OpenId);
                        //                string content3 = string.Format("您推荐的三级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                        //                Custom.SendText(token, tg3.OpenId, content3);
                        //            }
                        //            #endregion
                        //        }
                        //    }

                        //    #endregion
                        //}

                        //user.WxTgUserId = tguser.UserId;
                        //user.Nonce = Nonce;
                        //cardweixinService.Save(user);


                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

            }
            return GetWelcomeInfo();
            #endregion

        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "有空再来";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出拍照或者相册发图（pic_photo_or_album）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicPhotoOrAlbumRequest(RequestMessageEvent_Pic_Photo_Or_Album requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出拍照或者相册发图";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出系统拍照发图(pic_sysphoto)
        /// 实际测试时发现微信并没有推送RequestMessageEvent_Pic_Sysphoto消息，只能接收到用户在微信中发送的图片消息。
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicSysphotoRequest(RequestMessageEvent_Pic_Sysphoto requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出系统拍照发图";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出微信相册发图器(pic_weixin)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicWeixinRequest(RequestMessageEvent_Pic_Weixin requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出微信相册发图器";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出地理位置选择器（location_select）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_LocationSelectRequest(RequestMessageEvent_Location_Select requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出地理位置选择器";
            return responseMessage;
        }
    }
}