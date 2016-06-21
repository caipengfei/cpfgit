using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 积分实体类
    /// </summary>
    public class IntegralModel : T_User_Integral
    {
        /// <summary>
        /// 是否为增加
        /// </summary>
        public bool IsAdd
        {
            get
            {
                if (t_UserIntergral_AddReward > 0)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 增加或减少文本
        /// </summary>
        public string AddOrReduce
        {
            get
            {
                if (t_UserIntergral_AddReward > 0)
                    return "+" + t_UserIntergral_AddReward.ToString();
                else
                    return "-" + t_UserIntegral_ReduceReward.ToString();
            }
        }
    }
}
