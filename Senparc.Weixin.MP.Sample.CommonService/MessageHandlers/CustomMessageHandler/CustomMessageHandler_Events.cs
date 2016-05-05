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
using xftwl.Common.Card;

namespace Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {

        xftwl.Common.Card.CardWeixinService cardweixinService = new xftwl.Common.Card.CardWeixinService();
        xftwl.Common.Card.VipCardService cardService = new xftwl.Common.Card.VipCardService();
        xftwl.Common.UserService userService = new xftwl.Common.UserService();
        xftwl.Common.SmsService smsService = new xftwl.Common.SmsService();
        xftwl.Common.Card.RePacketsService repackService = new xftwl.Common.Card.RePacketsService();

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

            responseMessage.Articles.Add(new Article
            {
                Title = "消费通商城",
                Description = "欢迎关注 让您睡觉都赚钱的平台 ----- 消费通商城 ！",
                PicUrl = "https://mmbiz.qlogo.cn/mmbiz/tEhdqg7SO3l8CnMQDzPk5ia2kUowXyltVz5C4Ug75GNCkWlDaIZckMKnVicibHmJhbcst5aMicTEhj3ILXfY3GLqTg/0?wx_fmt=jpeg",
                Url = "https://wx.xftka.com/ahome"
            });

            //增加图文消息
            responseMessage.Articles.Add(new Article
            {
                Title = "用户注册",
                Description = "开启“消费能省钱，不消费也能赚钱”的全新体验",
                PicUrl = "https://mmbiz.qlogo.cn/mmbiz/tEhdqg7SO3nb8rW3PoDrTQ2PTX6L5jy0xFJCiaia0Yib2lfLwmicx7gOdqb7pq1GkBo2h4kFmb5jIxhctehUbvZJPQ/0?wx_fmt=png",
                Url = url
            });

            responseMessage.Articles.Add(new Article
            {
                Title = "分享赚钱",
                Description = "点击“分享赚钱”了解如何帮您赚钱",
                PicUrl = "https://mmbiz.qlogo.cn/mmbiz/tEhdqg7SO3nb8rW3PoDrTQ2PTX6L5jy0gOqYatqjpYDMiaAe0Rh25dmaw9wXTSzz6v8bZ5P1rr2vqiadylH03wtA/0?wx_fmt=png",
                Url = "https://wx.xftka.com/relay/index"
            });

