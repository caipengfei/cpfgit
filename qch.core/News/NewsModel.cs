using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 文章实体类
    /// </summary>
    public class NewsModel : T_News
    {
        
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool Recommand { get; set; }
        public int CateId1 { get; set; }
        public int CateId2 { get; set; }
        public int CateId3 { get; set; }
    }
}
