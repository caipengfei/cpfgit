using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 系统日志映射类
    /// </summary>
    [TableName("T_Logs")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Logs
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 错误等级
        /// </summary>
        [Column]
        public int ErrorLevel { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [Column]
        public string ErrorInfo { get; set; }
        /// <summary>
        /// 错误来源 ios/android
        /// </summary>
        [Column]
        public string ErrorFrom { get; set; }
        /// <summary>
        /// 发生错误的行为
        /// </summary>
        [Column]
        public string ErrorAction { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [Column]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [Column]
        public string Remark { get; set; }
    }
}
