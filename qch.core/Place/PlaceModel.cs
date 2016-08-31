using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 空间信息实体类
    /// </summary>
    public class PlaceModel : T_Place
    {
        public IEnumerable<PicModel> Pics { get; set; }
    }
}
