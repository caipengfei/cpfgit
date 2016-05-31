using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 投资机构入驻成员
    /// </summary>
    public class InvestPlaceMember
    {
        /// <summary>
        /// 投资机构guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 入驻成员职位
        /// </summary>
        public string UserPosition { get; set; }
        /// <summary>
        /// 入驻成员头像
        /// </summary>
        public string UserPic { get; set; }
        /// <summary>
        /// 入驻成员姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 入驻成员用户guid
        /// </summary>
        public string UserGuid { get; set; }
    }
}
