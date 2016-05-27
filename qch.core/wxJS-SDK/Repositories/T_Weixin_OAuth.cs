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
    [TableName("T_Weixin_OAuth")]
    [PrimaryKey("Id")]
    [ExplicitColumns]
    public class T_Weixin_OAuth
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public int Id { get; set; }
        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        [Column]
        public string access_token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        [Column]
        public int expires_in { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        [Column]
        public string refresh_token { get; set; }
        /// <summary>
        /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        [Column]
        public string openid { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        [Column]
        public string scope { get; set; }
        /// <summary>
        /// 授权类型
        /// 1：公众平台授权；
        /// 2：开放平台授权
        /// </summary>
        [Column]
        public int TypeId { get; set; }
        /// <summary>
        /// 授权时间
        /// </summary>
        [Column]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 刷新时间
        /// </summary>
        [Column]
        public DateTime UpdateDate { get; set; }
    }
}
