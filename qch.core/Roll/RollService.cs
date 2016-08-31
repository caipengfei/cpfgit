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
        /// 获取某用户的中奖记录
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
        /// <summary>
        /// 获取所有中奖记录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<object> GetAllRollRecords(int page, int pagesize)
        {
            try
            {
                using (var db = new PetaPoco.Database("qch"))
                {
                    var model = db.Page<object>(page, pagesize, "select a.t_AddDate as RollDate,b.t_Roll_Title as GoodsName,c.t_User_RealName as UserName,c.t_user_pic as UserPic from t_roll_records as a left join t_roll as b on a.t_Roll_Guid=b.guid left join t_users as c on a.t_User_Guid=c.guid order by b.t_Roll_Reward,a.t_AddDate desc");
                    return model;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 转盘抽奖
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public Msg zp(string UserGuid)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "系统异常，请稍后再试";
            try
            {
                log.Info("---------------------------------------------------转盘抽奖开始-------------------------------------------");
                string orderno = "";
                //int Probability = 1000;//概率按照千分之几计算
                //int Probability = 10000;//概率按照万分之几计算，06-22修改
                int Probability = 100000;//概率按照十万分之几计算，07-07修改
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    //获取用户积分信息
                    var integral = db.SingleOrDefault<T_User_Integral>("select top 1 * from t_user_integral where t_User_Guid=@0 and t_DelState=0 order by t_AddDate desc", new object[] { UserGuid });
                    if (integral == null)
                    {
                        msg.Data = "您还没有积分呢,快去赚积分吧~";
                        log.Info("转盘抽奖时未能获取到用户的积分信息，UserGuid：" + UserGuid);
                        return msg;
                    }
                    if (integral.t_UserIntegral_Reward < NeedIntegral)
                    {
                        msg.Data = "积分不足,快去赚积分吧";
                        log.Info("转盘抽奖时用户积分不足，UserGuid：" + UserGuid + "，用户积分余额：" + integral.t_UserIntegral_Reward);
                        return msg;
                    }
                    //获取奖品信息
                    var roll = db.Query<T_Roll>(" where t_DelState=0 order by t_Roll_Reward");
                    if (roll == null || roll.Count() <= 0)
                    {
                        msg.Data = "奖品君消失了...";
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
                              let zgz = Rnd.Next(0, Probability)
                              where zgz < sjcp.t_Roll_Probability
                              select sjcp).First();
                    msg.Result = xy;
                    log.Info("抽奖：奖品类型=" + xy.t_Roll_Type);
                    //处理是否中奖
                    //如果抽中实物
                    if (xy.t_Roll_Type == 3)
                    {
                        DateTime statetime = qch.Infrastructure.TimeHelper.GetStartDateTime(Convert.ToDateTime("2016-07-07"));
                        string sql = "select count(1) from T_Roll_Records where t_AddDate>@0 and t_Roll_Guid in (select Guid from T_Roll where t_Roll_Reward=@1 and t_DelState=0) and t_DelState=0";
                        if (xy.t_Roll_Reward == 1)
                        {
                            //如果抽中一等奖，查询数据库一等奖已存在多少个，如果有1个了，重新抽奖       
                            log.Info("用户抽奖，抽中1等奖，UserGuid：" + UserGuid);
                            var roll1 = db.ExecuteScalar<object>(sql, new object[] { statetime, 1 });
                            log.Info(roll1);
                            if (roll1 != null)
                            {
                                int xy1 = Convert.ToInt32(roll1);
                                log.Info(xy1);
                                if (xy1 >= 2)
                                {
                                    log.Info("--------------------  一等奖名额已满，需要重新抽奖.............");
                                    //更改该奖项的中奖率为0
                                    var r1 = db.SingleOrDefault<T_Roll>("select top 1 * from T_Roll where t_Roll_Reward=1 and t_DelState=0");
                                    if (r1 != null)
                                    {
                                        log.Info(string.Format("修改奖项【{0}】的中奖几率为0", r1.t_Roll_Title));
                                        r1.t_Roll_Probability = 0;
                                        db.Save(r1);
                                    }
                                    Thread.Sleep(100);
                                    //重新获取奖品列表
                                    roll = db.Query<T_Roll>(" where t_DelState=0 order by t_Roll_Reward");
                                    if (roll == null || roll.Count() <= 0)
                                    {
                                        msg.Data = "奖品君消失了...";
                                        log.Info("转盘抽奖重新抽取一等奖时未能获取到奖品信息，UserGuid：" + UserGuid);
                                        return msg;
                                    }
                                    //重新抽奖
                                    xy = (from x in Enumerable.Range(0, 1000000)  //最多随机100万次
                                          let sjcp = roll.ToList()[Rnd.Next(roll.ToList().Count())]
                                          let zgz = Rnd.Next(0, Probability)
                                          where zgz < sjcp.t_Roll_Probability
                                          select sjcp).First();
                                    msg.Result = xy;
                                    log.Info("--------------------执行重新抽奖后的结果：" + xy.t_Roll_Title);
                                }
                            }
                        }
                        if (xy.t_Roll_Reward == 2)
                        {
                            //如果抽中2等奖，查询数据库2等奖已存在多少个，如果有4个了，重新抽奖 
                            log.Info("用户抽奖，抽中2等奖，UserGuid：" + UserGuid);
                            var roll2 = db.ExecuteScalar<object>(sql, new object[] { statetime, 2 });
                            if (roll2 != null)
                            {
                                int xy2 = Convert.ToInt32(roll2);
                                if (xy2 >= 4)
                                {
                                    log.Info("--------------------  二等奖名额已满，需要重新抽奖.............");
                                    //更改该奖项的中奖率为0
                                    var r2 = db.SingleOrDefault<T_Roll>("select top 1 * from T_Roll where t_Roll_Reward=2 and t_DelState=0");
                                    if (r2 != null)
                                    {
                                        log.Info(string.Format("修改奖项【{0}】的中奖几率为0", r2.t_Roll_Title));
                                        r2.t_Roll_Probability = 0;
                                        db.Save(r2);
                                    }
                                    Thread.Sleep(100);
                                    //重新获取奖品列表
                                    roll = db.Query<T_Roll>(" where t_DelState=0 order by t_Roll_Reward");
                                    if (roll == null || roll.Count() <= 0)
                                    {
                                        msg.Data = "奖品君消失了...";
                                        log.Info("转盘抽奖重新抽取一等奖时未能获取到奖品信息，UserGuid：" + UserGuid);
                                        return msg;
                                    }
                                    //重新抽奖
                                    xy = (from x in Enumerable.Range(0, 1000000)  //最多随机100万次
                                          let sjcp = roll.ToList()[Rnd.Next(roll.ToList().Count())]
                                          let zgz = Rnd.Next(0, Probability)
                                          where zgz < sjcp.t_Roll_Probability
                                          select sjcp).First();
                                    msg.Result = xy;
                                    log.Info("--------------------执行重新抽奖后的结果：" + xy.t_Roll_Title);
                                }
                            }
                        }
                        if (xy.t_Roll_Reward == 3)
                        {
                            //如果抽中3等奖，查询数据库3等奖已存在多少个，如果有3个了，重新抽奖       
                            log.Info("用户抽奖，抽中3等奖，UserGuid：" + UserGuid);
                            var roll3 = db.ExecuteScalar<object>(sql, new object[] { statetime, 3 });
                            log.Info(roll3);
                            if (roll3 != null)
                            {
                                int xy3 = Convert.ToInt32(roll3);
                                log.Info(xy3);
                                if (xy3 >= 3)
                                {
                                    log.Info("--------------------  三等奖名额已满，需要重新抽奖.............");
                                    //更改该奖项的中奖率为0
                                    var r3 = db.SingleOrDefault<T_Roll>("select top 1 * from T_Roll where t_Roll_Reward=3 and t_DelState=0");
                                    if (r3 != null)
                                    {
                                        log.Info(string.Format("修改奖项【{0}】的中奖几率为0", r3.t_Roll_Title));
                                        r3.t_Roll_Probability = 0;
                                        db.Save(r3);
                                    }
                                    Thread.Sleep(100);
                                    //重新获取奖品列表
                                    roll = db.Query<T_Roll>(" where t_DelState=0 order by t_Roll_Reward");
                                    if (roll == null || roll.Count() <= 0)
                                    {
                                        msg.Data = "奖品君消失了...";
                                        log.Info("转盘抽奖重新抽取一等奖时未能获取到奖品信息，UserGuid：" + UserGuid);
                                        return msg;
                                    }
                                    //重新抽奖
                                    xy = (from x in Enumerable.Range(0, 1000000)  //最多随机100万次
                                          let sjcp = roll.ToList()[Rnd.Next(roll.ToList().Count())]
                                          let zgz = Rnd.Next(0, Probability)
                                          where zgz < sjcp.t_Roll_Probability
                                          select sjcp).First();
                                    msg.Result = xy;
                                    log.Info("--------------------执行重新抽奖后的结果：" + xy.t_Roll_Title);
                                }
                            }
                        }
                    }
                    //2016-07-06 取消空奖，最低5积分
                    //if (xy.t_Roll_Reward == 8)
                    //{
                    //    //未中奖
                    //}
                    //else
                    //{    

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
                    else if (xy.t_Roll_Type == 3)
                    {
                        var goods = db.SingleOrDefault<T_Goods_Convert>(" where guid=@0 and t_delstate=0", new object[] { xy.t_RollGoods_Guid });
                        if (goods != null)
                        {
                            //生成用户兑换记录   
                            orderno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                            T_Goods_Convert_List list = new T_Goods_Convert_List
                            {
                                Guid = Guid.NewGuid().ToString(),
                                t_Cnee_Guid = "",
                                t_Convert_CreateDate = DateTime.Now,
                                t_Convert_OrderNo = orderno,
                                t_DelState = 0,
                                t_Goods_Guid = xy.t_RollGoods_Guid,
                                t_Logistics_Company = "",
                                t_Logistics_Status = 0,
                                t_Logistics_WaybillNo = "",
                                t_User_Guid = UserGuid,
                                t_List_Type = 2
                            };
                            db.Insert(list);
                            xy.t_Roll_Pic = goods.t_Goods_Pic;
                        }
                        else
                        {
                            log.Info(string.Format("转盘抽奖，抽中实物之后，未能获取到商品信息。商品guid：{0}，用户guid：{1}", xy.t_RollGoods_Guid, UserGuid));
                        }
                    }
                    //}
                    db.CompleteTransaction();
                }
                msg.type = "success";
                msg.Data = orderno;
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
