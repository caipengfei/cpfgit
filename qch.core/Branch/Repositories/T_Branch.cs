using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 部门映射表
    /// </summary>
    [TableName("T_Branch")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Branch
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [Column]
        public string t_Branch_Name { get; set; }
        /// <summary>
        /// 上级部门
        /// </summary>
        [Column]
        public int t_ParentId { get; set; }
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
