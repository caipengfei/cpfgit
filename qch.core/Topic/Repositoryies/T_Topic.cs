using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositoryies
{
    /// <summary>
    /// 动态映射表
    /// </summary>
    [TableName("T_Topic")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Topic
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 发布人Guid
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 动态内容
        /// </summary>
        [Column]
        public string t_Topic_Contents { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        [Column]
        public string t_Topic_City { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [Column]
        public string t_Topic_Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [Column]
        public string t_Topic_Latitude { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        [Column]
        public string t_Topic_Address { get; set; }
        /// <summary>
        /// 置顶
        /// </summary>
        [Column]
        public int t_Topic_Top { get; set; }
        /// <summary>
        /// 发布时间
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
