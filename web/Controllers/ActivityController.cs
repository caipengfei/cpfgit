using qch.core;
using qch.Models;
using Senparc.Weixin.MP.AdvancedAPIs;
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
            #region 微信验证
            string Nonce = qch.Infrastructure.CookieHelper.GetCookieValue("Nonce");
            string wxuserguid = Guid.NewGuid().ToString();
            qch.Infrastructure.CookieHelper.SetCookie("WxUserGuid", wxuserguid);
            ViewBag.ActivityGuid = ActivityGuid;
            ViewBag.OpenId = "";
            ViewBag.UserLogo = "";
            ViewBag.Name = "";

            OAuthAccessTokenResult result = null;
            if (Session["AccountTokenResult"] != null)
                result = (OAuthAccessTokenResult)Session["AccountTokenResult"];
            else
            {
                //通过，用code换取access_token
                if (string.IsNullOrWhiteSpace(code))
                {
                    return Redirect("/wxUser/wxlogin");
                }
                try
                {
                    result = OAuth.GetAccessToken(appId, appSecret, code);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    return Redirect("/wxUser/wxlogin");
                }
                if (result == null || result.access_token == null)
                {
                    return Redirect("/wxUser/wxlogin");
                }
                Session["AccountTokenResult"] = result;
            }
            OAuthUserInfo userInfo = null;
            if (Session["AccountUserResult"] != null)
                userInfo = (OAuthUserInfo)Session["AccountUserResult"];
            else
            {
                userInfo = OAuth.GetUserInfo(result.access_token, result.openid);
                if (userInfo == null || userInfo.openid == null)
                {
                    return Redirect("/wxUser/wxlogin");
                }
                Session["AccountUserResult"] = userInfo;
            }

            if (userInfo != null)
            {
                ViewBag.OpenId = userInfo.openid;
                ViewBag.UserLogo = userInfo.headimgurl;
                ViewBag.Name = userInfo.nickname;
                qch.Infrastructure.CookieHelper.SetCookie("wxLoginName", userInfo.nickname.ToString());
 
                string avatorFileName = Guid.NewGuid().ToString() + ".jpg"; ;
                log.Info("avatorFileName=" + avatorFileName);
                var msg = wxservice.Save(new WXUserModel
                {
                    Guid = "",
                    OpenId = "",
                    Nonce = Nonce,
                    UserGuid = wxuserguid,
                    WxTgUserGuid = "",
                    MediaId = "",
                    QrCode = "",
                    UnionId = userInfo.unionid.ToString(),
                    Avator = "small_" + avatorFileName,
                    CreateDate = DateTime.Now,
                    KFDate = DateTime.Now,
                    KFOpenId = userInfo.openid.ToString(),
                    MediaDate = DateTime.Now,
                    Name = userInfo.nickname.ToString(),
                    UserType = 2
                });
                if (msg.type == "success")
                {
                    log.Info("插入成功");
                    //下载微信头像到指定文件夹
                    Senparc.Weixin.MP.Sample.CommonService.ImageHelper.DownloadFile(100, 100, userInfo.headimgurl.ToString(), avatorFileName);
                }
                else { log.Error("插入失败"); }
            }

            #endregion
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
