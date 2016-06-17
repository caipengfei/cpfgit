using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 文章资源层
    /// </summary>
    public class NewsRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<NewsModel> rp = new Repository<NewsModel>();

        /// <summary>
        /// 分页获取所有
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public PetaPoco.Page<NewsModel> GetAll(int page, int pagesize, string title)
        {
            try
            {
                string sql = "select * from T_News where t_DelState=0 and t_News_Title like (@0) order by t_News_Date desc";
                return rp.GetPageData(page, pagesize, sql, new object[] { "%" + title + "%" });
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
        /// <param name="typeId"></param>
        /// <returns></returns>
        public PetaPoco.Page<NewsModel> GetAll(int page, int pagesize, int typeId)
        {
            try
            {
                string sql = "select a.[Guid],a.[t_News_Style],a.[t_News_Pic],a.[t_News_Title],a.[t_News_LimitContents],a.[t_News_Author],a.[t_News_Province],a.[t_News_City],a.[t_News_Date],a.[t_News_Counts],a.[t_News_Index],a.[t_News_Recommand],a.[t_DelState],b.[t_Style_Name] from [T_News] as a left join [T_Style] as b on a.[t_News_Style]=b.id where b.t_fid=39 and b.id=@0 and a.t_DelState=0 order by a.t_News_Recommand desc,a.t_News_Index,a.t_News_Date desc";
                return rp.GetPageData(page, pagesize, sql, new object[] { typeId });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取所有推荐文章
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NewsModel> GetAll()
        {
            try
            {
                string sql = "select top 20 a.[Guid],a.[t_News_Style],a.[t_News_Pic],a.[t_News_Title],a.[t_News_LimitContents],a.[t_News_Author],a.[t_News_Province],a.[t_News_City],a.[t_News_Date],a.[t_News_Counts],a.[t_News_Index],a.[t_News_Recommand],a.[t_DelState],b.[t_Style_Name] from [T_News] as a left join [T_Style] as b on a.[t_News_Style]=b.id where b.t_fid=39 and a.t_DelState=0 and a.t_News_Recommand=1 order by a.t_News_Index, a.t_News_Date desc";
                return rp.GetAll(sql);
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
        public PetaPoco.Page<NewsModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select a.[Guid],a.[t_News_Style],a.[t_News_Pic],a.[t_News_Title],a.[t_News_LimitContents],a.[t_News_Author],a.[t_News_Province],a.[t_News_City],a.[t_News_Date],a.[t_News_Counts],a.[t_News_Index],a.[t_News_Recommand],a.[t_DelState],b.[t_Style_Name] from [T_News] as a left join [T_Style] as b on a.[t_News_Style]=b.id where b.t_fid=39 and a.t_DelState=0 order by  a.t_News_Recommand desc,a.t_News_Index,a.t_News_Date desc";
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
        public NewsModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_News where guid=@0";
                return rp.Get(sql, new object[] { Guid });
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
        public bool Add(NewsModel model)
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
        public bool Edit(NewsModel model)
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
        public bool Del(NewsModel model)
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
