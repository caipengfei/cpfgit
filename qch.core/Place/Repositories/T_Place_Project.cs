using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 空间信息映射表
    /// </summary>
    [TableName("T_Place_Project")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Place_Project
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 关联项目
        /// </summary>
        [Column]
        public string t_Project_Guid { get; set; }
        /// <summary>
        /// 关联用户
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 关联空间
        /// </summary>
        [Column]
        public string t_Place_Guid { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [Column]
        public int t_State { get; set; }
        /// <summary>
        /// 区分路演和入驻
        /// 0：入驻；
        /// 1：路演
        /// </summary>
        [Column]
        public int t_Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public DateTime t_AddDate { get; set; }
    }
}
