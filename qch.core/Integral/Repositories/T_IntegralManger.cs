using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 积分基础信息映射表
    /// </summary>
    [TableName("T_IntegralManger")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_IntegralManger
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 产生积分的行为名称
        /// </summary>
        [Column]
        public string t_Name { get; set; }
        /// <summary>
        /// 产生积分的行为拼音（全拼）
        /// </summary>
        [Column]
        public string t_PinYin { get; set; }
        /// <summary>
        /// 产生多少积分
        /// </summary>
        [Column]
        public int t_Integral { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [Column]
        public string t_Reamrk { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
