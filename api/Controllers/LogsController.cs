using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class LogsController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        LogsService service = new LogsService();

        /// <summary>
        /// 保存错误日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public Msg CreateLogs(string ErrorInfo, string ErrorFrom, string ErrorAction, int ErrorLevel)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "保存日志失败";
            try
            {
                if (string.IsNullOrWhiteSpace(ErrorInfo))
                {
                    msg.Data = "错误信息不能为空";
                    log.Info("保存日志信息的时候，错误信息为空");
                    return msg;
                }
                if (string.IsNullOrWhiteSpace(ErrorFrom))
                {
                    msg.Data = "错误来源平台不能为空";
                    log.Info("保存日志信息的时候，错误来源平台为空");
                    return msg;
                }
                if (string.IsNullOrWhiteSpace(ErrorAction))
                {
                    msg.Data = "产生错误的行为不能为空";
                    log.Info("保存日志信息的时候，产生错误的行为为空");
                    return msg;
                }
                LogsModel model = new LogsModel
                {
                    ErrorAction = ErrorAction,
                    ErrorFrom = ErrorFrom,
                    ErrorInfo = ErrorInfo,
                    ErrorLevel = ErrorLevel,
                    CreateDate = DateTime.Now,
                    Guid = "",
                    Remark = ""
                };
                return service.Save(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
    }
}
