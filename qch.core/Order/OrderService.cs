﻿using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 订单业务层
    /// </summary>
    public class OrderService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static object obj = new object();
        OrderRepository rp = new OrderRepository();


        #region 预约空间订单
        /// <summary>
        /// 生成用户预约空间订单
        /// </summary>
        /// <param name="StyleGuid"></param>
        /// <param name="TimeGuid"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public Msg CreatePlaceOrder(string StyleGuid, string TimeGuid, string UserGuid)
        {
            /*
             * 1、验证空间类型以及预约时间信息
             * 2、生成一条未支付的订单
             * 3、生成一条删除状态的place ordered信息
             * 4、如果支付成功后，更改预约时间点的状态为已被预约
             */
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "创建订单失败";
            try
            {
                string orderno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    db.BeginTransaction();
                    #region 验证
                    //查询当天是否有预约
                    var isorder = db.SingleOrDefault<T_Place_Ordered>("select top 1 * from T_Place_Ordered where t_User_Guid=@0 and t_State!=3 and t_delstate=0 and t_AddDate between @1 and @2", new object[] { UserGuid, qch.Infrastructure.TimeHelper.GetStartDateTime(DateTime.Now), qch.Infrastructure.TimeHelper.GetEndDateTime(DateTime.Now) });
                    if (isorder != null)
                    {
                        msg.Data = "您今天已经预约过了";
                        return msg;
                    }
                    var style = db.SingleOrDefault<T_Place_Style>(" where Guid=@0 and t_DelState=0", new object[] { StyleGuid });
                    if (style == null)
                    {
                        log.Info("空间预约下单的时候，未能获取到空间类型的信息，styleGuid=" + StyleGuid);
                        return msg;
                    }
                    var time = db.SingleOrDefault<T_PlaceOrder_Time>(" where Guid=@0 and t_PlaceOder_Ordered=0 and t_DelState=0", new object[] { TimeGuid });
                    if (time == null)
                    {
                        log.Info("空间预约下单的时候，未能获取到空间预约时间点信息，TimeGuid=" + TimeGuid + "，StyleGuid=" + StyleGuid);
                        return msg;
                    }
                    #endregion
                    //生成一条删除状态的预约信息
                    T_Place_Ordered ordered = new T_Place_Ordered
                    {
                        Guid = Guid.NewGuid().ToString(),
                        t_AddDate = DateTime.Now,
                        t_DelState = 1,
                        t_Ordered_NO = orderno,
                        t_Ordered_Remark = "未支付(微信)",
                        t_PlaceOrder_Guid = time.Guid,
                        t_State = 0,
                        t_User_Guid = UserGuid
                    };
                    var x = db.Insert(ordered);
                    if (x == null)
                    {
                        log.Info("空间预约下单的时候，生成预约信息失败");
                        return msg;
                    }
                    //生成一条未支付的订单
                    T_User_Order order = new T_User_Order
                    {
                        t_Associate_Guid = time.Guid,
                        Guid = Guid.NewGuid().ToString(),
                        t_DelState = 0,
                        t_Order_Date = DateTime.Now,
                        t_Order_Money = (decimal)style.t_Place_Money,
                        t_Order_Name = style.t_Place_StyleName,
                        t_Order_No = orderno,
                        t_Order_OrderType = 5,
                        t_Order_PayType = "微信支付 空间预约",
                        t_Order_Remark = x.ToString(),
                        t_Order_State = 0,
                        t_User_Guid = UserGuid
                    };
                    var value = db.Insert(order);
                    if (value != null)
                    {
                        //生成订单中的商品信息
                        T_UserOrder_Good ug = new T_UserOrder_Good
                        {
                            Guid = Guid.NewGuid().ToString(),
                            t_Associate_Guid = time.Guid,
                            t_Date = DateTime.Now,
                            t_Order_Guid = value.ToString()
                        };
                        db.Insert(ug);
                    }
                    db.CompleteTransaction();
                }
                msg.type = "success";
                msg.Data = "创建订单成功";
                msg.Remark = orderno;
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
        #endregion
        /// <summary>
        /// 获取众筹金额（已众筹了多少钱）
        /// </summary>
        /// <param name="Guid">众筹guid</param>
        /// <returns></returns>
        public decimal GetFundCourseMoney(string Guid)
        {
            try
            {
                return rp.GetFundCourseMoney(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 众筹人次
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public int GetFundCourseCount(string Guid)
        {
            try
            {
                return rp.GetFundCourseCount(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 处理支付业务
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public Msg Pay(string orderNo, decimal Money)
        {
            /*
             * 处理支付业务
             * 1、验证订单
             * 2、查看订单类型
             * 3、如果是活动报名的支付，支付更改订单状态即可；如果是充值订单，更改订单状态，并且增加用户余额以及流水。
             */
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "支付处理失败";
            lock (obj)
            {
                try
                {
                    using (var db = new PetaPoco.Database(DbConfig.qch))
                    {
                        db.BeginTransaction();
                        var order = db.SingleOrDefault<T_User_Order>(" where t_Order_No=@0", new object[] { orderNo });
                        if (order == null)
                        {
                            msg.Data = "订单信息不存在";
                            log.Info("订单信息不存在，订单号：" + orderNo);
                            return msg;
                        }
                        var user = db.SingleOrDefault<T_Users>(" where guid=@0", new object[] { order.t_User_Guid });
                        if (user == null)
                        {
                            msg.Data = "用户信息异常";
                            log.Info("支付成功后未能获取到用户信息，订单号=" + orderNo + ",userGuid=" + order.t_User_Guid);
                            return msg;
                        }
                        if (order.t_Order_State != 0)
                        {
                            msg.Data = "订单状态异常";
                            log.Info("订单状态异常，订单号：" + orderNo);
                            return msg;
                        }
                        //更改订单状态
                        order.t_Order_State = 1;
                        order.t_Order_PayType = "微信支付";
                        order.t_Order_Money = Money;
                        #region 创业币充值业务
                        if (order.t_Order_OrderType == 1)
                        {
                            //表示充值订单
                            //处理充值后续业务
                            order.t_Order_Remark = "微信充值";
                            //16-06-15 取消以下处理业务，因为app在支付成功后又调用了别的接口处理。

                            decimal money = 0;
                            var model = db.SingleOrDefault<T_User_Account>(" where t_UserAccount_No=@0", new object[] { orderNo });
                            if (model != null)
                            {
                                msg.Data = "重复处理的流水";
                                log.Error(string.Format("用户在使用微信进行充值操作的时候，流水中已存在该订单号，{0}", orderNo));
                            }
                            //获取用户流水
                            var account = db.SingleOrDefault<T_User_Account>("select Top 1 * from T_User_Account where t_User_Guid=@0 order by t_AddDate desc", new object[] { order.t_User_Guid });
                            if (account != null)
                            {
                                money = account.t_UserAccount_Reward;
                            }
                            AccountModel m = new AccountModel()
                            {
                                t_AddDate = DateTime.Now,
                                Guid = Guid.NewGuid().ToString(),
                                t_DelState = 0,
                                t_Remark = "微信充值",
                                t_User_Guid = order.t_User_Guid,
                                t_UserAccount_AddReward = order.t_Order_Money,
                                t_UserAccount_No = order.t_Order_No,
                                t_UserAccount_ReduceReward = 0,
                                t_UserAccount_Reward = money + order.t_Order_Money
                            };
                            //保存用户流水
                            db.Insert(m);
                        }
                        #endregion
                        #region 其它业务
                        else
                        {
                            //表示活动订单，处理活动支付后续业务
                            //处理生成活动报名信息
                            //do
                            if (order.t_Order_OrderType == 2)
                            {
                                var apply = db.SingleOrDefault<T_Activity_Apply>(" where guid=@0 and t_DelState=1", new object[] { order.t_Order_Remark });
                                if (apply != null)
                                {
                                    apply.t_DelState = 0;
                                    db.Save(apply);
                                }
                            }
                            //空间预约订单
                            if (order.t_Order_OrderType == 5)
                            {
                                var ordered = db.SingleOrDefault<T_Place_Ordered>(" where guid=@0 and t_delstate=1", new object[] { order.t_Order_Remark });
                                if (ordered != null)
                                {
                                    ordered.t_DelState = 0;
                                    ordered.t_Ordered_Remark = "微信支付";
                                    db.Save(ordered);
                                }
                                //更改空间该时间段的预约状态
                                var time = db.SingleOrDefault<T_PlaceOrder_Time>(" where Guid=@0 and t_PlaceOder_Ordered=0 and t_DelState=0", new object[] { order.t_Associate_Guid });
                                if (time != null)
                                {
                                    time.t_PlaceOder_Ordered = 1;
                                    db.Save(time);
                                }
                            }
                        }
                        #endregion
                        //更新订单
                        db.Save(order);
                        db.CompleteTransaction();
                    }
                    msg.type = "success";
                    msg.Data = "OK";
                    return msg;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return msg;
                }
            }
        }
        /// <summary>
        /// 获取某人的所有订单
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<OrderModel> GetAll(string Guid)
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
        public OrderModel GetById(string Guid)
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
        public OrderModel GetByOrderNo(string OrderNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(OrderNo))
                    return null;
                return rp.GetByOrderNo(OrderNo);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 生成众筹订单以及订单中的商品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool PayCourse(OrderModel model)
        {
            try
            {
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    db.BeginTransaction();
                    string orderGuid = Guid.NewGuid().ToString();
                    model.Guid = orderGuid;
                    var value = db.Insert(model);
                    if (value != null)
                    {
                        T_UserOrder_Good ug = new T_UserOrder_Good
                        {
                            t_Order_Guid = orderGuid,
                            t_Date = DateTime.Now,
                            t_Associate_Guid = model.t_Associate_Guid,
                            Guid = Guid.NewGuid().ToString()
                        };
                        db.Insert(ug);
                    }
                    else
                        return false;
                    db.CompleteTransaction();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Save(OrderModel model)
        {
            try
            {
                if (model == null)
                    return false;
                var tt = GetById(model.Guid);
                if (tt != null)
                    return rp.Edit(model);
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    model.t_Order_Date = DateTime.Now;
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
