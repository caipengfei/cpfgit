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
        UserService userService = new UserService();
        FoucsService foucsService = new FoucsService();

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public object GetUser(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                var model = userService.GetById(Guid);
                if (model == null)
                    return null;
                var target = new
                {
                    username = model.t_User_RealName,
                    userAvatar = model.t_User_Pic,
                    job = model.t_User_Position,
                    company = model.t_User_Commpany,
                    city = model.t_User_City,
                    numFollow = 0,
                    fans = 0,
                    dynamicThumbnail = "",
                    dynamicDate = model.t_User_Date,
                    description = model.t_User_Remark
                };
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
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
