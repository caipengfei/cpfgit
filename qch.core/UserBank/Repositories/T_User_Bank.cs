using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户银行卡信息以及实名认证映射表
    /// </summary>
    [TableName("T_User_Bank")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_User_Bank
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        [Column]
        public string t_Bank_Name { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        [Column]
        public string t_Bank_NO { get; set; }
        /// <summary>
        /// 开户行地址
        /// </summary>
        [Column]
        public string t_Bank_OpenAddress { get; set; }
        /// <summary>
        /// 开户名
        /// </summary>
        [Column]
        public string t_Bank_OpenUser { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [Column]
        public string t_Bank_OpenUserNo { get; set; }
        /// <summary>
        /// 管理用户
        /// </summary>
        [Column]
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 时间
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
