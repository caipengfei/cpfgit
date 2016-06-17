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
    public class NewsController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        NewsService service = new NewsService();
        StyleService styleService = new StyleService();

        [HttpGet]
        public object EditCount(string NewsGuid)
        {
            try
            {
                var model = service.GetById(NewsGuid);
                if (model == null)
                    return null;
                model.t_News_Counts++;
                return service.Save(model);
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
        /// 
        [HttpGet]
        public IEnumerable<NewsModel> GetAll()
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
        /// 
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IEnumerable<StyleModel> GetNewsMenu(int fid)
        {
            try
            {
                return styleService.GetByfId(fid);
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
        /// 
        [HttpGet]
        public NewsModel GetById(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                var model = service.GetById(Guid);
                if (model != null)
                {
                    model.t_News_Counts++;
                    service.Save(model);
                }
                return model;
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
        /// 
        [HttpGet]
        public PetaPoco.Page<NewsModel> GetAll(int page, int pagesize, int typeId)
        {
            try
            {
                return service.GetAll(page, pagesize, typeId);
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
        /// 
        [HttpGet]
        public PetaPoco.Page<NewsModel> GetAll(int page, int pagesize)
        {
            try
            {
                return service.GetAll(page, pagesize);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
