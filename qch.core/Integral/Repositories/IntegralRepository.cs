using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 积分资源层
    /// </summary>
    public class IntegralRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<IntegralModel> rp = new Repository<IntegralModel>();

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
                int xy = 0;
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select sum(t_UserIntegral_Reward) from T_User_Integral where t_User_Guid=@0 and t_DelState=0";
                    xy = Convert.IsDBNull(db.ExecuteScalar<object>(sql, new object[] { UserGuid })) ? 0 : db.ExecuteScalar<int>(sql, new object[] { UserGuid });
                }
                return xy;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 获取某用户的某项积分总额
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public int GetIntegral(string UserGuid, string Pinyin)
        {
            try
            {
                int xy = 0;
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select sum(t_UserIntegral_Reward) from T_User_Integral where t_User_Guid=@0 and t_IntegralManager_PinYin=@1 and t_DelState=0";
                    xy = Convert.IsDBNull(db.ExecuteScalar<object>(sql, new object[] { UserGuid, Pinyin })) ? 0 : db.ExecuteScalar<int>(sql, new object[] { UserGuid, Pinyin });
                }
                return xy;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 分页获取所有
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<IntegralModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_User_Integral where t_DelState=0 order by t_adddate desc";
                return rp.GetPageData(page, pagesize, sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// getbyid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IntegralModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_User_Integral where guid=@0";
                return rp.Get(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(IntegralModel model)
        {
            try
            {
                return rp.Insert(model) == null ? false : true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(IntegralModel model)
        {
            try
            {
                return (int)rp.Update(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Del(IntegralModel model)
        {
            try
            {
                return (int)rp.Delete(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
    }
}
