using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 动态点赞的资源层
    /// </summary>
    public class PraiseRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<PraiseModels> rp = new Repository<PraiseModels>();
        Repository<UserPraise> rp1 = new Repository<UserPraise>();

        /// <summary>
        /// 查询某用户是否点赞某条动态
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="TopicGuid"></param>
        /// <returns></returns>
        public bool IsPraise(string UserGuid, string TopicGuid)
        {
            try
            {
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select count(1) from T_Praise where t_DelState=0 and t_User_Guid=@0 and t_Associate_Guid=@1";
                    var m = db.ExecuteScalar<object>(sql, new object[] { UserGuid, TopicGuid });
                    if (m != null)
                    {
                        if (Convert.ToInt32(m) > 0)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 获取点赞用户信息
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<UserPraise> GetAllByTopicGuid(string Guid)
        {
            try
            {
                string sql = "select a.guid as Guid,a.t_Date,b.t_User_Pic as UserAvator,b.Guid as t_User_Guid,b.t_User_RealName as UserName,b.t_User_Style,b.t_UserStyleAudit from T_Praise as a left join t_users as b on a.t_User_Guid=b.guid where a.t_Associate_Guid=@0 and a.t_DelState=0 order by a.t_Date desc";
                return rp1.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 创业圈点赞用户信息
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<UserPraise> GetAllByTopicGuid2(string Guid)
        {
            try
            {
                string sql = "select a.guid as Guid,a.t_Date,b.Guid as t_User_Guid,b.t_User_RealName as UserName,b.t_User_Style,b.t_UserStyleAudit from T_Praise as a left join t_users as b on a.t_User_Guid=b.guid where a.t_Associate_Guid=@0 and a.t_DelState=0 order by a.t_Date desc";
                return rp1.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<PraiseModels> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_Praise where t_DelState=0 order by t_Date desc";
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
        public PraiseModels GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_Praise where guid=@0";
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
        public bool Add(PraiseModels model)
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
        public bool Edit(PraiseModels model)
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
        public bool Del(PraiseModels model)
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
