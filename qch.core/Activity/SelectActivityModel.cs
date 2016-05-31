using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectActivityModel
    {
        /// <summary>
        /// 活动id
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 关联用户
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// 活动图片
        /// </summary>
        public string ActivityPic { get; set; }
        /// <summary>
        /// 活动标题
        /// </summary>
        public string ActivityTitle { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 报名人数
        /// </summary>
        public int Members { get; set; }
    }
}
