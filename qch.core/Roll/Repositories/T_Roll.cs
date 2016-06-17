using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 转盘抽奖映射表
    /// </summary>
    [TableName("T_Roll")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Roll
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 转盘标题
        /// </summary>
        [Column]
        public string t_Roll_Title { get; set; }
        /// <summary>
        /// 类型
        /// 1：优惠券；
        /// 2：积分；
        /// 3：实物
        /// </summary>
        [Column]
        public int t_Roll_Type { get; set; }
        /// <summary>
        /// 奖励关联优惠券
        /// </summary>
        [Column]
        public string t_Voucher_Guid { get; set; }
        /// <summary>
        /// 奖励积分
        /// </summary>
        [Column]
        public int t_Integral { get; set; }
        /// <summary>
        /// 关联商品
        /// </summary>
        [Column]
        public string t_RollGoods_Guid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string t_Roll_Remark { get; set; }
        /// <summary>
        /// 几率
        /// </summary>
        [Column]
        public int t_Roll_Probability { get; set; }
        /// <summary>
        /// 奖励Id
        /// </summary>
        [Column]
        public int t_Roll_Reward { get; set; }
        /// <summary>
        /// 关联商品图片
        /// </summary>
        [Column]
        public string t_Roll_Pic { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        [Column]
        public DateTime t_AddDate { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
