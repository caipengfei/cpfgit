﻿using qch.Models;
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
        Repository<TopicsModel> rp2 = new Repository<TopicsModel>();

        /// <summary>
        /// 动态圈积攒列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<SelectTopic> GetPariseList(int page, int pagesize)
        {
            try
            {
                string sql = "select b.t_user_realname as UserName,b.t_user_pic as Pic,Count(c.Guid) as Parises from t_topic as a left join t_users as b on a.t_user_guid=b.guid left join T_Praise as c on a.guid=c.t_Associate_Guid and c.t_DelState=0 where a.t_Topic_Top=999 and a.t_delstate=0 and c.t_Date<'2016-08-18 17:05:00' group by b.t_user_realname,b.t_user_pic order by Count(c.Guid) desc";
                return rps.GetPageData(page, pagesize, sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public IEnumerable<SelectTopic> GetPariseList()
        {
            try
            {
                string sql = "select top 20 b.t_user_realname as UserName,b.t_user_pic as Pic,Count(c.Guid) as Parises from t_topic as a left join t_users as b on a.t_user_guid=b.guid left join T_Praise as c on a.guid=c.t_Associate_Guid and c.t_DelState=0 where a.t_Topic_Top=999 and a.t_delstate=0 and c.t_Date<'2016-08-18 17:05:00' group by b.t_user_realname,b.t_user_pic order by Count(c.Guid) desc";
                return rps.GetAll(sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某个用户的所有动态
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<TopicsModel> MyTopics(int page, int pagesize, string UserGuid)
        {
            try
            {
                string sql = "select a.Guid,a.t_Date,a.t_Topic_Contents,a.t_Topic_City,a.t_Topic_Address,a.t_Topic_Top,b.guid as t_User_Guid,b.t_User_Style,b.t_UserStyleAudit,b.t_User_RealName,b.t_User_Pic,b.t_User_Intention,b.t_User_NowNeed,b.t_User_Best,b.t_User_Position, b.t_User_Commpany from T_Topic as a left join t_users as b on a.t_user_guid=b.guid where a.t_delstate=0 and a.t_user_guid=@0 order by a.t_date desc";
                return rp2.GetPageData(page, pagesize, sql, new object[] { UserGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 动态圈
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<TopicsModel> GetTopics(int page, int pagesize, string CityName)
        {
            try
            {
                string sql = "select a.Guid,a.t_Date,a.t_Topic_Contents,a.t_Topic_City,a.t_Topic_Address,a.t_Topic_Top,b.guid as t_User_Guid,b.t_User_Style,b.t_UserStyleAudit,b.t_User_RealName,b.t_User_Pic,b.t_User_Intention,b.t_User_NowNeed,b.t_User_Best,b.t_User_Position, b.t_User_Commpany from T_Topic as a left join t_users as b on a.t_user_guid=b.guid where a.t_delstate=0 ";
                if (!string.IsNullOrWhiteSpace(CityName))
                {
                    sql += " and a.t_Topic_City=@0 ";
                }
                sql += " order by a.t_date desc";
                return rp2.GetPageData(page, pagesize, sql, new object[] { CityName });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public PetaPoco.Page<TopicsModel> GetTopics(int page, int pagesize, string CityName, string UserGuid)
        {
            try
            {
                string sql = "select a.Guid,a.t_Date,a.t_Topic_Contents,a.t_Topic_City,a.t_Topic_Address,a.t_Topic_Top,b.guid as t_User_Guid,b.t_User_Style,b.t_UserStyleAudit,b.t_User_RealName,b.t_User_Pic,b.t_User_Intention,b.t_User_NowNeed,b.t_User_Best,b.t_User_Position, b.t_User_Commpany from T_Topic as a left join t_users as b on a.t_user_guid=b.guid left join T_User_Foucs as c on a.t_User_Guid=c.t_Focus_Guid where c.t_User_Guid=@0 and a.t_delstate=0 and c.t_delstate=0";
                if (!string.IsNullOrWhiteSpace(CityName))
                {
                    sql += " and a.t_Topic_City=@1 ";
                }
                sql += " order by a.t_date desc";
                return rp2.GetPageData(page, pagesize, sql, new object[] { UserGuid, CityName });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public TopicsModel GetTopicsModel(string Guid)
        {
            try
            {
                string sql = "select a.Guid,a.t_Date,a.t_Topic_Contents,a.t_Topic_City,a.t_Topic_Address,a.t_Topic_Top,b.guid as t_User_Guid,b.t_User_Style,b.t_User_RealName,b.t_User_Pic,b.t_User_Intention,b.t_User_NowNeed,b.t_User_Best,b.t_User_Position, b.t_User_Commpany from T_Topic as a left join t_users as b on a.t_user_guid=b.guid where a.t_delstate=0  and a.Guid=@0";
                return rp2.Get(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
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
