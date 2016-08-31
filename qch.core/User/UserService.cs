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

        //创建随机字符串  
        public string createNonceStr(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string str = "";
            Random rad = new Random();
            for (int i = 0; i < length; i++)
            {
                str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
            }
            return str;
        }


        public bool CheckReommuser(string UserGuid)
        {
            try
            {
                string shangxiaoying = "daab6404-0d70-46cf-98e7-d9699f4634de";//异地推||尚晓英
                string ReommUser = "";
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = " where guid=@0 and t_delstate=0";
                    var user = db.SingleOrDefault<T_Users>(sql, new object[] { UserGuid });
                    if (user == null)
                        return false;
                    for (int i = 0; i < 10; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(user.t_ReommUser))
                        {
                            ReommUser = user.t_ReommUser;
                            user = db.SingleOrDefault<T_Users>(sql, new object[] { ReommUser });
                            if (user.Guid == shangxiaoying)
                                return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public PetaPoco.Page<TuijianInfo> GetReferral(int page, int pagesize, string UserGuid, DateTime? b, DateTime? e)
        {
            try
            {
                DateTime begin = b == null ? Convert.ToDateTime("1990-01-01") : qch.Infrastructure.TimeHelper.GetStartDateTime(Convert.ToDateTime(b));
                DateTime end = e == null ? DateTime.Now : qch.Infrastructure.TimeHelper.GetEndDateTime(Convert.ToDateTime(e));
                return rp.GetReferral(page, pagesize, UserGuid, begin, end);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某用户的审核状态
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public object GetUserStyleAudit(string UserGuid)
        {
            try
            {
                return rp.GetUserStyleAudit(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                    //生成会员信息
                    var userguid = db.Insert(model);
                    if (userguid == null)
                    {
                        log.Info("用户注册失败，注册手机号：" + model.t_User_Mobile);
                        return msg;
                    }
                    //查询优惠券信息
                    var voucher = db.SingleOrDefault<T_Voucher>(" where T_Voucher_Scope='yonghuzhuce' and T_DelState=0");
                    if (voucher == null)
                    {
                        log.Info("用户注册时未能获取到优惠券信息");
                    }
                    else
                    {
                        //为用户生成优惠券信息
                        T_User_Voucher uv = new T_User_Voucher
                        {
                            Guid = Guid.NewGuid().ToString(),
                            T_DelState = 0,
                            T_GetDate = DateTime.Now,
                            T_User_Guid = userguid.ToString(),
                            T_Voucher_Guid = voucher.Guid,
                            T_Voucher_Pwd = createNonceStr(8),
                            T_Voucher_State = 0
                        };
                        db.Insert(uv);
                    }
                    //如果有推荐人
                    if (!string.IsNullOrWhiteSpace(model.t_ReommUser))
                    {
                        //给推荐人赠送积分、优惠券
                        var tjuser = db.SingleOrDefault<T_Users>(" where guid=@0 and t_DelState=0", new object[] { model.t_ReommUser });
                        if (tjuser != null)
                        {
                            #region 给推荐人赠送积分（已取消）
                            //查询积分信息
                            //var integral = db.SingleOrDefault<T_IntegralManger>(" where t_PinYin='yonghuzhuce' and t_DelState=0");
                            //if (integral == null)
                            //{
                            //    log.Info(string.Format("用户注册时未能查询到注册赠送积分的信息，表名：T_IntegralManger，注册手机号：{0},推荐人Guid：{1}", model.t_User_Mobile, model.t_ReommUser));
                            //}
                            //else
                            //{
                            //    //获取用户现有积分余额
                            //    int jifen = 0;
                            //    string sql = "select top 1 * from t_user_integral where t_User_Guid='" + model.t_ReommUser + "' and t_DelState=0 order by t_AddDate desc";
                            //    var nowIntegral = db.SingleOrDefault<T_User_Integral>(sql);
                            //    if (nowIntegral != null)
                            //        jifen = (int)nowIntegral.t_UserIntegral_Reward;
                            //    T_User_Integral ui = new T_User_Integral
                            //    {
                            //        Guid = Guid.NewGuid().ToString(),
                            //        t_AddDate = DateTime.Now,
                            //        t_DelState = 0,
                            //        t_IntegralManager_Guid = integral.Guid,
                            //        t_IntegralManager_PinYin = integral.t_PinYin,
                            //        t_Remark = "推荐用户注册赠送（微信端）",
                            //        t_User_Guid = model.t_ReommUser,
                            //        t_UserIntegral_ReduceReward = 0,
                            //        t_UserIntegral_Reward = jifen + integral.t_Integral,
                            //        t_UserIntergral_AddReward = integral.t_Integral
                            //    };
                            //    if (db.Insert(ui) != null)
                            //    {
                            //        string tmpguid = "";
                            //        //向推荐人推送消息
                            //        if (!string.IsNullOrWhiteSpace(tjuser.t_Andriod_Rid))
                            //        {
                            //            #region 客户推送并存储推送信息
                            //            string strRid = tjuser.t_Andriod_Rid;
                            //            string mes = string.Format("您推荐的【{0}】已加入青创汇，您获得了{1}积分奖励！", model.t_User_RealName, integral.t_Integral);

                            //            string message = "{\"platform\": \"all\",\"audience\" : {\"registration_id\":[\"" + tjuser.t_Andriod_Rid + "\"]},\"notification\": { \"android\": { \"alert\": \"" + mes + "\",\"title\": \"" + mes + "\",\"builder_id\": 1,\"extras\": {\"Guid\": \"98d51a31-47c5-4743-bde6-5e1a8d86f89e\",\"type\": \"activity\"}}, \"ios\": {\"alert\": \"" + mes + "\",\"sound\": \"default\",\"badge\": \"+1\", \"extras\": {\"Guid\": \"98d51a31-47c5-4743-bde6-5e1a8d86f89e\",\"type\": \"activity\"}}},\"options\": { \"time_to_live\": 259200,\"apns_production\": false}}";
                            //            JPush.PushMsg(message);
                            //            //存入数据库
                            //            T_HistoryPush modelpush = new T_HistoryPush()
                            //            {
                            //                Guid = Guid.NewGuid().ToString(),
                            //                t_Alert = mes,
                            //                t_Associate_Guid = tjuser.t_Andriod_Rid,
                            //                t_Date = DateTime.Now,
                            //                t_DelState = 0,
                            //                t_Title = mes,
                            //                t_Type = "message",
                            //                t_User_Guid = tjuser.Guid
                            //            };
                            //            db.Insert(modelpush);
                            //            #endregion
                            //        }
                            //    }
                            //}
                            #endregion
                            #region 给推荐人赠送优惠券 (已取消 2016-07-07)
                            //给推荐人赠送优惠券
                            //查询优惠券信息
                            //var tjvoucher = db.SingleOrDefault<T_Voucher>(" where T_Voucher_Scope='zhijietuijian' and T_DelState=0");
                            //if (tjvoucher == null)
                            //{
                            //    log.Info("用户注册时未能获取到需要赠送给推荐人的优惠券信息");
                            //}
                            //else
                            //{
                            //    //为用户生成优惠券信息
                            //    T_User_Voucher uv = new T_User_Voucher
                            //    {
                            //        Guid = Guid.NewGuid().ToString(),
                            //        T_DelState = 0,
                            //        T_GetDate = DateTime.Now,
                            //        T_User_Guid = tjuser.Guid,
                            //        T_Voucher_Guid = tjvoucher.Guid,
                            //        T_Voucher_Pwd = createNonceStr(8),
                            //        T_Voucher_State = 0
                            //    };
                            //    db.Insert(uv);
                            //}
                            #endregion
                        }
                        else
                        {
                            log.Info("用户注册时，推荐人信息异常，未能完成给推荐人赠送优惠券等功能");
                        }
                    }
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
        /// 获取某用户的所有一级推荐人(数量)
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public int GetReferral1(string UserGuid, DateTime? b, DateTime? e)
        {
            try
            {
                DateTime begin = b == null ? Convert.ToDateTime("1990-01-01") : qch.Infrastructure.TimeHelper.GetStartDateTime(Convert.ToDateTime(b));
                DateTime end = e == null ? DateTime.Now : qch.Infrastructure.TimeHelper.GetEndDateTime(Convert.ToDateTime(e));
                return rp.GetReferral1(UserGuid, begin, end);
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
        public int GetReferral2(string UserGuid, DateTime? b, DateTime? e)
        {
            try
            {
                DateTime begin = b == null ? Convert.ToDateTime("1990-01-01") : qch.Infrastructure.TimeHelper.GetStartDateTime(Convert.ToDateTime(b));
                DateTime end = e == null ? DateTime.Now : qch.Infrastructure.TimeHelper.GetEndDateTime(Convert.ToDateTime(e));
                return rp.GetReferral2(UserGuid, begin, end);
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
                    model.UserName = user1.t_User_RealName;
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
        /// 分页获取所有用户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserModel> GetAll(int page, int pagesize, string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                    return rp.GetAll(page, pagesize);
                else
                    return rp.GetAll(page, pagesize, phone);
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
