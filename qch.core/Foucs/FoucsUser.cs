using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 关注的人
    /// </summary>
    public class FoucsUser
    {
        /// <summary>
        /// 关注表的guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 被关注人的guid
        /// </summary>
        public string t_Focus_Guid { get; set; }
        /// <summary>
        /// 粉丝姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 粉丝头像
        /// </summary>
        public string UserAvator { get; set; }
        /// <summary>
        /// 粉丝Guid
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// 关注日期
        /// </summary>
        public DateTime FoucsDate { get; set; }
    }
}
