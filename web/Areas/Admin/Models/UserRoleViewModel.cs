using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.Areas.Admin.Models
{
    public class UserRoleViewModel
    {
        public string UserGuid { get; set; }

        public string UserName { get; set; }
        /// <summary>
        /// 系统所有角色
        /// </summary>
        public IEnumerable<qch.Models.RoleModel> Roles { get; set; }
        /// <summary>
        /// 用户所有角色
        /// </summary>
        public string UserRoles { get; set; }
    }
}