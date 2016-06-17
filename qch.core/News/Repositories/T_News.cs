using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 文章映射表
    /// </summary>
    [TableName("T_News")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_News
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 新闻状态
        /// </summary>
        [Column]
        public int t_News_Style { get; set; }
        /// <summary>
        /// 新闻图片
        /// </summary>
        [Column]
        public string t_News_Pic { get; set; }
        /// <summary>
        /// 新闻辩题
        /// </summary>
        [Column]
        public string t_News_Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Column]
        public string t_News_LimitContents { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Column]
        public string t_News_Contents { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [Column]
        public string t_News_Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Column]
        public string t_News_City { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [Column]
        public string t_News_Author { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        [Column]
        public DateTime t_News_Date { get; set; }
        /// <summary>
        /// 点击量
        /// </summary>
        [Column]
        public int t_News_Counts { get; set; }
        /// <summary>
        /// index
        /// </summary>
        [Column]
        public int t_News_Index { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        [Column]
        public int t_News_Recommand { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
        /// <summary>
        /// 新闻样式
        /// </summary>
        //[Column]
        public string t_Style_Name { get; set; }
    }
}
