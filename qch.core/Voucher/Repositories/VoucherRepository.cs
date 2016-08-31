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
        Repository<UserVoucherModel> uvRp = new Repository<UserVoucherModel>();

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
        /// <summary>
        /// 获取某用户的某种行为产生的优惠券
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="Guid"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserVoucherModel> GetAlluvByUser(int page, int pagesize, string Guid, string actionName)
        {
            try
            {
                string sql = "select * from T_User_Voucher where t_DelState=0 and T_User_Guid=@0 and T_Voucher_Guid in(select guid from T_Voucher where [T_Voucher_Scope]=@1 and T_Voucher_Audit=1 and t_delstate=0)";
                return uvRp.GetPageData(page, pagesize, sql, new object[] { Guid, actionName });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某用户的某种类型的优惠券
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="Guid"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserVoucherModel> GetAlluvByUser(int page, int pagesize, string Guid, int typeId)
        {
            try
            {
                string sql = "select * from T_User_Voucher where t_DelState=0 and T_User_Guid=@0 and T_Voucher_Guid in(select guid from T_Voucher where T_Voucher_Type=@1 and T_Voucher_Audit=1 and t_delstate=0) order by t_getdate desc";
                return uvRp.GetPageData(page, pagesize, sql, new object[] { Guid, typeId });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某用户的所有优惠券
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserVoucherModel> GetAlluvByUser(int page, int pagesize, string Guid)
        {
            try
            {
                string sql = "select * from T_User_Voucher where t_DelState=0 and T_User_Guid=@0";
                return uvRp.GetPageData(page, pagesize, sql, new object[] { Guid });
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
        public PetaPoco.Page<UserVoucherModel> GetAlluv(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_User_Voucher where t_DelState=0";
                return uvRp.GetPageData(page, pagesize, sql);
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
        public UserVoucherModel GetuvById(string Guid)
        {
            try
            {
                string sql = "select * from T_User_Voucher where guid=@0";
                return uvRp.Get(sql, new object[] { Guid });
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
        public bool Adduv(UserVoucherModel model)
        {
            try
            {
                return uvRp.Insert(model) == null ? false : true;
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
        public bool Edituv(UserVoucherModel model)
        {
            try
            {
                return (int)uvRp.Update(model) > 0 ? true : false;
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
        public bool Deluv(UserVoucherModel model)
        {
            try
            {
                return (int)uvRp.Delete(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        #endregion
    }
}
