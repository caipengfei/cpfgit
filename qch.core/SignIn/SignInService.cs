using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 用户签到业务层
    /// </summary>
    public class SignInService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SignInRepository rp = new SignInRepository();


        /// <summary>
        /// 用户签到
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="SignInDate"></param>
        /// <returns></returns>
        public Msg SignIn(string UserGuid)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "签到失败";
            try
            {
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    db.BeginTransaction();
                    var b = qch.Infrastructure.TimeHelper.GetStartDateTime(DateTime.Now);
                    var c = qch.Infrastructure.TimeHelper.GetEndDateTime(DateTime.Now);
                    var b1 = qch.Infrastructure.TimeHelper.GetStartDateTime(DateTime.Now.AddDays(-1));
                    var c1 = qch.Infrastructure.TimeHelper.GetEndDateTime(DateTime.Now.AddDays(-1));
                    var model1 = db.SingleOrDefault<T_User_SignIn>(" where T_SignIn_Date between @0 and @1 and T_User_Guid=@2 and T_DelState=0", new object[] { b, c, UserGuid });
                    if (model1 != null)
                    {
                        msg.Data = "您今日已经签过到了~";
                        return msg;
                    }
                    //随机生成1-10积分，作为基本的签到积分奖励
                    int integral = new Random().Next(1, 10);
                    //获取用户的积分信息
                    int nowintegral = 0;
                    var itr = db.SingleOrDefault<T_User_Integral>("select Top 1 * from t_user_integral where t_User_Guid=@0 and t_DelState=0 order by t_AddDate desc", new object[] { UserGuid });
                    if (itr != null)
                    {
                        nowintegral = (int)itr.t_UserIntegral_Reward;
                    }
                    int extra = 0;
                    int value = 1;
                    int value1 = 1;
                    var model = db.SingleOrDefault<T_User_SignIn>(" where T_SignIn_Date between @0 and @1 and T_User_Guid=@2 and T_DelState=0", new object[] { b1, c1, UserGuid });
                    if (model != null)
                    {
                        //连续签到
                        value = model.T_SignIn_Days + 1;
                        value1 = model.T_SignIn_Days + 1;
                        model.T_SignIn_Days = value;
                        if (value == 3)
                        {
                            extra = 20;
                            integral += 20;
                        }
                        else if (value == 7)
                        {
                            extra = 50;
                            //签到7天为一个周期，连续签到7天后，重新累计
                            value = 0;
                            integral += 50;
                        }
                    }
                    //为用户生成积分增加记录
                    T_User_Integral userIntegral = new T_User_Integral
                    {
                        Guid = Guid.NewGuid().ToString(),
                        t_AddDate = DateTime.Now,
                        t_DelState = 0,
                        t_IntegralManager_Guid = "",
                        t_IntegralManager_PinYin = "qiandao",
                        t_Remark = "微信签到",
                        t_User_Guid = UserGuid,
                        t_UserIntegral_ReduceReward = 0,
                        t_UserIntegral_Reward = nowintegral + integral,
                        t_UserIntergral_AddReward = integral
                    };
                    db.Insert(userIntegral);
                    msg.Remark = string.Format("您已连续签到{0}天，获得积分{1}\r\n含额外奖励积分：{2}", value1, integral, extra);
                    //生成签到信息
                    T_User_SignIn us = new T_User_SignIn
                    {
                        T_SignIn_Days = value,
                        Guid = Guid.NewGuid().ToString(),
                        T_DelState = 0,
                        T_Extra_Integral = extra,
                        T_Integral = integral,
                        T_Remark = "微信签到",
                        T_SignIn_Date = DateTime.Now,
                        T_User_Guid = UserGuid
                    };
                    db.Insert(us);
                    db.CompleteTransaction();
                }

                msg.type = "success";
                msg.Data = "签到成功";
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
        /// <summary>
        /// getbyid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public SignInModel GetById(string Guid)
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
        public Msg Save(SignInModel model)
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
                    if (rp.Edit(model))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    model.T_SignIn_Date = DateTime.Now;
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
                model.T_DelState = DelState;
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
