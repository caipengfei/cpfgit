using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 活动映射类
    /// </summary>
    /// 
    [TableName("T_Activity")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Activity
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 活动标题
        /// </summary>
        [Column]
        public string t_Activity_Title { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        [Column]
        public string t_Activity_CoverPic { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        [Column]
        public DateTime t_Activity_sDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        [Column]
        public DateTime t_Activity_eDate { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [Column]
        public int t_Activity_Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Column]
        public int t_Activity_City { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        [Column]
        public int t_Activity_District { get; set; }
        /// <summary>
        /// 街道
        /// </summary>
        [Column]
        public string t_Activity_Street { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [Column]
        public string t_Activity_Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [Column]
        public string t_Activity_Longitude { get; set; }
        /// <summary>
        /// 活动说明
        /// </summary>
        [Column]
        public string t_Activity_Instruction { get; set; }
        /// <summary>
        /// 限制人数
        /// </summary>
        [Column]
        public int t_Activity_LimitPerson { get; set; }
        /// <summary>
        /// 费用类型
        /// </summary>
        [Column]
        public string t_Activity_FeeType { get; set; }
        /// <summary>
        /// 费用
        /// </summary>
        [Column]
        public decimal t_Activity_Fee { get; set; }
        /// <summary>
        /// 咨询电话
        /// </summary>
        [Column]
        public string t_Activity_Tel { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        [Column]
        public string t_Activity_CityName { get; set; }
        /// <summary>
        /// 主办方
        /// </summary>
        [Column]
        public string t_Activity_Holder { get; set; }
        /// <summary>
        /// 是否审核
        /// 0：未审核；
        /// 1：已审核
        /// </summary>
        [Column]
        public int t_Activity_Audit { get; set; }
        /// <summary>
        /// 是否推荐
        /// 0：未推荐；
        /// 1：已推荐
        /// </summary>
        [Column]
        public int t_Activity_Recommand { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        [Column]
        public DateTime t_AddDate { get; set; }
        /// <summary>
        /// 发布人
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
