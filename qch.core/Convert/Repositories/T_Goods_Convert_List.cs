using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 兑换记录映射表
    /// </summary>
    [TableName("T_Goods_Convert_List")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Goods_Convert_List
    {
        #region Model
        private string _guid;
        private string _t_user_guid;
        private string _t_goods_guid;
        private string _t_cnee_guid;
        private string _t_convert_orderno;
        private DateTime? _t_convert_createdate;
        private string _t_logistics_company;
        private string _t_logistics_waybillno;
        private int? _t_logistics_status;
        private int? _t_delstate;
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
        /// 关联商品
        /// </summary>
        /// 
        [Column]
        public string t_Goods_Guid
        {
            set { _t_goods_guid = value; }
            get { return _t_goods_guid; }
        }
        /// <summary>
        /// 关联的收货地址
        /// </summary>
        /// 
        [Column]
        public string t_Cnee_Guid
        {
            set { _t_cnee_guid = value; }
            get { return _t_cnee_guid; }
        }
        /// <summary>
        /// 兑换订单号
        /// </summary>
        /// 
        [Column]
        public string t_Convert_OrderNo
        {
            set { _t_convert_orderno = value; }
            get { return _t_convert_orderno; }
        }
        /// <summary>
        /// 兑换日期
        /// </summary>
        /// 
        [Column]
        public DateTime? t_Convert_CreateDate
        {
            set { _t_convert_createdate = value; }
            get { return _t_convert_createdate; }
        }
        /// <summary>
        /// 物流公司
        /// </summary>
        /// 
        [Column]
        public string t_Logistics_Company
        {
            set { _t_logistics_company = value; }
            get { return _t_logistics_company; }
        }
        /// <summary>
        /// 物流单号
        /// </summary>
        /// 
        [Column]
        public string t_Logistics_WaybillNo
        {
            set { _t_logistics_waybillno = value; }
            get { return _t_logistics_waybillno; }
        }
        /// <summary>
        /// 运单状态
        /// 0：未发货；
        /// 1：已发货；
        /// 2：已签收
        /// </summary>
        public int? t_Logistics_Status
        {
            set { _t_logistics_status = value; }
            get { return _t_logistics_status; }
        }
        /// <summary>
        /// 删除状态
        /// </summary>
        public int? t_DelState
        {
            set { _t_delstate = value; }
            get { return _t_delstate; }
        }
        #endregion Model
    }
}
