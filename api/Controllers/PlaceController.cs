using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    /// <summary>
    /// 空间预约
    /// </summary>
    public class PlaceController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PlaceService service = new PlaceService();
        PicService picService = new PicService();
        ActivityApplyService aaService = new ActivityApplyService();

        /// <summary>
        /// 获取某用户的服务信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="typeId"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public object GetMyServices(int page, int pagesize, int typeId, string UserGuid)
        {
            try
            {
                if (typeId == 1)
                    return service.GetOrderedByUser(page, pagesize, UserGuid);
                else if (typeId == 2)
                    return aaService.GetProofList(page, pagesize, UserGuid);
                else if (typeId == 3)
                    return null;
                //{
                //    using (var db = new PetaPoco.Database("qch"))
                //    {
                //        var model = db.Query<object>("select a.t_Course_Title,a.t_Course_Pic,t_Course_Times,a.t_Course_Price, from ")
                //    }
                //}
                else
                    return null;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取可预约的空间类型（青创汇）
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PlaceStyleModel> GetPlaceStyle()
        {
            try
            {
                return service.GetPlaceStyle("7d2d5371-b96d-4a8d-8629-4e29932101b4");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取能预约的时间
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<PlaceOrderModel> GetTimes(string Guid)
        {
            try
            {
                var model = service.GetPlaceOrder(Guid);
                if (model != null && model.Count() > 0)
                {
                    foreach (var item in model)
                    {
                        item.PlaceTimes = service.GetPlaceOrderTime(item.Guid);
                    }
                }
                //var data = service.GetPlaceOrder(Guid);
                //var time = service.GetPlaceOrderTime(Guid);
                //var target = new
                //{
                //    d = data,
                //    t = time
                //};
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public PlaceStyleModel GetPlaceStyleByGuid(string Guid)
        {
            try
            {
                return service.GetPlaceStyleByGuid(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
