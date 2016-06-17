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


        public PetaPoco.Page<ActivityModel> GetListFroWX(int page, int pagesize, string CityName, int days, string payType)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select [Guid],[t_User_Guid],[t_Activity_Title],[t_Activity_CoverPic],[t_Activity_sDate],t_Activity_eDate,[t_Activity_CityName] from [T_Activity] where t_DelState=0 and [t_Activity_Audit]=1 ");
                if (!string.IsNullOrWhiteSpace(CityName))
                {
                    sql.Append("and t_Activity_CityName='" + CityName + "'");
                }
                if (days > 0)
                {
                    sql.Append("and DATEDIFF(day,t_Activity_sDate,getDate()) between 0 and " + days + " ");
                }
                if (!string.IsNullOrWhiteSpace(payType))
                {
                    sql.Append("and t_Activity_FeeType='" + payType + "'");
                }
                sql.Append("order by t_Activity_Recommand,t_Activity_sDate desc");
                log.Info("获取活动列表的Sql：" + sql);
                return rp.GetPageData(page, pagesize, sql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取所有活动，按照推荐和开始日期排序
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<ActivityModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_Activity where t_DelState=0 order by t_Activity_Recommand,t_Activity_sDate desc";
                return rp.GetPageData(page, pagesize, sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
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
