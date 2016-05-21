using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 项目映射表
    /// </summary>
    [TableName("T_Project")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Project
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 发起人guid
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 空间guid
        /// </summary>
        [Column]
        public string t_Place_Guid { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [Column]
        public string t_Project_Name { get; set; }
        /// <summary>
        /// 一句话描述
        /// </summary>
        [Column]
        public string t_Project_OneWord { get; set; }
        /// <summary>
        /// 项目简介
        /// </summary>
        [Column]
        public string t_Project_Instruction { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        [Column]
        public string t_Project_CityName { get; set; }
        /// <summary>
        /// 省份Id
        /// </summary>
        [Column]
        public int t_Project_Province { get; set; }
        /// <summary>
        /// 城市Id
        /// </summary>
        [Column]
        public int t_Project_City { get; set; }
        /// <summary>
        /// 区县Id
        /// </summary>
        [Column]
        public int t_Project_District { get; set; }
        /// <summary>
        /// 项目领域
        /// </summary>
        [Column]
        public int t_Project_Field { get; set; }
        /// <summary>
        /// 项目阶段
        /// </summary>
        [Column]
        public int t_Project_Phase { get; set; }
        /// <summary>
        /// 融资金额
        /// </summary>
        [Column]
        public string t_Project_Finance { get; set; }
        /// <summary>
        /// 融资金额用途
        /// </summary>
        [Column]
        public string t_Project_FinanceUse { get; set; }
        /// <summary>
        /// 融资阶段
        /// </summary>
        [Column]
        public int t_Project_FinancePhase { get; set; }
        /// <summary>
        /// 合伙人需求
        /// </summary>
        [Column]
        public string t_Project_ParterWant { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        [Column]
        public string t_Project_ConverPic { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        [Column]
        public int t_Project_Recommend { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        [Column]
        public int t_Project_Audit { get; set; }
        /// <summary>
        /// 是否入驻项目
        /// </summary>
        [Column]
        public int t_Project_In { get; set; }
        /// <summary>
        /// 是否路演项目
        /// </summary>
        [Column]
        public int t_Project_RoadShow { get; set; }
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
