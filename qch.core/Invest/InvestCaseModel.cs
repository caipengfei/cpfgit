using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 投资案例实体类
    /// </summary>
    public class InvestCaseModel : T_Invest_Case
    {
        //StyleRepository styleRp = new StyleRepository();
        //public object InvestPhase
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(t_Invest_Phase))
        //            return null;
        //        else
        //            return styleRp.GetByIds(t_Invest_Phase);
        //    }
        //}
        public object InvestPhase { get; set; }
    }
}
