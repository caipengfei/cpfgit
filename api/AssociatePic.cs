using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Maticsoft.Common;

namespace Maticsoft.Web
{
    public class AssociatePic
    {
        //定义一个私有变量
        private static DataTable dtPic;
        /// <summary> 
        /// 定义访问点 
        /// </summary> 
        /// <returns></returns> 
        public static DataTable DTPics()
        {
            string key = "DTPic";
            if (dtPic == null)
            {
                dtPic = new BLL.T_Associate_Pic().GetList("").Tables[0];
                MemcachedHelper.Set(key, dtPic, DateTime.Now.AddHours(2));
            }
            else
            {
                object c = MemcachedHelper.Get(key);
                dtPic = (DataTable)c;
            }
            return dtPic;
        }
    }
}