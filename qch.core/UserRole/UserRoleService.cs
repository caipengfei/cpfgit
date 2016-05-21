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
    }
}
