using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 订单映射表
    /// </summary>
    [TableName("T_User_Order")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_User_Order
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 关联的用户优惠券id
        /// </summary>
        [Column]
        public string T_UserVoucher_Guid { get; set; }
        /// <summary>
        /// 关联用户
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 关联对象
        /// </summary>
        [Column]
        public string t_Associate_Guid { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [Column]
        public int t_Order_OrderType { get; set; }
        /// <summary>
        /// 订单支付类型
        /// </summary>
        [Column]
        public string t_Order_PayType { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [Column]
        public string t_Order_No { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        [Column]
        public decimal t_Order_Money { get; set; }
        /// <summary>
        /// 订单名称
        /// </summary>
        [Column]
        public string t_Order_Name { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [Column]
        public DateTime t_Order_Date { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        [Column]
        public int t_Order_State { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [Column]
        public string t_Order_Remark { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
