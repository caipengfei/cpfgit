using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户资金表
    /// </summary>
    [TableName("T_User_Money")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_User_Money
    {
        #region Model
        private string _guid;
        private decimal? _t_user_money;
        private decimal? _t_user_canmoney;
        private decimal? _t_user_backmoney;
        private decimal? _t_user_appmoney;
        private decimal? _t_user_usedmoney;
        private decimal? _t_user_lockmoney;
        private decimal? _t_user_backreducemoney;
        private string _t_user_guid;
        private DateTime? _t_date;
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
        /// 余额
        /// </summary>
        /// 
        [Column]
        public decimal? t_User_Money
        {
            set { _t_user_money = value; }
            get { return _t_user_money; }
        }
        /// <summary>
        /// 可提金额
        /// </summary>
        /// 
        [Column]
        public decimal? t_User_CanMoney
        {
            set { _t_user_canmoney = value; }
            get { return _t_user_canmoney; }
        }
        /// <summary>
        /// 后台充值金额
        /// </summary>
        /// 
        [Column]
        public decimal? t_User_BackMoney
        {
            set { _t_user_backmoney = value; }
            get { return _t_user_backmoney; }
        }
        /// <summary>
        /// 前台充值金额
        /// </summary>
        /// 
        [Column]
        public decimal? t_User_AppMoney
        {
            set { _t_user_appmoney = value; }
            get { return _t_user_appmoney; }
        }
        /// <summary>
        /// 已提金额
        /// </summary>
        /// 
        [Column]
        public decimal? t_User_UsedMoney
        {
            set { _t_user_usedmoney = value; }
            get { return _t_user_usedmoney; }
        }
        /// <summary>
        /// 冻结金额
        /// </summary>
        /// 
        [Column]
        public decimal? t_User_LockMoney
        {
            set { _t_user_lockmoney = value; }
            get { return _t_user_lockmoney; }
        }
        /// <summary>
        /// 后台删除减少金额
        /// </summary>
        /// 
        [Column]
        public decimal? t_User_BackReduceMoney
        {
            set { _t_user_backreducemoney = value; }
            get { return _t_user_backreducemoney; }
        }
        /// <summary>
        /// 用户主键
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
        public DateTime? t_Date
        {
            set { _t_date = value; }
            get { return _t_date; }
        }
        #endregion Model
    }
}
