using qch.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    /// <summary>
    /// 投资相关接口
    /// </summary>
    public class InvestController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserService userService = new UserService();
        InvestService service = new InvestService();
        StyleService styleService = new StyleService();

        /// <summary>
        /// 投资机构详情
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetInvestPlace(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                var model = service.GetPlaceById(Guid);
                if (model == null)
                    return null;
                //接收项目
                int xy = 0;
                var case1 = service.GetPlaceCase(model.Guid);
                if (case1 != null)
                    xy = case1.Count();
                //入驻成员
                int xy2 = 0;
                var member = service.GetPlaceMember(model.Guid);
                if (member != null)
                    xy2 = member.Count();
                var target = new
                {
                    t_InvestPlace_ConverPic = model.t_InvestPlace_ConverPic,  //logo
                    t_InvestPlace_Title = model.t_InvestPlace_Title,  //标题
                    count = xy,//接收项目
                    memberCount = xy2,//入驻成员
                    t_InvestPlace_Phase = styleService.GetByIds(model.t_InvestPlace_Phase),//投资阶段
                    t_InvestPlace_Area = styleService.GetByIds(model.t_InvestPlace_Area),//投资领域
                    t_InvestPlace_Money = model.t_InvestPlace_Money,//投资金额
                    t_InvestPlace_Instruction = model.t_InvestPlace_Instruction,//详细介绍
                    caseList = case1,//接收项目数量
                    memberList = member//入驻成员数量
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
