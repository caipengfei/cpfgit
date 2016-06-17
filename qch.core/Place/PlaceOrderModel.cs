using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 能预约的空间实体类
    /// </summary>
    public class PlaceOrderModel : T_Place_Order
    {
        public IEnumerable<PlaceOrderTimeModel> PlaceTimes { get; set; }
    }
}
