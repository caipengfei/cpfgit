using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 用户工作经历业务层
    /// </summary>
    public class HistoryWorkService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        HistoryWorkRepository rp = new HistoryWorkRepository();

        /// <summary>
        /// 获取某人的工作经历
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public IEnumerable<HistoryWorkModel> GetByUser(string UserGuid)
        {
            try
            {
                return rp.GetByUser(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
