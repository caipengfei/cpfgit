using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 投资案例映射表
    /// </summary>
    [TableName("T_Invest_Place")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Invest_Place
    {
        #region Model
        private string _guid;
        private string _t_investplace_title;
        private string _t_investplace_converpic;
        private string _t_investplace_phase;
        private string _t_investplace_area;
        private string _t_investplace_money;
        private string _t_investplace_instruction;
        private string _t_investplace_case;
        private string _t_investplace_member;
        private int? _t_investplace_recommend;
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
        /// 投资空间标题
        /// </summary>
        /// 
        [Column]
        public string t_InvestPlace_Title
        {
            set { _t_investplace_title = value; }
            get { return _t_investplace_title; }
        }
        /// <summary>
        /// 封面图片
        /// </summary>
        /// 
        [Column]
        public string t_InvestPlace_ConverPic
        {
            set { _t_investplace_converpic = value; }
            get { return _t_investplace_converpic; }
        }
        /// <summary>
        /// 投资阶段
        /// </summary>
        /// 
        [Column]
        public string t_InvestPlace_Phase
        {
            set { _t_investplace_phase = value; }
            get { return _t_investplace_phase; }
        }
        /// <summary>
        /// 投资领域
        /// </summary>
        /// 
        [Column]
        public string t_InvestPlace_Area
        {
            set { _t_investplace_area = value; }
            get { return _t_investplace_area; }
        }
        /// <summary>
        /// 投资金额
        /// </summary>
        /// 
        [Column]
        public string t_InvestPlace_Money
        {
            set { _t_investplace_money = value; }
            get { return _t_investplace_money; }
        }
        /// <summary>
        /// 详细介绍
        /// </summary>
        /// 
        [Column]
        public string t_InvestPlace_Instruction
        {
            set { _t_investplace_instruction = value; }
            get { return _t_investplace_instruction; }
        }
        /// <summary>
        /// 详细介绍
        /// </summary>
        /// 
        [Column]
        public string t_InvestPlace_Case
        {
            set { _t_investplace_case = value; }
            get { return _t_investplace_case; }
        }
        /// <summary>
        /// 详细介绍
        /// </summary>
        /// 
        [Column]
        public string t_InvestPlace_Member
        {
            set { _t_investplace_member = value; }
            get { return _t_investplace_member; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public int? t_InvestPlace_Recommend
        {
            set { _t_investplace_recommend = value; }
            get { return _t_investplace_recommend; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public DateTime? t_AddDate
        {
            set { _t_adddate = value; }
            get { return _t_adddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public string t_AddBy
        {
            set { _t_addby = value; }
            get { return _t_addby; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public DateTime? t_ModifydDate
        {
            set { _t_modifyddate = value; }
            get { return _t_modifyddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public string t_ModifyBy
        {
            set { _t_modifyby = value; }
            get { return _t_modifyby; }
        }
        /// <summary>
        /// 
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
