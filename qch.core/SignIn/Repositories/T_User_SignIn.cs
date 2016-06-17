using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户签到映射表
    /// </summary>
    [TableName("T_User_SignIn")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_User_SignIn
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 关联用户
        /// </summary>
        [Column]
        public string T_User_Guid { get; set; }
        /// <summary>
        /// 签到日期
        /// </summary>
        [Column]
        public DateTime T_SignIn_Date { get; set; }
        /// <summary>
        /// 签到赠送积分
        /// </summary>
        [Column]
        public int T_Integral { get; set; }
        /// <summary>
        /// 连续签到天数
        /// </summary>
        [Column]
        public int T_SignIn_Days { get; set; }
        /// <summary>
        /// 连续签到额外赠送的积分
        /// </summary>
        [Column]
        public int T_Extra_Integral { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [Column]
        public string T_Remark { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int T_DelState { get; set; }
    }
}
