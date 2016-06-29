using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 工作经历实体类
    /// </summary>
    public class HistoryWorkModel : T_HistoryWork
    {
        public string SDate
        {
            get {
                if (t_sDate != null)
                    return t_sDate.ToString("yyyy.MM");
                else
                    return "";
            }
        }
        public string EDate
        {
            get
            {
                if (t_eDate != null)
                    return t_eDate.ToString("yyyy.MM");
                else
                    return "至今";
            }
        }
    }
}
