using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace qch.Infrastructure
{
    /// <summary>
    /// 32位MD5加密
    /// </summary>
    public class Encrypt
    {
        /// <summary>
        /// 32位MD5标准加密
        /// </summary>
        /// <param name="strText">要加密字符串</param>
        /// <param name="IsLower">是否以小写方式返回</param>
        /// <returns></returns>
        public static string MD5Encrypt(string strText, bool IsLower)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strText);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0');
        }
    }


}
