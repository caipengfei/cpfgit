using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.Areas.Admin.Models
{
    public class CateJson
    {
        public IList<CateJson> children { get; set; }
        public int id { get; set; }
        public String text { get; set; }
        public int pid { get; set; }

    }
}