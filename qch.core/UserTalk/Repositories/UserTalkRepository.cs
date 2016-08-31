using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using qch.Repositories;

namespace qch.Repositories
{
    /// <summary>
    /// 评论资源层
    /// </summary>
    public class UserTalkRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<UserTalkModel> rp = new Repository<UserTalkModel>();
        Repository<SelectTalkModel> rp1 = new Repository<SelectTalkModel>();
        Repository<TopicTalkModel> rp2 = new Repository<TopicTalkModel>();

        /// <summary>
        /// 获取某个对象的所有评论（数量）
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public int GetCount(string Guid)
        {
            try
            {
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select Count(1) from T_User_Talk where t_Associate_Guid=@0 and t_DelState=0";
                    var m = db.ExecuteScalar<object>(sql, new object[] { Guid });
                    if (m != null)
                        return Convert.ToInt32(m);
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
        /// 动态圈评论
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public PetaPoco.Page<TopicTalkModel> GetForTopic(int page, int pagesize, string Guid)
        {
            try
            {
                string sql = "select a.Guid,a.t_Talk_FromContent,a.t_Talk_FromUserGuid,a.t_Talk_ToUserGuid,b.t_User_RealName as FromUser,b.t_User_Style as ToUserStyle,b.t_UserStyleAudit as ToUserStyleAudit,d.t_User_RealName as ToUser,d.t_User_Style as FromUserStyle,b.t_UserStyleAudit as FromUserStyleAudit from T_User_Talk as a left join t_users as b on a.t_Talk_FromUserGuid=b.guid left join t_users as d on a.t_Talk_ToUserGuid=d.guid where a.t_Associate_Guid=@0 and a.t_delstate=0 order by a.t_Talk_FromDate desc";
                return rp2.GetPageData(page, pagesize, sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 动态详情评论
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public PetaPoco.Page<TopicTalkModel> GetForTopic2(int page, int pagesize, string Guid)
        {
            try
            {
                string sql = "select a.Guid,a.t_Talk_FromContent,a.t_Talk_FromUserGuid,a.t_Talk_ToUserGuid,b.t_User_RealName as FromUser,b.t_User_Pic,b.t_User_Style,b.t_UserStyleAudit,d.t_User_RealName as ToUser from T_User_Talk as a left join t_users as b on a.t_Talk_FromUserGuid=b.guid left join t_users as d on a.t_Talk_ToUserGuid=d.guid where a.t_Associate_Guid=@0 and a.t_delstate=0 order by a.t_Talk_FromDate desc";
                return rp2.GetPageData(page, pagesize, sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取某个对象的所有评论
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public PetaPoco.Page<SelectTalkModel> GetAll(int page, int pagesize, string Guid)
        {
            try
            {
                string sql = "select a.*,b.t_User_RealName as UserName,b.t_User_Pic as UserAvator from t_user_talk as a left join t_users as b on a.t_Talk_FromUserGuid=b.Guid where a.t_Associate_Guid=@0 and a.t_DelState=0 order by t_Talk_FromDate desc";
                return rp1.GetPageData(page, pagesize, sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某个对象的所有评论
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<UserTalkModel> GetAll(string Guid)
        {
            try
            {
                string sql = "select a.*,b.t_User_RealName as UserName,b.t_User_Pic as UserAvator from t_user_talk as a left join t_users as b on a.t_Talk_FromUserGuid=b.Guid where a.t_Associate_Guid=@0 and a.t_DelState=0";
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
        public UserTalkModel GetById(string Guid)
        {
            try
            {
                string sql = "select a.*,b.t_User_RealName as UserName,b.t_User_Pic as UserAvator from t_user_talk as a left join t_users as b on a.t_Talk_FromUserGuid=b.Guid where a.Guid=@0";
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
        public bool Add(UserTalkModel model)
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
        /// 编辑评论信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(UserTalkModel model)
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
        /// 删除评论信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Del(UserTalkModel model)
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
