using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.Areas.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserMsg
    {
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string BranchName { get; set; }
        public int Tuijian1 { get; set; }
        public int Tuijian2 { get; set; }
        /// <summary>
        /// 直推实名认证人数
        /// </summary>
        public int Certification1 { get; set; }
        /// <summary>
        /// 简推实名认证人数
        /// </summary>
        public int Certification2 { get; set; }
    }
}