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

        /// <summary>
        /// 获取某个空间的孵化案例
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public PlaceCaseModel GetPlaceCase(string Guid)
        {
            try
            {
                return rp.GetPlaceCase(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public IEnumerable<PlaceCaseModel> GetPlaceCaseAll(string Guid)
        {
            try
            {
                return rp.GetPlaceCaseAll(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        #region 用户预约过的空间业务
        public bool IsOrderToday(string UserGuid)
        {
            try
            {
                return rp.IsOrderToday(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="PlaceName"></param>
        /// <returns></returns>
        public PlaceModel GetPlaceInfo(string UserGuid, string PlaceName)
        {
            try
            {
                return rp.GetPlaceInfo(UserGuid, PlaceName);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存空间信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SavePlace(PlaceModel model)
        {
            try
            {
                if (model == null)
                    return false;
                var mm = GetPlaceStyleByGuid(model.Guid);
                if (mm != null)
                {
                    model.t_ModifydDate = DateTime.Now;
                    return rp.EditPlace(model);
                }
                else
                {
                    model.t_AddDate = DateTime.Now;
                    model.Guid = Guid.NewGuid().ToString();
                    return rp.AddPlace(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 修改某个空间的删除状态（删除）
        /// </summary>
        /// <param name="RoomGuid"></param>
        /// <returns></returns>
        public bool DelPlace(string PlaceGuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PlaceGuid))
                    return false;
                var model = GetPlaceInfo(PlaceGuid);
                if (model == null)
                    return false;
                model.t_DelState = 1;
                return rp.EditPlace(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 获取空间详情
        /// </summary>
        /// <param name="PlaceGuid"></param>
        /// <returns></returns>
        public PlaceModel GetPlaceInfo(string PlaceGuid)
        {
            try
            {
                return rp.GetPlaceInfo(PlaceGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有空间信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<SelectPlace> GetAllPlace(int page, int pagesize)
        {
            try
            {
                return rp.GetAllPlace(page, pagesize);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public PetaPoco.Page<SelectPlace> GetAllPlace(int page, int pagesize, string UserGuid, int isAudit)
        {
            try
            {
                return rp.GetAllPlace(page, pagesize, UserGuid, isAudit);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取某个用户上传的所有空间信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<SelectPlace> GetAllPlace(int page, int pagesize, string UserGuid)
        {
            try
            {
                return rp.GetAllPlace(page, pagesize, UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
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
        /// 
        /// </summary>
        /// <param name="RoomName"></param>
        /// <returns></returns>
        public PlaceStyleModel GetPlaceStyleByName(string RoomName)
        {
            try
            {
                return rp.GetPlaceStyleByName(RoomName);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存房间信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveRoom(PlaceStyleModel model)
        {
            try
            {
                if (model == null)
                    return false;
                var mm = GetPlaceStyleByGuid(model.Guid);
                if (mm != null)
                {
                    model.t_ModifydDate = DateTime.Now;
                    return rp.EditPlaceRoom(model);
                }
                else
                {
                    model.t_AddDate = DateTime.Now;
                    model.Guid = Guid.NewGuid().ToString();
                    return rp.AddPlaceRoom(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 修改某个房间的删除状态（删除）
        /// </summary>
        /// <param name="RoomGuid"></param>
        /// <returns></returns>
        public bool DelRoom(string RoomGuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(RoomGuid))
                    return false;
                var model = GetPlaceStyleByGuid(RoomGuid);
                if (model == null)
                    return false;
                model.t_DelState = 1;
                return rp.EditPlaceRoom(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
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
