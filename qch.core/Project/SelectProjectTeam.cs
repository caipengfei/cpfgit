using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    public class SelectProjectTeam
    {
        public string Guid { get; set; }
        public string t_Project_Guid { get; set; }
        public string t_User_Guid { get; set; }
        public string UserName { get; set; }
        public string UserAvator { get; set; }
        public string UserPosition { get; set; }
        public string UserRemark { get; set; }
    }
}
