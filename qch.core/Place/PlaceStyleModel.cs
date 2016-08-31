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
        public string TypeText
        {
            get
            {
                string xy = "";
                switch (t_Place_Type)
                {
                    case 1:
                        xy = "按天出租";
                        break;
                    case 2:
                        xy = "按周出租";
                        break;
                    case 3:
                        xy = "按月出租";
                        break;
                    case 4:
                        xy = "按季度出租";
                        break;
                }
                return xy;
            }
        }
    }
}
