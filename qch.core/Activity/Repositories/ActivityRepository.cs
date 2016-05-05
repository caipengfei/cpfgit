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
    public class ActivityRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<ActivityModel> rp = new Repository<ActivityModel>();

        /// <summary>
        /// 分页获取某人发布的所有未删除活动
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public PetaPoco.Page<ActivityModel> GetAll(int page, int pagesize, string Guid)
        {
            try
            {
                string sql = "select * from T_Activity where t_User_Guid=@0 and t_DelState=0";
                return rp.GetPageData(page, pagesize, sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某人的所有未删除活动
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<ActivityModel> GetAll(string Guid)
        {
            try
            {
                string sql = "select * from T_Activity where t_User_Guid=@0 and t_DelState=0";
                return rp.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public ActivityModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_Activity where Guid=@0";
                return rp.Get(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 添加评论信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(ActivityModel model)
        {
            try
            {
                var db = rp.Insert(model);
                if (db != null)
                    return true;
                else
                    return false;
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
        public bool Edit(ActivityModel model)
        {
            try
            {
                var db = rp.Update(model);
                if (db != null)
                    return true;
                else
                    return false;
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
        public bool Del(ActivityModel model)
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
