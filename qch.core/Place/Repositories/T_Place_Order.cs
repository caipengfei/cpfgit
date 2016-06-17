using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 能预约的空间映射表
    /// </summary>
    [TableName("T_Place_Order")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Place_Order
    {
        #region Model
        private string _guid;
        private string _t_place_guid;
        private string _t_placestyle_guid;
        private DateTime? _t_order_date;
        private int? _t_placeorder_activate = 0;
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
        /// 场地类型主键
        /// </summary>
        /// 
        [Column]
        public string t_PlaceStyle_Guid
        {
            set { _t_placestyle_guid = value; }
            get { return _t_placestyle_guid; }
        }
        /// <summary>
        /// 能预约时间
        /// </summary>
        /// 
        [Column]
        public DateTime? t_Order_Date
        {
            set { _t_order_date = value; }
            get { return _t_order_date; }
        }
        /// <summary>
        /// 是否激活
        /// </summary>
        /// 
        [Column]
        public int? t_PlaceOrder_Activate
        {
            set { _t_placeorder_activate = value; }
            get { return _t_placeorder_activate; }
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
        /// 修改时间
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
