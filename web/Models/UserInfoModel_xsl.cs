using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class UserInfoModel_xsl
    {
        /// <summary>
        /// 激活分
        /// </summary>
        public double activateMoney { get; set; }
        /// <summary>
        /// 奖金分
        /// </summary>
        public double bonusMoney { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 政策分
        /// </summary>
        public double onlyActivateMoney { get; set; }
        /// <summary>
        /// 密码，两次MD5
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string realName { get; set; }
        /// <summary>
        /// 购物分
        /// </summary>
        public double shoppingMoney { get; set; }
        /// <summary>
        /// 消费币
        /// </summary>
        public double specialEMoney { get; set; }
    }
}