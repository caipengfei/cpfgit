using qch.Repositories;
using qch.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 众筹实体类
    /// </summary>
    public class FundCourseModel : T_FundCourse
    {
        public IEnumerable<UserTalkModel> Talk { get; set; }
        //UserTalkRepository userTalkRp = new UserTalkRepository();
        ///// <summary>
        ///// 评论列表
        ///// </summary>
        //public IEnumerable<UserTalkModel> Talk
        //{
        //    get
        //    {
        //        var model = userTalkRp.GetAll(1, 10, Guid);
        //        if (model != null && model.Items != null)
        //            return model.Items;
        //        else
        //            return null;
        //    }
        //}
    }
}
