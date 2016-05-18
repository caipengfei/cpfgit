using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    /// <summary>
    /// 动态
    /// </summary>
    public class TopicController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserTalkService talkService = new UserTalkService();
        UserService userService = new UserService();
        TopicService service = new TopicService();
        PraiseService praiseService = new PraiseService();
        PicService picService = new PicService();

        /// <summary>
        /// 动态详情
        /// </summary>
        /// <param name="Guid">分享过来的动态Guid</param>
        /// <returns></returns>
        [HttpGet]
        public object TopicInfo(string Guid)
        {
            try
            {
                var model = service.GetById(Guid);
                if (model != null)
                {
                    //获取评论
                    var talk = talkService.GetAll(1, 999, model.Guid);
                    //用户信息
                    var user = userService.GetById(model.t_User_Guid);
                    if (user == null)
                        return null;
                    //点赞信息
                    IEnumerable<UserPraise> praise = null;
                    praise = praiseService.GetAllByTopicGuid(model.Guid);
                    if (praise == null)
                        praise = new List<UserPraise>();
                    //点赞人数
                    int xy = 0;
                    if (praise != null)
                        xy = praise.Count();
                    //动态关联的图片
                    var pic = picService.GetByGuid(model.Guid);
                    //封装返回对象
                    var target = new
                    {
                        userAvatar = user.t_User_Pic,
                        username = user.t_User_RealName,
                        usertype = user.UserTypeText,
                        company = user.t_User_Commpany,
                        dynamicText = model.t_Topic_Contents,
                        currentPosition = model.t_Topic_City,
                        date = model.t_Date,
                        userTalk = talk,
                        avatarList = praise,
                        counts = xy,
                        dynamicThumbnailList = pic
                    };
                    return target;
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
