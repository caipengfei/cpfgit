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
    public class VoucherController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        VoucherService service = new VoucherService();

        /// <summary>
        /// 获取某用户的某种类型的优惠券
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="Guid"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserVoucherModel> GetAlluvByUser(int page, int pagesize, string UserGuid, int typeId)
        {
            try
            {
                return service.GetAlluvByUser(page, pagesize, UserGuid, typeId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
