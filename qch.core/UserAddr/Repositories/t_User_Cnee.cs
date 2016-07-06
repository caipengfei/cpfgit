using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户收货地址映射表
    /// </summary>
    [TableName("t_User_Cnee")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class t_User_Cnee
    {
        #region Model
        private string _guid;
        private string _t_user_guid;
        private string _t_cnee_name;
        private string _t_cnee_phone;
        private string _t_cnee_addr;
        private int? _t_isdefault = 0;
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
        /// 
        [Column]
        public string t_User_Guid
        {
            set { _t_user_guid = value; }
            get { return _t_user_guid; }
        }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        /// 
        [Column]
        public string t_Cnee_Name
        {
            set { _t_cnee_name = value; }
            get { return _t_cnee_name; }
        }
        /// <summary>
        /// 收货人手机号
        /// </summary>
        /// 
        [Column]
        public string t_Cnee_Phone
        {
            set { _t_cnee_phone = value; }
            get { return _t_cnee_phone; }
        }
        /// <summary>
        /// 收货地址
        /// </summary>
        /// 
        [Column]
        public string t_Cnee_Addr
        {
            set { _t_cnee_addr = value; }
            get { return _t_cnee_addr; }
        }
        /// <summary>
        /// 是否为默认地址
        /// </summary>
        /// 
        [Column]
        public int? t_IsDefault
        {
            set { _t_isdefault = value; }
            get { return _t_isdefault; }
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
