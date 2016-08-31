using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 部门以及部门员工信息资源层
    /// </summary>
    public class BranchRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<BranchModel> rp1 = new Repository<BranchModel>();
        Repository<BranchUserModel> rp = new Repository<BranchUserModel>();
        Repository<SelectBranchUser> rp2 = new Repository<SelectBranchUser>();
        Repository<UserMsg> rp3 = new Repository<UserMsg>();

        #region 部门信息资源
        /// <summary>
        /// 获取所有部门信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BranchModel> GetAllBranch()
        {
            try
            {
                string sql = "select * from t_branch where t_delstate=0 order by t_createdate desc";
                return rp1.GetAll(sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<BranchModel> GetAllBranch(int page, int pagesize)
        {
            try
            {
                string sql = "select * from t_branch where t_delstate=0 order by t_createdate desc";
                return rp1.GetPageData(page, pagesize, sql);
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
        public BranchModel GetBranchById(string Guid)
        {
            try
            {
                string sql = "select * from T_Branch where guid=@0";
                return rp1.Get(sql, new object[] { Guid });
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
        public bool AddBranch(BranchModel model)
        {
            try
            {
                return rp1.Insert(model) == null ? false : true;
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
        public bool EditBranch(BranchModel model)
        {
            try
            {
                return (int)rp1.Update(model) > 0 ? true : false;
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
        public bool DelBranch(BranchModel model)
        {
            try
            {
                return (int)rp1.Delete(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        #endregion
        #region 部门员工信息资源
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="BranchGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserMsg> GetByBranch(int page, int pagesize, string BranchGuid)
        {
            try
            {
                string sql = "select a.t_User_Guid as UserGuid,a.t_User_RealName as UserName,a.t_User_Phone as UserPhone,b.t_Branch_Name as BranchName from t_branch_User as a left join t_branch as b on a.t_branch_guid=b.guid where a.t_branch_guid=@0 and a.t_delstate=0 order by a.t_createdate desc";
                return rp3.GetPageData(page, pagesize, sql, new object[] { BranchGuid });
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
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserMsg> GetByUser(int page, int pagesize, string UserGuid)
        {
            try
            {
                string sql = "select a.t_User_Guid as UserGuid,a.t_User_RealName as UserName,a.t_User_Phone as UserPhone,b.t_Branch_Name as BranchName from t_branch_User as a left join t_branch as b on a.t_branch_guid=b.guid where a.t_user_guid=@0 and a.t_delstate=0 order by a.t_createdate desc";
                return rp3.GetPageData(page, pagesize, sql, new object[] { UserGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某部门所有员工信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="BranchGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<SelectBranchUser> GetAll(int page, int pagesize, string BranchGuid)
        {
            try
            {
                string sql = "select a.Guid,a.t_User_RealName as UserName,a.t_User_Phone as UserPhone,a.t_Editor as Editor,a.t_CreateDate as CreateDate,b.t_Branch_Name as BranchName from t_branch_User as a left join t_branch as b on a.t_branch_guid=b.guid where a.t_branch_guid=@0 and a.t_delstate=0 order by a.t_createdate desc";
                return rp2.GetPageData(page, pagesize, sql, new object[] { BranchGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某部门所有员工信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="BranchGuid"></param>
        /// <returns></returns>
        //public PetaPoco.Page<BranchUserModel> GetAll(int page, int pagesize, string BranchGuid)
        //{
        //    try
        //    {
        //        string sql = "select * from t_branch_User where t_branch_guid=@0 and t_delstate=0 order by t_createdate desc";
        //        return rp.GetPageData(page, pagesize, sql, new object[] { BranchGuid });
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        return null;
        //    }
        //}
        /// <summary>
        /// 分页获取所有
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<BranchUserModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from t_branch_User where t_delstate=0 order by t_createdate desc";
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
        public BranchUserModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_Branch_User where guid=@0";
                return rp.Get(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public BranchUserModel GetByUserGuid(string UserGuid, string BranchGuid)
        {
            try
            {
                string sql = "select * from T_Branch_User where t_user_guid=@0 and t_branch_guid=@1";
                return rp.Get(sql, new object[] { UserGuid, BranchGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public BranchUserModel GetByUserGuid(string UserGuid)
        {
            try
            {
                string sql = "select top 1 * from T_Branch_User where t_user_guid=@0 order by t_CreateDate desc";
                return rp.Get(sql, new object[] { UserGuid });
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
        public bool Add(BranchUserModel model)
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
        public bool Edit(BranchUserModel model)
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
        public bool Del(BranchUserModel model)
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
        #endregion
    }
}
