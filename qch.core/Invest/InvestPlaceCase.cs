using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 投资机构接收项目实体类
    /// </summary>
    public class InvestPlaceCase
    {
        /// <summary>
        /// 投资机构id
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 项目图标
        /// </summary>
        public string ProjectPic { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public string ProjectGuid { get; set; }
    }
}
