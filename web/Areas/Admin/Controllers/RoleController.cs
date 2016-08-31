using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web.filters;
namespace web.Areas.Admin.Controllers
{
    [AdminAuthorization(Roles = "admin")]
    public class RoleController : Controller
    {
        UserRoleService roleService = new UserRoleService();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int page, int pagesize)
        {
            var model = roleService.GetAllRole(page, pagesize);
            return Json(new { Rows = model.Items, Total = model.TotalItems });
        }

        //添加角色
        public ActionResult Edit(int? Id)
        {
            RoleModel model = new RoleModel();
            model.IsActive = true;
            if (Id != null)
            {
                model = roleService.GetRoleById((int)Id);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(RoleModel model)
        {
            Msg msg = new Msg { type = "error", Data = "数据格式不正确" };
            if (ModelState.IsValid)
            {
                if (model.Id <= 0)
                {
                    if (roleService.IsExist(model.RoleName))
                    {
                        msg.type = "error";
                        msg.Data = "该角色名己存在";
                        return Json(msg);
                    }
                }
                if (roleService.SaveRole(model))
                {
                    msg.type = "success";
                    msg.Data = "保存成功";
                }
                else
                {
                    msg.type = "error";
                    msg.type = "保存失败";
                }
            }
            return Json(msg);
        }

        [HttpPost]
        public ActionResult Del(int Id)
        {
            Msg msg = new Msg();
            if (roleService.DelRole(Id))
            {
                msg.type = "success";
                msg.Data = "操作成功";
            }
            else
            {
                msg.type = "error";
                msg.Data = "操作失败";
            }
            return Json(msg);
        }
    }
}
