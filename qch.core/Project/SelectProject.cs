using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectProject
    {
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Pic { get; set; }
        public string Contents { get; set; }
        public string t_User_RealName { get; set; }
        public string t_Project_Field { get; set; }
        public string t_Project_FinancePhase { get; set; }
    }
}
