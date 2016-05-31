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
    /// 项目
    /// </summary>
    public class ProjectController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        StyleService styleService = new StyleService();
        ProjectService service = new ProjectService();

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
                var target = new
                {
                    logo = model.t_Project_ConverPic,
                    projectName = model.t_Project_Name,
                    projectDescription = model.t_Project_OneWord,
                    city = model.t_Project_CityName,
                    projectField = styleService.GetById(model.t_Project_Field),
                    projectStage = styleService.GetById(model.t_Project_Phase),
                    financingState = styleService.GetById(model.t_Project_FinancePhase),
                    financingMoney = model.t_Project_Finance,
                    UseOfFunds = model.t_Project_FinanceUse,
                    aboutProject = model.t_Project_Instruction,
                    partnerDemand = model.t_Project_ParterWant
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
