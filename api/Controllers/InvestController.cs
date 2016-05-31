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
                    logo = model.t_InvestPlace_ConverPic,
                    name = model.t_InvestPlace_Title,
                    count = xy,
                    memberCount = xy2,
                    stage = styleService.GetByIds(model.t_InvestPlace_Phase),
                    field = styleService.GetByIds(model.t_InvestPlace_Area),
                    money = model.t_InvestPlace_Money,
                    description = model.t_InvestPlace_Instruction,
                    caseList = case1,
                    memberList = member
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
