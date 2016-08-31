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
    [AdminAuthorization(Roles = "admin,financial")]
    public class BranchController : Controller
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserService userService = new UserService();
        BranchService branchService = new BranchService();
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
        // GET: /Admin/Branch/

        //获取所有部门信息
        [HttpPost]
        public ActionResult GetAllBranch()
        {
            var model = branchService.GetAllBranch();
            if (model != null)
                return Json(model);
            else
                return Json(null);
        }

        public ActionResult Index(string BranchGuid)
        {
            ViewData["BranchGuid"] = BranchGuid;
            //获取所有部门信息
            var branch = branchService.GetAllBranch();
            if (branch != null)
            {
                ViewData["Branchs"] = new SelectList(branch, "Guid", "t_branch_name");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(int page, int pagesize, string BranchGuid)
        {
            var model = branchService.GetAll(page, pagesize, BranchGuid);

            if (model != null)
            {
                return Json(new { Rows = model.Items, Total = model.TotalItems });
            }
            else
            {
                return Json(new { Rows = 0, Total = 0 });
            }
        }
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(int? page, int? pagesize)
        {
            int Page = page ?? 1;
            int PageSize = pagesize ?? 30;
            var model = branchService.GetAllBranch(Page, PageSize);
            if (model != null)
                return Json(new { Rows = model.Items, Total = model.TotalItems });
            else
                return Json(new { Rows = "", Total = 0 });
        }
        //保存部门信息
        public ActionResult Edit(string id)
        {
            var model = branchService.GetBranchById(id);
            if (model == null)
                model = new BranchModel();
            if (LoginUser != null)
                model.t_Editor = LoginUser.t_User_RealName;
            model.t_CreateDate = DateTime.Now;
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(BranchModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "保存失败";
            if (branchService.SaveBranch(model))
            {
                msg.Data = "保存成功";
                msg.type = "success";
            }
            return Json(msg);
        }
        //保存部门员工信息
        public ActionResult Save(string id)
        {
            //获取所有部门信息
            var branch = branchService.GetAllBranch();
            if (branch != null)
            {
                ViewData["Branchs"] = new SelectList(branch, "Guid", "t_branch_name");
            }

            var model = branchService.GetById(id);
            if (model == null)
                model = new BranchUserModel();
            if (LoginUser != null)
                model.t_Editor = LoginUser.t_User_RealName;
            model.t_CreateDate = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(BranchUserModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "保存失败";
            var user = userService.GetDetail(model.t_User_Phone);
            if (user == null)
            {
                msg.Data = "用户不存在";
                return Json(msg);
            }
            var b = branchService.GetBranchById(model.t_Branch_Guid);
            if (b == null)
            {
                msg.Data = "不存在的部门";
                return Json(msg);
            }
            model.t_User_Guid = user.Guid;
            var branch = branchService.GetByUserGuid(user.Guid, model.t_Branch_Guid);
            if (branch != null)
            {
                msg.Data = "请勿重复添加";
                return Json(msg);
            }
            model.t_User_RealName = user.t_User_RealName;
            model.t_User_Phone = user.t_User_LoginId;
            if (branchService.Save(model))
            {
                msg.Data = "保存成功";
                msg.type = "success";
            }
            return Json(msg);
        }
        //删除部门信息
        [HttpPost]
        public ActionResult Delete(string id)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "删除失败";
            if (branchService.DelBranch(id))
            {
                msg.Data = "删除成功";
                msg.type = "success";
            }
            return Json(msg);
        }
        //删除部门员工信息
        [HttpPost]
        public ActionResult Del(string id)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "删除失败";
            if (branchService.Del(id))
            {
                msg.Data = "删除成功";
                msg.type = "success";
            }
            return Json(msg);
        }
    }
}
