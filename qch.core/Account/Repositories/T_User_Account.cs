using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 账目映射表
    /// </summary>
    [TableName("T_User_Account")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_User_Account
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [Column]
        public string t_UserAccount_No { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 增加金额
        /// </summary>
        [Column]
        public decimal t_UserAccount_AddReward { get; set; }
        /// <summary>
        /// 减少金额
        /// </summary>
        [Column]
        public decimal t_UserAccount_ReduceReward { get; set; }
        /// <summary>
        /// 当前余额
        /// </summary>
        [Column]
        public decimal t_UserAccount_Reward { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column]
        public string t_Remark { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [Column]
        public DateTime t_AddDate { get; set; }
        /// <summary>
        /// 0：减少，1:后台充值2：创业币充值3：推荐获得4：订单返佣
        /// </summary>
        [Column]
        public int t_UserAccount_Type { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
