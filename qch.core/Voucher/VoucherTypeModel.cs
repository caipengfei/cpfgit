using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    public class VoucherTypeModel
    {
        public string VoucherType { get; set; }
        public int VoucherCount { get; set; }
        public int VoucherMoney { get; set; }
        public int T_Voucher_Type { get; set; }
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
