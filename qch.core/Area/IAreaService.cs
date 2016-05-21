using System;
using System.Collections.Generic;
using qch.Models;
using qch.Repositories;
using cpf.core;

namespace qch.core
{
    /// <summary>
    /// 省市区
    /// </summary>
    public interface IAreaService : IDependency
    {
        /// <summary>
        /// 获取所有的区域信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<AreaModel> GetAll();
        /// <summary>
        /// 获取所有的省
        /// </summary>
        /// <returns></returns>
        IEnumerable<inf_province> GetAllProvince();
        /// <summary>
        /// 根据某市的信息
        /// </summary>
        /// <param name="Id">市Id</param>
        /// <returns></returns>
        inf_city GetCity(int Id);
        /// <summary>
        /// 根据区名字获取 搜索用  cpf 8月29日添加
        /// </summary>
        /// <param name="DisName"></param>
        /// <returns></returns>
        inf_district GetCityByDisName(string DisName);
        /// <summary>
        /// 根据省获取市
        /// </summary>
        /// <param name="ProvinceId"></param>
        /// <returns></returns>
        IEnumerable<inf_city> GetCityByProvince(int ProvinceId);
        /// <summary>
        /// 根据市获取区
        /// </summary>
        /// <param name="CityId"></param>
        /// <returns></returns>
        IEnumerable<inf_district> GetDistrictByCity(int CityId);
        /// <summary>
        /// 获取某个省
        /// </summary>
        /// <param name="ProvinceId"></param>
        /// <returns></returns>
        inf_province GetProvince(int ProvinceId);
    }
}
