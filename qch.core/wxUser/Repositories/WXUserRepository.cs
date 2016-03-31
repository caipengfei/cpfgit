using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户资源层
    /// </summary>
    public class WXUserRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<WXUserModel> rp = new Repository<WXUserModel>();

        /// <summary>
        /// 分页获取所有微信用户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<WXUserModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_User_Weixin";
                return rp.GetPageData(page, pagesize, sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// getbyuserid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public WXUserModel GetByUserId(string Guid)
        {
            try
            {
                string sql = "select * from T_User_Weixin where userguid=@0";
                return rp.Get(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// getbyopenid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public WXUserModel GetByOpenId(string OpenId)
        {
            try
            {
                string sql = "select * from T_User_Weixin where OpenId=@0";
                return rp.Get(sql, new object[] { OpenId });
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
        public WXUserModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_User_Weixin where guid=@0";
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
        public bool Add(WXUserModel model)
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
        public bool Edit(WXUserModel model)
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
        public bool Del(WXUserModel model)
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
