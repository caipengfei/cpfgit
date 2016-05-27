using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 优惠券资源层
    /// </summary>
    public class VoucherRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<VoucherModel> rp = new Repository<VoucherModel>();

        #region 优惠券基础资源
        /// <summary>
        /// 根据全拼获取
        /// </summary>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public VoucherModel GetByAction(string Pinyin)
        {
            try
            {
                string sql = "select top 1 * from T_voucher where T_Voucher_Scope=@0 and [T_DelState]=0 and [T_Voucher_Audit]=1";
                return rp.Get(sql, new object[] { Pinyin });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<VoucherModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_Voucher where t_DelState=0 order by T_CreateDate desc";
                return rp.GetPageData(page, pagesize, sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// getbyid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public VoucherModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_Voucher where guid=@0";
                return rp.Get(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(VoucherModel model)
        {
            try
            {
                return rp.Insert(model) == null ? false : true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(VoucherModel model)
        {
            try
            {
                return (int)rp.Update(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Del(VoucherModel model)
        {
            try
            {
                return (int)rp.Delete(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 用户优惠券信息资源

        #endregion
    }
}
