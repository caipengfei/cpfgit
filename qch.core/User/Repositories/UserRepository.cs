using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户资源层
    /// </summary>
    public class UserRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<UserModel> rp = new Repository<UserModel>();
        Repository<TuijianInfo> rp1 = new Repository<TuijianInfo>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public PetaPoco.Page<TuijianInfo> GetReferral(int page, int pagesize, string UserGuid, DateTime b, DateTime e)
        {
            try
            {
                string sql = "select a.Guid,a.t_User_LoginId,a.t_User_RealName,a.t_User_Date,a.t_Andriod_Rid,b.t_Bank_OpenUser,b.t_AddDate from t_users as a left join T_User_Bank as b on a.guid=b.t_User_Guid where a.t_reommuser=@0 and a.t_User_Date between @1 and @2 order by a.t_user_date desc";
                return rp1.GetPageData(page, pagesize, sql, new object[] { UserGuid, b, e });
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
                object xy;
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select t_UserStyleAudit from t_users where guid=@0 and t_DelState=0";
                    xy = db.ExecuteScalar<object>(sql, new object[] { UserGuid });
                }
                return xy;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        } 
        /// <summary>
        /// 获取某用户的所有一级推荐人
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserModel> GetReferral1(int page, int pagesize, string UserGuid, DateTime b, DateTime e)
        {
            try
            {
                string sql = "select * from t_users where t_ReommUser=@0 and t_DelState=0 and t_user_date between @1 and @2";
                return rp.GetPageData(page, pagesize, sql, new object[] { UserGuid, b, e });
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
        public int GetReferral1(string UserGuid, DateTime b, DateTime e)
        {
            try
            {
                int xy = 0;
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select count(1) from t_users where t_ReommUser=@0 and t_DelState=0 and t_user_date between @1 and @2";
                    xy = Convert.IsDBNull(db.ExecuteScalar<object>(sql, new object[] { UserGuid, b, e })) ? 0 : db.ExecuteScalar<int>(sql, new object[] { UserGuid, b, e });
                }
                return xy;
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
        public int GetReferral2(string UserGuid, DateTime b, DateTime e)
        {
            try
            {
                int xy = 0;
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select count(1) from t_users where t_ReommUser in (select Guid from t_users where t_ReommUser=@0 and t_DelState=0) and t_DelState=0 and t_user_date between @1 and @2";
                    xy = Convert.IsDBNull(db.ExecuteScalar<object>(sql, new object[] { UserGuid, b, e })) ? 0 : db.ExecuteScalar<int>(sql, new object[] { UserGuid, b, e });
                }
                return xy;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
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
                string sql = "select * from t_users where t_ReommUser=@0 and t_DelState=0 order by t_user_date desc";
                return rp.GetPageData(page, pagesize, sql, new object[] { UserGuid });
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
                int xy = 0;
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select count(1) from t_users where t_ReommUser=@0 and t_DelState=0";
                    xy = Convert.IsDBNull(db.ExecuteScalar<object>(sql, new object[] { UserGuid })) ? 0 : db.ExecuteScalar<int>(sql, new object[] { UserGuid });
                }
                return xy;
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
                int xy = 0;
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select count(1) from t_users where t_ReommUser in (select Guid from t_users where t_ReommUser=@0 and t_DelState=0) and t_DelState=0";
                    xy = Convert.IsDBNull(db.ExecuteScalar<object>(sql, new object[] { UserGuid })) ? 0 : db.ExecuteScalar<int>(sql, new object[] { UserGuid });
                }
                return xy;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="LoginPwd"></param>
        /// <returns></returns>
        public UserModel GetLoginUser(string LoginName, string LoginPwd)
        {
            try
            {
                string sql = "select * from T_Users where t_User_LoginId=@0 and t_User_Pwd=@1 and t_DelState=0";
                return rp.Get(sql, new object[] { LoginName, LoginPwd });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="LoginPwd"></param>
        /// <returns></returns>
        public UserModel Login(string LoginName, string LoginPwd)
        {
            try
            {
                string sql = "select * from T_Users where t_User_LoginId=@0 and t_User_Pwd=@1 and t_DelState=0";
                return rp.Get(sql, new object[] { LoginName, LoginPwd });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
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
                string sql = "select * from T_Users where (t_User_LoginId=@0 or Guid=@1) and t_User_LoginId is not null and t_User_LoginId != '' and t_delstate=0";
                return rp.Get(sql, new object[] { LoginName, LoginName, LoginName });
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
                string sql = "select * from T_Users where t_User_Style=@0 and t_DelState=0 order by t_User_Date desc";
                return rp.GetPageData(page, pagesize, sql, new object[] { UserStyle });
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
                string sql = "select * from T_Users where t_DelState=0 order by t_User_Date desc";
                return rp.GetPageData(page, pagesize, sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserModel> GetAll(int page, int pagesize, string phone)
        {
            try
            {
                string sql = "select * from T_Users where t_DelState=0 and t_user_loginid=@0 order by t_User_Date desc";
                return rp.GetPageData(page, pagesize, sql, new object[] { phone });
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
                string sql = "select * from T_Users where guid=@0";
                return rp.Get(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(UserModel model)
        {
            try
            {
                return rp.Insert(model) == null ? false : true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(UserModel model)
        {
            try
            {
                return (int)rp.Update(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Del(UserModel model)
        {
            try
            {
                return (int)rp.Delete(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
    }
}
