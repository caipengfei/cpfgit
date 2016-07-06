using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 用户收货地址业务层
    /// </summary>
    public class CneeService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CneeRepository rp = new CneeRepository();


        /// <summary>
        /// 用户设置默认收货地址
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="CennGuid">需要设置为默认收货地址的Id</param>
        /// <returns></returns>
        public bool SetDefault(string UserGuid, string CennGuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserGuid) || string.IsNullOrWhiteSpace(CennGuid))
                    return false;
                var list = rp.GetReceiptByUserId(UserGuid);
                if (list != null && list.ToList().Count > 0)
                {
                    int value = 0;
                    foreach (var item in list.ToList())
                    {
                        if (item.Guid == CennGuid)
                        {
                            item.t_IsDefault = 1;
                            if (!rp.Edit(item))
                                value++;
                        }
                        else
                        {
                            item.t_IsDefault = 0;
                            if (!rp.Edit(item))
                                value++;
                        }
                    }
                    return value > 0 ? false : true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        /// <summary>
        /// 判断是否重复添加时用
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public CneeModel GetReceipt(string addr, string name, string phone)
        {
            try
            {
                return rp.GetReceipt(addr, name, phone);
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// 获取某用户的默认收货地址
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public CneeModel GetDefaultReceipt(string UserGuid)
        {
            try
            {
                return rp.GetDefaultReceipt(UserGuid);
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
        public IEnumerable<CneeModel> GetReceiptByUserId(string UserGuid)
        {
            try
            {
                return rp.GetReceiptByUserId(UserGuid);
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// 获取某个用户的最近5个收货地址
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        //public IEnumerable<CneeModel> GetReceiptByUserIdTop5(string UserGuid)
        //{
        //    try
        //    {
        //        return rp.GetReceiptByUserIdTop5(UserGuid);
        //    }
        //    catch (Exception)
        //    {

        //        return null;
        //    }
        //}
        /// <summary>
        /// getbyid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public CneeModel GetById(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                return rp.GetById(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Msg Save(CneeModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "保存失败";
            try
            {
                if (model == null)
                    return msg;
                var tt = GetById(model.Guid);
                if (tt == null)
                {
                    var m = GetReceipt(model.t_Cnee_Addr, model.t_Cnee_Name, model.t_Cnee_Phone);
                    if (m != null)
                    {
                        msg.Data = "请勿重复添加";
                        return msg;
                    }
                    model.Guid = Guid.NewGuid().ToString();
                    if (rp.Add(model))
                    {
                        msg.type = "success";
                        msg.Data = "添加成功";
                    }
                }
                else
                {
                    if (rp.Edit(model))
                    {
                        msg.type = "success";
                        msg.Data = "修改成功";
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
    }
}
