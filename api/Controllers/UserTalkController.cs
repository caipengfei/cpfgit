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
    public class UserTalkController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserTalkService service = new UserTalkService();


        public object GetAll(int page, int pagesize, string Guid)
        {
            try
            {
                return service.GetAll(page, pagesize, Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<UserTalkModel> GetAll(string Guid)
        {
            try
            {
                return service.GetAll(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public UserTalkModel GetById(string Guid)
        {
            try
            {
                return service.GetById(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public Msg Save(UserTalkModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "保存失败";
            try
            {
                if (model == null)
                    return msg;
                if (string.IsNullOrWhiteSpace(model.Guid))
                    model.Guid = Guid.NewGuid().ToString();
                model.t_Talk_FromDate = DateTime.Now;
                model.t_Talk_ToDate = DateTime.Now;
                if (service.Save(model))
                {
                    msg.type = "success";
                    msg.Data = "保存成功";
                }
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
    }
}
