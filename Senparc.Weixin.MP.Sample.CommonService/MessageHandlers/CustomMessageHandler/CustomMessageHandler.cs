using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Configuration;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        /*
         * 重要提示：v1.5起，MessageHandler提供了一个DefaultResponseMessage的抽象方法，
         * DefaultResponseMessage必须在子类中重写，用于返回没有处理过的消息类型（也可以用于默认消息，如帮助信息等）；
         * 其中所有原OnXX的抽象方法已经都改为虚方法，可以不必每个都重写。若不重写，默认返回DefaultResponseMessage方法中的结果。
         */


#if DEBUG
        string agentUrl = "http://localhost:12222/App/Weixin/4";
        string agentToken = "27C455F496044A87";
        string wiweihiKey = "CNadjJuWzyX5bz5Gn+/XoyqiqMa5DjXQ";
#else
        //下面的Url和Token可以用其他平台的消息，或者到www.weiweihi.com注册微信用户，将自动在“微信营销工具”下得到
        private string agentUrl = WebConfigurationManager.AppSettings["WeixinAgentUrl"];//这里使用了www.weiweihi.com微信自动托管平台
        private string agentToken = WebConfigurationManager.AppSettings["WeixinAgentToken"];//Token
        private string wiweihiKey = WebConfigurationManager.AppSettings["WeixinAgentWeiweihiKey"];//WeiweihiKey专门用于对接www.Weiweihi.com平台，获取方式见：http://www.weiweihi.com/ApiDocuments/Item/25#51
