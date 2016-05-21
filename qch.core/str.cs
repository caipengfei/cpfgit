using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 生成随机字符串
    /// </summary>
    public class str
    {
        /// <summary>
        /// 生成12位的随机数字，中间带空格
        /// </summary>
        /// <param name="Nums"></param>
        /// <returns></returns>
        public static string Ran(int Nums)
        {
            int a = Nums;
            ArrayList MyArray = new ArrayList();
            Random random = new Random();
            string str = null;
            while (Nums > 0)
            {
                int i = random.Next(0, 9);
                if (MyArray.Count < a)
                {
                    MyArray.Add(i);
                }
                Nums -= 1;
            }
            for (int j = 0; j <= MyArray.Count - 1; j++)
            {
                str += MyArray[j].ToString();
                if (j == 3 || j == 7)
                    str += " ";
            }
            return str;
        }
    }
}
