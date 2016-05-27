using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 导师实体类
    /// </summary>
    public class LecturerModel : T_Lecturer
    {
        PicRepository picRp = new PicRepository();
        /// <summary>
        /// 导师风采展示
        /// </summary>
        public IEnumerable<PicModel> Pics
        {
            get
            {
                var model = picRp.GetByGuid(Guid);
                if (model != null)
                    return model;
                else
                    return new List<PicModel>();
            }
        }
    }
}
