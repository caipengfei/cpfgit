using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户工作经历资源层
    /// </summary>
    public class HistoryWorkRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<HistoryWorkModel> rp = new Repository<HistoryWorkModel>();


        /// <summary>
        /// 获取某人的工作经历
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public IEnumerable<HistoryWorkModel> GetByUser(string UserGuid)
        {
            try
            {
                string sql = "select * from T_HistoryWork where t_User_Guid=@0 and t_DelState=0";
                return rp.GetAll(sql, new object[] { UserGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
