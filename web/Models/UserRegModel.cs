using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.Models
{
    /// <summary>
    /// 用户注册模型
    /// </summary>
    public class UserRegModel
    {
        public string Phone { get; set; }
        public string SafeCode { get; set; }
        public string TjUser { get; set; }
        public string Password { get; set; }
    }
}