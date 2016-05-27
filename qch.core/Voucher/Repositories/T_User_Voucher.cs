using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户优惠券明细映射表
    /// </summary>
    [TableName("T_User_Voucher")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_User_Voucher
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 关联的优惠券基础信息
        /// </summary>
        [Column]
        public string T_Voucher_Guid { get; set; }
        /// <summary>
        /// 关联的用户id
        /// </summary>
        [Column]
        public string T_User_Guid { get; set; }
        /// <summary>
        /// 优惠券密码
        /// </summary>
        [Column]
        public string T_Voucher_Pwd { get; set; }
        /// <summary>
        /// 使用状态：
        /// 0：未使用；
        /// 1：已使用
        /// </summary>
        [Column]
        public int T_Voucher_State { get; set; }
        /// <summary>
        /// 优惠券获得日期
        /// </summary>
        [Column]
        public DateTime T_GetDate { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int T_DelState { get; set; }
    }
}
