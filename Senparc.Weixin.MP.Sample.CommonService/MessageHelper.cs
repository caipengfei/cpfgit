using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Weixin.MP.Sample.CommonService
{
  public  class MessageHelper
    {
        public void SendMsg()
        {
            //#region 扫码后的操作
            //try
            //{

            //    //扫码关注后，得到扫码人的微信会员信息
            //    string appId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"].ToString();
            //    string openId = requestMessage.FromUserName;
            //    log.Info("订阅公众号openid=" + openId);
            //    string token = CommonAPIs.AccessTokenContainer.GetToken(appId);
            //    var wxuser = CommonAPIs.CommonApi.GetUserInfo(token, openId);
            //    //推广人ID
            //    int TgUserId = Convert.ToInt32(requestMessage.EventKey);
            //    //获取推广用户OpenId
            //    string txOpenId = "";
            //    var tguser = cardweixinService.Get(TgUserId);
            //    if (tguser != null)
            //    {
            //        txOpenId = tguser.OpenId;
            //    }
            //    else { log.Error("通过扫描关注后未能在xft_card_weixin表中获取到推广人的用户绑定信息"); }
            //    //保存微信用户信息
            //    var user = cardweixinService.Get(openId);
            //    //如果不存在，直接新增 
            //    if (user == null)
            //    {
            //        CardWeixinModel cm = new CardWeixinModel()
            //        {
            //            CreateDate = DateTime.Now,
            //            Id = 0,
            //            MediaId = "",
            //            Nonce = "",
            //            OpenId = openId,
            //            QrCode = "",
            //            UserId = 0,
            //            WxTgUserId = tguser.UserId
            //        };
            //        cardweixinService.Save(cm);

            //        string content = string.Format("您推荐的一级合伙人:{0}己加入了消费通平台", wxuser.nickname);
            //        //推送消息给推广人
            //        Custom.SendText(token, tguser.OpenId, content);

            //        #region 给推广人的上级发消息
            //        if (tguser.WxTgUserId > 0)
            //        {
            //            var tg2 = cardweixinService.Get(tguser.WxTgUserId);
            //            if (tg2 != null)
            //            {
            //                string content2 = string.Format("您推荐的二级合伙人:{0}己加入了消费通平台", wxuser.nickname);
            //                Custom.SendText(token, tg2.OpenId, content2);
            //            }
            //        }

            //        #endregion
            //    }
            //    else
            //    {
            //        //如果存在，查看该微信用户是否有会员卡信息，如果没有会员信息，或者推荐人为空，那么更新xft_card_weixin WxTgUserId字段，其它情况不处理
            //        //var userinfo = userService.GetDetail(user.UserId);
            //        //if (userinfo == null || string.IsNullOrEmpty(userinfo.ReferrerCardNo))
            //        //{
            //        //    user.WxTgUserId = tguser.UserId;
            //        //    cardweixinService.Save(user);
            //        //}
            //        if (user.WxTgUserId != tguser.UserId)
            //        {
            //            string content = string.Format("您推荐的一级合伙人:{0}己加入了消费通平台", wxuser.nickname);
            //            //推送消息给推广人
            //            Custom.SendText(token, tguser.OpenId, content);

            //            #region 给推广人的上级发消息
            //            if (tguser.WxTgUserId > 0)
            //            {
            //                var tg2 = cardweixinService.Get(tguser.WxTgUserId);
            //                if (tg2 != null)
            //                {
            //                    string content2 = string.Format("您推荐的二级合伙人:{0}己加入了消费通平台", wxuser.nickname);
            //                    Custom.SendText(token, tg2.OpenId, content2);
            //                }
            //            }

            //            #endregion
            //        }

            //        user.WxTgUserId = tguser.UserId;
            //        cardweixinService.Save(user);


            //    }


            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex.Message);

            //}

            //#endregion
        }
    }
}
