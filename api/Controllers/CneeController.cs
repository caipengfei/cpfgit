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
    /// 用户收货地址相关接口
    /// </summary>
    public class CneeController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CneeService service = new CneeService();


        /// <summary>
        /// 保存用户收货地址
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public Msg Save(CneeModel model)
        {
            try
            {
                var msg = service.Save(model);
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 用户设置默认收货地址
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="CennGuid">需要设置为默认收货地址的Id</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool SetDefault([FromBody]string UserGuid, [FromBody]string CennGuid)
        {
            try
            {
                return service.SetDefault(UserGuid, CennGuid);
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// getbyid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public CneeModel GetById(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                return service.GetById(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某用户的默认收货地址
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public CneeModel GetDefaultReceipt(string UserGuid)
        {
            try
            {
                return service.GetDefaultReceipt(UserGuid);
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// 获取某个用户的所有收货地址
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IEnumerable<CneeModel> GetReceiptByUserId(string UserGuid)
        {
            try
            {
                return service.GetReceiptByUserId(UserGuid);
            }
            catch (Exception)
            {

                return null;
            }
        }

    }
}
