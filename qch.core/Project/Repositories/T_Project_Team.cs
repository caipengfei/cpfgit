using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 项目团队映射表
    /// </summary>
    [TableName("T_Project_Team")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Project_Team
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
        /// 用户姓名
        /// </summary>
        [Column]
        public string t_User_Name { get; set; }
        /// <summary>
        /// 用户职位
        /// </summary>
        [Column]
        public string t_User_Position { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [Column]
        public string t_User_Pic { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [Column]
        public string t_User_Remark { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [Column]
        public int t_Audit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public DateTime t_AddDate { get; set; }
    }
}
