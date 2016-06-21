using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 项目团队实体类
    /// </summary>
    public class ProjectTeamModel : T_Project_Team
    {
        public string UserName { get; set; }
        public string UserAvator { get; set; }
        public string UserPosition { get; set; }
    }
}
