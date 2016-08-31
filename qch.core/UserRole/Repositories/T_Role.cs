using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 角色映射表
    /// </summary>
    [TableName("T_Role")]
    [PrimaryKey("Id")]
    [ExplicitColumns]
    public class T_Role
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public bool? IsActive { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [Column]
        public string Remark { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Column]
        public string RoleName { get; set; }
    }
}
