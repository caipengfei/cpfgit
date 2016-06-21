using qch.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class IntegralController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IntegralService servicer = new IntegralService();
        /// <summary>
        /// 获取某用户的积分总额
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public int GetIntegral(string UserGuid)
        {
            try
            {
                return servicer.GetIntegral(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 分页获取某用户的积分记录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public object GetAll(int page, int pagesize, string UserGuid, int typeId)
        {
            try
            {
                return servicer.GetAll(page, pagesize, UserGuid, typeId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
