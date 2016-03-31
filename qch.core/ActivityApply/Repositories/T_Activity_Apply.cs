using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 活动报名映射类
    /// </summary>
    /// 
    [TableName("T_Activity_Apply")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Activity_Apply
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 关联的活动id
        /// </summary>
        [Column]
        public string t_Activity_Guid { get; set; }
        /// <summary>
        /// 关联的用户id
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Column]
        public string t_ActivityApply_UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Column]
        public string t_ActivityApply_Mobile { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column]
        public string t_ActivityApply_Remark { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        [Column]
        public DateTime t_AddDate { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
    }
}
