using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    public interface IUserTalkService
    {
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        UserTalkModel GetById(string Guid);
        /// <summary>
        /// 获取某个对象的所有评论
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        IEnumerable<UserTalkModel> GetAll(string Guid);
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Save(UserTalkModel model);
    }
}
