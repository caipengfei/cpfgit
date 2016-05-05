using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 课程映射表
    /// </summary>
    [TableName("T_Course")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Course
    {
        public T_Course()
        { }
        #region Model
        private string _guid;
        private string _t_lecturer_guid;
        private string _t_course_title;
        private string _t_course_pic;
        private string _t_course_src;
        private string _t_course_oneword;
        private string _t_course_instruction;
        private string _t_course_tips;
        private string _t_course_times;
        private int? _t_course_counts = 0;
        private int? _t_course_good = 0;
        private int? _t_course_bad = 0;
        private decimal? _t_course_price;
        private string _t_course_style;
        private int? _t_course_recommand = 0;
        private int? _t_course_index = 0;
        private string _t_course_group = "0";
        private DateTime? _t_add_date;
        private int? _t_delstate = 0;
        /// <summary>
        /// guid
        /// </summary>
        /// 
        [Column]
        public string Guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 讲师主键
        /// </summary>
        /// 
        [Column]
        public string t_Lecturer_Guid
        {
            set { _t_lecturer_guid = value; }
            get { return _t_lecturer_guid; }
        }
        /// <summary>
        /// 课程标题
        /// </summary>
        /// 
        [Column]
        public string t_Course_Title
        {
            set { _t_course_title = value; }
            get { return _t_course_title; }
        }
        /// <summary>
        /// 课程封面图片
        /// </summary>
        /// 
        [Column]
        public string t_Course_Pic
        {
            set { _t_course_pic = value; }
            get { return _t_course_pic; }
        }
        /// <summary>
        /// 链接地址
        /// </summary>
        /// 
        [Column]
        public string t_Course_Src
        {
            set { _t_course_src = value; }
            get { return _t_course_src; }
        }
        /// <summary>
        /// 一句话描述
        /// </summary>
        /// 
        [Column]
        public string t_Course_OneWord
        {
            set { _t_course_oneword = value; }
            get { return _t_course_oneword; }
        }
        /// <summary>
        /// 详细说明
        /// </summary>
        /// 
        [Column]
        public string t_Course_Instruction
        {
            set { _t_course_instruction = value; }
            get { return _t_course_instruction; }
        }
        /// <summary>
        /// 标签
        /// </summary>
        /// 
        [Column]
        public string t_Course_Tips
        {
            set { _t_course_tips = value; }
            get { return _t_course_tips; }
        }
        /// <summary>
        /// 时长
        /// </summary>
        /// 
        [Column]
        public string t_Course_Times
        {
            set { _t_course_times = value; }
            get { return _t_course_times; }
        }
        /// <summary>
        /// 观看次数
        /// </summary>
        /// 
        [Column]
        public int? t_Course_Counts
        {
            set { _t_course_counts = value; }
            get { return _t_course_counts; }
        }
        /// <summary>
        /// 点赞次数
        /// </summary>
        /// 
        [Column]
        public int? t_Course_Good
        {
            set { _t_course_good = value; }
            get { return _t_course_good; }
        }
        /// <summary>
        /// 踩次数
        /// </summary>
        /// 
        [Column]
        public int? t_Course_Bad
        {
            set { _t_course_bad = value; }
            get { return _t_course_bad; }
        }
        /// <summary>
        /// 价格
        /// </summary>
        /// 
        [Column]
        public decimal? t_Course_Price
        {
            set { _t_course_price = value; }
            get { return _t_course_price; }
        }
        /// <summary>
        /// 课程类型 
        /// </summary>
        /// 
        [Column]
        public string t_Course_Style
        {
            set { _t_course_style = value; }
            get { return _t_course_style; }
        }
        /// <summary>
        /// 是否推荐
        /// </summary>
        /// 
        [Column]
        public int? t_Course_Recommand
        {
            set { _t_course_recommand = value; }
            get { return _t_course_recommand; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// 
        [Column]
        public int? t_Course_Index
        {
            set { _t_course_index = value; }
            get { return _t_course_index; }
        }
        /// <summary>
        /// 所属分组
        /// </summary>
        /// 
        [Column]
        public string t_Course_Group
        {
            set { _t_course_group = value; }
            get { return _t_course_group; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        /// 
        [Column]
        public DateTime? t_Add_Date
        {
            set { _t_add_date = value; }
            get { return _t_add_date; }
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
