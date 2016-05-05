using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qch.Models
{
    /// <summary>
    /// 评论实体类
    /// </summary>
    public class UserTalkModel : T_User_Talk
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserAvator { get; set; }
    }
}
