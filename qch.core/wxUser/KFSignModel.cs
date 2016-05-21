using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 微信开发平台扫码登录返回数据类
    /// </summary>
    public class KFSignModel
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public object access_token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public object expires_in { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public object refresh_token { get; set; }
        /// <summary>
        /// 授权用户唯一标识
        /// </summary>
        public object openid { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public object scope { get; set; }
        /// <summary>
        /// 当且仅当该网站应用已获得该用户的userinfo授权时，才会出现该字段。
        /// </summary>
        public object unionid { get; set; }
    }
}
