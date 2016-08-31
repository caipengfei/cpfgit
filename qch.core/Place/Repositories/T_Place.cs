using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 空间信息映射表
    /// </summary>
    [TableName("T_Place")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Place
    {
        #region Model
        private string _guid;
        private int? _t_place_groupid;
        private string _t_place_name;
        private string _t_place_oneword;
        private int? _t_place_province;
        private int? _t_place_city;
        private int? _t_place_district;
        private string _t_place_cityname;
        private string _t_place_districtname;
        private string _t_place_street;
        private string _t_place_latitude;
        private string _t_place_longitude;
        private string _t_place_checkin;
        private string _t_place_policy;
        private string _t_place_provideservice;
        private string _t_place_instruction;
        private string _t_place_tips;
        private int? _t_place_audit = 0;
        private int? _t_place_recommand = 0;
        private int? _t_place_ifin = 0;
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
        /// 关联用户
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public int? t_Place_GroupID
        {
            set { _t_place_groupid = value; }
            get { return _t_place_groupid; }
        }
        /// <summary>
        /// 空间名称
        /// </summary>
        /// 
        [Column]
        public string t_Place_Name
        {
            set { _t_place_name = value; }
            get { return _t_place_name; }
        }
        /// <summary>
        /// 一句话描述
        /// </summary>
        /// 
        [Column]
        public string t_Place_OneWord
        {
            set { _t_place_oneword = value; }
            get { return _t_place_oneword; }
        }
        /// <summary>
        /// 省
        /// </summary>
        /// 
        [Column]
        public int? t_Place_Province
        {
            set { _t_place_province = value; }
            get { return _t_place_province; }
        }
        /// <summary>
        /// 市
        /// </summary>
        /// 
        [Column]
        public int? t_Place_City
        {
            set { _t_place_city = value; }
            get { return _t_place_city; }
        }
        /// <summary>
        /// 区
        /// </summary>
        /// 
        [Column]
        public int? t_Place_District
        {
            set { _t_place_district = value; }
            get { return _t_place_district; }
        }
        /// <summary>
        /// 城市名称
        /// </summary>
        /// 
        [Column]
        public string t_Place_CityName
        {
            set { _t_place_cityname = value; }
            get { return _t_place_cityname; }
        }
        /// <summary>
        /// 区县名称
        /// </summary>
        /// 
        [Column]
        public string t_Place_DistrictName
        {
            set { _t_place_districtname = value; }
            get { return _t_place_districtname; }
        }
        /// <summary>
        /// 街道
        /// </summary>
        /// 
        [Column]
        public string t_Place_Street
        {
            set { _t_place_street = value; }
            get { return _t_place_street; }
        }
        /// <summary>
        /// 纬度
        /// </summary>
        /// 
        [Column]
        public string t_Place_Latitude
        {
            set { _t_place_latitude = value; }
            get { return _t_place_latitude; }
        }
        /// <summary>
        /// 经度
        /// </summary>
        /// 
        [Column]
        public string t_Place_Longitude
        {
            set { _t_place_longitude = value; }
            get { return _t_place_longitude; }
        }
        /// <summary>
        /// 入住条件
        /// </summary>
        /// 
        [Column]
        public string t_Place_CheckIn
        {
            set { _t_place_checkin = value; }
            get { return _t_place_checkin; }
        }
        /// <summary>
        /// 政策扶持
        /// </summary>
        /// 
        [Column]
        public string t_Place_Policy
        {
            set { _t_place_policy = value; }
            get { return _t_place_policy; }
        }
        /// <summary>
        /// 提供服务
        /// </summary>
        /// 
        [Column]
        public string t_Place_ProvideService
        {
            set { _t_place_provideservice = value; }
            get { return _t_place_provideservice; }
        }
        /// <summary>
        /// 详细介绍
        /// </summary>
        /// 
        [Column]
        public string t_Place_Instruction
        {
            set { _t_place_instruction = value; }
            get { return _t_place_instruction; }
        }
        /// <summary>
        /// 标签
        /// </summary>
        /// 
        [Column]
        public string t_Place_Tips
        {
            set { _t_place_tips = value; }
            get { return _t_place_tips; }
        }
        /// <summary>
        /// 是否审核
        /// </summary>
        /// 
        [Column]
        public int? t_Place_Audit
        {
            set { _t_place_audit = value; }
            get { return _t_place_audit; }
        }
        /// <summary>
        /// 是否推荐
        /// </summary>
        /// 
        [Column]
        public int? t_Place_Recommand
        {
            set { _t_place_recommand = value; }
            get { return _t_place_recommand; }
        }
        /// <summary>
        /// 是否允许项目入驻
        /// </summary>
        /// 
        [Column]
        public int? t_Place_IfIn
        {
            set { _t_place_ifin = value; }
            get { return _t_place_ifin; }
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
