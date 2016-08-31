using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 评论查询实体
    /// </summary>
    public class SelectModel
    {
        /// <summary>
        /// 关联主键Guid
        /// </summary>
        public string t_Associate_Guid { get; set; }
        /// <summary>
        /// 评论人Guid
        /// </summary>
        public string t_Talk_FromUserGuid { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string t_Talk_FromContent { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime t_Talk_FromDate { get; set; }
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
