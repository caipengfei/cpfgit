using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 优惠券基础信息实体类
    /// </summary>
    public class VoucherModel : T_Voucher
    {
        /// <summary>
        /// 优惠券类型
        /// </summary>
        public string VoucherTypeText
        {
            get
            {
                if (T_Voucher_Type == 1)
                    return "抵扣券";
                else if (T_Voucher_Type == 2)
                    return "代用券";
                else if (T_Voucher_Type == 3)
                    return "随机折扣券";
                else
                    return "";
            }
        }
    }
}
