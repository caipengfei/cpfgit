using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qch.Models
{
    public class SignModel
    {
        private int errcode;
        public int Errcode
        {
            get { return errcode; }
            set { errcode = value; }
        }
        private string errmsg;
        public string Errmsg
        {
            get { return errmsg; }
            set { errmsg = value; }
        }
        /// <summary>
        /// 票证
        /// </summary>
        private string ticket;
        public string Ticket
        {
            get { return ticket; }
            set { ticket = value; }
        }
        private int expires_in;
        public int Expires_in
        {
            get { return expires_in; }
            set { expires_in = value; }
        }
    }
}