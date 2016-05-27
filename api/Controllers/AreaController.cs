using qch.core;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class AreaController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AreaService areaService = new AreaService();

        //获取所有的省份
        [HttpGet]
        public IEnumerable<inf_province> GetCity()
        {
            var model = areaService.GetAllProvince();
            return model;
        }
        //根据省获取市
        [HttpGet]
        public IEnumerable<inf_city> GetCity(long Id)
        {
            var model = areaService.GetCityByProvince(Id);
            return model;
        }
        //根据市获取区县
        [HttpGet]
        public IEnumerable<inf_district> GetDis(long Id)
        {
            var model = areaService.GetDistrictByCity(Id);
            return model;
        }
    }
}
