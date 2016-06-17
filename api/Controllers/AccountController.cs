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
    public class AccountController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AccountService service = new AccountService();

        /// <summary>
        /// 获取某人的所有创业币流水
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
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
    }
}
