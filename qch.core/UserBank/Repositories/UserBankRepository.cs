using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class UserBankRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<UserBankModel> rp = new Repository<UserBankModel>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public UserBankModel Get(string UserGuid)
        {
            try
            {
                string sql = "select * from t_user_bank where t_DelState=0 and t_User_Guid=@0";
                return rp.Get(sql, new object[] { UserGuid });
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
        public int GetCertification1(string UserGuid, DateTime b, DateTime e)
        {
            try
            {
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    var tuijian = db.ExecuteScalar<object>("select count(1) from t_user_bank where t_DelState=0 and t_User_Guid in (select guid from t_users where t_ReommUser=@0 and t_DelState=0) and t_AddDate between @1 and @2", new object[] { UserGuid, b, e });
                    if (tuijian != null)
                    {
                        return Convert.ToInt32(tuijian);
                    }
                    else
                        return 0;
                }
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
        public int GetCertification2(string UserGuid, DateTime b, DateTime e)
        {
            try
            {
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    var tuijian = db.ExecuteScalar<object>("select count(1) from t_user_bank where t_DelState=0 and t_User_Guid in (select guid from t_users where t_ReommUser in (select Guid from t_users where t_ReommUser=@0 and t_DelState=0) and t_DelState=0) and t_AddDate between @1 and @2", new object[] { UserGuid, b, e });
                    if (tuijian != null)
                    {
                        return Convert.ToInt32(tuijian);
                    }
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
    }
}
