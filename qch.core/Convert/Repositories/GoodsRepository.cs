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

        #region 商品资源
        /// <summary>
        /// 获取所有积分兑换商品
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T_Goods_Convert> GetAll()
        {
            try
            {
                var model = rp.GetAll(" where t_delstate=0 order by t_Goods_Recmmend desc,t_Goods_Index desc");
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
        #endregion
    }
}
