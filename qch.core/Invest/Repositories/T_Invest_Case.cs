using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 投资案例映射表
    /// </summary>
    [TableName("T_Invest_Case")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Invest_Case
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 关联用户
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 投资项目
        /// </summary>
        [Column]
        public string t_Invest_Project { get; set; }
        /// <summary>
        /// 投资阶段
        /// </summary>
        [Column]
        public string t_Invest_Phase { get; set; }
        /// <summary>
        /// 投资时间
        /// </summary>
        [Column]
        public DateTime t_Invest_Date { get; set; }
        /// <summary>
        /// 关联用户
        /// </summary>
        [Column]
        public string t_Invest_Pic { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        [Column]
        public DateTime t_Date { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
