using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AccountRepository rp = new AccountRepository();


        /// <summary>
        /// 获取某人的所有流水
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<AccountModel> GetAll(string Guid)
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
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public AccountModel GetById(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                return rp.GetById(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Save(AccountModel model)
        {
            try
            {
                if (model == null)
                    return false;
                var tt = GetById(model.Guid);
                if (tt != null)
                    return rp.Edit(model);
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    model.t_AddDate = DateTime.Now;
                    return rp.Add(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
    }
}
