using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 
    /// </summary>
    public class WXUserService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        WXUserRepository rp = new WXUserRepository();

        /// <summary>
        /// GetByUserId
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public WXUserModel GetByUserId(string Guid)
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
        /// GetByOpenId
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public WXUserModel GetByOpenId(string OpenId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(OpenId))
                    return null;
                return rp.GetById(OpenId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// getbyid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public WXUserModel GetById(string Guid)
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
        /// 保存用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Msg Save(WXUserModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "操作失败";
            try
            {
                if (model == null)
                    return msg;
                var tt = GetById(model.Guid);
                if (tt != null)
                {
                    model.CreateDate = DateTime.Now;
                    model.MediaDate = DateTime.Now;
                    if (rp.Edit(model))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
                else
                {
                    var a = rp.GetByOpenId(model.OpenId);
                    if (a != null)
                    {
                        msg.Data = "openid已存在";
                        return msg;
                    }
                    //var b = rp.GetByUserId(model.UserGuid);
                    //if (b != null)
                    //{
                    //    msg.Data = "已存在";
                    //    return msg;
                    //}
                    model.Guid = Guid.NewGuid().ToString();
                    model.CreateDate = DateTime.Now;
                    model.MediaDate = DateTime.Now;
                    if (rp.Add(model))
                    {
                        msg.type = "success";
                        msg.Data = "新增成功";
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public bool Del(string Guid)
        {
            try
            {
                var model = GetById(Guid);
                if (model == null)
                    return false;
                return rp.Del(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
    }
}
