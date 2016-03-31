using qch.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    public interface IJsapiService
    {
        /// <summary>
        /// 获取JsSDK签名对象
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        JsapiModel GetSign(string ShareUrl, string PageName);
    }
}
