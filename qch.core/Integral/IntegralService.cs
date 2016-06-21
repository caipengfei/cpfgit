using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 积分业务层
    /// </summary>
    public class IntegralService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IntegralRepository rp = new IntegralRepository();



        /// <summary>
        /// 分页获取某用户的积分记录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<IntegralModel> GetAll(int page, int pagesize, string UserGuid, int typeId)
        {
            try
            {
                return rp.GetAll(page, pagesize, UserGuid, typeId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某人推荐会员的时候当时获取的积分奖励
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="regDate"></param>
        /// <returns></returns>
        public int GetTJIntegral(string UserGuid, DateTime regDate)
        {
            try
            {
                return rp.GetTJIntegral(UserGuid, regDate);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 获取某用户的积分总额
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public int GetIntegral(string UserGuid)
        {
            try
            {
                return rp.GetIntegral(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 获取某用户的某项积分总额
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public int GetIntegral(string UserGuid, string Pinyin)
        {
            try
            {
                return rp.GetIntegral(UserGuid, Pinyin);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// getbyid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IntegralModel GetById(string Guid)
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
        public Msg Save(IntegralModel model)
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
                    //model.t_News_Date = DateTime.Now;
                    if (rp.Edit(model))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    model.t_AddDate = DateTime.Now;
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
        /// 更改删除状态
        /// </summary>
        /// <param name="Guid"></param>
        /// <param name="DelState"></param>
        /// <returns></returns>
        public bool EditStyle(string Guid, int DelState)
        {
            try
            {
                var model = GetById(Guid);
                if (model == null)
                    return false;
                model.t_DelState = DelState;
                return rp.Edit(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
    }
}
