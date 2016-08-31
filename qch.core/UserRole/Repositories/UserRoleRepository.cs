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
        Repository<RoleModel> rp1 = new Repository<RoleModel>();

        #region 系统角色资源
        /// <summary>
        /// 角色名是否存在
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public bool IsExist(string RoleName)
        {
            var role = rp1.Get("select * from t_role where RoleName=@0", new object[] { RoleName });
            return role == null ? false : true;
        }

        /// <summary>
        /// 获取系统中所有角色
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<RoleModel> GetAllRole()
        {
            try
            {
                string sql = "select * from t_Role";
                return rp1.GetAll(sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public PetaPoco.Page<RoleModel> GetAllRole(int page, int pagesize)
        {
            try
            {
                string sql = "select * from t_Role";
                return rp1.GetPageData(page, pagesize, sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// GetRoleById
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public RoleModel GetRoleById(int Id)
        {
            try
            {
                string sql = "select * from t_role where id=@0";
                return rp1.Get(sql, new object[] { Id });
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
        public bool AddRole(RoleModel model)
        {
            try
            {
                var db = rp1.Insert(model);
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
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditRole(RoleModel model)
        {
            try
            {
                var db = rp1.Update(model);
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
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DelRole(RoleModel model)
        {
            try
            {
                return (int)rp1.Delete(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        #endregion
        #region 用户角色资源
        /// <summary>
        ///获取指定用户所有角色字符串
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns>返回结果用逗号隔开</returns>
        public string GetUserRole(string UserGuid)
        {
            try
            {
                string target = "";
                var UserRoles = rp.GetAll("select * from t_user_role where UserGuid=@0", new object[] { UserGuid });
                if (UserRoles != null)
                {
                    var roles = UserRoles.ToList();
                    foreach (var item in roles)
                    {
                        if (roles.IndexOf(item) == 0)
                        {
                            target += item.RoleName;
                        }
                        else
                        {
                            target += "," + item.RoleName;
                        }
                    }
                }
                return target;
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        /// 清除某用户的所有角色
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public bool ClearRole(string UserGuid)
        {
            try
            {
                ///获取用户现有角色
                var userRoles = rp.GetAll("select * from t_user_role where UserGuid=@0", new object[] { UserGuid });
                if (userRoles != null && userRoles.Count() > 0)
                {
                    foreach (var item in userRoles)
                    {
                        this.Del(item);
                    }

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
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
        public UserRoleModel GetById(int Id)
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
        /// 添加
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
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(UserRoleModel model)
        {
            try
            {
                var db = rp.Update(model);
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
        /// 删除
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
        #endregion
    }
}
