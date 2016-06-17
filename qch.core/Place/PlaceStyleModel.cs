using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 空间类型实体类
    /// </summary>
    public class PlaceStyleModel : T_Place_Style
    {
        PicRepository picRp = new PicRepository();
        /// <summary>
        /// 空间类型图片
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
