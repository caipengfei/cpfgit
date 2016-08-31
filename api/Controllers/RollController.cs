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
    /// <summary>
    /// 转盘
    /// </summary>
    public class RollController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        RollService service = new RollService();


        /// <summary>
        /// 获取所有中奖记录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public PetaPoco.Page<object> GetAllRollRecords(int page, int pagesize)
        {
            try
            {
                return service.GetAllRollRecords(page, pagesize);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某用户的中奖记录
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetAllByUser(int page, int pagesize, string UserGuid)
        {
            try
            {
                return service.GetAllByUser(page, pagesize, UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取所有转盘抽奖物品
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IEnumerable<RollModel> GetAll()
        {
            try
            {
                return service.GetAll();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 转盘抽奖
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public Msg zp(string UserGuid)
        {
            try
            {
                return service.zp(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