#endif

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            WeixinContext.ExpireMinutes = 3;
        }

        public override void OnExecuting()
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            //创建文字响应内容
            var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageText>();
            if (requestMessage.Content.Substring(0, 4) == "jcbd")
            {
                string OpenId = requestMessage.FromUserName;
                if (wxservice.DelByOpenId(OpenId))
                    responseMessage.Content = "解除绑定成功！";
                else
                    responseMessage.Content = "解除绑定失败！";
                return responseMessage;
            }

            if (requestMessage.Content.ToLower() == "t")
            {
                #region 我的推广二维码

                //获取用户OpenId，如果用户没有绑定会员卡，提示登录并绑定
                string OpenId = requestMessage.FromUserName;

                var bindInfo = wxservice.GetByOpenId(OpenId);
                //未绑定会员帐户
                if (bindInfo == null)
                {
                    //时间戳
                    //string timestamp = xftwl.Infrastructure.TimeHelper.ConvertDateTime(DateTime.Now);
                    //string token = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaToken"].ToString();//这个是微信与www.xftka.com之间的约定
                    //string xftkaSecret = System.Web.Configuration.WebConfigurationManager.AppSettings["xftkaSecret"].ToString(); //签名密钥
                    //                                                                                                             //应用签名=token+签名密钥
                    //string signature = xftwl.Infrastructure.DESEncrypt.Encrypt(xftkaSecret, token);
                    ////加密OpenId
                    //OpenId = xftwl.Infrastructure.DESEncrypt.Encrypt(OpenId, token);
                    //responseMessage.Content = string.Format("<a href='https://www.xftka.com/weixin/login?OpenId={0}&timestamp={1}&signature={2}'>请先绑定惠民卡帐户</a>", OpenId, timestamp, signature);













                    return responseMessage;
                }
                else
                {

                    string appId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
                    string apptoken = CommonAPIs.AccessTokenContainer.GetToken(appId);

                    string openId = requestMessage.FromUserName;
                    var wxuser = CommonAPIs.CommonApi.GetUserInfo(apptoken, openId);
                    //微信用户的头像
                    var wxavator = wxuser.headimgurl;
                    //微信用户的昵称
                    var wxname = wxuser.nickname;
                    //推广用户的UserId
                    int userId = 0;
                    //发送创建临时二维码ticket的请求
                    var qrcode = QrCode.Create(apptoken, 604800, userId);
                    if (qrcode != null)
                    {
                        //新建一个目标文件，如果文件已存在会抛错
                        //FileStream outStream = new FileStream(DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + userId + ".jpg", FileMode.CreateNew);
                        //QrCode.ShowQrCode(qrcode.ticket, outStream);
                        //换取二维码
                        var code = QrCode.GetShowQrCodeUrl(qrcode.ticket);
                        //复制背景图到指定文件夹
                        if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/content/images/code")))
                        {
                            // 目录不存在，建立目录
                            System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/content/images/code"));
                        }
                        String sourcePath = System.Web.HttpContext.Current.Server.MapPath("~/content/images/bg.jpg");
                        String targetPath = System.Web.HttpContext.Current.Server.MapPath("~/content/images/code" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + userId + ".jpg");
                        bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                        System.IO.File.Copy(sourcePath, targetPath, isrewrite);
                        //贴微信昵称
                        ImageHelper.LetterWatermark(targetPath, 12, "我是:" + wxname, System.Drawing.Color.Black, "WxUserName");
                        //贴二维码过期时间
                        ImageHelper.LetterWatermark(targetPath, 12, "二维码将在" + DateTime.Now.AddSeconds(604795).ToString("yy-MM-dd hh:mm:ss") + "后过期", System.Drawing.Color.Black, "WxExpiryDate");
                        //贴二维码
                        ImageHelper.ImageWatermark(targetPath, code, "WxQrCode");
                        //贴微信头像
                        ImageHelper.ImageWatermark(targetPath, wxavator, "WxUserLogo");

                        responseMessage.Content = string.Format("<img src='{0}' />", targetPath);
                        return responseMessage;
                    }
                    else
                    {
                        responseMessage.Content = "生成二维码失败";
                        return responseMessage;
                    }
                }
                #endregion
            }





            //responseMessage.Content="你的OpenId是："+requestMessage.FromUserName
            //	+" \r\n您发送了文字信息"+requestMessage.Content;
            // responseMessage.Content = string.Format("欢迎访问消费通会员中心");
            return responseMessage;


            //方法一（v0.1），此方法调用太过繁琐，已过时（但仍是所有方法的核心基础），建议使用方法二到四
            //var responseMessage =
            //    ResponseMessageBase.CreateFromRequestMessage(RequestMessage, ResponseMsgType.Text) as
            //    ResponseMessageText;

            //方法二（v0.4）
            //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(RequestMessage);

            //方法三（v0.4），扩展方法，需要using Senparc.Weixin.MP.Helpers;
            //var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageText>();

            //方法四（v0.6+），仅适合在HandlerMessage内部使用，本质上是对方法三的封装
            //注意：下面泛型ResponseMessageText即返回给客户端的类型，可以根据自己的需要填写ResponseMessageNews等不同类型。
            //var responseMessage = base.CreateResponseMessage<ResponseMessageText>();


        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var locationService = new LocationService();
            var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
            return responseMessage;
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = "您刚才发送了图片信息",
                Description = "您发送的图片将会显示在边上",
                PicUrl = requestMessage.PicUrl,
                Url = "http://www.cn-qch.com/app"
            });
            responseMessage.Articles.Add(new Article()
            {
                Title = "第二条",
                Description = "第二条带连接的内容",
                PicUrl = requestMessage.PicUrl,
                Url = "http://www.cn-qch.com/app"
            });
            return responseMessage;
        }

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
            responseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
            responseMessage.Music.Title = "这里是一条音乐消息";
            responseMessage.Music.Description = "来自Jeffrey Su的美妙歌声~~";
            responseMessage.Music.ThumbMediaId = "mediaid";
            return responseMessage;
        }

        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您发送了一条视频信息，ID：" + requestMessage.MediaId;
            return responseMessage;
        }

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = string.Format(@"您发送了一条连接信息：
Title：{0}
Description:{1}
Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
            return responseMessage;
        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
            //TODO: 对Event信息进行统一操作
            return eventResponseMessage;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
             * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
             * 只需要在这里统一发出委托请求，如：
             * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
             * return responseMessage;
             */

            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }
    }
}
