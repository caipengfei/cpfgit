using qch.Repositoryies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 动态实体类
    /// </summary>
    public class TopicModel : T_Topic
    {
        /// <summary>
        /// 距离
        /// </summary>
        public string Distance { get; set; }
    }
}
