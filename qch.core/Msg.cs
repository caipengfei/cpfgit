using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qch.core
{
    /// <summary>
    /// 公用处理结果类
    /// </summary>
    public class Msg
    {
        /// <summary>
        /// 返回结果error/success
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 处理结果
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 返回url
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 其它信息
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 其它结果
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// 关联订单号
        /// </summary>
        public string OrderNo { get; set; }
        public string PayType { get; set; }
        public decimal PayMoney { get; set; }
    }
}
