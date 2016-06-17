using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 空间预约时间映射表
    /// </summary>
    [TableName("T_PlaceOrder_Time")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_PlaceOrder_Time
    {
        #region Model
        private string _guid;
        private string _t_placeoder_guid;
        private int? _t_placeoder_stime;
        private int? _t_placeoder_etime;
        private int? _t_placeoder_ordered = 0;
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
        /// 能预约日期主键
        /// </summary>
        /// 
        [Column]
        public string t_PlaceOder_Guid
        {
            set { _t_placeoder_guid = value; }
            get { return _t_placeoder_guid; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// 
        [Column]
        public int? t_PlaceOder_sTime
        {
            set { _t_placeoder_stime = value; }
            get { return _t_placeoder_stime; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// 
        [Column]
        public int? t_PlaceOder_eTime
        {
            set { _t_placeoder_etime = value; }
            get { return _t_placeoder_etime; }
        }
        /// <summary>
        /// 是否被预约
        /// </summary>
        /// 
        [Column]
        public int? t_PlaceOder_Ordered
        {
            set { _t_placeoder_ordered = value; }
            get { return _t_placeoder_ordered; }
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