            responseMessage.Articles.Add(new Article
            {
                Title = "服务中心",
                Description = "点击“服务中心”让您更了解消费通网络公司",
                PicUrl = "https://mmbiz.qlogo.cn/mmbiz/tEhdqg7SO3nb8rW3PoDrTQ2PTX6L5jy0RxlSzP2M4Uk0vxdcGVwiaw6CCIqXDBqEB2m2o01AYBGyCiagVhS7GV8Q/0?wx_fmt=png",
                Url = "https://wx.xftka.com/ahome/kf"
            });

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
                    strongResponseMessage.Content = "<a href='http://service.vip-wifi.com/Portal/Wx/login?weixin=xfthmk'>欢迎来到消费通网络公司，点击此处开始免费上网</a>";
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
            xftwl.Common.UserModel userInfo = new xftwl.Common.UserModel();
            IResponseMessageBase reponseMessage = null;
            string appId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
            //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定
            string OpenId = requestMessage.FromUserName;
            //菜单点击，需要跟创建菜单时的Key匹配
            switch (requestMessage.EventKey)
            {
                case "mycode":
                    {
                        #region 我的推广二维码
                        try
                        {

                            string Nonce = ToolHelper.createNonceStr();//随机数
                            log.Info("****服务器生成的随机数：" + Nonce);
                            log.Info("****服务器段的OpenId：" + OpenId);
                            var bindInfo = cardweixinService.Get(OpenId);
                            //未绑定会员帐户
                            if (bindInfo == null || bindInfo.UserId == 0)
                            {
                                if (bindInfo == null)
                                { //己关注，但数据库中无信息时
                                    bindInfo = new CardWeixinModel
                                    {
                                        CreateDate = DateTime.Now,
                                        Id = 0,
                                        Nonce = Nonce,
                                        OpenId = OpenId,
                                        QrCode = "",
                                        UserId = 0,
                                        WxTgUserId = 0
                                    };
                                }

                                bindInfo.Nonce = Nonce; //该用户随机数
                                cardweixinService.Save(bindInfo);

                                var responseMessage = CreateResponseMessage<ResponseMessageText>();
                                string url = OAuth.GetAuthorizeUrl(appId, "https://wx.xftka.com/user/bind.html", Nonce, OAuthScope.snsapi_userinfo);
                                responseMessage.Content = "<a href='" + url + "'>请先绑定消费通会员帐户</a>";
                                return responseMessage;
                            }
                            else
                            {
                                ImageHelper imgService = new ImageHelper();
                                /*
                                    生成二维码，
                                    用背景图再生成带水印的图片，
                                    把水印图上传到微信服务器，
                                    发送图片给微信用户
                                */
                                //accesstoken
                                string apptoken = CommonAPIs.AccessTokenContainer.GetToken(appId);
                                //openid
                                string openId = requestMessage.FromUserName;
                                //微信用户
                                var wxuser = CommonAPIs.CommonApi.GetUserInfo(apptoken, openId);
                                //微信用户的头像
                                var wxavator = wxuser.headimgurl;

                                log.Info("**********************微信头像：" + wxavator + "************************");
                                //如果该用户请求过临时二维码
                                if (!string.IsNullOrWhiteSpace(bindInfo.MediaId))
                                {

                                    log.Info("当前用户的MediaId为" + bindInfo.MediaId);
                                    //二维码修改为3天过期，在即将过期的时间 提前60秒
                                    if (bindInfo.MediaDate.AddSeconds(259140) > DateTime.Now)
                                    {
                                        log.Info("如果二维码未过期");
                                        log.Info(bindInfo.MediaDate.AddSeconds(259140).ToString());
                                        //如果二维码在有效期内，直接返回
                                        var responseMsg = CreateResponseMessage<ResponseMessageImage>();
                                        log.Info(string.Format("apptoken={0},openid={1},mediaid={2}", apptoken, openId, bindInfo.MediaId));
                                        Senparc.Weixin.MP.AdvancedAPIs.Custom.SendImage(apptoken, openId, bindInfo.MediaId);
                                        responseMsg.FromUserName = openId;
                                        responseMsg.Image.MediaId = bindInfo.MediaId;
                                        return responseMsg;
                                    }
                                    else
                                    {
                                        log.Info("如果二维码已过期");
                                        //如果二维码已过期，清除掉该图片节省空间，重新请求
                                        System.IO.File.Delete(bindInfo.QrCode);
                                        //获取二维码
                                        string media_id = imgService.CreateUserQrCode(wxavator, wxuser.nickname, bindInfo.UserId);
                                        if (media_id != "")
                                        {
                                            //2发送图片给用户
                                            log.Info(string.Format("apptoken={0},openid={1},mediaid={2}", apptoken, openId, media_id));
                                            var responseMsg = CreateResponseMessage<ResponseMessageImage>();
                                            Senparc.Weixin.MP.AdvancedAPIs.Custom.SendImage(apptoken, openId, media_id);
                                            responseMsg.FromUserName = openId;
                                            responseMsg.Image.MediaId = media_id;
                                            return responseMsg;
                                        }
                                        else
                                        {
                                            var responseMsg = CreateResponseMessage<ResponseMessageText>();
                                            responseMsg.Content = "系统繁忙,暂无法生成二维码";
                                            return responseMsg;
                                        }
                                    }
                                }
                                else
                                {
                                    log.Info("当前用户的MediaId不存在");
                                    //获取二维码
                                    string media_id = imgService.CreateUserQrCode(wxavator, wxuser.nickname, bindInfo.UserId);
                                    log.Info("新生成的MediaId=" + media_id);
                                    if (media_id != "")
                                    {
                                        log.Info("保存新生成mediaId");
                                        //2发送图片给用户
                                        var responseMsg = CreateResponseMessage<ResponseMessageImage>();
                                        log.Info("开始向这个用户发消息" + openId);
                                        Senparc.Weixin.MP.AdvancedAPIs.Custom.SendImage(apptoken, openId, media_id);
                                        responseMsg.FromUserName = openId;
                                        responseMsg.Image.MediaId = media_id;
                                        return responseMsg;
                                    }
                                    else
                                    {
                                        var responseMsg = CreateResponseMessage<ResponseMessageText>();
                                        responseMsg.Content = "系统繁忙,暂无法生成二维码";
                                        return responseMsg;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                        }

                        #endregion
                    }
                    break;
                case "myorder": //我的订单
                    {
                        #region 我的订单
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;

                        strongResponseMessage.Content = "正在完善";
                        #endregion
                    }
                    break;
                case "scsp": //上传商品
                    {
                        #region 上传商品
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;

                        strongResponseMessage.Content = "正在完善";
                        #endregion
                    }
                    break;

                case "account": //查询我的帐户
                    {
                        string Nonce = ToolHelper.createNonceStr();//随机数
                        log.Info("****服务器生成的随机数：" + Nonce);
                        log.Info("****服务器段的OpenId：" + OpenId);
                        var bindInfo = cardweixinService.Get(OpenId);
                        //未绑定会员帐户
                        if (bindInfo == null || bindInfo.UserId == 0)
                        {
                            if (bindInfo == null)
                            { //己关注，但数据库中无信息时
                                bindInfo = new CardWeixinModel
                                {
                                    CreateDate = DateTime.Now,
                                    Id = 0,
                                    Nonce = Nonce,
                                    OpenId = OpenId,
                                    QrCode = "",
                                    UserId = 0,
                                    WxTgUserId = 0
                                };
                            }

                            bindInfo.Nonce = Nonce; //该用户随机数
                            cardweixinService.Save(bindInfo);

                            var responseMessage = CreateResponseMessage<ResponseMessageText>();
                            string url = OAuth.GetAuthorizeUrl(appId, "https://wx.xftka.com/user/bind.html", Nonce, OAuthScope.snsapi_userinfo);
                            responseMessage.Content = "<a href='" + url + "'>请先绑定消费通会员帐户</a>";
                            return responseMessage;
                        }
                        else
                        {
                            log.Info("查询己绑定用户会员卡信息");
                            //如果己绑定查询该用户会员卡信息
                            var responseMessage = CreateResponseMessage<ResponseMessageText>();

                            xftwl.OAuth.Client.Signature signSerivce = new xftwl.OAuth.Client.Signature();
                            signSerivce.SetParameter("OpenId", OpenId);
                            var sign = signSerivce.CreateSign("AppSecret", System.Web.Configuration.WebConfigurationManager.AppSettings["OauthClientSecret"].ToString());
                            var package = signSerivce.GetContent();

                            xftwl.OAuth.Client.GetResult<Models.CardInfoModel> client = new xftwl.OAuth.Client.GetResult<Models.CardInfoModel>();
                            client.ApiServerUrl = "https://cardapi.xftka.com";
                            System.Collections.Generic.Dictionary<string, string> dir = new System.Collections.Generic.Dictionary<string, string>();
                            dir.Add("AppId", System.Web.Configuration.WebConfigurationManager.AppSettings["OauthClient"].ToString());
                            dir.Add("PackageContent", package);
                            dir.Add("Sign", sign);
                            var target = client.PostData("/api/card/getinfo", dir);

                            if (target == null)
                            {
                                responseMessage.Content = "什么也没有取到，请稍后再试";
                                return responseMessage;
                            }


                            if (target != null)
                            {
                                log.Info("己取到这个微信用户数据");

                                responseMessage.Content = string.Format("日期:{0}\r\n姓名:{1}\r\n卡号:{2}\r\n", DateTime.Now, target.TrueName, target.CardNo);


                                responseMessage.Content += string.Format("总余额:{0}\r\n可用余额:{1}\r\n可退余额:{2}\r\n活动保证金:{3}\r\n", target.Balance, target.AvailableBalance, target.TixianBalance, target.Deposit);

                                responseMessage.Content += string.Format("直推人数:{0}\r\n间推人数:{1}\r\n", target.T1, target.T2);
                                responseMessage.Content += string.Format("总奖励:{0}\r\n", target.Awards);
                                responseMessage.Content += string.Format("区域:{0} {1} {2}\r\n", target.Province, target.City, target.Dis);
                                string url = string.Format("<a href='https://wx.xftka.com/relay/MyRelay?OpenId={0}'>查看我的分享赚</a>", OpenId);

                                responseMessage.Content += url;
                                return responseMessage;
                            }

                            responseMessage.Content = "什么也没有取到，请稍后再试";
                            return responseMessage;
                        }

                    }
                    //break;

                /***********************************************************/
                case "bangding"://绑定
                    {
                        #region 卡绑定
                        //思路，微信生成签名去访问https://www.xftka.com
                        //未绑定用户执行用户绑定

                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;

                        //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定


                        var bindInfo = cardweixinService.Get(requestMessage.FromUserName);
                        //未绑定会员帐户
                        if (bindInfo == null)
                        {
                            //时间戳
                            string timestamp = xftwl.Infrastructure.TimeHelper.ConvertDateTime(DateTime.Now);
                            string token = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaToken"].ToString();//这个是微信与www.xftka.com之间的约定
                            string xftkaSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaSecret"].ToString(); //签名密钥
                            //应用签名=token+签名密钥
                            string signature = xftwl.Infrastructure.DESEncrypt.Encrypt(xftkaSecret, token);
                            //加密OpenId
                            OpenId = xftwl.Infrastructure.DESEncrypt.Encrypt(OpenId, token);
                            strongResponseMessage.Content = string.Format("<a href='https://www.xftka.com/weixin/login?OpenId={0}&timestamp={1}&signature={2}'>点此绑定惠民卡帐户</a>", OpenId, timestamp, signature);
                        }
                        else
                        {

                            //如果己绑定查询该用户会员卡信息
                            userInfo = userService.GetDetail(bindInfo.UserId);

                            //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。

                            var income = cardService.GetIncome(userInfo.CardNo);//总收益
                            var balance = cardService.GetBalance(userInfo.CardNo);//卡余额
                            var award = cardService.SumAward(userInfo.CardNo);//提成
                            if (string.IsNullOrWhiteSpace(userInfo.CardNo))
                            {
                                strongResponseMessage.Content = "您的会员卡号没有找到";

                            }
                            else
                            {
                                strongResponseMessage.Content = "您已经绑定过会员卡了~";
                            }
                        }
                        #endregion
                    }
                    break;
                case "balance": //查询余额及红包
                    {
                        #region 余额以及红包
                        //思路，微信生成签名去访问https://www.xftka.com
                        //未绑定用户执行用户绑定

                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;

                        //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定


                        var bindInfo = cardweixinService.Get(requestMessage.FromUserName);
                        //未绑定会员帐户
                        if (bindInfo == null)
                        {
                            //时间戳
                            string timestamp = xftwl.Infrastructure.TimeHelper.ConvertDateTime(DateTime.Now);
                            string token = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaToken"].ToString();//这个是微信与www.xftka.com之间的约定
                            string xftkaSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaSecret"].ToString(); //签名密钥
                            //应用签名=token+签名密钥
                            string signature = xftwl.Infrastructure.DESEncrypt.Encrypt(xftkaSecret, token);
                            //加密OpenId
                            OpenId = xftwl.Infrastructure.DESEncrypt.Encrypt(OpenId, token);
                            strongResponseMessage.Content = string.Format("<a href='https://wx.xftka.com/user/bind.html'>请先绑定惠民卡帐户</a>", OpenId, timestamp, signature);
                        }
                        else
                        {

                            //如果己绑定查询该用户会员卡信息
                            userInfo = userService.GetDetail(bindInfo.UserId);

                            //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。
                            int hongbao = 0;//红包总数
                            int redpack = 0;//可用红包数量
                            var balance = cardService.GetBalance(userInfo.CardNo);//卡余额
                            string phone = "";
                            if (!string.IsNullOrEmpty(userInfo.Phone))
                                phone = userInfo.Phone;
                            if (phone != "")
                            {
                                //红包总数
                                var redList = repackService.GetByPhone(phone, false);
                                if (redList != null && redList.ToList().Count > 0)
                                    hongbao = redList.ToList().Count;
                                //现有红包数量
                                var redList1 = repackService.GetByPhone(phone, true);
                                if (redList1 != null && redList1.ToList().Count > 0)
                                    redpack = redList1.ToList().Count;
                            }
                            //hongbao = repackService.Search(userInfo.CardNo);
                            if (string.IsNullOrWhiteSpace(userInfo.CardNo))
                            {
                                strongResponseMessage.Content = "您的会员卡号没有找到";

                            }
                            else
                            {
                                strongResponseMessage.Content = string.Format("截止日期:{0}\r\n会员卡号:{1}\r\n余额:{2}\r\n红包数量:{3}个(单个价值10元)", DateTime.Now, userInfo.CardNo, balance, redpack);
                            }
                        }
                        #endregion
                    }
                    break;

                case "liucheng": //办卡流程
                    {
                        # region 办卡流程
                        //思路，微信生成签名去访问https://www.xftka.com
                        //未绑定用户执行用户绑定

                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;

                        //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定

                        var bindInfo = cardweixinService.Get(requestMessage.FromUserName);
                        //未绑定会员帐户
                        if (bindInfo == null)
                        {
                            //时间戳
                            string timestamp = xftwl.Infrastructure.TimeHelper.ConvertDateTime(DateTime.Now);
                            string token = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaToken"].ToString();//这个是微信与www.xftka.com之间的约定
                            string xftkaSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaSecret"].ToString(); //签名密钥
                            //应用签名=token+签名密钥
                            string signature = xftwl.Infrastructure.DESEncrypt.Encrypt(xftkaSecret, token);
                            //加密OpenId
                            OpenId = xftwl.Infrastructure.DESEncrypt.Encrypt(OpenId, token);
                            strongResponseMessage.Content = string.Format("<a href='https://www.xftka.com/weixin/login?OpenId={0}&timestamp={1}&signature={2}'>请先绑定惠民卡帐户</a>", OpenId, timestamp, signature);
                        }
                        else
                        {

                            //如果己绑定查询该用户会员卡信息
                            userInfo = userService.GetDetail(bindInfo.UserId);

                            //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。
                            int hongbao = 0;//红包
                            var balance = cardService.GetBalance(userInfo.CardNo);//卡余额
                            var redList = repackService.Search(userInfo.CardNo);
                            hongbao = redList * 10;
                            if (string.IsNullOrWhiteSpace(userInfo.CardNo))
                            {
                                strongResponseMessage.Content = "您的会员卡号没有找到";

                            }
                            else
                            {
                                strongResponseMessage.Content = string.Format("截止日期:{0}\r\n会员卡号:{1}\r\n余额为{2}\r\n红包为:{3}", DateTime.Now, userInfo.CardNo, balance, hongbao);
                            }
                        }
                        #endregion
                    }
                    break;

                case "income"://收益查询
                    {
                        #region 收益查询
                        //思路，微信生成签名去访问https://www.xftka.com
                        //未绑定用户执行用户绑定

                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;

                        //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定


                        var bindInfo = cardweixinService.Get(requestMessage.FromUserName);
                        //未绑定会员帐户
                        if (bindInfo == null)
                        {
                            //时间戳
                            string timestamp = xftwl.Infrastructure.TimeHelper.ConvertDateTime(DateTime.Now);
                            string token = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaToken"].ToString();//这个是微信与www.xftka.com之间的约定
                            string xftkaSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaSecret"].ToString(); //签名密钥
                            //应用签名=token+签名密钥
                            string signature = xftwl.Infrastructure.DESEncrypt.Encrypt(xftkaSecret, token);
                            //加密OpenId
                            OpenId = xftwl.Infrastructure.DESEncrypt.Encrypt(OpenId, token);
                            strongResponseMessage.Content = string.Format("<a href='https://www.xftka.com/weixin/login?OpenId={0}&timestamp={1}&signature={2}'>请先绑定惠民卡帐户</a>", OpenId, timestamp, signature);
                        }
                        else
                        {

                            //如果己绑定查询该用户会员卡信息
                            userInfo = userService.GetDetail(bindInfo.UserId);

                            //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。

                            var income = cardService.GetIncome(userInfo.CardNo);//总收益
                            var balance = cardService.GetBalance(userInfo.CardNo);//卡余额
                            var award = cardService.SumAward(userInfo.CardNo);//提成
                            if (string.IsNullOrWhiteSpace(userInfo.CardNo))
                            {
                                strongResponseMessage.Content = "您的会员卡号没有找到";

                            }
                            else
                            {
                                strongResponseMessage.Content = string.Format("截止日期:{0}\r\n会员卡号:{1}\r\n余额:{2}\r\n累计收益:{3}\r\n累计奖励:{4}", DateTime.Now, userInfo.CardNo, balance, income, award);
                            }
                        }
                        #endregion
                    }
                    break;
                case "award"://奖励查询
                    {
                        //思路，微信生成签名去访问https://www.xftka.com
                        //未绑定用户执行用户绑定
                        #region 奖励查询
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;

                        //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定


                        var bindInfo = cardweixinService.Get(requestMessage.FromUserName);
                        //未绑定会员帐户
                        if (bindInfo == null)
                        {
                            //时间戳
                            string timestamp = xftwl.Infrastructure.TimeHelper.ConvertDateTime(DateTime.Now);
                            string token = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaToken"].ToString();//这个是微信与www.xftka.com之间的约定
                            string xftkaSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaSecret"].ToString(); //签名密钥
                            //应用签名=token+签名密钥
                            string signature = xftwl.Infrastructure.DESEncrypt.Encrypt(xftkaSecret, token);
                            //加密OpenId
                            OpenId = xftwl.Infrastructure.DESEncrypt.Encrypt(OpenId, token);
                            strongResponseMessage.Content = string.Format("<a href='https://www.xftka.com/weixin/login?OpenId={0}&timestamp={1}&signature={2}'>请先绑定惠民卡帐户</a>", OpenId, timestamp, signature);
                        }
                        else
                        {

                            //如果己绑定查询该用户会员卡信息
                            userInfo = userService.GetDetail(bindInfo.UserId);

                            //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。

                            var income = cardService.GetIncome(userInfo.CardNo);//总收益
                            var balance = cardService.GetBalance(userInfo.CardNo);//卡余额
                            var award = cardService.SumAward(userInfo.CardNo);//提成
                            if (string.IsNullOrWhiteSpace(userInfo.CardNo))
                            {
                                strongResponseMessage.Content = "您的会员卡号没有找到";

                            }
                            else
                            {
                                var ReferralUserList = userService.GetReferralUser(userInfo.CardNo, 1, 10);
                                long rs = 0;
                                if (ReferralUserList != null)
                                {
                                    rs = ReferralUserList.TotalItems;
                                }
                                strongResponseMessage.Content = string.Format("截止日期:{0}\r\n会员卡号:{1}\r\n己推广{2}人\r\n累计奖励:{3}", DateTime.Now, userInfo.CardNo, rs, award);


                            }
                        }
                        #endregion
                    }
                    break;
                case "change"://立即充值
                    {

                        //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定

                        //验证该OpenId用户是否绑定
                        var record = cardweixinService.Get(OpenId);
                        //如果没有绑定进行用户绑定
                        if (record == null)
                        {
                            requestMessage.EventKey = "OAuth";
                        }
                        else
                        {
                            //如果己绑定查询该用户会员卡信息
                            userInfo = userService.GetDetail(record.UserId);
                        }

                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "微信支付正在申请中。";
                    }
                    break;


                case "hd"://精彩活动
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        reponseMessage = strongResponseMessage;

                        //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定


                        var bindInfo = cardweixinService.Get(requestMessage.FromUserName);
                        //未绑定会员帐户
                        if (bindInfo == null)
                        {
                            //时间戳
                            string timestamp = xftwl.Infrastructure.TimeHelper.ConvertDateTime(DateTime.Now);
                            string token = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaToken"].ToString();//这个是微信与www.xftka.com之间的约定
                            string xftkaSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaSecret"].ToString(); //签名密钥
                            //应用签名=token+签名密钥
                            string signature = xftwl.Infrastructure.DESEncrypt.Encrypt(xftkaSecret, token);
                            //加密OpenId
                            OpenId = xftwl.Infrastructure.DESEncrypt.Encrypt(OpenId, token);
                            strongResponseMessage.Articles.Add(new Article()
                            {
                                Title = "您还未绑定卡",
                                Description = "点击图片进行绑定。",
                                PicUrl = "http://wx.xftka.com/Images/xft-card.jpg",
                                Url = string.Format("https://www.xftka.com/weixin/login?OpenId={0}&timestamp={1}&signature={2}", OpenId, timestamp, signature)

                            });
                        }
                        else
                        {

                            //如果己绑定查询该用户会员卡信息
                            userInfo = userService.GetDetail(bindInfo.UserId);

                            //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。

                            var income = cardService.GetIncome(userInfo.CardNo);//总收益
                            var balance = cardService.GetBalance(userInfo.CardNo);//卡余额
                            var award = cardService.SumAward(userInfo.CardNo);//提成
                            if (string.IsNullOrWhiteSpace(userInfo.CardNo))
                            {
                                strongResponseMessage.Articles.Add(new Article()
                                {
                                    Title = "关注惠民卡，抢消费通红包",
                                    Description = "点击图片转发给朋友即可得红包。",
                                    PicUrl = "http://wx.xftka.com/Images/xft-card.jpg",
                                    Url = "http://wx.xftka.com/xft/Index"
                                });
                            }
                            else
                            {
                                strongResponseMessage.Articles.Add(new Article()
                                {
                                    Title = "关注惠民卡，抢消费通红包",
                                    Description = "点击图片转发给朋友即可得红包。",
                                    PicUrl = "http://wx.xftka.com/Images/xft-card.jpg",
                                    Url = "http://wx.xftka.com/xft/Index#" + userInfo.UserId
                                });
                            }
                        }


                    }
                    break;
                case "question"://常见问题
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "<a href='https://www.xftka.com/help/Index.html?pid=2'>常见问题</a>";
                    }
                    break;
                case "introduce"://惠民卡介绍
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        reponseMessage = strongResponseMessage;

                        //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定


                        var bindInfo = cardweixinService.Get(requestMessage.FromUserName);
                        //未绑定会员帐户
                        if (bindInfo == null)
                        {
                            //时间戳
                            string timestamp = xftwl.Infrastructure.TimeHelper.ConvertDateTime(DateTime.Now);
                            string token = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaToken"].ToString();//这个是微信与www.xftka.com之间的约定
                            string xftkaSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaSecret"].ToString(); //签名密钥
                            //应用签名=token+签名密钥
                            string signature = xftwl.Infrastructure.DESEncrypt.Encrypt(xftkaSecret, token);
                            //加密OpenId
                            OpenId = xftwl.Infrastructure.DESEncrypt.Encrypt(OpenId, token);
                            strongResponseMessage.Articles.Add(new Article()
                            {
                                Title = "您还未绑定卡",
                                Description = "点击图片进行绑定。",
                                PicUrl = "http://wx.xftka.com/Images/xft-card.jpg",
                                Url = string.Format("https://www.xftka.com/weixin/login?OpenId={0}&timestamp={1}&signature={2}", OpenId, timestamp, signature)
                            });
                        }
                        else
                        {

                            //如果己绑定查询该用户会员卡信息
                            userInfo = userService.GetDetail(bindInfo.UserId);

                            //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。

                            var income = cardService.GetIncome(userInfo.CardNo);//总收益
                            var balance = cardService.GetBalance(userInfo.CardNo);//卡余额
                            var award = cardService.SumAward(userInfo.CardNo);//提成
                            if (string.IsNullOrWhiteSpace(userInfo.CardNo))
                            {
                                strongResponseMessage.Articles.Add(new Article()
                                {
                                    Title = "三天充值七千万，小小卡片引无数市民竞“折腰”",
                                    Description = "点击图片查看消费通惠民卡详情。",
                                    PicUrl = "http://wx.xftka.com/Images/xft-card.jpg",
                                    Url = "http://wx.xftka.com/Article/Index"
                                });
                            }
                            else
                            {
                                strongResponseMessage.Articles.Add(new Article()
                                {
                                    Title = "三天充值七千万，小小卡片引无数市民竞“折腰”",
                                    Description = "点击图片查看消费通惠民卡详情。",
                                    PicUrl = "http://wx.xftka.com/Images/xft-card.jpg",
                                    Url = "http://wx.xftka.com/Article/Index?id=" + userInfo.UserId
                                });
                            }
                        }


                    }
                    break;
                case "OAuth"://OAuth授权测试
                    {
                        //var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        //strongResponseMessage.Articles.Add(new Article()
                        //{
                        //	Title = "消费通会员微信绑定",
                        //	Description = "消费通会员微信绑定"+requestMessage.FromUserName,
                        //	Url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx041c67ba2f9b8cdb&redirect_uri=http://wx.xftka.com/oauth/UserInfoCallback&response_type=code&scope=snsapi_userinfo&state=xftka#wechat_redirect",
                        //	PicUrl = "http://wx.xftka.com/Images/qrcode.jpg"
                        //});
                        //reponseMessage = strongResponseMessage;

                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "<a href='https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx041c67ba2f9b8cdb&redirect_uri=https://www.xftka.com/oauth/UserInfoCallback&response_type=code&scope=snsapi_userinfo&state=xftka#wechat_redirect'>点击此链接完成帐号绑定</a>";
                        reponseMessage = strongResponseMessage;

                    }
                    break;

                case "kf"://联系客服
                    {

                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "客服转接中，请稍侯......\r\n 客服电话：400-822-7897";//显示欢迎信息
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
                    xftwl.Infrastructure.CookieHelper.ClearCookie("TgUId");
                    xftwl.Infrastructure.CookieHelper.SetCookie("TgUId", TgUserId.ToString());

                    //获取推广用户OpenId
                    string txOpenId = "";
                    var tguser = cardweixinService.Get(TgUserId);
                    if (tguser != null)
                    {
                        txOpenId = tguser.OpenId;
                    }
                    else { log.Error("通过扫描关注后未能在xft_card_weixin表中获取到推广人的用户绑定信息"); }
                    //保存微信用户信息
                    var user = cardweixinService.Get(openId);
                    //如果不存在，直接新增 
                    if (user == null)
                    {

                        CardWeixinModel cm = new CardWeixinModel()
                        {
                            CreateDate = DateTime.Now,
                            Id = 0,
                            MediaId = "",
                            Nonce = Nonce,
                            OpenId = openId,
                            QrCode = "",
                            UserId = 0,
                            WxTgUserId = TgUserId
                        };
                        cardweixinService.Save(cm);

                        string content = string.Format("您推荐的一级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                        //推送消息给推广人
                        Custom.SendText(token, tguser.OpenId, content);

                        #region 给推广人的上级发消息
                        if (tguser.WxTgUserId > 0)
                        {
                            //上上级用户
                            var tg2 = cardweixinService.Get(tguser.WxTgUserId);

                            if (tg2 != null)
                            {
                                log.Info("上级的上级用户UserId=" + tg2.UserId + "  openid=" + tg2.OpenId);
                                string content2 = string.Format("您推荐的二级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                                Custom.SendText(token, tg2.OpenId, content2);
                                #region 给3级发消息
                                //获取上上上级用户
                                var tg3 = cardweixinService.Get(tg2.WxTgUserId);
                                if (tg3 != null)
                                {
                                    log.Info("上级的上级的上级用户UserId=" + tg3.UserId + "  openid=" + tg3.OpenId);
                                    string content3 = string.Format("您推荐的三级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                                    Custom.SendText(token, tg3.OpenId, content3);
                                }
                                #endregion
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //如果存在，查看该微信用户是否有会员卡信息，如果没有会员信息，或者推荐人为空，那么更新xft_card_weixin WxTgUserId字段，其它情况不处理
                        var userinfo = userService.GetDetail(user.UserId);
                        if (userinfo == null || string.IsNullOrEmpty(userinfo.ReferrerCardNo))
                        {
                            user.WxTgUserId = tguser.UserId;
                            cardweixinService.Save(user);
                        }
                        if (user.WxTgUserId != tguser.UserId)
                        {
                            string content = string.Format("您推荐的一级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                            //推送消息给推广人
                            Custom.SendText(token, tguser.OpenId, content);

                            #region 给推广人的上级发消息
                            if (tguser.WxTgUserId > 0)
                            {
                                var tg2 = cardweixinService.Get(tguser.WxTgUserId);
                                if (tg2 != null)
                                {
                                    string content2 = string.Format("您推荐的二级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                                    Custom.SendText(token, tg2.OpenId, content2);
                                    #region 给3级发消息
                                    //获取上上上级用户
                                    var tg3 = cardweixinService.Get(tg2.WxTgUserId);
                                    if (tg3 != null)
                                    {
                                        log.Info("上级的上级的上级用户UserId=" + tg3.UserId + "  openid=" + tg3.OpenId);
                                        string content3 = string.Format("您推荐的三级合伙人:{0}己关注了消费通平台", wxuser.nickname);
                                        Custom.SendText(token, tg3.OpenId, content3);
                                    }
                                    #endregion
                                }
                            }

                            #endregion
                        }

                        user.WxTgUserId = tguser.UserId;
                        user.Nonce = Nonce;
                        cardweixinService.Save(user);


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