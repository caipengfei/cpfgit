﻿using qch.Models;
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
                string sql = "select * from T_Users where (t_User_LoginId=@0 or Guid=@1) and t_User_LoginId is not null and t_User_LoginId != ''";
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
