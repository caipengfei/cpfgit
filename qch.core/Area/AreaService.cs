using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using qch.Models;
using qch.Repositories;

namespace qch.core
{
    /// <summary>
    /// 区域信息业务层
    /// </summary>
    public class AreaService
    {
        ProvinceRepository provinceRepository = new ProvinceRepository();
        CityRepository cityRepository = new CityRepository();
        DistrictRepository districtRepository = new DistrictRepository();
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取所有的区域信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AreaModel> GetAll()
        {
            try
            {
                List<AreaModel> target = new List<AreaModel>();
                var provinces = GetAllProvince();//获取所有的省
                if (provinces != null && provinces.Count() > 0)
                {
                    target = provinces.Select(p => new AreaModel
                      {
                          ProvinceId = p.ProvinceID,//省份id
                          Province = p.ProvinceName,//省份名称
                          CityList = GetCityByProvince(p.ProvinceID).Select(c => new City
                          {
                              CityId = c.CityID, //市iD
                              CityName = c.CityName,//市名称
                              DistrictList = GetDistrictByCity(c.CityID).Select(d =>
                                  new District { DistrictId = d.DistrictID, DistrictName = d.DistrictName }).ToList()
                          }).ToList()
                      }).ToList(); 
                }

                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return null;
            }
        }


        /// <summary>
        /// 获取所有的省
        /// </summary>
        /// <returns></returns>
        public IEnumerable<inf_province> GetAllProvince()
        {
            try
            {
                var model = provinceRepository.GetAll(" order by ProvinceID", null);
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// 获取某个省
        /// </summary>
        /// <param name="ProvinceId"></param>
        /// <returns></returns>
        public inf_province GetProvince(long ProvinceId)
        {
            try
            {
                var model = provinceRepository.Get("where ProvinceID=@0", new object[] { ProvinceId });
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// 根据省获取市
        /// </summary>
        /// <param name="ProvinceId"></param>
        /// <returns></returns>
        public IEnumerable<inf_city> GetCityByProvince(long ProvinceId)
        {
            try
            {
                var model = cityRepository.GetAll("where ProvinceID=@0", new object[] { ProvinceId });
                return model;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// 获取某个市
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public inf_city GetCity(long Id)
        {
            try
            {
                var model = cityRepository.Get("where CityID=@0", new object[] { Id });
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// 根据区名字获取 搜索用
        /// </summary>
        /// <param name="DisName"></param>
        /// <returns></returns>
        public inf_district GetCityByDisName(string DisName)
        {
            try
            {
                var model = districtRepository.Get("where DistrictName=@0", new object[] { DisName });
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return null;
            }
        }
        /// <summary>
        /// 根据市获取区
        /// </summary>
        /// <param name="CityId"></param>
        /// <returns></returns>
        public IEnumerable<inf_district> GetDistrictByCity(long CityId)
        {
            try
            {
                var model = districtRepository.GetAll("where CityID=@0", new object[] { CityId });
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return null;
            }
        }
    }
}
