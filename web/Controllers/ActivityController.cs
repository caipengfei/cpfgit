using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class ActivityController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AreaService areaService = new AreaService();
        WXUserService wxservice = new WXUserService();
        UserService userService = new UserService();
        WXAuthService wxauthService = new WXAuthService();
        ActivityService service = new ActivityService();
        /// <summary>
        /// 图片保存路径
        /// </summary>        
        public static string ImageUrl { get { return string.Format("/images/{0}/{1}/{2}/", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); } }

        //
        // GET: /Activity/
        string appId = System.Configuration.ConfigurationManager.AppSettings["kfAppID"].ToString();
        string appSecret = System.Configuration.ConfigurationManager.AppSettings["kfAppSecret"].ToString();
        public ActionResult Index()
        {
            return View();
        }
        /*
         * 1、
         */
        //发布活动
        public ActionResult Publish(string ActivityGuid, string code, string state)
        {
            
            var list = areaService.GetAllProvince();
            return View(list);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Publish(ActivityModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "发布失败";
            try
            {
                string name = qch.Infrastructure.CookieHelper.GetCookieValue("wxLoginName");
                model.t_AddBy = name;
                string openid = qch.Infrastructure.CookieHelper.GetCookieValue("wxLoginOpenId");
                string cname = "";
                var city = areaService.GetCity(model.t_Activity_City);
                if (city != null)
                    cname = city.CityName;
                log.Info("发布活动所选城市：" + cname);
                model.t_Activity_CityName = cname;
                msg = service.Publish("oU0q_uOO4dSfO4m3Ekpc1GrHhHhw", model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return Json(msg);
        }
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpImg(string Guid, HttpPostedFileBase upfile)
        {
            //获取id活动信息
            var activity = service.GetById(Guid);

            Msg msg = new Msg();
            //为指定商品添加图片
            qch.Infrastructure.FileUpService fileUpService = new qch.Infrastructure.FileUpService();

            var img = fileUpService.SaveImageFile(upfile, fileUpService.ImageUrl, false);

            if (img != null)
            {
                //保存到数据库                
                if (activity != null)
                {
                    activity.t_Activity_CoverPic = img.OriginalImg;
                    if (service.Save(activity))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
            }
            return Json(msg);
        }
        //获取所有省份
        [HttpPost]
        public ActionResult GetProvince()
        {
            var list = areaService.GetAllProvince();
            return Json(list);
        }
        //根据省获取市

        public ActionResult GetCity(long Id)
        {
            var model = areaService.GetCityByProvince(Id);
            return View(model);
        }
        //根据市获取区县

        public ActionResult GetDis(long Id)
        {
            var model = areaService.GetDistrictByCity(Id);
            return View(model);
        }

    }
}
