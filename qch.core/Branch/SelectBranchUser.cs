using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectBranchUser
    {
        public string Guid { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string Editor { get; set; }
        public string CreateDate { get; set; }
    }
}
