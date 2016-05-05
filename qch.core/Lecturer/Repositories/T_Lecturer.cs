using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 导师映射表
    /// </summary>
    [TableName("T_Lecturer")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Lecturer
    {
        public T_Lecturer()
        { }
        #region Model
        private string _guid;
        private string _t_lecturer_name;
        private string _t_lecturer_pic;
        private string _t_lecturer_position;
        private string _t_lecturer_intor;
        private string _t_lecturer_goodarea;
        private string _t_lecturer_subject;
        private DateTime? _t_lecturer_createdate;
        private int? _t_lecturer_count;
        private string _t_lecturer_remark;
        private int? _t_lecturer_sort;
        private int? _t_lecturer_recommend;
        private int? _t_lecturer_good;
        private int? _t_lecturer_bad;
        private int? _t_delstate;
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
        /// 导师姓名
        /// </summary>
        /// 
        [Column]
        public string T_Lecturer_Name
        {
            set { _t_lecturer_name = value; }
            get { return _t_lecturer_name; }
        }
        /// <summary>
        /// 导师头像
        /// </summary>
        /// 
        [Column]
        public string T_Lecturer_Pic
        {
            set { _t_lecturer_pic = value; }
            get { return _t_lecturer_pic; }
        }
        /// <summary>
        /// 导师职位
        /// </summary>
        /// 
        [Column]
        public string T_Lecturer_Position
        {
            set { _t_lecturer_position = value; }
            get { return _t_lecturer_position; }
        }
        /// <summary>
        /// 导师介绍
        /// </summary>
        /// 
        [Column]
        public string T_Lecturer_Intor
        {
            set { _t_lecturer_intor = value; }
            get { return _t_lecturer_intor; }
        }
        /// <summary>
        /// 擅长领域
        /// </summary>
        /// 
        [Column]
        public string T_Lecturer_GoodArea
        {
            set { _t_lecturer_goodarea = value; }
            get { return _t_lecturer_goodarea; }
        }
        /// <summary>
        /// 授课方向
        /// </summary>
        /// 
        [Column]
        public string T_Lecturer_Subject
        {
            set { _t_lecturer_subject = value; }
            get { return _t_lecturer_subject; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        /// 
        [Column]
        public DateTime? T_Lecturer_CreateDate
        {
            set { _t_lecturer_createdate = value; }
            get { return _t_lecturer_createdate; }
        }
        /// <summary>
        /// 授课次数
        /// </summary>
        /// 
        [Column]
        public int? T_Lecturer_Count
        {
            set { _t_lecturer_count = value; }
            get { return _t_lecturer_count; }
        }
        /// <summary>
        /// 备注信息
        /// </summary>
        /// 
        [Column]
        public string T_Lecturer_Remark
        {
            set { _t_lecturer_remark = value; }
            get { return _t_lecturer_remark; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// 
        [Column]
        public int? T_Lecturer_Sort
        {
            set { _t_lecturer_sort = value; }
            get { return _t_lecturer_sort; }
        }
        /// <summary>
        /// 推荐
        /// </summary>
        /// 
        [Column]
        public int? T_Lecturer_Recommend
        {
            set { _t_lecturer_recommend = value; }
            get { return _t_lecturer_recommend; }
        }
        /// <summary>
        /// 好评次数
        /// </summary>
        /// 
        [Column]
        public int? T_Lecturer_Good
        {
            set { _t_lecturer_good = value; }
            get { return _t_lecturer_good; }
        }
        /// <summary>
        /// 差评次数
        /// </summary>
        /// 
        [Column]
        public int? T_Lecturer_Bad
        {
            set { _t_lecturer_bad = value; }
            get { return _t_lecturer_bad; }
        }
        /// <summary>
        /// 删除状态
        /// </summary>
        public int? T_DelState
        {
            set { _t_delstate = value; }
            get { return _t_delstate; }
        }
        #endregion Model

    }
}
