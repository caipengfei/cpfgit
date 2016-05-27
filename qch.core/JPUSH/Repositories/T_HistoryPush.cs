using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户优惠券明细映射表
    /// </summary>
    [TableName("T_HistoryPush")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_HistoryPush
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 关联用户
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 推送消息标题
        /// </summary>
        [Column]
        public string t_Title { get; set; }
        /// <summary>
        /// 推送消息内容
        /// </summary>
        [Column]
        public string t_Alert { get; set; }
        /// <summary>
        /// 接收消息设备ID
        /// </summary>
        [Column]
        public string t_Associate_Guid { get; set; }
        /// <summary>
        /// 消息类型（主题）
        /// </summary>
        [Column]
        public string t_Type { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [Column]
        public DateTime t_Date { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
