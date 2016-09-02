using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 活动业务层
    /// </summary>
    public class ActivityService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static object obj = new object();
        ActivityRepository rp = new ActivityRepository();
        WXUserService wxuserService = new WXUserService();


        /// <summary>
        /// 微信端活动列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="CityName"></param>
        /// <param name="days"></param>
        /// <param name="payType"></param>
        /// <returns></returns>
        public PetaPoco.Page<ActivityModel> GetListFroWX(int page, int pagesize, string CityName, int days, string payType)
        {
            try
            {
                var model = rp.GetListFroWX(page, pagesize, CityName, days, payType);
                if (model != null && model.Items != null)
                {
                    foreach (var item in model.Items)
                    {
                        item.Applys = this.GetApplys(item.Guid);
                    }
                    //model.Items.Select(o => o.Applys = this.GetApplys(o.Guid));
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
        /// 获取某个活动报名人数
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public int GetApplys(string Guid)
        {
            try
            {
                int xy = 0;
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select count(1) from t_activity_apply where t_DelState=0 and t_Activity_Guid=@0";
                    var model = db.ExecuteScalar<object>(sql, new object[] { Guid });
                    if (model != null)
                        xy = Convert.ToInt32(model);
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
        /// 获取所有活动，按照推荐和开始日期排序
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<ActivityModel> GetAll(int page, int pagesize)
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
        /// 发布活动
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Msg Publish(string UserGuid, ActivityModel model)
        {
            /*
             * 微信扫码登录发布活动流程：
             * 1、根据微信用户信息检索系统是否存在关联的用户信息
             * 2、如果有用户信息，生成活动数据
             * 3、如果没有用户信息，生成用户信息，生成活动信息
             */
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "发布失败";
            try
            {
                if (string.IsNullOrWhiteSpace(UserGuid))
                {
                    msg.Data = "UserGuid为空";
                    log.Info("UserGuid为空");
                    return msg;
                }
                
                if (model == null)
                {
                    return msg;
                }
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    db.BeginTransaction();
                    var target = db.SingleOrDefault<T_Activity>(" where t_Activity_Title=@0 and t_Activity_Instruction=@1 and t_Activity_Holder=@2", new object[] { model.t_Activity_Title, model.t_Activity_Instruction, model.t_Activity_Holder });
                    if (target != null)
                    {
                        msg.Data = "请勿重复发布";
                        log.Info("重复发布");
                        return msg;
                    }
                    string activityGuid = Guid.NewGuid().ToString();
                    msg.Remark = activityGuid;
                    model.t_Activity_Latitude = "";
                    model.t_Activity_Longitude = "";
                    model.t_Activity_Audit = 0;
                    model.t_DelState = 0;
                    model.t_Activity_Recommand = 0;
                    model.Guid = activityGuid;
                    model.t_AddDate = DateTime.Now;
                    model.t_ModifydDate = DateTime.Now;
                    model.t_ModifyBy = "";
                    model.t_User_Guid = UserGuid;
                    db.Insert(model);
                    db.CompleteTransaction();
                }
                msg.type = "success";
                msg.Data = "发布成功";
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
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
                return rp.GetAll(page, pagesize, Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某人发布的所有未删除活动
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<ActivityModel> GetAll(string Guid)
        {
            try
            {
                return rp.GetAll(Guid);
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
        public bool Save(ActivityModel model)
        {
            try
            {
                if (model == null)
                    return false;
                var tt = GetById(model.Guid);
                if (tt != null)
                {
                    model.t_ModifydDate = DateTime.Now;
                    return rp.Edit(model);
                }
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    model.t_AddDate = DateTime.Now;
                    model.t_ModifydDate = DateTime.Now;
                    return rp.Add(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
    }
}
