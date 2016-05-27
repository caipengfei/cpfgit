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
    /// 课程
    /// </summary>
    public class CourseController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CourseService service = new CourseService();
        string ImageUrl = System.Configuration.ConfigurationManager.AppSettings["ImageUrl"].ToString();
        string AvatorUrl = System.Configuration.ConfigurationManager.AppSettings["AvatorUrl"].ToString();
        UserTalkService talkService = new UserTalkService();
        /// <summary>
        /// 获取课程详情
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        [HttpGet]
        public SelectCourse GetById(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                var model = service.GetById(Guid);
                if (model != null)
                {
                    if (model.t_Course_Pic != null)
                        model.t_Course_Pic = ImageUrl + model.t_Course_Pic;
                    if (model.LecturerAvator != null)
                        model.LecturerAvator = ImageUrl + model.LecturerAvator;
                    var talk = talkService.GetAll(1, 10, model.Guid);
                    if (talk != null && talk.Items != null)
                        model.Talk = talk.Items;
                    if (model.Talk != null)
                    {
                        foreach (var item in model.Talk)
                        {
                            if (item.UserAvator != null)
                                item.UserAvator = AvatorUrl + item.UserAvator;
                        }
                    }
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
        /// 分页获取所有推荐
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        public PetaPoco.Page<SelectCourse> GetRecommendCourse(int page, int pagesize)
        {
            try
            {
                var model = service.GetRecommendCourse(page, pagesize);
                if (model != null && model.Items != null)
                {
                    foreach (var item in model.Items)
                    {
                        if (item.t_Course_Pic != null)
                            item.t_Course_Pic = ImageUrl + item.t_Course_Pic;
                        if (item.LecturerAvator != null)
                            item.LecturerAvator = ImageUrl + item.LecturerAvator;
                        var talk = talkService.GetAll(1, 10, item.Guid);
                        if (talk != null && talk.Items != null)
                            item.Talk = talk.Items;
                        if (item.Talk != null)
                        {
                            foreach (var tt in item.Talk)
                            {
                                if (tt.UserAvator != null)
                                    tt.UserAvator = ImageUrl + tt.UserAvator;
                            }
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
