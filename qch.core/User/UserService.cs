using qch.Infrastructure;
using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

namespace qch.core
{
    /// <summary>
    /// 用户业务层
    /// </summary>
    public class UserService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserRepository rp = new UserRepository();


        public Msg Reg(UserModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "注册失败";
            try
            {
                /*
                 * 1、验证注册信息
                 * 2、验证推荐人信息
                 * 3、save user
                 * 4、查询需要赠送新注册会员什么优惠券并执行
                 * 5、查询需要赠送推荐人多少积分并执行
                 */
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    db.BeginTransaction();
                    
                    db.CompleteTransaction();
                }
                msg.type = "success";
                msg.Data = "注册成功";
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
        /// <summary>
        /// 获取某用户的所有一级推荐人
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserModel> GetReferral1(int page, int pagesize, string UserGuid)
        {
            try
            {
                return rp.GetReferral1(page, pagesize, UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某用户的所有一级推荐人(数量)
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public int GetReferral1(string UserGuid)
        {
            try
            {
                return rp.GetReferral1(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 获取某用户的所有二级推荐人(数量)
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public int GetReferral2(string UserGuid)
        {
            try
            {
                return rp.GetReferral2(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 检测页面验证码
        /// </summary>
        /// <param name="safecode"></param>
        /// <returns></returns>
        public bool ValidateSafeCode(string safecode)
        {
            try
            {
                string code = System.Web.HttpContext.Current.Session["ValidateCode"].ToString().ToLower();
                return code.Equals(safecode) ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 用户登录
        /// 手机号，登录名都可登录
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>返回登录结果</returns>
        public Msg Login(UserLoginModel model)
        {
            //用户登录业务
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "错误";
            try
            {
                if (string.IsNullOrWhiteSpace(model.LoginName))
                    return msg;
                if (string.IsNullOrWhiteSpace(model.LoginPwd))
                    return msg;

                //if (model.LoginName.Trim() != "cpf" && model.LoginName.Trim() != "admin")
                //{
                //    msg.type = "error";
                //        msg.Data = "系统维护中，请稍后登录,预计8:20分后恢复正常";
                //    //msg.Data = "系统维护中，请稍后登录";
                //    return Json(msg);
                //}

                //验证登录用户是否存在
                var user = GetDetail(model.LoginName);
                if (user == null)
                {
                    msg.type = "error";
                    msg.Data = "用户名不存在";
                    return msg;
                }

                model.LoginPwd = qch.Infrastructure.DESEncrypt.Encrypt(model.LoginPwd);
                var user1 = rp.Login(model.LoginName, model.LoginPwd);
                if (user1 != null)
                {
                    SetAuthCookie(model);
                    msg.type = "success";
                    msg.Data = "登录成功";
                    msg.ReturnUrl = "/home";
                }
                else
                {
                    msg.type = "error";
                    msg.Data = "用户名或密码错误";
                    return msg;
                }
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                msg.type = "error";
                msg.Data = "系统异常";
                return msg;
            }
        }
        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public UserModel GetLoginUser()
        {
            try
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie 
                    FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密 
                    var loginUser = SerializeHelper.Instance.JsonDeserialize<UserLoginModel>(Ticket.UserData);//反序列化 

                    return GetDetail(loginUser.LoginName);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 设置用户登录信息
        /// </summary>
        /// <param name="model"></param>
        public void SetAuthCookie(UserLoginModel model)
        {
            //1.序列化用信息
            string UserData = SerializeHelper.Instance.JsonSerialize(model);
            //2.设置票证
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.LoginName, DateTime.Now, DateTime.Now.AddMinutes(30), false, UserData);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));//加密身份信息到cookie
            HttpContext.Current.Response.Cookies.Add(cookie);
            CookieHelper.SetCookie("UserName", model.LoginName);
            string[] roles = new string[] { };
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(model.LoginName), roles);
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        public void LogOut()
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                authCookie.Expires = DateTime.Now.AddHours(-1);
                HttpContext.Current.Response.Cookies.Add(authCookie);
                CookieHelper.ClearCookie("UserName");
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public UserModel GetDetail(string LoginName)
        {
            try
            {
                return rp.GetDetail(LoginName);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 根据UserStyle分页获取
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserStyle"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserModel> GetAll(int page, int pagesize, int UserStyle)
        {
            try
            {
                return rp.GetAll(page, pagesize, UserStyle);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有用户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserModel> GetAll(int page, int pagesize)
        {
            try
            {
                return rp.GetAll(page, pagesize);
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
        public UserModel GetById(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                return rp.GetById(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Msg Save(UserModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "操作失败";
            try
            {
                if (model == null)
                    return msg;
                var tt = GetById(model.Guid);
                if (tt != null)
                {
                    if (rp.Edit(model))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    model.t_User_Date = DateTime.Now;
                    if (rp.Add(model))
                    {
                        msg.type = "success";
                        msg.Data = "新增成功";
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
        /// <summary>
        /// 更改用户的删除状态
        /// </summary>
        /// <param name="Guid"></param>
        /// <param name="DelState"></param>
        /// <returns></returns>
        public bool EditStyle(string Guid, int DelState)
        {
            try
            {
                var model = GetById(Guid);
                if (model == null)
                    return false;
                model.t_DelState = DelState;
                return rp.Edit(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
    }
}
