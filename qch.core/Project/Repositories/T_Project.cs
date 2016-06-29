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

        #region Model
        private string _guid;
        private string _t_user_guid;
        private string _t_place_guid;
        private string _t_project_name;
        private string _t_project_oneword;
        private string _t_project_instruction;
        private string _t_project_cityname;
        private int? _t_project_province;
        private int? _t_project_city;
        private int? _t_project_district;
        private int? _t_project_field;
        private int? _t_project_phase;
        private string _t_project_finance;
        private string _t_project_financeuse;
        private int? _t_project_financephase;
        private string _t_project_parterwant;
        private string _t_project_converpic;
        private int? _t_project_recommend = 0;
        private int? _t_project_audit = 0;
        private int? _t_project_in = 0;
        private int? _t_project_roadshow = 0;
        private string _t_project_profitway;
        private string _t_project_perfer;
        private string _t_project_client;
        private string _t_project_website;
        private string _t_project_link;
        private string _t_project_weixin;
        private DateTime? _t_adddate;
        private string _t_addby;
        private DateTime? _t_modifyddate;
        private string _t_modifyby;
        private int? _t_delstate = 0;
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public string Guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 发起人guid
        /// </summary>
        /// 
        [Column]
        public string t_User_Guid
        {
            set { _t_user_guid = value; }
            get { return _t_user_guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public string t_Place_Guid
        {
            set { _t_place_guid = value; }
            get { return _t_place_guid; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        /// 
        [Column]
        public string t_Project_Name
        {
            set { _t_project_name = value; }
            get { return _t_project_name; }
        }
        /// <summary>
        /// 一句话描述
        /// </summary>
        /// 
        [Column]
        public string t_Project_OneWord
        {
            set { _t_project_oneword = value; }
            get { return _t_project_oneword; }
        }
        /// <summary>
        /// 项目简介
        /// </summary>
        /// 
        [Column]
        public string t_Project_Instruction
        {
            set { _t_project_instruction = value; }
            get { return _t_project_instruction; }
        }
        /// <summary>
        /// 城市名称
        /// </summary>
        /// 
        [Column]
        public string t_Project_CityName
        {
            set { _t_project_cityname = value; }
            get { return _t_project_cityname; }
        }
        /// <summary>
        /// 省份Id
        /// </summary>
        /// 
        [Column]
        public int? t_Project_Province
        {
            set { _t_project_province = value; }
            get { return _t_project_province; }
        }
        /// <summary>
        /// 城市Id
        /// </summary>
        /// 
        [Column]
        public int? t_Project_City
        {
            set { _t_project_city = value; }
            get { return _t_project_city; }
        }
        /// <summary>
        /// 区县Id
        /// </summary>
        /// 
        [Column]
        public int? t_Project_District
        {
            set { _t_project_district = value; }
            get { return _t_project_district; }
        }
        /// <summary>
        /// 项目领域
        /// </summary>
        /// 
        [Column]
        public int? t_Project_Field
        {
            set { _t_project_field = value; }
            get { return _t_project_field; }
        }
        /// <summary>
        /// 项目阶段
        /// </summary>
        /// 
        [Column]
        public int? t_Project_Phase
        {
            set { _t_project_phase = value; }
            get { return _t_project_phase; }
        }
        /// <summary>
        /// 融资金额
        /// </summary>
        /// 
        [Column]
        public string t_Project_Finance
        {
            set { _t_project_finance = value; }
            get { return _t_project_finance; }
        }
        /// <summary>
        /// 融资金额用途
        /// </summary>
        /// 
        [Column]
        public string t_Project_FinanceUse
        {
            set { _t_project_financeuse = value; }
            get { return _t_project_financeuse; }
        }
        /// <summary>
        /// 融资阶段
        /// </summary>
        /// 
        [Column]
        public int? t_Project_FinancePhase
        {
            set { _t_project_financephase = value; }
            get { return _t_project_financephase; }
        }
        /// <summary>
        /// 合伙人需求
        /// </summary>
        /// 
        [Column]
        public string t_Project_ParterWant
        {
            set { _t_project_parterwant = value; }
            get { return _t_project_parterwant; }
        }
        /// <summary>
        /// 封面图片
        /// </summary>
        /// 
        [Column]
        public string t_Project_ConverPic
        {
            set { _t_project_converpic = value; }
            get { return _t_project_converpic; }
        }
        /// <summary>
        /// 是否推荐
        /// </summary>
        /// 
        [Column]
        public int? t_Project_Recommend
        {
            set { _t_project_recommend = value; }
            get { return _t_project_recommend; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        /// 
        [Column]
        public int? t_Project_Audit
        {
            set { _t_project_audit = value; }
            get { return _t_project_audit; }
        }
        /// <summary>
        /// 是否申请入驻项目
        /// </summary>
        /// 
        [Column]
        public int? t_Project_In
        {
            set { _t_project_in = value; }
            get { return _t_project_in; }
        }
        /// <summary>
        /// 是否路演项目
        /// </summary>
        /// 
        [Column]
        public int? t_Project_RoadShow
        {
            set { _t_project_roadshow = value; }
            get { return _t_project_roadshow; }
        }
        /// <summary>
        /// 盈利途径
        /// </summary>
        /// 
        [Column]
        public string t_Project_ProfitWay
        {
            set { _t_project_profitway = value; }
            get { return _t_project_profitway; }
        }
        /// <summary>
        /// 项目优势
        /// </summary>
        /// 
        [Column]
        public string t_Project_Perfer
        {
            set { _t_project_perfer = value; }
            get { return _t_project_perfer; }
        }
        /// <summary>
        /// 目标用户
        /// </summary>
        /// 
        [Column]
        public string t_Project_Client
        {
            set { _t_project_client = value; }
            get { return _t_project_client; }
        }
        /// <summary>
        /// 官网
        /// </summary>
        /// 
        [Column]
        public string t_Project_Website
        {
            set { _t_project_website = value; }
            get { return _t_project_website; }
        }
        /// <summary>
        /// app链接
        /// </summary>
        /// 
        [Column]
        public string t_Project_Link
        {
            set { _t_project_link = value; }
            get { return _t_project_link; }
        }
        /// <summary>
        /// 微信链接
        /// </summary>
        /// 
        [Column]
        public string t_Project_Weixin
        {
            set { _t_project_weixin = value; }
            get { return _t_project_weixin; }
        }
        /// <summary>
        /// 添加日期
        /// </summary>
        /// 
        [Column]
        public DateTime? t_AddDate
        {
            set { _t_adddate = value; }
            get { return _t_adddate; }
        }
        /// <summary>
        /// 添加人
        /// </summary>
        /// 
        [Column]
        public string t_AddBy
        {
            set { _t_addby = value; }
            get { return _t_addby; }
        }
        /// <summary>
        /// 修改日期
        /// </summary>
        /// 
        [Column]
        public DateTime? t_ModifydDate
        {
            set { _t_modifyddate = value; }
            get { return _t_modifyddate; }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        /// 
        [Column]
        public string t_ModifyBy
        {
            set { _t_modifyby = value; }
            get { return _t_modifyby; }
        }
        /// <summary>
        /// 删除状态
        /// </summary>
        /// 
        [Column]
        public int? t_DelState
        {
            set { _t_delstate = value; }
            get { return _t_delstate; }
        }
        #endregion Model
    }
}
