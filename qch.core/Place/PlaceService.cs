using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 空间相关业务
    /// </summary>
    public class PlaceService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PlaceRepository rp = new PlaceRepository();

        #region 用户预约过的空间业务
        /// <summary>
        /// 获取某用户的空间预约信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<OrderedPlaceModel> GetOrderedByUser(int page, int pagesize, string UserGuid)
        {
            try
            {
                return rp.GetOrderedByUser(page, pagesize, UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion

        #region 空间信息
        #endregion

        #region 能预约的空间时间信息（日期）
        /// <summary>
        /// 获取某个空间类型的预约时间信息（日期）
        /// </summary>
        /// <param name="PlaceStyleGuid"></param>
        /// <returns></returns>
        public IEnumerable<PlaceOrderModel> GetPlaceOrder(string PlaceStyleGuid)
        {
            try
            {
                return rp.GetPlaceOrder(PlaceStyleGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion

        #region 空间能预约的空间时间信息（时间）
        /// <summary>
        /// 获取某个空间类型的预约时间信息（时间）
        /// </summary>
        /// <param name="PlaceStyleGuid"></param>
        /// <returns></returns>
        public IEnumerable<PlaceOrderTimeModel> GetPlaceOrderTime(string PlaceStyleGuid)
        {
            try
            {
                return rp.GetPlaceOrderTime(PlaceStyleGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion

        #region 空间类型
        /// <summary>
        /// 获取某个空间的所有类型
        /// </summary>
        /// <param name="PlaceGuid"></param>
        /// <returns></returns>
        public IEnumerable<PlaceStyleModel> GetPlaceStyle(string PlaceGuid)
        {
            try
            {
                return rp.GetPlaceStyle(PlaceGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
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
                return rp.GetPlaceStyleByGuid(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
