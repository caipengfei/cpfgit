using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class GetUserModel
    {
        public UserInfoModel_xsl data { get; set; }
        public string flag { get; set; }
        public string message { get; set; }
        public string msgEmpty { get; set; }
        public GetUserModel()
        {
            data = new UserInfoModel_xsl();
        }
    }
}