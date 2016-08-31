using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 
    /// </summary>
    public class UserBankService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserBankRepository rp = new UserBankRepository();

        public object Get(string UserGuid)
        {
            try
            {
                return rp.Get(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某人所有直推的用户中实名认证的数量
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public int GetCertification1(string UserGuid, DateTime? b, DateTime? e)
        {
            try
            {
                DateTime begin = b == null ? Convert.ToDateTime("1990-01-01") : qch.Infrastructure.TimeHelper.GetStartDateTime(Convert.ToDateTime(b));
                DateTime end = e == null ? DateTime.Now : qch.Infrastructure.TimeHelper.GetEndDateTime(Convert.ToDateTime(e));
                return rp.GetCertification1(UserGuid, begin, end);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 获取某人所有间推的用户中实名认证的数量
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public int GetCertification2(string UserGuid, DateTime? b, DateTime? e)
        {
            try
            {
                DateTime begin = b == null ? Convert.ToDateTime("1990-01-01") : qch.Infrastructure.TimeHelper.GetStartDateTime(Convert.ToDateTime(b));
                DateTime end = e == null ? DateTime.Now : qch.Infrastructure.TimeHelper.GetEndDateTime(Convert.ToDateTime(e));
                return rp.GetCertification2(UserGuid, begin, end);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
    }
}
