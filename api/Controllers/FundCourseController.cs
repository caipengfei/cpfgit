using qch.core;
using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    /// <summary>
    /// 众筹
    /// </summary>
    public class FundCourseController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        FundCourseService service = new FundCourseService();
        string ImageUrl = System.Configuration.ConfigurationManager.AppSettings["ImageUrl"].ToString();
        string AvatorUrl = System.Configuration.ConfigurationManager.AppSettings["AvatorUrl"].ToString();
        UserTalkService talkService = new UserTalkService();
        OrderService orderService = new OrderService();
        /// <summary>
        /// 获取众筹详情
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        [HttpGet]
        public SelectFundCourse GetById(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                var model = service.GetById(Guid);
                if (model != null)
                {
                    //已筹多少钱
                    model.NowMoney = orderService.GetFundCourseMoney(model.Guid);
                    //众筹人次
                    model.FundCourseCount = orderService.GetFundCourseCount(model.Guid);
                    if (model.T_FundCourse_Pic != null)
                        model.T_FundCourse_Pic = ImageUrl + model.T_FundCourse_Pic;
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
    }
}
