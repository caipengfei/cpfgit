using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户收货地址资源层
    /// </summary>
    public class CneeRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<CneeModel> rp = new Repository<CneeModel>();

        /// <summary>
        /// 判断是否重复添加时用
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public CneeModel GetReceipt(string addr, string name, string phone)
        {
            try
            {
                string sql = "select top 1 * from t_User_Cnee where t_Cnee_Name=@0 and t_Cnee_Phone=@1 and t_Cnee_Addr=@2 and t_delstate=0";
                return rp.Get(sql, new object[] { name, phone, addr });
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// 获取某用户的默认收货地址
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public CneeModel GetDefaultReceipt(string UserGuid)
        {
            try
            {
                string sql = "select top 1 * from t_User_Cnee where t_IsDefault=1 and t_delstate=0 and t_user_guid=@0";
                return rp.Get(sql, new object[] { UserGuid });
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// 获取某个用户的所有收货地址
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IEnumerable<CneeModel> GetReceiptByUserId(string UserGuid)
        {
            try
            {
                string sql = "select * from t_User_Cnee where t_user_guid=@0 and t_delstate=0";
                return rp.GetAll(sql, new object[] { UserGuid });
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// 获取某个用户的最近5个收货地址
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IEnumerable<CneeModel> GetReceiptByUserIdTop5(string UserGuid)
        {
            try
            {
                string sql = "select top 5 * from t_User_Cnee where t_user_guid=@0 and t_delstate=0 order by ... desc";
                return rp.GetAll(sql, new object[] { UserGuid });
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// getbyid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public CneeModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from t_User_Cnee where guid=@0";
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
        public bool Add(CneeModel model)
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
        public bool Edit(CneeModel model)
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
        public bool Del(CneeModel model)
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
    }
}
