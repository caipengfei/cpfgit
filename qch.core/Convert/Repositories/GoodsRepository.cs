using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 商品资源层
    /// </summary>
    public class GoodsRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<T_Goods_Convert> rp = new Repository<T_Goods_Convert>();
        Repository<T_Goods_Convert_List> listRp = new Repository<T_Goods_Convert_List>();
        Repository<GsModel> gsrp = new Repository<GsModel>();

        #region 商品资源
        /// <summary>
        /// 获取所有积分兑换商品
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T_Goods_Convert> GetAll()
        {
            try
            {
                var model = rp.GetAll(" where t_delstate=0 and t_goods_type in(0,1) order by t_Goods_Recmmend desc,t_Goods_Index desc");
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public T_Goods_Convert GetDetail(string Guid)
        {
            try
            {
                var model = rp.Get(" where (guid=@0 or t_goods_code=@1) and t_delstate=0", new object[] { Guid, Guid });
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion
        #region 商品兑换记录资源
        /// <summary>
        /// 获取某用户的兑换商品记录
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public IEnumerable<T_Goods_Convert_List> GetList(string UserGuid)
        {
            try
            {
                var model = listRp.GetAll(" where t_user_guid=@0 and t_delstate=0", new object[] { UserGuid });
                return model;
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
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<T_Goods_Convert_List> GetList(int page, int pagesize,string type, string UserGuid)
        {
            try
            {
                string sql="select a.*,b.t_Goods_Name,b.t_Goods_Pic  from T_Goods_Convert_List as a left join T_Goods_Convert as b on a.t_Goods_Guid=b.Guid where a.t_user_guid=@0 and a.t_List_Type=@1 and a.t_delstate=0";
                var model = listRp.GetPageData(page, pagesize, sql, new object[] { UserGuid, type });
                return model;
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
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public T_Goods_Convert_List GetById(string Guid)
        {
            try
            {
                string sql = "select a.*,b.t_Goods_Name,b.t_Goods_Pic,c.t_Cnee_Name,c.t_Cnee_Phone,c.t_Cnee_Addr  from T_Goods_Convert_List as a left join T_Goods_Convert as b on a.t_Goods_Guid=b.Guid left join T_User_Cnee as c on a.t_Cnee_Guid=c.Guid where a.guid=@0 and a.t_delstate=0";
                var model = listRp.Get(sql, new object[] { Guid });
                return model;
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
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<GsModel> GetList1(int page, int pagesize, string type, string UserGuid)
        {
            try
            {
                string sql = "select a.*,b.t_Goods_Name,b.t_Goods_Pic,b.t_Need_Integral  from T_Goods_Convert_List as a left join T_Goods_Convert as b on a.t_Goods_Guid=b.Guid where a.t_user_guid=@0 and a.t_List_Type=@1 and a.t_delstate=0 order by a.t_convert_createdate desc";
                var model = gsrp.GetPageData(page, pagesize, sql, new object[] { UserGuid, type });
                return model;
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
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public GsModel GetById1(string Guid)
        {
            try
            {
                string sql = "select a.*,b.t_Goods_Name,b.t_Goods_Pic,b.t_Need_Integral,c.t_Cnee_Name,c.t_Cnee_Phone,c.t_Cnee_Addr  from T_Goods_Convert_List as a left join T_Goods_Convert as b on a.t_Goods_Guid=b.Guid left join T_User_Cnee as c on a.t_Cnee_Guid=c.Guid where a.guid=@0 and a.t_delstate=0";
                var model = gsrp.Get(sql, new object[] { Guid });
                return model;
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
