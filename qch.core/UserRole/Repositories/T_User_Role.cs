using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户角色映射表
    /// </summary>
    [TableName("T_User_Role")]
    [PrimaryKey("Id")]
    [ExplicitColumns]
    public class T_User_Role
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public int Id { get; set; }
        /// <summary>
        /// 关联用户
        /// </summary>
        [Column]
        public string UserGuid { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Column]
        public string RoleName { get; set; }
    }
}
