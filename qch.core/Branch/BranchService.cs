using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 部门相关业务
    /// </summary>
    public class BranchService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BranchRepository rp = new BranchRepository();
        UserRepository userRp = new UserRepository();
        UserBankRepository ubRp = new UserBankRepository();

        #region 部门信息资源
        /// <summary>
        /// 获取所有部门信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BranchModel> GetAllBranch()
        {
            try
            {
                return rp.GetAllBranch();
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
                return rp.GetAllBranch(page, pagesize);
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
                return rp.GetBranchById(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存部门信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveBranch(BranchModel model)
        {
            try
            {
                if (model == null)
                    return false;
                var tt = GetBranchById(model.Guid);
                if (tt != null)
                    return rp.EditBranch(model);
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    return rp.AddBranch(model);
                }
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
        public bool DelBranch(string Guid)
        {
            try
            {
                var model = GetBranchById(Guid);
                if (model == null)
                    return false;
                return rp.DelBranch(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        #endregion
        #region 部门员工信息资源
        public bool CheckLoginUser(string LoginUserGuid, string UserGuid, string BranchGuid)
        {
            try
            {
                string branch = BranchGuid;
                if(!string.IsNullOrWhiteSpace(UserGuid))
                {
                    var m = GetByUserGuid(UserGuid);
                    if(m!=null)
                        branch = m.t_Branch_Guid;
                }
                var b = GetByUserGuid(LoginUserGuid);
                if (b != null)
                {
                    if (b.t_Branch_Guid == branch)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        public BranchUserModel GetByUserGuid(string UserGuid)
        {
            try
            {
                return rp.GetByUserGuid(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 查询某个部门的业绩
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="BranchGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserMsg> GetByBranch(int page, int pagesize, string BranchGuid, DateTime? b, DateTime? e)
        {
            try
            {
                DateTime begin = b == null ? Convert.ToDateTime("1990-01-01") : qch.Infrastructure.TimeHelper.GetStartDateTime(Convert.ToDateTime(b));
                DateTime end = e == null ? DateTime.Now : qch.Infrastructure.TimeHelper.GetEndDateTime(Convert.ToDateTime(e));
                var model = rp.GetByBranch(page, pagesize, BranchGuid);
                if (model != null && model.Items != null && model.Items.Count > 0)
                {
                    foreach (var item in model.Items)
                    {
                        item.Tuijian1 = userRp.GetReferral1(item.UserGuid, begin, end);
                        item.Tuijian2 = userRp.GetReferral2(item.UserGuid, begin, end);
                        item.Certification1 = ubRp.GetCertification1(item.UserGuid, begin, end);
                        item.Certification2 = ubRp.GetCertification2(item.UserGuid, begin, end);
                    }
                    //model.Items.Select(o => o.Tuijian1 = userRp.GetReferral1(o.UserGuid, begin, end));
                    //model.Items.Select(o => o.Tuijian2 = userRp.GetReferral2(o.UserGuid, begin, end));
                    //model.Items.Select(o => o.Certification1 = ubRp.GetCertification1(o.UserGuid, begin, end));
                    //model.Items.Select(o => o.Certification2 = ubRp.GetCertification2(o.UserGuid, begin, end));
                }
                return model;
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
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public PetaPoco.Page<UserMsg> GetByUser(int page, int pagesize, string UserGuid, DateTime? b, DateTime? e)
        {
            try
            {
                DateTime begin = b == null ? Convert.ToDateTime("1990-01-01") : qch.Infrastructure.TimeHelper.GetStartDateTime(Convert.ToDateTime(b));
                DateTime end = e == null ? DateTime.Now : qch.Infrastructure.TimeHelper.GetEndDateTime(Convert.ToDateTime(e));
                var model = rp.GetByUser(page, pagesize, UserGuid);
                if (model != null && model.Items != null && model.Items.Count > 0)
                {
                    foreach (var item in model.Items)
                    {
                        item.Tuijian1 = userRp.GetReferral1(item.UserGuid, begin, end);
                        item.Tuijian2 = userRp.GetReferral2(item.UserGuid, begin, end);
                        item.Certification1 = ubRp.GetCertification1(item.UserGuid, begin, end);
                        item.Certification2 = ubRp.GetCertification2(item.UserGuid, begin, end);
                    }
                    //model.Items.Select(o => o.Tuijian1 = userRp.GetReferral1(o.UserGuid, begin, end));
                    //model.Items.Select(o => o.Tuijian2 = userRp.GetReferral2(o.UserGuid, begin, end));
                    //model.Items.Select(o => o.Certification1 = ubRp.GetCertification1(o.UserGuid, begin, end));
                    //model.Items.Select(o => o.Certification2 = ubRp.GetCertification2(o.UserGuid, begin, end));
                }
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某个部门的所有员工信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="BranchGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<SelectBranchUser> GetAll(int page, int pagesize, string BranchGuid)
        {
            try
            {
                return rp.GetAll(page, pagesize, BranchGuid);
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
        public PetaPoco.Page<BranchUserModel> GetAll(int page, int pagesize)
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
        public BranchUserModel GetById(string Guid)
        {
            try
            {
                return rp.GetById(Guid);
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
                return rp.GetByUserGuid(UserGuid, BranchGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存员工信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Save(BranchUserModel model)
        {
            try
            {
                if (model == null)
                    return false;
                var tt = GetById(model.Guid);
                if (tt != null)
                    return rp.Edit(model);
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    return rp.Add(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除员工信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Del(string Guid)
        {
            try
            {
                var model = GetById(Guid);
                if (model == null)
                    return false;
                return rp.Del(model);
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
