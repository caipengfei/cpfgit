using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 系统样式、属性映射表
    /// </summary>
    [TableName("T_Style")]
    [PrimaryKey("Id")]
    [ExplicitColumns]
    public class T_Style
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public int Id { get; set; }
        /// <summary>
        /// 样式名称
        /// </summary>
        [Column]
        public string t_Style_Name { get; set; }
        /// <summary>
        /// 样式图片
        /// </summary>
        [Column]
        public string t_Style_Pic { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column]
        public string t_Style_Remark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Column]
        public int t_SortIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public int t_fId { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        [Column]
        public DateTime t_AddDate { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        [Column]
        public string t_AddBy { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        [Column]
        public DateTime t_ModifydDate { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [Column]
        public string t_ModifyBy { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
