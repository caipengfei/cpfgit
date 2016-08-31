using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 兑换用商品映射表
    /// </summary>
    [TableName("T_Goods_Convert")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Goods_Convert
    {
        #region Model
        private string _guid;
        private string _t_goods_code;
        private string _t_goods_name;
        private int? _t_need_integral;
        private string _t_goods_pic;
        private string _t_goods_unit;
        private string _t_goods_size;
        private int? _t_goods_stock;
        private string _t_goods_desc;
        private string _t_goods_info;
        private decimal? _t_goods_freight;
        private DateTime? _t_createdate;
        private int? _t_goods_audit;
        private int? _t_goods_issale;
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
        /// 商品编码
        /// </summary>
        /// 
        [Column]
        public string t_Goods_Code
        {
            set { _t_goods_code = value; }
            get { return _t_goods_code; }
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        /// 
        [Column]
        public string t_Goods_Name
        {
            set { _t_goods_name = value; }
            get { return _t_goods_name; }
        }
        /// <summary>
        /// 所需积分
        /// </summary>
        /// 
        [Column]
        public int? t_Need_Integral
        {
            set { _t_need_integral = value; }
            get { return _t_need_integral; }
        }
        /// <summary>
        /// 商品封面图
        /// </summary>
        /// 
        [Column]
        public string t_Goods_Pic
        {
            set { _t_goods_pic = value; }
            get { return _t_goods_pic; }
        }
        /// <summary>
        /// 单位
        /// </summary>
        /// 
        [Column]
        public string t_Goods_Unit
        {
            set { _t_goods_unit = value; }
            get { return _t_goods_unit; }
        }
        /// <summary>
        /// 规格
        /// </summary>
        /// 
        [Column]
        public string t_Goods_Size
        {
            set { _t_goods_size = value; }
            get { return _t_goods_size; }
        }
        /// <summary>
        /// 库存
        /// </summary>
        /// 
        [Column]
        public int? t_Goods_Stock
        {
            set { _t_goods_stock = value; }
            get { return _t_goods_stock; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        /// 
        [Column]
        public string t_Goods_Desc
        {
            set { _t_goods_desc = value; }
            get { return _t_goods_desc; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        /// 
        [Column]
        public string t_Goods_Info
        {
            set { _t_goods_info = value; }
            get { return _t_goods_info; }
        }
        /// <summary>
        /// 运费
        /// </summary>
        /// 
        [Column]
        public decimal? t_Goods_Freight
        {
            set { _t_goods_freight = value; }
            get { return _t_goods_freight; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        /// 
        [Column]
        public DateTime? t_CreateDate
        {
            set { _t_createdate = value; }
            get { return _t_createdate; }
        }
        /// <summary>
        /// 审核状态
        /// </summary>
        /// 
        [Column]
        public int? t_Goods_Audit
        {
            set { _t_goods_audit = value; }
            get { return _t_goods_audit; }
        }
        /// <summary>
        /// 上架状态
        /// </summary>
        /// 
        [Column]
        public int? t_Goods_IsSale
        {
            set { _t_goods_issale = value; }
            get { return _t_goods_issale; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        [Column]
        public int t_Goods_Index { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        [Column]
        public int t_Goods_Recmmend { get; set; }
        /// <summary>
        /// 兑换次数
        /// </summary>
        [Column]
        public int t_Convert_Count { get; set; }
        /// <summary>
        /// 商品类型
        /// 0：全部；
        /// 1：积分兑换商品；
        /// 2：转盘抽奖商品
        /// </summary>
        [Column]
        public int t_Goods_Type { get; set; }
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
