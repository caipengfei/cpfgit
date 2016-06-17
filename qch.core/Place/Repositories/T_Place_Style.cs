using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 空间类型映射表
    /// </summary>
    [TableName("T_Place_Style")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Place_Style
    {
        #region Model
        private string _guid;
        private string _t_place_guid;
        private string _t_place_stylename;
        private string _t_place_pic;
        private string _t_place_instruction;
        private string _t_place_remark;
        private decimal? _t_place_money;


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
        /// 场地主键
        /// </summary>
        /// 
        [Column]
        public string t_Place_Guid
        {
            set { _t_place_guid = value; }
            get { return _t_place_guid; }
        }
        /// <summary>
        /// 场地类型名称
        /// </summary>
        /// 
        [Column]
        public string t_Place_StyleName
        {
            set { _t_place_stylename = value; }
            get { return _t_place_stylename; }
        }
        /// <summary>
        /// 场地类型图片
        /// </summary>
        /// 
        [Column]
        public string t_Place_Pic
        {
            set { _t_place_pic = value; }
            get { return _t_place_pic; }
        }
        /// <summary>
        /// 场地类型介绍
        /// </summary>
        /// 
        [Column]
        public string t_Place_Instruction
        {
            set { _t_place_instruction = value; }
            get { return _t_place_instruction; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        /// 
        [Column]
        public string t_Place_Remark
        {
            set { _t_place_remark = value; }
            get { return _t_place_remark; }
        }
        /// <summary>
        /// 费用
        /// </summary>
        /// 
        [Column]
        public decimal? t_Place_Money
        {
            get { return _t_place_money; }
            set { _t_place_money = value; }
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
        /// 是否删除
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
