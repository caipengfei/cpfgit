using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 被预约过的空间映射表
    /// </summary>
    [TableName("T_Place_Ordered")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Place_Ordered
    {
        #region Model
        private string _guid;
        private string _t_placeorder_guid;
        private string _t_ordered_no;
        private string _t_user_guid;
        private int? _t_state = 0;
        private string _t_ordered_remark;
        private DateTime? _t_adddate;
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
        /// 被预约时间主键
        /// </summary>
        /// 
        [Column]
        public string t_PlaceOrder_Guid
        {
            set { _t_placeorder_guid = value; }
            get { return _t_placeorder_guid; }
        }
        /// <summary>
        /// 编号
        /// </summary>
        /// 
        [Column]
        public string t_Ordered_NO
        {
            set { _t_ordered_no = value; }
            get { return _t_ordered_no; }
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
        /// 状态，0未到，1已到，2已离开，3取消
        /// </summary>
        /// 
        [Column]
        public int? t_State
        {
            set { _t_state = value; }
            get { return _t_state; }
        }
        /// <summary>
        /// 预约的备注信息
        /// </summary>
        /// 
        [Column]
        public string t_Ordered_Remark
        {
            set { _t_ordered_remark = value; }
            get { return _t_ordered_remark; }
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
        public int? t_DelState
        {
            set { _t_delstate = value; }
            get { return _t_delstate; }
        }
        #endregion Model
    }
}
