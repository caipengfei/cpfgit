using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 系统样式、属性资源层
    /// </summary>
    public class StyleRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<StyleModel> rp = new Repository<StyleModel>();
        Repository<SelectStyle> rp1 = new Repository<SelectStyle>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public IEnumerable<StyleModel> GetByfId(int fid)
        {
            try
            {
                string sql = "select * from T_Style where t_fid=@0 and t_DelState=0 order by t_SortIndex";
                return rp.GetAll(sql, new object[] { fid });
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
                string sql = "select id,t_SortIndex,t_Style_Name,t_fId from T_Style where Id in (" + ids + ") and t_DelState=0 order by t_SortIndex";
                return rp1.GetAll(sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
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
                string sql = "select id,t_SortIndex,t_Style_Name,t_fId from T_Style where t_DelState=0 order by t_AddDate desc";
                return rp.GetPageData(page, pagesize, sql);
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
                string sql = "select id,t_SortIndex,t_Style_Name,t_fId from T_Style where Id=@0";
                return rp1.Get(sql, new object[] { Id });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(StyleModel model)
        {
            try
            {
                return rp.Insert(model) == null ? false : true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(StyleModel model)
        {
            try
            {
                return (int)rp.Update(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Del(StyleModel model)
        {
            try
            {
                return (int)rp.Delete(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
    }
}
