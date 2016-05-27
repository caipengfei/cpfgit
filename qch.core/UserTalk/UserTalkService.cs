using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qch.core
{
    /// <summary>
    /// 评论业务层
    /// </summary>
    public class UserTalkService : IUserTalkService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserTalkRepository rp = new UserTalkRepository();


        /// <summary>
        /// 分页获取某个对象的所有评论
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public PetaPoco.Page<SelectTalkModel> GetAll(int page, int pagesize, string Guid)
        {
            try
            {
                return rp.GetAll(page, pagesize, Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某个对象的所有评论
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<UserTalkModel> GetAll(string Guid)
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
        public UserTalkModel GetById(string Guid)
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
        public bool Save(UserTalkModel model)
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
                    model.t_Talk_FromDate = DateTime.Now;
                    model.t_Talk_ToDate = DateTime.Now;
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
