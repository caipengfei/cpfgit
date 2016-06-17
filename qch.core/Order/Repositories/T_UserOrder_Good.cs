using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 订单中的商品映射表
    /// </summary>
    [TableName("T_UserOrder_Good")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_UserOrder_Good
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 关联订单
        /// </summary>
        [Column]
        public string t_Order_Guid { get; set; }
        /// <summary>
        /// 关联商品
        /// </summary>
        [Column]
        public string t_Associate_Guid { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        [Column]
        public DateTime t_Date { get; set; }
    }
}
