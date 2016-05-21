using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectTalkModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 关联主键Guid
        /// </summary>
        public string t_Associate_Guid { get; set; }
        /// <summary>
        /// 回复人Guid
        /// </summary>
        public string t_Talk_FromUserGuid { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string t_Talk_FromContent { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime t_Talk_FromDate { get; set; }
        /// <summary>
        /// 回复人Guid
        /// </summary>
        public string t_Talk_ToUserGuid { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string t_Talk_ToContent { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime t_Talk_ToDate { get; set; }
        /// <summary>
        /// 赞
        /// </summary>
        public int t_Talk_Good { get; set; }
        /// <summary>
        /// 踩
        /// </summary>
        public int t_Talk_Bad { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public int t_Talk_Audit { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        public int t_DelState { get; set; }
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
