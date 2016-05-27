using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 图片映射表
    /// </summary>
    [TableName("T_Associate_Pic")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Associate_Pic
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 关联guid
        /// </summary>
        [Column]
        public string t_Associate_Guid { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        [Column]
        public string t_Pic_Url { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [Column]
        public string t_Pic_Remark { get; set; }
        /// <summary>
        /// 添加时间
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
