using qch.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Maticsoft.Common;
using Maticsoft.Model;

namespace api.Controllers
{
    public class UserController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Maticsoft.BLL.T_Users bll = new Maticsoft.BLL.T_Users();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Msg Get()
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "未获取到值";
            try
            {
                log.Info(msg.Data);
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }

        /// <summary>
        /// 获取对象实体
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Maticsoft.Model.T_Users GetModel(string Guid)
        {
            var model = bll.GetModel(Guid);
            return model;
        }
    }
}
