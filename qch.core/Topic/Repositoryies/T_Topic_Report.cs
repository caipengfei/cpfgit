using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositoryies
{
    /// <summary>
    /// 动态举报映射表
    /// </summary>
    [TableName("T_Topic_Report")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Topic_Report
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
        /// 被举报用户id
        /// </summary>
        [Column]
        public string t_Topic_Guid { get; set; }
        /// <summary>
        /// 举报内容
        /// </summary>
        [Column]
        public string t_Report_Remark { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [Column]
        public DateTime t_Date { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
