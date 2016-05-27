using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 关注管理映射类
    /// </summary>
    [TableName("T_User_Foucs")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_User_Foucs
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 被关注的人
        /// </summary>
        [Column]
        public string t_Focus_Guid { get; set; }
        /// <summary>
        /// 用户Guid
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
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
