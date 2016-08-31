using qch.core;
using qch.Infrastructure;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using web.filters;

namespace web.Areas.Admin.Controllers
{
    public class JXController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BranchService service = new BranchService();
        UserService userService = new UserService();
        BranchService branchService = new BranchService();
        UserRoleService userRoleService = new UserRoleService();
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
        //
        // GET: /Admin/JX/
        [AdminAuthorization(Roles = "admin,financial,service,tuiguang,areaMgr")]
        public ActionResult Index(string phone, DateTime? begin, DateTime? end)
        {
            ViewBag.phone = phone;
            ViewBag.BeginTime = begin ?? Convert.ToDateTime("1900-01-01");
            ViewBag.EndTime = end ?? DateTime.Now;
            return View();
        }
        [AdminAuthorization(Roles = "admin,financial,service,tuiguang,areaMgr")]
        [HttpPost]
        public ActionResult Index(string phone, int? page, int? pagesize, DateTime? begin, DateTime? end)
        {
            int P = page ?? 1;
            int S = pagesize ?? 20;
            var user = userService.GetDetail(phone);
            if (user == null)
                return Json(new { Rows = 0, Total = 0 });
            if (LoginUser == null)
                return Json(new { Rows = 0, Total = 0 });

            var model = userService.GetReferral(P, S, user.Guid, begin, end);

            if (model != null)
            {
                return Json(new { Rows = model.Items, Total = model.TotalItems });
            }
            else
            {
                return Json(new { Rows = 0, Total = 0 });
            }
        }
        #region 员工绩效
        [AdminAuthorization(Roles = "admin,financial,service,tuiguang,areaMgr")]
        public ActionResult Jx(DateTime? begin, DateTime? end)
        {
            ViewBag.BeginTime = begin ?? Convert.ToDateTime("1900-01-01");
            ViewBag.EndTime = end ?? DateTime.Now;
            return View();
        }
        [AdminAuthorization(Roles = "admin,financial,service,tuiguang,areaMgr")]
        [HttpPost]
        public ActionResult Jx(int? page, int? pagesize, DateTime? begin, DateTime? end)
        {
            int P = page ?? 1;
            int S = pagesize ?? 20;
            if (LoginUser == null)
                return Json(new { Rows = 0, Total = 0 });

            var model = service.GetByUser(P, S, LoginUser.Guid, begin, end);

            if (model != null)
            {
                return Json(new { Rows = model.Items, Total = model.TotalItems });
            }
            else
            {
                return Json(new { Rows = 0, Total = 0 });
            }
        }
        [AdminAuthorization(Roles = "admin,financial,service,areaMgr")]
        public ActionResult Jx1(string phone, DateTime? begin, DateTime? end)
        {
            ViewBag.phone = phone;
            ViewBag.BeginTime = begin ?? Convert.ToDateTime("1900-01-01");
            ViewBag.EndTime = end ?? DateTime.Now;
            return View();
        }
        [AdminAuthorization(Roles = "admin,financial,service,areaMgr")]
        [HttpPost]
        public ActionResult Jx1(string phone, int? page, int? pagesize, DateTime? begin, DateTime? end)
        {
            int P = page ?? 1;
            int S = pagesize ?? 20;
            var user = userService.GetDetail(phone);
            if (user == null)
                return Json(new { Rows = 0, Total = 0 });
            if (LoginUser == null)
                return Json(new { Rows = 0, Total = 0 });
            var userRole = userRoleService.GetUserRole(LoginUser.Guid);//当前用户角色
            if (userRole.Contains("areaMgr"))
            {
                //如果是客服权限
                if (!branchService.CheckLoginUser(LoginUser.Guid, user.Guid, ""))
                    return Json(new Msg { type = "error", Data = "没有管理权限无法访问！" });
            }

            var model = service.GetByUser(P, S, user.Guid, begin, end);

            if (model != null)
            {
                return Json(new { Rows = model.Items, Total = model.TotalItems });
            }
            else
            {
                return Json(new { Rows = 0, Total = 0 });
            }
        }
        #endregion
        #region 部门绩效
        [AdminAuthorization(Roles = "admin,financial,service,areaMgr")]
        public ActionResult Jx2(string BranchGuid, DateTime? begin, DateTime? end)
        {
            //获取所有部门信息
            var branch = branchService.GetAllBranch();
            if (branch != null)
            {
                ViewData["Branchs"] = new SelectList(branch, "Guid", "t_branch_name");
            }
            ViewBag.BranchGuid = BranchGuid;
            ViewBag.BeginTime = begin ?? Convert.ToDateTime("1900-01-01");
            ViewBag.EndTime = end ?? DateTime.Now;
            return View();
        }
        [AdminAuthorization(Roles = "admin,financial,service,areaMgr")]
        [HttpPost]
        public ActionResult Jx2(string BranchGuid, int? page, int? pagesize, DateTime? begin, DateTime? end)
        {
            var userRole = userRoleService.GetUserRole(LoginUser.Guid);//当前用户角色
            if (userRole.Contains("service"))
            {
                //如果是客服权限
                if (!branchService.CheckLoginUser(LoginUser.Guid, "", BranchGuid))
                    return Content("权限不足！");
            }

            int P = page ?? 1;
            int S = pagesize ?? 20;
            var model = service.GetByBranch(P, S, BranchGuid, begin, end);

            if (model != null)
            {
                return Json(new { Rows = model.Items, Total = model.TotalItems });
            }
            else
            {
                return Json(new { Rows = 0, Total = 0 });
            }
        }
        #endregion

    }
}
