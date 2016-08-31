using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 用户角色业务层
    /// </summary>
    public class UserRoleService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserRoleRepository rp = new UserRoleRepository();

        #region 系统角色业务
        /// <summary>
        /// 查询角色名称是否存在
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public bool IsExist(string RoleName)
        {
            try
            {
                return rp.IsExist(RoleName);
            }
            catch (Exception)
            {
                return false;
            }
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
                return rp.GetAllRole();
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
                return rp.GetAllRole(page, pagesize);
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
                if (Id <= 0)
                    return null;
                return rp.GetRoleById(Id);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存系统角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveRole(RoleModel model)
        {
            try
            {
                if (model == null)
                    return false;
                var tt = GetRoleById(model.Id);
                if (tt != null)
                    return rp.EditRole(model);
                else
                    return rp.AddRole(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除系统角色
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DelRole(int Id)
        {
            try
            {
                var model = GetRoleById(Id);
                if (model == null)
                    return false;
                return rp.DelRole(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        #endregion
        #region 用户角色业务
        /// <summary>
        ///获取指定用户所有角色字符串
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns>返回结果用逗号隔开</returns>
        public string GetUserRole(string UserGuid)
        {
            try
            {
                return rp.GetUserRole(UserGuid);
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
                return rp.ClearRole(UserGuid);
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 保存用户角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Save(UserRoleModel model)
        {
            try
            {
                if (model == null)
                    return false;
                var tt = GetById(model.Id);
                if (tt != null)
                    return rp.Edit(model);
                else
                    return rp.Add(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
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
                if (Id <= 0)
                    return null;
                return rp.GetById(Id);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
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
                return rp.GetAll(Guid);
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
