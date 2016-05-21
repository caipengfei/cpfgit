using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户角色资源层
    /// </summary>
    public class UserRoleRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<UserRoleModel> rp = new Repository<UserRoleModel>();


        /// <summary>
        /// 获取某个用户的所有角色
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<UserRoleModel> GetAll(string Guid)
        {
            try
            {
                string sql = "select * from t_user_Role where UserGuid=@0";
                return rp.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public UserRoleModel GetById(string Id)
        {
            try
            {
                string sql = "select * from t_user_role where id=@0";
                return rp.Get(sql, new object[] { Id });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 添加评论信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(UserRoleModel model)
        {
            try
            {
                var db = rp.Insert(model);
                if (db != null)
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
        /// 编辑评论信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(UserRoleModel model)
        {
            try
            {
                var db = rp.Insert(model);
                if (db != null)
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
        /// 删除评论信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Del(UserRoleModel model)
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
