using qch.core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace api.Controllers
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        StyleService styleService = new StyleService();
        ProjectService service = new ProjectService();
        UserTalkService usertalkService = new UserTalkService();
        PicService picService = new PicService();


        /// <summary>
        /// 获取项目团队
        /// </summary>
        /// <param name="ProjectGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetTeam(string ProjectGuid)
        {
            try
            {
                return service.GetTeam(ProjectGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetProject(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                var model = service.GetById(Guid);
                if (model == null)
                    return null;
                //获取评论
                var talk = new List<qch.Models.SelectTalkModel>();
                var usertalk = usertalkService.GetAll(1, 10, model.Guid);
                if (usertalk != null && usertalk.Items != null)
                    talk = usertalk.Items;
                var target = new
                {
                    logo = model.t_Project_ConverPic,  //封面图
                    t_Project_Name = model.t_Project_Name, //项目名称
                    t_Project_OneWord = model.t_Project_OneWord, //一句话说明
                    t_Project_CityName = model.t_Project_CityName, //城市名称
                    t_Project_Field = styleService.GetById(model.t_Project_Field ?? 0), //项目领域
                    t_Project_Phase = styleService.GetById(model.t_Project_Phase ?? 0), //项目阶段
                    t_Project_FinancePhase = styleService.GetById(model.t_Project_FinancePhase ?? 0), //融资阶段
                    t_Project_Finance = model.t_Project_Finance,  //融资金额
                    t_Project_FinanceUse = model.t_Project_FinanceUse,  //融资金额用途
                    t_Project_Instruction = model.t_Project_Instruction, //项目介绍
                    t_Project_Perfer = model.t_Project_Perfer,  //项目优势
                    t_Project_Client = model.t_Project_Client,  //目标用户
                    t_Project_ProfitWay = model.t_Project_ProfitWay, //盈利途径
                    t_Project_Website = model.t_Project_Website,//官网
                    t_Project_Weixin = model.t_Project_Weixin,//微信链接
                    t_Project_Link = model.t_Project_Link,//app链接
                    t_Project_ParterWant = styleService.GetByIds(model.t_Project_ParterWant), //合伙人需求
                    team = service.GetTeam(model.Guid),//核心团队
                    images = picService.GetByGuid(model.Guid),  //关联图片
                    userTalk = talk  //最新评论
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
