using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 空间相关资源
    /// </summary>
    public class PlaceRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<PlaceModel> rRp = new Repository<PlaceModel>();
        Repository<PlaceOrderModel> poRp = new Repository<PlaceOrderModel>();
        Repository<PlaceOrderTimeModel> potRp = new Repository<PlaceOrderTimeModel>();
        Repository<PlaceStyleModel> psRp = new Repository<PlaceStyleModel>();
        Repository<OrderedPlaceModel> orderedRp = new Repository<OrderedPlaceModel>();

        #region 用户预约过的空间资源

        public bool IsOrderToday(string UserGuid)
        {
            try
            {
                string sql = "select * from T_Place_Ordered where t_User_Guid=@0 and t_State!=3 and t_delstate=0 and t_AddDate between @1 and @2";
                var model = orderedRp.Get(sql, new object[] { UserGuid, qch.Infrastructure.TimeHelper.GetStartDateTime(DateTime.Now), qch.Infrastructure.TimeHelper.GetEndDateTime(DateTime.Now) });
                if (model == null)
                    return true;
                else
                    return false;
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
                StringBuilder sql = new StringBuilder();
                sql.Append("select a.Guid,a.t_Ordered_NO as OrderNo,a.t_State as OrderState,b.t_Place_StyleName as PlaceStyle,b.t_Place_Pic as PlacePic,b.t_Place_Money as PlacePrice,c.t_Order_Date as OrderDate,d.t_PlaceOder_sTime as OrderStartTime,d.t_PlaceOder_eTime as OrderEndTime,e.t_Place_Name as PlaceName,e.t_Place_CityName as CityName ");
                sql.Append("from T_Place_Ordered as a ");
                sql.Append("left join T_PlaceOrder_Time as d on a.t_PlaceOrder_Guid=d.Guid ");
                sql.Append("left join T_Place_Order as c on d.t_PlaceOder_Guid=c.Guid ");
                sql.Append("left join T_Place_Style as b on c.t_PlaceStyle_Guid=b.Guid ");
                sql.Append("left join T_Place as e on b.t_Place_Guid=e.Guid ");
                sql.Append("where a.t_User_Guid=@0 and a.t_delstate=0");
                return orderedRp.GetPageData(page, pagesize, sql.ToString(), new object[] { UserGuid });
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

        #region 能预约的空间
        /// <summary>
        /// 获取某个空间类型的预约时间信息（日期）
        /// </summary>
        /// <param name="PlaceStyleGuid"></param>
        /// <returns></returns>
        public IEnumerable<PlaceOrderModel> GetPlaceOrder(string PlaceStyleGuid)
        {
            try
            {
                string sql = "select * from [T_Place_Order] where [t_PlaceStyle_Guid]=@0 and [t_DelState]=0 and t_Order_Date>='" + DateTime.Now + "' order by t_Order_Date";
                return poRp.GetAll(sql, new object[] { PlaceStyleGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion

        #region 空间能预约的时间
        /// <summary>
        /// 获取某个空间类型的预约时间信息（时间）
        /// </summary>
        /// <param name="PlaceStyleGuid"></param>
        /// <returns></returns>
        public IEnumerable<PlaceOrderTimeModel> GetPlaceOrderTime(string PlaceStyleGuid)
        {
            try
            {
                string sql = "select * from [T_PlaceOrder_Time] where [t_PlaceOder_Guid]=@0 and [t_DelState]=0 order by t_PlaceOder_sTime";
                return potRp.GetAll(sql, new object[] { PlaceStyleGuid });
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
                string sql = "select * from [T_Place_Style] where [t_Place_Guid]=@0 and [t_DelState]=0 order by [t_SortIndex]";
                return psRp.GetAll(sql, new object[] { PlaceGuid });
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
                string sql = "select * from [T_Place_Style] where [Guid]=@0 and [t_DelState]=0";
                return psRp.Get(sql, new object[] { Guid });
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
