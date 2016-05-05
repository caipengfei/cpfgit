using qch.Models;
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
        /// 处理支付业务
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public Msg Pay(string orderNo)
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
                        if (string.IsNullOrWhiteSpace(order.t_Associate_Guid))
                        {
                            //表示充值订单
                            //处理充值后续业务
                            order.t_Order_Remark = "微信充值";
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
                        else
                        {
                            //表示活动订单，处理活动支付后续业务
                            //处理生成活动报名信息
                            //do
                            var apply = db.SingleOrDefault<T_Activity_Apply>(" where guid=@0 and t_DelState=1", new object[] { order.t_Order_Remark });
                            if (apply != null)
                            {
                                apply.t_DelState = 0;
                                db.Save(apply);
                            }
                        }
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
