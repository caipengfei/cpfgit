using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 微信用户映射表
    /// </summary>
    [TableName("T_User_Weixin")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_User_Weixin
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
        public string UserGuid { get; set; }
        /// <summary>
        /// 微信OpenId
        /// </summary>
        [Column]
        public string OpenId { get; set; }
        /// <summary>
        /// 生成日期
        /// </summary>
        [Column]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        [Column]
        public string QrCode { get; set; }
        /// <summary>
        /// 推广关联用户
        /// </summary>
        [Column]
        public string WxTgUserGuid { get; set; }
        /// <summary>
        /// 随机数
        /// </summary>
        [Column]
        public string Nonce { get; set; }
        /// <summary>
        /// 在微信服务器的媒体文件Id
        /// </summary>
        [Column]
        public string MediaId { get; set; }
        /// <summary>
        /// 媒体文件生成时间
        /// </summary>
        [Column]
        public DateTime MediaDate { get; set; }
        /// <summary>
        /// 开放平台openid
        /// </summary>
        [Column]
        public string KFOpenId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string UnionId { get; set; }
        /// <summary>
        /// 开放平台授权时间
        /// </summary>
        [Column]
        public DateTime? KFDate { get; set; }
        /// <summary>
        /// 1：公众平台；
        /// 2：扫码登录（开放平台）；
        /// 3：app
        /// </summary>
        [Column]
        public int UserType { get; set; }
        /// <summary>
        /// 微信昵称
        /// </summary>
        [Column]
        public string Name { get; set; }
        /// <summary>
        /// 微信头像
        /// </summary>
        [Column]
        public string Avator { get; set; }
    }
}
