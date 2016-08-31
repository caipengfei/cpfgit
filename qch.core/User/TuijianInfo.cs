using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    public class TuijianInfo
    {
        public string Guid { get; set; }
        public string t_User_LoginId { get; set; }
        public string t_User_RealName { get; set; }
        public DateTime t_User_Date { get; set; }
        public string t_Andriod_Rid { get; set; }
        public string t_Bank_OpenUser { get; set; }
        public DateTime t_AddDate { get; set; }
        public string IsApprove
        {
            get
            {
                if (string.IsNullOrWhiteSpace(t_Bank_OpenUser) || t_Bank_OpenUser == "null" || t_Bank_OpenUser == "Null" || t_Bank_OpenUser == "NULL")                
                    return "否";                
                else
                    return "";
            }
        }
        public string UserPhone
        {
            get
            {
                if (t_User_LoginId.Length > 8)
                    return t_User_LoginId.Substring(0, 2) + "****" + t_User_LoginId.Substring(6);
                else
                    return t_User_LoginId;
            }
        }
    }
}
