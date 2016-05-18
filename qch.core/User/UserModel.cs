using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    public class UserModel : T_Users
    {
        public string UserTypeText
        {
            get {
                string xy = "游客";
                switch (t_User_Style)
                {
                    case 1:
                        xy = "创客";
                        break;
                    case 2:
                        xy = "投资人";
                        break;
                    case 3:
                        xy = "合伙人";
                        break;
                    case 8:
                        xy = "微信用户";
                        break;
                }
                return xy;
            }
        }
    }
}
