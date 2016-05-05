using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 报名凭证实体类
    /// </summary>
    public class ProofModel
    {
        public string QrCodeImage
        {
            get {
                return "http://120.25.106.244:8002/Attach/Images/" + QrCode;
            }
        }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// 报名成功的凭证码
        /// </summary>
        public string ProofCode { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        public string QrCode { get; set; }
        /// <summary>
        /// 报名费用
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 活动开始日期
        /// </summary>
        public DateTime ActivityDate { get; set; }
        /// <summary>
        /// 活动地点
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 活动主办方
        /// </summary>
        public string Holder { get; set; }
        /// <summary>
        /// 报名人姓名
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 报名人电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        public string t_Activity_Guid { get; set; }
        /// <summary>
        /// 报名人id
        /// </summary>
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 报名id
        /// </summary>
        public string ApplyGuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Fee
        {
            get
            {

                if (Money > 0)
                    return Money.ToString();
                else
                    return "免费";
            }
        }
    }
}
