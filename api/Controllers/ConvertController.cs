using api.Models;
using qch.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    /// <summary>
    /// 积分兑换商品相关接口
    /// </summary>
    public class ConvertController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        GoodsService service = new GoodsService();

        /// <summary>
        /// 保存兑换记录
        /// </summary>
        /// <param name="CneeGuid"></param>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public bool SaveRecord(string CneeGuid, string OrderNo)
        {
            try
            {
                return service.SaveRecord(CneeGuid, OrderNo);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="Sign"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public string CheckSign(string UserGuid, string Sign)
        {
            if (string.IsNullOrWhiteSpace(UserGuid))
            {
                return "0";
            }
            string s = qch.Infrastructure.Encrypt.MD5Encrypt(UserGuid + "150919", true);
            if (Sign == s)
            {
                //签名正确
                return "1";
            }
            else
            {
                //签名错误
                return "0";
            }
        }
        /// <summary>
        /// 获取所有积分兑换商品
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetAll()
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
        /// 获取商品详情
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetDetail(string Guid)
        {
            try
            {
                return service.GetDetail(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某用户的兑换商品记录
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetList(string UserGuid)
        {
            try
            {
                return service.GetList(UserGuid);
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
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetList(int page, int pagesize,string type, string UserGuid)
        {
            try
            {
                return service.GetList1(page, pagesize,type, UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 商品兑换
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="GoodsCode"></param>
        /// <param name="Cnee"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public Msg CreateRecord([FromBody]ConvertModel model)
        {
            try
            {
                if (model == null)
                    return null;
                var msg = service.CreateRecord(model.UserGuid, model.GoodsCode, model.Cnee);
                return msg;
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
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetById(string Guid)
        {
            try
            {
                return service.GetById1(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
