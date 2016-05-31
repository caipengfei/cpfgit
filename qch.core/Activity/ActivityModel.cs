﻿using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 活动实体类
    /// </summary>
    public class ActivityModel : T_Activity
    {
        /// <summary>
        /// 报名人数
        /// </summary>
        public int Applys { get; set; }
    }
}
