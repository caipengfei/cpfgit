using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 动态资源层
    /// </summary>
    public class TopicRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<TopicModel> rp = new Repository<TopicModel>();
        Repository<SelectTopic> rps = new Repository<SelectTopic>();
        /// <summary>
        /// 获取附近活动
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public PetaPoco.Page<TopicModel> GetNearbySeller(int page, int pagesize, string lon, string lat, int scope)
        {
            log.Info("查询数据库附近活动");
            if (scope < 10) scope = 10;
            try
            {
                log.Info("lon经度" + lon);
                log.Info("lat纬度" + lat);
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from T_Topic where t_DelState=(@0) and");
                //sql.Append("(select [UserId] from [bmb_task_cate] where id=@0) and");
                sql.Append(" sqrt( ( ((@1-convert(numeric(18,12),t_Topic_Longitude))*PI()*12656*cos(((@2+convert(numeric(18,12),t_Topic_Latitude))/2)*PI()/180)/180)");
                sql.Append(" * ((@3-convert(numeric(18,12),t_Topic_Longitude))*PI()*12656*cos (((@4+convert(numeric(18,12),t_Topic_Latitude))/2)*PI()/180)/180) )");
                sql.Append(" + ( ((@5-convert(numeric(18,12),t_Topic_Latitude))*PI()*12656/180) * ((@6-convert(numeric(18,12),t_Topic_Latitude))*PI()*12656/180) ) )");
                sql.Append("<@7 and SellerLon is not null and SellerLat is not null and SellerLon<>'' and SellerLat<>''");
                var data = rp.GetPageData(page, pagesize, sql.ToString(), new object[] { 0, lon, lat, lon, lat, lat, lat, scope });
                log.Info(sql);
                if (data == null)
                    return null;
                log.Info("共查询到数据" + data.TotalItems.ToString());

                return data;

            }
            catch (Exception ex)
            {
                log.Error("方法：GetNearbySeller" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有项目
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<TopicModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_Topic where t_DelState=0 order by t_Date desc";
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
        public TopicModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_Topic where guid=@0";
                return rp.Get(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某人的第一条动态（app转发的微信，获取详情用到）
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public SelectTopic GetTop1(string Guid)
        {
            try
            {
                string sql = "select top 1 a.Guid,left(a.t_topic_contents,20) as Contents,a.t_topic_city as CityName,a.t_date,b.[t_Pic_Url] as Pic from t_topic as a left join [T_Associate_Pic] as b on a.guid=b.t_Associate_Guid where a.t_user_guid=@0 and a.t_delstate=0 order by a.t_date desc,b.t_Pic_Url";
                return rps.Get(sql, new object[] { Guid });
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
        public bool Add(TopicModel model)
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
        public bool Edit(TopicModel model)
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
        public bool Del(TopicModel model)
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
        #region 动态举报资源
        Repository<TopicReportModel> rp1 = new Repository<TopicReportModel>();
        /// <summary>
        /// 获取某用户的所有举报信息
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<TopicReportModel> GetReporyByUser(string Guid)
        {
            try
            {
                string sql = "select * from T_Topic_Report where t_User_Guid=@0 and t_DelState=0";
                return rp1.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 根据动态的Guid获取其所有举报信息
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<TopicReportModel> GetReport(string Guid)
        {
            try
            {
                string sql = "select * from T_Topic_Report where t_Topic_Guid=@0 and t_DelState=0";
                return rp1.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有项目
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<TopicReportModel> GetReport(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_Topic_Report where t_DelState=0 order by t_Date desc";
                return rp1.GetPageData(page, pagesize, sql);
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
        public TopicReportModel GetReportById(string Guid)
        {
            try
            {
                string sql = "select * from T_News where guid=@0";
                return rp1.Get(sql, new object[] { Guid });
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
        public bool AddReport(TopicReportModel model)
        {
            try
            {
                return rp1.Insert(model) == null ? false : true;
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
        public bool EditReport(TopicReportModel model)
        {
            try
            {
                return (int)rp1.Update(model) > 0 ? true : false;
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
        public bool DelReport(TopicReportModel model)
        {
            try
            {
                return (int)rp1.Delete(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        #endregion
    }
}
