using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace xftwl.bmb.admin.Areas.Admin
{
    public class AdminHelper
    {
        /// <summary>
        /// 弹出消息
        /// </summary>
        /// <param name="msg"></param>
        public static ActionResult AlertMsg( String msg )
        {
            return new ContentResult() { Content = "<script type=\"text/javascript\">alert(\"" + msg.Replace( "\"", "\\\"" ).Replace( "\\", "\\\\" ) + "\");</script>" };
        }

        public static ActionResult RunJs( String jsCon )
        {
            return new ContentResult() { Content = "<script type=\"text/javascript\"> " + jsCon + "</script>" };
        }





    }
}