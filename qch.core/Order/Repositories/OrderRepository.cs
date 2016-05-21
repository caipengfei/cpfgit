using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 订单资源层
    /// </summary>
    public class OrderRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<OrderModel> rp = new Repository<OrderModel>();

        /// <summary>
        /// 获取众筹金额（已众筹了多少钱）
        /// </summary>
        /// <param name="Guid">众筹guid</param>
        /// <returns></returns>
        public decimal GetFundCourseMoney(string Guid)
        {
            try
            {
                decimal money = 0;
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select sum(t_Order_Money) from T_User_Order where t_Order_State=1 and t_DelState=0 and t_Associate_Guid=" + Guid;
                    money = Convert.IsDBNull(db.ExecuteScalar<object>(sql)) ? 0 : db.ExecuteScalar<decimal>(sql);
                }
                return money;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 获取某个人的所有订单
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<OrderModel> GetAll(string Guid)
        {
            try
            {
                string sql = "select * from T_User_Order where t_User_Guid=@0 and t_DelState=0";
                return rp.GetAll(sql, new object[] { Guid });
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
                string sql = "select * from T_User_Order where Guid=@0";
                return rp.Get(sql, new object[] { Guid });
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
                string sql = "select * from T_User_Order where t_Order_No=@0";
                return rp.Get(sql, new object[] { OrderNo });
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
        public bool Add(OrderModel model)
        {
            try
            {
                var db = rp.Insert(model);
                if (db != null)
                    return true;
                else
                    return false;
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
        public bool Edit(OrderModel model)
        {
            try
            {
                var db = rp.Insert(model);
                if (db != null)
                    return true;
                else
                    return false;
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
        public bool Del(OrderModel model)
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
    }
}
