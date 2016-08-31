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
        Repository<PlaceCaseModel> pcRp = new Repository<PlaceCaseModel>();

        /// <summary>
        /// 获取某个空间的孵化案例
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public PlaceCaseModel GetPlaceCase(string Guid)
        {
            try
            {
                string sql = "select a.Guid,b.t_Project_Name as ProjectName,b.t_Project_ConverPic as ProjectImage from T_Place_Project as a left join T_Project as b on a.t_Project_Guid=b.guid where a.t_State=1 and a.t_Type=0 and a.t_Place_Guid=@0 and b.t_delstate=0 and b.t_Project_Audit=1";
                return pcRp.Get(sql, new object[] { Guid });
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
                string sql = "select a.Guid,b.t_Project_Name as ProjectName,b.t_Project_ConverPic as ProjectImage from T_Place_Project as a left join T_Project as b on a.t_Project_Guid=b.guid where a.t_State=1 and a.t_Type=0 and a.t_Place_Guid=@0 and b.t_delstate=0 and b.t_Project_Audit=1";
                return pcRp.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

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
        /// <summary>
        /// 获取空间详情
        /// </summary>
        /// <param name="PlaceGuid"></param>
        /// <returns></returns>
        public PlaceModel GetPlaceInfo(string PlaceGuid)
        {
            try
            {
                string sql = "select * from T_Place where guid=@0 and t_DelState=0";
                return rRp.Get(sql, new object[] { PlaceGuid });
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
                Repository<SelectPlace> r = new Repository<SelectPlace>();
                string sql = "select a.Guid,a.t_Place_Name as PlaceName,a.t_Place_Street as PlaceAddr,b.t_Pic_Url as PlacePic from t_place as a left join T_Associate_Pic as b on b.t_Associate_Guid=a.Guid where a.t_delstate=0";
                return r.GetPageData(page, pagesize, sql);
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
                Repository<SelectPlace> r = new Repository<SelectPlace>();
                string sql = "select a.Guid,a.t_Place_Name as PlaceName,a.t_Place_Street as PlaceAddr,b.t_Pic_Url as PlacePic from t_place as a left join T_Associate_Pic as b on b.t_Associate_Guid=a.Guid where a.t_delstate=0 and a.t_user_guid=@0";
                return r.GetPageData(page, pagesize, sql, new object[] { UserGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public PetaPoco.Page<SelectPlace> GetAllPlace(int page, int pagesize, string UserGuid,int isAudit)
        {
            try
            {
                Repository<SelectPlace> r = new Repository<SelectPlace>();
                string sql = "select a.Guid,a.t_Place_Name as PlaceName,a.t_Place_Street as PlaceAddr,a.t_Place_ProvideService,a.t_Place_Tips,a.t_Place_CheckIn,b.t_Pic_Url as PlacePic from t_place as a left join T_Associate_Pic as b on b.t_Associate_Guid=a.Guid where a.t_delstate=0 and a.t_user_guid=@0 and a.t_Place_Audit=@1";
                return r.GetPageData(page, pagesize, sql, new object[] { UserGuid, isAudit });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
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
        /// 
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public PlaceOrderTimeModel GetPlaceOrderTimeById(string Guid)
        {
            try
            {
                string sql = "select * from T_PlaceOrder_Time where guid=@0 and t_delstate=0";
                return potRp.Get(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
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
                string sql = "select * from [T_Place_Style] where [t_Place_Guid]=@0 and [t_DelState]=0 order by [t_SortIndex],t_adddate desc";
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
