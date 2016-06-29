using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 关注关系业务层
    /// </summary>
    public class FoucsService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        FoucsRepository rp = new FoucsRepository();


        /// <summary>
        /// 获取某人关注的所有东西
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public int GetMyFoucs(string UserGuid)
        {
            try
            {
                return rp.GetMyFoucs(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 获取我关注的所有人
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<FoucsUser> GetFoucsFroMe(string Guid)
        {
            try
            {
                return rp.GetFoucsFroMe(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某人的所有粉丝
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<FoucsUser> GetFoucs(string Guid)
        {
            try
            {
                return rp.GetFoucs(Guid);
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
        public FoucsModel GetById(string Guid)
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
        public Msg Save(FoucsModel model)
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
                    model.t_Date = DateTime.Now;
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
