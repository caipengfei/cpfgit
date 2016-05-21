using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 用户点赞
    /// </summary>
    public class UserPraise
    {
        /// <summary>
        /// 点赞信息表的主键
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 话题表的主键
        /// </summary>
        public string TopicGuid { get; set; }
        /// <summary>
        /// 点赞用户的头像
        /// </summary>
        public string UserAvator { get; set; }
        /// <summary>
        /// 点赞用户的姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 点赞时间
        /// </summary>
        public DateTime t_Date { get; set; }
    }
}
