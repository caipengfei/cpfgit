using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectFundCourse
    {
        /// <summary>
        /// 已众筹了多少钱
        /// </summary>
        public decimal NowMoney { get; set; }
        public IEnumerable<SelectTalkModel> Talk { get; set; }
        #region Model
        private string _guid = "0";
        private string _t_lecturerguid;
        private string _t_fundcourse_pic;
        private string _t_fundcourse_title;
        private decimal? _t_fundcourse_money;
        private decimal? _t_paymoney_online;
        private decimal? _t_paymoney_offline;
        private string _t_fundcourse_info;
        private string _t_fundcourse_oneword;
        private string _t_fundcourse_tip;
        private DateTime? _t_fundcourse_date;
        private string _t_fundcourse_stime;
        private string _t_fundcourse_etime;
        private int? _t_fundcourse_count;
        private int? _t_fundcourse_sort = 0;
        private int? _t_fundcourse_recommend = 0;
        private int? _t_fundcourse_good = 0;
        private int? _t_fundcourse_bad = 0;
        private int? _t_fundcourse_limitperson = 0;
        private string _t_fundcourse_remark;
        private string _t_fundcourse_style;
        private DateTime? _t_adddate;
        private int? _t_delstate;
        private int? _t_fundcourse_province;
        private int? _t_fundcourse_city;
        private int? _t_fundcourse_district;
        private string _t_fundcourse_street;
        private int _t_fundCourse_state;
        /// <summary>
        /// guid
        /// </summary>
        ///
        public string Guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 关联导师
        /// </summary>
        ///
        public string T_LecturerGuid
        {
            set { _t_lecturerguid = value; }
            get { return _t_lecturerguid; }
        }
        /// <summary>
        /// 众筹封面图
        /// </summary>
        ///
        public string T_FundCourse_Pic
        {
            set { _t_fundcourse_pic = value; }
            get { return _t_fundcourse_pic; }
        }
        /// <summary>
        /// 众筹标题
        /// </summary>
        ///
        public string T_FundCourse_Title
        {
            set { _t_fundcourse_title = value; }
            get { return _t_fundcourse_title; }
        }
        /// <summary>
        /// 众筹金额
        /// </summary>
        ///
        public decimal? T_FundCourse_Money
        {
            set { _t_fundcourse_money = value; }
            get { return _t_fundcourse_money; }
        }
        /// <summary>
        /// 在线观看支付金额
        /// </summary>
        ///
        public decimal? T_PayMoney_Online
        {
            set { _t_paymoney_online = value; }
            get { return _t_paymoney_online; }
        }
        /// <summary>
        /// 线下观看支付金额
        /// </summary>
        ///
        public decimal? T_PayMoney_Offline
        {
            set { _t_paymoney_offline = value; }
            get { return _t_paymoney_offline; }
        }
        /// <summary>
        /// 众筹详情
        /// </summary>
        ///
        public string T_FundCourse_Info
        {
            set { _t_fundcourse_info = value; }
            get { return _t_fundcourse_info; }
        }
        /// <summary>
        /// 众筹一句话描述
        /// </summary>
        ///
        public string T_FundCourse_OneWord
        {
            set { _t_fundcourse_oneword = value; }
            get { return _t_fundcourse_oneword; }
        }
        /// <summary>
        /// 众筹标签
        /// </summary>
        ///
        public string T_FundCourse_Tip
        {
            set { _t_fundcourse_tip = value; }
            get { return _t_fundcourse_tip; }
        }
        /// <summary>
        /// 开课时间
        /// </summary>
        ///
        public DateTime? T_FundCourse_Date
        {
            set { _t_fundcourse_date = value; }
            get { return _t_fundcourse_date; }
        }
        /// <summary>
        /// 众筹开始日期
        /// </summary>
        ///
        public string T_FundCourse_sTime
        {
            set { _t_fundcourse_stime = value; }
            get { return _t_fundcourse_stime; }
        }
        /// <summary>
        /// 众筹截止日期
        /// </summary>
        ///
        public string T_FundCourse_eTime
        {
            set { _t_fundcourse_etime = value; }
            get { return _t_fundcourse_etime; }
        }
        /// <summary>
        /// 点击次数
        /// </summary>
        ///
        public int? T_FundCourse_Count
        {
            set { _t_fundcourse_count = value; }
            get { return _t_fundcourse_count; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        ///
        public int? T_FundCourse_Sort
        {
            set { _t_fundcourse_sort = value; }
            get { return _t_fundcourse_sort; }
        }
        /// <summary>
        /// 推荐
        /// </summary>
        ///
        public int? T_FundCourse_Recommend
        {
            set { _t_fundcourse_recommend = value; }
            get { return _t_fundcourse_recommend; }
        }
        /// <summary>
        /// 好评次数
        /// </summary>
        ///
        public int? T_FundCourse_Good
        {
            set { _t_fundcourse_good = value; }
            get { return _t_fundcourse_good; }
        }
        /// <summary>
        /// 差评次数
        /// </summary>
        ///
        public int? T_FundCourse_Bad
        {
            set { _t_fundcourse_bad = value; }
            get { return _t_fundcourse_bad; }
        }
        /// <summary>
        /// 限制人数
        /// </summary>
        ///
        public int? T_FundCourse_LimitPerson
        {
            set { _t_fundcourse_limitperson = value; }
            get { return _t_fundcourse_limitperson; }
        }
        /// <summary>
        /// 备注信息
        /// </summary>
        ///
        public string T_FundCourse_Remark
        {
            set { _t_fundcourse_remark = value; }
            get { return _t_fundcourse_remark; }
        }
        /// <summary>
        /// 样式
        /// </summary>
        ///
        public string T_FundCourse_Style
        {
            set { _t_fundcourse_style = value; }
            get { return _t_fundcourse_style; }
        }
        /// <summary>
        /// 添加日期
        /// </summary>
        ///
        public DateTime? T_AddDate
        {
            set { _t_adddate = value; }
            get { return _t_adddate; }
        }
        /// <summary>
        /// 删除状态
        /// </summary>
        ///
        public int? T_DelState
        {
            set { _t_delstate = value; }
            get { return _t_delstate; }
        }
        /// <summary>
        /// 省
        /// </summary>
        ///
        public int? t_FundCourse_Province
        {
            set { _t_fundcourse_province = value; }
            get { return _t_fundcourse_province; }
        }
        /// <summary>
        /// 市
        /// </summary>
        ///
        public int? t_FundCourse_City
        {
            set { _t_fundcourse_city = value; }
            get { return _t_fundcourse_city; }
        }
        /// <summary>
        /// 区
        /// </summary>
        ///
        public int? t_FundCourse_District
        {
            set { _t_fundcourse_district = value; }
            get { return _t_fundcourse_district; }
        }
        /// <summary>
        /// 区县名称
        /// </summary>
        ///
        public string t_FundCourse_Street
        {
            set { _t_fundcourse_street = value; }
            get { return _t_fundcourse_street; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int T_FundCourse_State
        {
            set { _t_fundCourse_state = value; }
            get { return _t_fundCourse_state; }
        }
        #endregion Model
        /// <summary>
        /// 导师名称
        /// </summary>
        public string LecturerName { get; set; }
        /// <summary>
        /// 导师头像
        /// </summary>
        public string LecturerAvator { get; set; }
        /// <summary>
        /// 导师详情
        /// </summary>
        public string LecturerInfo { get; set; }
        /// <summary>
        /// 众筹人次
        /// </summary>
        public int FundCourseCount { get; set; }
         
    }
}
