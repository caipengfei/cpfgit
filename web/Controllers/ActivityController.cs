using qch.core;
using qch.Infrastructure;
using qch.Models;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Sample.CommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        UserModel _loginUser;
        UserModel LoginUser
        {
            get
            {
                if (_loginUser == null)
                {
                    try
                    {
                        HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie 
                        if (authCookie == null)
                            return null;
                        FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密 
                        var loginUser = SerializeHelper.Instance.JsonDeserialize<UserLoginModel>(Ticket.UserData);//反序列化  
                        return userService.GetDetail(loginUser.LoginName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        return null;
                    }
                }
                return _loginUser;
            }
            set
            {
                this._loginUser = value;
            }
        }
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
        public ActionResult WxLogin(string code, string state)
        {
            #region 微信验证
            string Nonce = qch.Infrastructure.CookieHelper.GetCookieValue("Nonce");
            string wxuserguid = Guid.NewGuid().ToString();
            qch.Infrastructure.CookieHelper.SetCookie("WxUserGuid", wxuserguid);
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
                qch.Infrastructure.CookieHelper.SetCookie("wxLoginUserAvator", userInfo.headimgurl);
                qch.Infrastructure.CookieHelper.SetCookie("wxLoginUserNickname", userInfo.nickname);
                var wxuser = wxservice.GetByOpenId(userInfo.openid, userInfo.unionid);
                if (wxuser != null)
                {
                    //已绑定，设置登录票证然后跳转至个人中心
                    //取绑定的用户信息设置票证
                    var user = userService.GetDetail(wxuser.UserGuid);
                    if (user != null)
                    {
                        #region 赋于该会员授权
                        userService.SetAuthCookie(new UserLoginModel
                        {
                            LoginName = user.t_User_LoginId,
                            LoginPwd = ToolHelper.createNonceStr(),
                            SafeCode = ToolHelper.createNonceStr(),
                            UserName = user.t_User_RealName
                        });
                        #endregion
                    }
                    else
                    {
                        return RedirectToAction("/qch/login");
                    }
                    return RedirectToAction("/");
                }
                else
                {
                    //未绑定，先生存微信用户信息
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
                    //跳转至绑定页面
                    return RedirectToAction("/qch/bind?OpenId=" + userInfo.openid + "&UnionId=" + userInfo.unionid);
                }
            }
            #endregion

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
                qch.Infrastructure.CookieHelper.SetCookie("wxLoginOpenId", userInfo.openid.ToString());
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
            //if (user == null)
            //{
            //    msg.Data = "请先登录";
            //    msg.ParentURL = "/User/Login?returnurl=/NewCart/AddCart?CartType=" + CartType + "&GoodsCode=" + GoodsCode + "&Quantity=" + Quantity + "&SellerId=" + SellerId;
            //    return Json(msg);
            //}
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
            //为指定**添加图片
            qch.Infrastructure.FileUpService fileUpService = new qch.Infrastructure.FileUpService();

            var img = fileUpService.SaveImageFile(upfile, "/Images/ActivityImgs/", false);

            if (img != null)
            {
                //保存到数据库                
                if (activity != null)
                {
                    string x = img.OriginalImg;
                    if (!string.IsNullOrWhiteSpace(img.OriginalImg) && img.OriginalImg.Length > 5)
                        x = img.OriginalImg.Substring(img.OriginalImg.LastIndexOf('/') + 1);
                    activity.t_Activity_CoverPic = x;
                    if (service.Save(activity))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                        string avatorFilePath = System.Web.HttpContext.Current.Server.MapPath("~" + img.OriginalImg);
                        bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                        System.IO.File.Copy(avatorFilePath, "D:\\QCH2.0\\Attach\\Images\\" + x, isrewrite);
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

        [HttpPost]
        public ActionResult CheckTopic(string UserGuid, string Sign)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "签名错误！";
            try
            {
                var s = qch.Infrastructure.Encrypt.MD5Encrypt(UserGuid + "150919", true);
                if (s != Sign)
                {
                    msg.Data = "请从正规渠道进入！";
                    return Json(msg);
                }
                if (TopicService.GetTop999(UserGuid))
                {
                    msg.type = "success";
                    msg.Data = "Check Result Is OK";
                }
                return Json(msg);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(msg);
            }
        }
        [HttpPost]
        public ActionResult SaveTopic(string UserGuid, string Sign, string Contents)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "操作失败";
            var s = qch.Infrastructure.Encrypt.MD5Encrypt(UserGuid + "150919", true);
            if (s != Sign)
            {
                msg.Data = "请从正规渠道进入！";
                return Json(msg);
            }
            var user = userService.GetDetail(UserGuid);
            if (user == null)
            {
                msg.Data = "操作失败";
                log.Error("参与活动时未能获取到用户信息，UserGuid=" + UserGuid);
                return Json(msg);
            }
            string city = "郑州";
            if (!string.IsNullOrWhiteSpace(user.t_User_City))
                city = user.t_User_City;
            TopicModel m = new TopicModel
            {
                Guid = "",
                Contents = Contents,
                Distance = "",
                Pic = "",
                t_Date = DateTime.Now,
                t_DelState = 0,
                t_Topic_Address = "",
                t_Topic_City = city,
                t_Topic_Contents = Contents,
                t_Topic_Latitude = "",
                t_Topic_Longitude = "",
                t_Topic_Top = 999,//本次活动特殊处理
                t_User_Guid = UserGuid
            };
            msg = new TopicService().Save(m);
            return Json(msg);
        }

    }
}
