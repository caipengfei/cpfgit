using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qch.Repositories
{
    /// <summary>
    /// 评论映射表
    /// </summary>
    [TableName("T_User_Talk")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_User_Talk
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 关联主键Guid
        /// </summary>
        [Column]
        public string t_Associate_Guid { get; set; }
        /// <summary>
        /// 回复人Guid
        /// </summary>
        [Column]
        public string t_Talk_FromUserGuid { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        [Column]
        public string t_Talk_FromContent { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        [Column]
        public DateTime t_Talk_FromDate { get; set; }
        /// <summary>
        /// 回复人Guid
        /// </summary>
        [Column]
        public string t_Talk_ToUserGuid { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        [Column]
        public string t_Talk_ToContent { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        [Column]
        public DateTime t_Talk_ToDate { get; set; }
        /// <summary>
        /// 赞
        /// </summary>
        [Column]
        public int t_Talk_Good { get; set; }
        /// <summary>
        /// 踩
        /// </summary>
        [Column]
        public int t_Talk_Bad { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        [Column]
        public int t_Talk_Audit { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
