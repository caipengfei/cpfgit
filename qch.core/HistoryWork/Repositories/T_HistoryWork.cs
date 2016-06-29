using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户工作经历映射表
    /// </summary>
    [TableName("T_Topic")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_HistoryWork
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
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        [Column]
        public DateTime t_sDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        [Column]
        public DateTime t_eDate { get; set; }
        /// <summary>
        /// 公司（文字）
        /// </summary>
        [Column]
        public string t_Commpany { get; set; }
        /// <summary>
        /// 职位（文字）
        /// </summary>
        [Column]
        public string t_Position { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        [Column]
        public string t_Date { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
