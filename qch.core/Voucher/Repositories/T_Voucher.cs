using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 优惠券基础信息映射表
    /// </summary>
    [TableName("T_Voucher")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Voucher
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 优惠券面值
        /// </summary>
        [Column]
        public string T_Voucher_Price { get; set; }
        /// <summary>
        /// 优惠券获得途径/行为
        /// </summary>
        [Column]
        public string T_Voucher_FromIn { get; set; }
        /// <summary>
        /// 优惠券创建日期
        /// </summary>
        [Column]
        public DateTime T_CreateDate { get; set; }
        /// <summary>
        /// 使用时间限制的开始时间
        /// </summary>
        [Column]
        public DateTime T_sDate { get; set; }
        /// <summary>
        /// 使用时间限制的截止时间
        /// </summary>
        [Column]
        public DateTime T_eDate { get; set; }
        /// <summary>
        /// 优惠券类型
        /// 1：抵扣券；
        /// 2：代用券；
        /// 3：随机折扣券
        /// </summary>
        [Column]
        public int T_Voucher_Type { get; set; }
        /// <summary>
        /// 优惠券使用范围
        /// </summary>
        [Column]
        public string T_Voucher_Scope { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [Column]
        public string T_Remark { get; set; }
        /// <summary>
        /// 审核状态
        /// 0：未审核；
        /// 1：已审核
        /// </summary>
        [Column]
        public int T_Voucher_Audit { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int T_DelState { get; set; }
    }
}
