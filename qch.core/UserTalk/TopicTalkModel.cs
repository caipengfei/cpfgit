using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TopicTalkModel
    {
        public string Guid { get; set; }
        public string t_Talk_FromContent { get; set; }
        public string FromUser { get; set; }
        public string t_User_Pic { get; set; }
        public string ToUser { get; set; }
        public string t_Talk_FromUserGuid { get; set; }
        public string t_Talk_ToUserGuid { get; set; }
        /// <summary>
        /// 发评论人用户类型
        /// </summary>
        public int ToUserStyle { get; set; }
        /// <summary>
        /// 发评论人审核状态
        /// </summary>
        public int ToUserStyleAudit { get; set; }
        /// <summary>
        /// 回复人用户类型
        /// </summary>
        public int FromUserStyle { get; set; }
        /// <summary>
        /// 回复人审核状态
        /// </summary>
        public int FromUserStyleAudit { get; set; }
    }
}
