using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 查询报名信息
    /// </summary>
    public class ApplyModel
    {
        /// <summary>
        /// 报名信息id
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserAvator { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        public string ActivityGuid { get; set; }
    }
}
