using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 用户预约过的空间实体
    /// </summary>
    public class OrderedPlaceModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 预约人
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// 预约的空间名称
        /// </summary>
        public string PlaceName { get; set; }
        /// <summary>
        /// 预约单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 预约的场地类型
        /// </summary>
        public string PlaceStyle { get; set; }
        /// <summary>
        /// 预约的日期
        /// </summary>
        public string OrderDate { get; set; }
        /// <summary>
        /// 预约的开始时间点
        /// </summary>
        public int OrderStartTime { get; set; }
        /// <summary>
        /// 预约的结束时间点
        /// </summary>
        public int OrderEndTime { get; set; }
        /// <summary>
        /// 预约的空间封面图
        /// </summary>
        public string PlacePic { get; set; }
        /// <summary>
        /// 使用状态：
        /// 0：未到；
        /// 1：已到；
        /// 2：已离开；
        /// 3：已取消
        /// </summary>
        public int OrderState { get; set; }
        /// <summary>
        /// 预约的空间所在城市
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 预约的空间费用
        /// </summary>
        public decimal PlacePrice { get; set; }
    }
}
