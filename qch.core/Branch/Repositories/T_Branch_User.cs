using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 部门员工映射表
    /// </summary>
    [TableName("T_Branch_User")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Branch_User
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 管理用户
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 关联部门
        /// </summary>
        [Column]
        public string t_Branch_Guid { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        [Column]
        public string t_User_RealName { get; set; }
        /// <summary>
        /// 员工电话
        /// </summary>
        [Column]
        public string t_User_Phone { get; set; }
        /// <summary>
        /// 编辑人
        /// </summary>
        [Column]
        public string t_Editor { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Column]
        public DateTime t_CreateDate { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
