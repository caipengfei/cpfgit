using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 动态点赞映射表
    /// </summary>
    [TableName("T_Praise")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Praise
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 话题主键
        /// </summary>
        [Column]
        public string t_Associate_Guid { get; set; }
        /// <summary>
        /// 用户guid
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [Column]
        public string t_Remark { get; set; }
        /// <summary>
        /// 点赞时间
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
