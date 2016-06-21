using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 动态业务层
    /// </summary>
    public class TopicService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        TopicRepository rp = new TopicRepository();
        #region 动态举报业务
        /// <summary>
        /// 获取某用户的所有举报信息
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<TopicReportModel> GetReporyByUser(string Guid)
        {
            try
            {
                return rp.GetReporyByUser(Guid);
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
                return rp.GetReport(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有动态举报
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<TopicReportModel> GetReport(int page, int pagesize)
        {
            try
            {
                return rp.GetReport(page, pagesize);
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
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                return rp.GetReportById(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Msg SaveReport(TopicReportModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "操作失败";
            try
            {
                if (model == null)
                    return msg;
                var tt = GetReportById(model.Guid);
                if (tt != null)
                {
                    model.t_Date = DateTime.Now;
                    if (rp.EditReport(model))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    model.t_Date = DateTime.Now;
                    if (rp.AddReport(model))
                    {
                        msg.type = "success";
                        msg.Data = "新增成功";
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
        /// <summary>
        /// 更改删除状态
        /// </summary>
        /// <param name="Guid"></param>
        /// <param name="DelState"></param>
        /// <returns></returns>
        public bool EditReportStyle(string Guid, int DelState)
        {
            try
            {
                var model = GetReportById(Guid);
                if (model == null)
                    return false;
                model.t_DelState = DelState;
                return rp.EditReport(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        #endregion
        /// <summary>
        /// 附近活动
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public PetaPoco.Page<TopicModel> GetNearby(int page, int pagesize, string lon, string lat, int scope)
        {
            try
            {
                var model = rp.GetNearbySeller(page, pagesize, lon, lat, scope);
                if (model != null && model.Items != null)
                {
                    SetDistance(model.Items, lat, lon);
                }
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有动态
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<TopicModel> GetAll(int page, int pagesize)
        {
            try
            {
                return rp.GetAll(page, pagesize);
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
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                return rp.GetById(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Msg Save(TopicModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "操作失败";
            try
            {
                if (model == null)
                    return msg;
                var tt = GetById(model.Guid);
                if (tt != null)
                {
                    model.t_Date = DateTime.Now;
                    if (rp.Edit(model))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    model.t_Date = DateTime.Now;
                    if (rp.Add(model))
                    {
                        msg.type = "success";
                        msg.Data = "新增成功";
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
        /// <summary>
        /// 更改删除状态
        /// </summary>
        /// <param name="Guid"></param>
        /// <param name="DelState"></param>
        /// <returns></returns>
        public bool EditStyle(string Guid, int DelState)
        {
            try
            {
                var model = GetById(Guid);
                if (model == null)
                    return false;
                model.t_DelState = DelState;
                return rp.Edit(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }


        private void SetDistance(IList<TopicModel> mList, String lat, String lon)
        {
            if (mList == null) return;
            foreach (var item in mList)
            {
                item.Distance = getDistance(lat, lon, item.t_Topic_Latitude, item.t_Topic_Longitude);
                item.Distance = (cvt.ToDecimal(item.Distance) * 1000).ToString().TrimEnd(new char[] { ' ', '0', '.' }) + "米";
            }
        }
        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="task"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private string getDistance(String lat1, String lon1, String lat2, String lon2)
        {
            double EARTH_RADIUS = 6378.137;//地球半径
            double taskLatRad = rad(cvt.ToDouble(lat1));
            double infoLatRad = rad(cvt.ToDouble(lat2));
            double lat = taskLatRad - infoLatRad;
            double lng = rad(cvt.ToDouble(lon1)) - rad(cvt.ToDouble(lon2));

            double dis = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(lat / 2), 2) + Math.Cos(taskLatRad) * Math.Cos(infoLatRad) * Math.Pow(Math.Sin(lng / 2), 2)));
            dis = dis * EARTH_RADIUS;
            dis = Math.Round(dis * 1e4) / 1e4;
            dis = Math.Abs(dis);
            if (dis < 1)
            {

            }
            return Math.Abs(dis).ToString();
        }
        private double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
    }
}
