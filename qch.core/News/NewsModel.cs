using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 文章实体类
    /// </summary>
    public class NewsModel : T_News
    {
        StyleRepository srp = new StyleRepository();
        public string t_Style_Name
        {
            get
            {
                if (t_News_Style > 0)
                {
                    var m = srp.GetById(t_News_Style);
                    if (m != null)
                        return m.t_Style_Name;
                    else
                        return "";
                }
                else
                    return "";
            }
        }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool Recommand { get; set; }
        public int CateId1 { get; set; }
        public int CateId2 { get; set; }
        public int CateId3 { get; set; }
    }
}
