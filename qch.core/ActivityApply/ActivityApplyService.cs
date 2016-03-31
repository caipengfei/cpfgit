using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 活动报名业务层
    /// </summary>
    public class ActivityApplyService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ActivityApplyRepository rp = new ActivityApplyRepository();

        /// <summary>
        /// 微信活动报名
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="phone"></param>
        /// <param name="avator"></param>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        public Msg Apply(string guid, string phone, string avator, string name, string sex)
        {
            log.Info("service------------------------------");
            log.Info("avator:" + avator);
            log.Info("name:" + name);
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "报名失败";
            try
            {
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    db.BeginTransaction();
                    #region 验证
                    var activity = db.SingleOrDefault<T_Activity>("select * from t_activity where [Guid]=@0", new object[] { guid });
                    if (activity == null)
                    {
                        msg.Data = "活动不存在";
                        log.Error("报名活动不存在，活动GUID=" + guid + "，报名电话：" + phone);
                        return msg;
                    }
                    //if (DateTime.Now < activity.t_Activity_sDate)
                    //{
                    //    msg.Data = "该活动尚未开始";
                    //    log.Error("报名活动尚未开始，活动GUID=" + guid + "，报名电话：" + phone);
                    //    return msg;
                    //}
                    if (DateTime.Now > activity.t_Activity_eDate)
                    {
                        msg.Data = "该活动已结束";
                        log.Error("报名活动已结束，活动GUID=" + guid + "，报名电话：" + phone);
                        return msg;
                    }
                    var members = db.Query<T_Activity_Apply>(" where t_Activity_Guid=@0", new object[] { guid });
                    if (members != null && members.Count() > 0)
                    {
                        if (members.Count() >= activity.t_Activity_LimitPerson)
                        {
                            msg.Data = "满员了，下次请早哦~";
                            log.Error("报名活动已达到限制人数，活动GUID=" + guid + "，报名电话：" + phone);
                            return msg;
                        }
                    }
                    #endregion
                    var user = db.SingleOrDefault<T_Users>(" where t_User_Mobile=@0", new object[] { phone });
                    if (user != null)
                    {
                        var useractivity = db.SingleOrDefault<T_Activity_Apply>(" where t_user_guid=@0 and t_Activity_Guid=@1", new object[] { user.Guid, guid });
                        if (useractivity != null)
                        {
                            msg.Data = "请勿重复报名";
                            log.Error("重复报名，活动GUID=" + guid + "，报名电话：" + phone);
                            return msg;
                        }
                        #region 用户信息存在
                        //已存在的用户
                        //直接生成报名信息
                        T_Activity_Apply model = new T_Activity_Apply
                        {
                            Guid = Guid.NewGuid().ToString(),
                            t_Activity_Guid = guid,
                            t_ActivityApply_Mobile = "",
                            t_ActivityApply_Remark = "微信报名",
                            t_ActivityApply_UserName = "",
                            t_AddDate = DateTime.Now,
                            t_DelState = 0,
                            t_User_Guid = user.Guid
                        };
                        db.Insert(model);
                        #endregion
                    }
                    else
                    {
                        #region 用户信息不存在
                        //如果没有用户信息，先生存用户信息，再生存报名信息
                        //生日暂定为当前日期，密码默认为123456
                        T_Users wxUser = new T_Users
                        {
                            Guid = Guid.NewGuid().ToString(),
                            t_Andriod_Rid = "",
                            t_DelState = 0,
                            t_IOS_Rid = "",
                            t_RongCloud_Token = "",
                            t_User_Best = "",
                            t_User_Birth = DateTime.Now,
                            t_User_BusinessCard = "",
                            t_User_City = "",
                            t_User_Commpany = "",
                            t_User_Complete = 0,
                            t_User_Date = DateTime.Now,
                            t_User_Email = "",
                            t_User_FocusArea = "",
                            t_User_InvestArea = "",
                            t_User_InvestMoney = "",
                            t_User_InvestPhase = "",
                            t_User_LoginId = phone,
                            t_User_Mobile = phone,
                            t_User_NickName = name,
                            t_User_Pic = avator,
                            t_User_Position = "",
                            t_User_Pwd = qch.Infrastructure.DESEncrypt.Encrypt("123456"),
                            t_User_RealName = name,
                            t_User_Remark = "微信报名生成",
                            t_User_Sex = sex,
                            t_User_Style = 1,
                            t_User_ThreeLogin = ""
                        };
                        var userid = db.Insert(wxUser);
                        log.Info("userid=" + userid);
                        if (userid != null)
                        {
                            //生存活动报名信息
                            T_Activity_Apply model = new T_Activity_Apply
                            {
                                Guid = Guid.NewGuid().ToString(),
                                t_Activity_Guid = guid,
                                t_ActivityApply_Mobile = "",
                                t_ActivityApply_Remark = "微信报名",
                                t_ActivityApply_UserName = "",
                                t_AddDate = DateTime.Now,
                                t_DelState = 0,
                                t_User_Guid = userid.ToString()
                            };
                            db.Insert(model);
                        }
                        #endregion
                    }
                    db.CompleteTransaction();
                    msg.type = "success";
                    msg.Data = "报名成功";
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
        /// 分页获取
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<ActivityApplyModel> GetAll(int page, int pagesize)
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
        public ActivityApplyModel GetById(string Guid)
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
        public Msg Save(ActivityApplyModel model)
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
                    model.t_AddDate = DateTime.Now;
                    if (rp.Edit(model))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    model.t_AddDate = DateTime.Now;
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
    }
}
