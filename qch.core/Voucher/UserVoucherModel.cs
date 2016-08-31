using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 用户优惠券明细实体类
    /// </summary>
    public class UserVoucherModel : T_User_Voucher
    {
        VoucherRepository rp = new VoucherRepository();

        public T_Voucher VoucherInfo
        {
            get
            {
                var model = rp.GetById(T_Voucher_Guid);
                if (model != null)
                    return model;
                else
                    return new T_Voucher();
            }
        }
        public DateTime EndDate
        {
            get {
                if (VoucherInfo.T_sDate != null)
                    return VoucherInfo.T_sDate.AddDays(90);
                else
                    return DateTime.Now;
            }
        }
    }
}
