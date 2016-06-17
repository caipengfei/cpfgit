using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace qch.core
{
    /// <summary>
    /// 转盘业务层
    /// </summary>
    public class RollService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        RollRepository rp = new RollRepository();
        //抽奖需要的积分
        static int NeedIntegral = 10;
        //随机数
        static Random Rnd = new Random();
        /// <summary>
        /// 获取所有转盘抽奖物品
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RollModel> GetAll()
        {
            try
            {
                return rp.GetAll();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某用户的中奖几率
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<object> GetAllByUser(int page, int pagesize, string UserGuid)
        {
            try
            {
                using (var db = new PetaPoco.Database("qch"))
                {
                    var model = db.Page<object>(page, pagesize, "select a.t_AddDate,b.t_Roll_Title as RollTitle from t_roll_records as a left join t_roll as b on a.t_Roll_Guid=b.guid where a.t_User_Guid=@0 and a.t_DelState=0 order by a.t_AddDate desc", new object[] { UserGuid });
                    return model;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public Msg zp(string UserGuid)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "系统异常，请稍后再试";
            try
            {
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    //获取用户积分信息
                    var integral = db.SingleOrDefault<T_User_Integral>("select top 1 * from t_user_integral where t_User_Guid=@0 and t_DelState=0 order by t_AddDate desc", new object[] { UserGuid });
                    if (integral == null)
                    {
                        msg.Data = "积分信息异常";
                        log.Info("转盘抽奖时未能获取到用户的积分信息，UserGuid：" + UserGuid);
                        return msg;
                    }
                    if (integral.t_UserIntegral_Reward < NeedIntegral)
                    {
                        msg.Data = "积分不足";
                        log.Info("转盘抽奖时用户积分不足，UserGuid：" + UserGuid + "，用户积分余额：" + integral.t_UserIntegral_Reward);
                        return msg;
                    }
                    //获取奖品信息
                    var roll = db.Query<T_Roll>(" where t_DelState=0 order by t_Roll_Reward");
                    if (roll == null || roll.Count() <= 0)
                    {
                        msg.Data = "系统异常";
                        log.Info("转盘抽奖时未能获取到奖品信息，UserGuid：" + UserGuid);
                        return msg;
                    }
                    //扣积分
                    int userintegral = (int)integral.t_UserIntegral_Reward;  //用户积分余额
                    msg.Remark = (userintegral - NeedIntegral).ToString();
                    //生成用户积分减少记录
                    T_User_Integral ui = new T_User_Integral
                    {
                        t_UserIntegral_Reward = userintegral - NeedIntegral,
                        Guid = Guid.NewGuid().ToString(),
                        t_AddDate = DateTime.Now,
                        t_DelState = 0,
                        t_IntegralManager_Guid = "",
                        t_IntegralManager_PinYin = "choujiangjianshao",
                        t_Remark = "抽奖减少",
                        t_User_Guid = UserGuid,
                        t_UserIntegral_ReduceReward = NeedIntegral,
                        t_UserIntergral_AddReward = 0
                    };
                    db.Insert(ui);
                    Thread.Sleep(100);
                    db.BeginTransaction();

                    //抽奖计算
                    var xy = (from x in Enumerable.Range(0, 1000000)  //最多随机100万次
                              let sjcp = roll.ToList()[Rnd.Next(roll.ToList().Count())]
                              let zgz = Rnd.Next(0, 1000)  //概率按照千分之几计算
                              where zgz < sjcp.t_Roll_Probability
                              select sjcp).First();
                    msg.Result = xy;

                    //处理是否中奖
                    if (xy.t_Roll_Reward == 8)
                    {
                        //未中奖
                    }
                    else
                    {
                        //中奖，生成用户中奖记录
                        T_Roll_Records trr = new T_Roll_Records
                        {
                            Guid = Guid.NewGuid().ToString(),
                            t_AddDate = DateTime.Now,
                            t_DelState = 0,
                            t_Roll_Guid = xy.Guid,
                            t_User_Guid = UserGuid
                        };
                        db.Insert(trr);
                        if (xy.t_Roll_Type == 1)
                        {
                            //抽中优惠券
                            T_User_Voucher tuv = new T_User_Voucher
                            {
                                Guid = Guid.NewGuid().ToString(),
                                T_DelState = 0,
                                T_GetDate = DateTime.Now,
                                T_User_Guid = UserGuid,
                                T_Voucher_Guid = xy.t_Voucher_Guid,
                                T_Voucher_Pwd = createNonceStr(8),
                                T_Voucher_State = 0
                            };
                            db.Insert(tuv);
                        }
                        else if (xy.t_Roll_Type == 2)
                        {
                            msg.Remark = (userintegral - NeedIntegral + xy.t_Integral).ToString();
                            //用户抽中积分
                            //生成用户积分增加记录
                            T_User_Integral tui = new T_User_Integral
                            {
                                t_UserIntegral_Reward = userintegral - NeedIntegral + xy.t_Integral,
                                Guid = Guid.NewGuid().ToString(),
                                t_AddDate = DateTime.Now,
                                t_DelState = 0,
                                t_IntegralManager_Guid = "",
                                t_IntegralManager_PinYin = "choujiangzengjia",
                                t_Remark = "抽奖增加",
                                t_User_Guid = UserGuid,
                                t_UserIntegral_ReduceReward = 0,
                                t_UserIntergral_AddReward = xy.t_Integral
                            };
                            db.Insert(tui);
                        }
                    }
                    db.CompleteTransaction();
                }
                msg.type = "success";
                msg.Data = "";
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
        //创建随机字符串  
        public string createNonceStr(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string str = "";
            Random rad = new Random();
            for (int i = 0; i < length; i++)
            {
                str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
            }
            return str;
        }
    }
}
