using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 系统样式、属性业务层
    /// </summary>
    public class StyleService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        StyleRepository rp = new StyleRepository();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public IEnumerable<StyleModel> GetByfId(int fid)
        {
            try
            {
                return rp.GetByfId(fid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 批量获取
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<SelectStyle> GetByIds(string ids)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ids) || ids == "NULL" || ids == "Null" || ids == "null")
                    return new List<SelectStyle>();
                if (ids.IndexOf(';') <= 0)
                {
                    var model = rp.GetByIds(ids);
                    if (model == null)
                        model = new List<SelectStyle>();
                    return model;
                }
                else
                {
                    string value = "";
                    var xy = ids.Split(';');
                    if (xy.Length > 0)
                    {
                        foreach (var item in xy)
                        {
                            value = value + item + ",";
                        }
                        value = value.Substring(0, value.Length - 1);
                        var model = rp.GetByIds(value);
                        if (model == null)
                            model = new List<SelectStyle>();
                        return model;
                    }
                    return new List<SelectStyle>();
                }
            }
            catch (Exception ex)
            {
                //数据库里有null这样的字符串
                log.Error("--------------" + ids);
                log.Error(ex.Message);
                return new List<SelectStyle>();
            }
        }
        public static string GetPosition(string Position)
        {
            string xy = "";
            var list = new StyleService().GetByIds(Position);
            if (list != null)
            {
                foreach (var item in list)
                {
                    xy += item.t_Style_Name + " ";
                }
            }
            return xy;
        }
        /// <summary>
        /// 分页获取所有
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<StyleModel> GetAll(int page, int pagesize)
        {
            try
            {
                return rp.GetAll(page, pagesize);
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
        public SelectStyle GetById(int Id)
        {
            try
            {
                if (Id <= 0)
                    return null;
                return rp.GetById(Id);
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
        public Msg Save(StyleModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "操作失败";
            try
            {
                if (model == null)
                    return msg;
                var tt = GetById(model.Id);
                if (tt != null)
                {
                    model.t_ModifydDate = DateTime.Now;
                    if (rp.Edit(model))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
                else
                {
                    model.t_AddDate = DateTime.Now;
                    model.t_ModifydDate = DateTime.Now;
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
        //public bool EditStyle(int Id, int DelState)
        //{
        //    try
        //    {
        //        var model = GetById(Id);
        //        if (model == null)
        //            return false;
        //        model.t_DelState = DelState;
        //        return rp.Edit(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        return false;
        //    }
        //}
    }
}
