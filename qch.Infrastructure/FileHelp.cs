using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;


namespace qch.Infrastructure
{
    /// <summary>
    /// 文件的导入, 导出
    /// </summary>
    public class FileHelp
    {

         
            

        public static void ExportFile<T>(T obj)
        {

            StringBuilder str = new StringBuilder();

            //列出obj 对象中的所有属性
            System.Reflection.PropertyInfo[] properties = obj.GetType().GetProperties();

            if (properties != null && properties.Length > 0)
            {
                // 局部变量 用来判断循环次数，输出换行和逗号
                int j = 0;

                foreach (var item in properties)
                {
                    // 获取属性值
                    object objvalue = item.GetValue(obj, null);
                    //是否是泛型类型
                    if (item.PropertyType.IsGenericType)
                    {

                        Type objType = objvalue.GetType();
                        // 获取泛型集合总数
                        int count = Convert.ToInt32(objType.GetProperty("Count").GetValue(objvalue, null));
                        // 遍历集合
                        for (int i = 0; i < count; i++)
                        {

                            object listitem = objType.GetProperty("Item").GetValue(objvalue, new object[] { i });

                            System.Reflection.PropertyInfo[] myPros = listitem.GetType().GetProperties();
                            // 局部变量 用来判断循环次数，输出换行和逗号
                            int k = 0;
                            // 遍历集合中的属性
                            foreach (var m in myPros)
                            {
                                 // 属性名
                                //str.Append(m.Name);
                                //str.Append("，");
                                //str.Append("\t");
                                if (m.GetValue(listitem, null) != null)
                                {
                                    // 属性值
                                    str.Append(m.GetValue(listitem, null));
                                }
                                else
                                {
                                    str.Append("空值");
                                }
                                // 换行
                                if ((k+1) % 2 == 0)
                                {
                                    str.Append("\n");
                                }
                                // 输出 逗号
                                else if (k % 2 == 0)
                                {
                                    str.Append("，");
                                }
                                k++;
                            }
                            }
                    }

                     // 非泛型类型
                    else
                    {
                        // 属性名
                        //str.Append(item.Name);
                        //str.Append("，");
                        //str.Append("\t");

                        //判断属性值
                        if (item.GetValue(obj, null) != null)
                        {
                            // 属性值
                            str.Append(item.GetValue(obj, null));
                        }
                        else
                        {
                            str.Append("空值");
                        }
                        // 换行
                        if ((j+1) % 2 == 0)
                        {
                            str.Append("\n");
                        }
                        // 输出逗号
                        else if (j % 2 == 0)
                        {
                            str.Append("，");
                        }
                        j++;
                    }
                }
            }

            HttpContext.Current.Response.Clear();
            // 启用缓存
            HttpContext.Current.Response.Buffer = true;
            //中文编码
            HttpContext.Current.Response.Charset = "GB2312";   //  或者 "utf-8"
            // 设置编码方式
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

            //文件名称
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt";

            /// 设置http 请求头，直接指向文件
            HttpContext.Current.Response.AddHeader("Content-Disposition",
                "attachment;filename=" + HttpContext.Current.Server.UrlEncode(filename));

            //指定返回的是一个不能被客户端读取的流，必须被下载     
            HttpContext.Current.Response.ContentType = "text/plain"; // 或者 application/ms-txt

            //把文件流发送到客户端
            HttpContext.Current.Response.Write(str.ToString());
            // 停止页面的执行
            HttpContext.Current.Response.End();
        }
    }
}
