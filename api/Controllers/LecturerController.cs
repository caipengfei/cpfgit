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
    /// 导师
    /// </summary>
    public class LecturerController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LecturerService service = new LecturerService();
        string ImageUrl = System.Configuration.ConfigurationManager.AppSettings["ImageUrl"].ToString();
        /// <summary>
        /// 获取导师详情
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        [HttpGet]
        public LecturerModel GetById(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                var model = service.GetById(Guid);
                if (model != null)
                {
                    model.T_Lecturer_Pic = ImageUrl + model.T_Lecturer_Pic;
                }
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        } 
    }
}
