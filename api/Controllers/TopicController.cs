using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        StyleService styleService = new StyleService();



        /// <summary>
        /// 积攒列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public PetaPoco.Page<SelectTopic> GetPariseList(int page, int pagesize)
        {
            try
            {
                return service.GetPariseList(page, pagesize);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public object GetPariseList()
        {
            try
            {
                return service.GetPariseList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某个用户的所有动态
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object MyTopics(int page, int pagesize, string userguid, string sign, string loginuser)
        {
            try
            {
                string s = qch.Infrastructure.DESEncrypt.MD5Encrypt(userguid + "150919", true);
                if (s != sign)
                {
                    return "签名错误";
                }
                var model = service.MyTopics(page, pagesize, userguid);
                if (model != null && model.Items != null)
                {
                    //model.Context = userService.GetUserStyleAudit(userguid);
                    foreach (var item in model.Items)
                    {
                        var pic = picService.GetByGuid(item.Guid);
                        if (pic != null && pic.Count() == 1)
                        {
                            foreach (var tt in pic)
                            {
                                string path = "D:\\QCH2.0\\Attach\\Images\\" + tt.t_Pic_Url;
                                if (!Directory.Exists(path))
                                {
                                    log.Info(path);
                                    System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                                    if (image != null)
                                    {
                                        tt.Height = image.Height;
                                        tt.Width = image.Width;
                                    }
                                    image.Dispose();
                                }
                            }
                        }
                        item.UserPraise = praiseService.GetAllByTopicGuid2(item.Guid);
                        item.Pics = pic;
                        item.t_User_Intention_Text = styleService.GetByIds(item.t_User_Intention);
                        item.t_User_NowNeed_Text = styleService.GetByIds(item.t_User_NowNeed);
                        item.t_User_Best_Text = styleService.GetByIds(item.t_User_Best);
                        item.t_User_Position_Text = styleService.GetByIds(item.t_User_Position);
                        item.talk = talkService.GetForTopic(1, 3, item.Guid).Items;
                        item.IfPraise = praiseService.IsPraise(loginuser, item.Guid);
                        item.TalkCount = talkService.GetCount(item.Guid);
                    }
                    return model;
                }
                return new PetaPoco.Page<TopicsModel>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new PetaPoco.Page<TopicsModel>();
            }
        }
        [HttpGet]
        public object MyTopics(int page, int pagesize, string userguid, string sign)
        {
            try
            {
                string s = qch.Infrastructure.DESEncrypt.MD5Encrypt(userguid + "150919", true);
                if (s != sign)
                {
                    return "签名错误";
                }
                var model = service.MyTopics(page, pagesize, userguid);
                if (model != null && model.Items != null)
                {
                    //model.Context = userService.GetUserStyleAudit(userguid);
                    foreach (var item in model.Items)
                    {
                        var pic = picService.GetByGuid(item.Guid);
                        if (pic != null && pic.Count() == 1)
                        {
                            foreach (var tt in pic)
                            {
                                string path = "D:\\QCH2.0\\Attach\\Images\\" + tt.t_Pic_Url;
                                if (!Directory.Exists(path))
                                {
                                    log.Info(path);
                                    System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                                    if (image != null)
                                    {
                                        tt.Height = image.Height;
                                        tt.Width = image.Width;
                                    }
                                    image.Dispose();
                                }
                            }
                        }
                        item.UserPraise = praiseService.GetAllByTopicGuid2(item.Guid);
                        item.Pics = pic;
                        item.t_User_Intention_Text = styleService.GetByIds(item.t_User_Intention);
                        item.t_User_NowNeed_Text = styleService.GetByIds(item.t_User_NowNeed);
                        item.t_User_Best_Text = styleService.GetByIds(item.t_User_Best);
                        item.t_User_Position_Text = styleService.GetByIds(item.t_User_Position);
                        item.talk = talkService.GetForTopic(1, 3, item.Guid).Items;
                        item.IfPraise = praiseService.IsPraise(userguid, item.Guid);
                        item.TalkCount = talkService.GetCount(item.Guid);
                    }
                    return model;
                }
                return new PetaPoco.Page<TopicsModel>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new PetaPoco.Page<TopicsModel>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object Topics(int page, int pagesize, string userguid, string sign, string CityName, int IsFocus)
        {
            try
            {
                /*
                 * 动态发布时间、发布者头像、名称、职位、公司、擅长、需要、标签、图片、点赞人姓名、
                 * 当前用户是否点赞该条动态、
                 */
                string s = qch.Infrastructure.DESEncrypt.MD5Encrypt(userguid + "150919", true);
                if (s != sign)
                {
                    return "签名错误";
                }
                var model = service.GetTopics(page, pagesize, userguid, CityName, IsFocus);
                if (model != null && model.Items != null)
                {
                    //model.Context = userService.GetUserStyleAudit(userguid);
                    foreach (var item in model.Items)
                    {
                        var pic = picService.GetByGuid(item.Guid);
                        if (pic != null && pic.Count() == 1)
                        {
                            foreach (var tt in pic)
                            {
                                string path = "D:\\QCH2.0\\Attach\\Images\\" + tt.t_Pic_Url;
                                if (!Directory.Exists(path))
                                {
                                    System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                                    if (image != null)
                                    {
                                        tt.Height = image.Height;
                                        tt.Width = image.Width;
                                    }
                                    image.Dispose();
                                }
                            }
                        }
                        item.UserPraise = praiseService.GetAllByTopicGuid2(item.Guid);
                        item.Pics = pic;
                        item.t_User_Intention_Text = styleService.GetByIds(item.t_User_Intention);
                        item.t_User_NowNeed_Text = styleService.GetByIds(item.t_User_NowNeed);
                        item.t_User_Best_Text = styleService.GetByIds(item.t_User_Best);
                        item.t_User_Position_Text = styleService.GetByIds(item.t_User_Position);
                        if (IsFocus == 1)
                            item.talk = talkService.GetForTopic(1, 50, item.Guid).Items;
                        else
                            item.talk = talkService.GetForTopic(1, 3, item.Guid).Items;
                        item.IfPraise = praiseService.IsPraise(userguid, item.Guid);
                        item.TalkCount = talkService.GetCount(item.Guid);
                    }
                    return model;
                }
                return new PetaPoco.Page<TopicsModel>();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new PetaPoco.Page<TopicsModel>();
            }
        }
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
                        t_User_Best = styleService.GetByIds(user.t_User_Best),  //我最擅长
                        t_User_NowNeed = styleService.GetByIds(user.t_User_NowNeed),  //现阶段需求
                        t_User_Position = styleService.GetByIds(user.t_User_Position), //职位
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
        [HttpGet]
        public object TopicInfo(string Guid, string UserGuid, string Sign)
        {
            try
            {
                string s = qch.Infrastructure.DESEncrypt.MD5Encrypt(UserGuid + "150919", true);
                if (s != Sign)
                {
                    //model.CodeMsg = "4002";
                    return "签名错误";
                }
                var model = service.GetTopicsModel(Guid);
                if (model != null)
                {
                    model.CodeMsg = "0000";
                    var pic = picService.GetByGuid(model.Guid);
                    if (pic != null && pic.Count() == 1)
                    {
                        foreach (var tt in pic)
                        {
                            string path = "D:\\QCH2.0\\Attach\\Images\\" + tt.t_Pic_Url;
                            if (!Directory.Exists(path))
                            {
                                log.Info(path);
                                System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                                if (image != null)
                                {
                                    tt.Height = image.Height;
                                    tt.Width = image.Width;
                                }
                                image.Dispose();
                            }
                        }
                    }
                    model.UserPraise = praiseService.GetAllByTopicGuid(model.Guid);
                    model.Pics = pic;
                    model.t_User_Intention_Text = styleService.GetByIds(model.t_User_Intention);
                    model.t_User_NowNeed_Text = styleService.GetByIds(model.t_User_NowNeed);
                    model.t_User_Best_Text = styleService.GetByIds(model.t_User_Best);
                    model.t_User_Position_Text = styleService.GetByIds(model.t_User_Position);
                    model.talk = talkService.GetForTopic2(1, 50, model.Guid).Items;
                    model.IfPraise = praiseService.IsPraise(UserGuid, model.Guid);
                    model.TalkCount = talkService.GetCount(model.Guid);
                }
                else
                {
                    model = new TopicsModel();
                    model.CodeMsg = "4001";
                }
                return model;
                //获取评论
                //var talk = talkService.GetForTopic2(1, 30, Guid).Items;
                ////用户信息
                //var user = userService.GetById(model.t_User_Guid);
                //if (user == null)
                //    return null;
                ////点赞信息
                //IEnumerable<UserPraise> praise = null;
                //praise = praiseService.GetAllByTopicGuid(model.Guid);
                //if (praise == null)
                //    praise = new List<UserPraise>();
                ////点赞人数
                //int xy = 0;
                //if (praise != null)
                //    xy = praise.Count();
                ////动态关联的图片
                //var pic = picService.GetByGuid(model.Guid);
                ////封装返回对象
                //var target = new
                //{
                //    t_User_Pic = user.t_User_Pic, //用户头像
                //    t_User_RealName = user.t_User_RealName, //真实姓名
                //    UserTypeText = user.UserTypeText, //用户类型 
                //    company = user.t_User_Commpany, //所在公司
                //    t_User_Best = styleService.GetByIds(user.t_User_Best),  //我最擅长
                //    t_User_NowNeed = styleService.GetByIds(user.t_User_NowNeed),  //现阶段需求
                //    t_User_Position = styleService.GetByIds(user.t_User_Position), //职位
                //    t_Topic_Contents = model.t_Topic_Contents,
                //    t_Topic_City = model.t_Topic_City,
                //    t_Date = model.t_Date,
                //    userTalk = talk,
                //    avatarList = praise,
                //    counts = xy,
                //    images = pic
                //};
                //return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                var model = new TopicsModel();
                model.CodeMsg = "4001";
                return model;
            }
        }
    }
}
