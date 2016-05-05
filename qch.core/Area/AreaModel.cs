using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 区域信息
    /// </summary>
    public class AreaModel
    {
        /// <summary>
        /// 省份Id
        /// </summary>
        public long ProvinceId { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 省包含的市列表
        /// </summary>
        public List<City> CityList { get; set; }
    }

    /// <summary>
    /// 市信息
    /// </summary>
    public class City
    {
        /// <summary>
        /// 市Id
        /// </summary>
        public long CityId { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 市包含的区列表
        /// </summary>
        public List<District> DistrictList { get; set; }
    }

    /// <summary>
    /// 区信息
    /// </summary>
    public class District
    {
        /// <summary>
        /// 区Id
        /// </summary>
        public long DistrictId { get; set; }
        /// <summary>
        /// 区名称
        /// </summary>
        public string DistrictName { get; set; }
    }

  
}
