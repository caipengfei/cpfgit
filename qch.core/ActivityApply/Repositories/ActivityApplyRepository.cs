using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 活动报名资源层
    /// </summary>
    public class ActivityApplyRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<ActivityApplyModel> rp = new Repository<ActivityApplyModel>();
        Repository<ProofModel> rp1 = new Repository<ProofModel>();
        Repository<ApplyModel> rp2 = new Repository<ApplyModel>();

        /// <summary>
        /// 获取某个对象的所有报名信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IEnumerable<ApplyModel> GetByActivityGuid(string Guid)
        {
            try
            {
                string sql = "select a.Guid,a.t_User_Guid as UserGuid,a.t_Activity_Guid as ActivityGuid,b.t_User_Pic as UserAvator from T_Activity_Apply as a left join t_users as b on a.t_User_Guid=b.Guid where a.t_Activity_Guid=@0 and a.t_DelState=0";
                return rp2.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取报名成功后报名信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public PetaPoco.Page<ProofModel> GetProofList(int page, int pagesize, string UserGuid)
        {
            try
            {
                string sql = "select a.Guid as 'ApplyGuid',a.t_Activity_Guid,a.t_User_Guid,a.t_QrCode as 'QrCode',a.t_ProofCode as 'ProofCode',b.t_Activity_Title as 'ActivityName',b.t_Activity_Fee as 'Money',b.t_Activity_sDate as 'ActivityDate',b.t_Activity_Street as 'Addr',b.t_Activity_Holder as 'Holder',c.t_User_RealName as 'ApplyUserName',c.t_User_Mobile as 'Phone' from T_Activity_Apply as a left join T_Activity as b on a.t_Activity_Guid=b.guid left join T_Users as c on a.t_User_Guid=c.guid where a.t_User_Guid=@0 and a.t_DelState=0";
                return rp1.GetPageData(page, pagesize, sql, new object[] { UserGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 报名成功后获取报名信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IEnumerable<ProofModel> GetProofList(string UserGuid)
        {
            try
            {
                string sql = "select a.Guid as 'ApplyGuid',a.t_Activity_Guid,a.t_User_Guid,a.t_QrCode as 'QrCode',a.t_ProofCode as 'ProofCode',b.t_Activity_Title as 'ActivityName',b.t_Activity_Fee as 'Money',b.t_Activity_sDate as 'ActivityDate',b.t_Activity_Street as 'Addr',b.t_Activity_Holder as 'Holder',c.t_User_RealName as 'ApplyUserName',c.t_User_Mobile as 'Phone' from T_Activity_Apply as a left join T_Activity as b on a.t_Activity_Guid=b.guid left join T_Users as c on a.t_User_Guid=c.guid where a.t_User_Guid=@0 and a.t_DelState=0";
                return rp1.GetAll(sql, new object[] { UserGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 报名成功后获取报名信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ProofModel GetProof(string ActivityGuid, string UserGuid)
        {
            try
            {
                string sql = "select a.Guid as 'ApplyGuid',a.t_Activity_Guid,a.t_User_Guid,a.t_QrCode as 'QrCode',a.t_ProofCode as 'ProofCode',b.t_Activity_Title as 'ActivityName',b.t_Activity_Fee as 'Money',b.t_Activity_sDate as 'ActivityDate',b.t_Activity_Street as 'Addr',b.t_Activity_Holder as 'Holder',c.t_User_RealName as 'ApplyUserName',c.t_User_Mobile as 'Phone' from T_Activity_Apply as a left join T_Activity as b on a.t_Activity_Guid=b.guid left join T_Users as c on a.t_User_Guid=c.guid where a.t_Activity_Guid=@0 and a.t_User_Guid=@1 and a.t_DelState=0";
                return rp1.Get(sql, new object[] { ActivityGuid, UserGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 报名成功后获取报名信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ProofModel GetProof(string guid)
        {
            try
            {
                string sql = "select a.Guid as 'ApplyGuid',a.t_Activity_Guid,a.t_User_Guid,a.t_QrCode as 'QrCode',a.t_ProofCode as 'ProofCode',b.t_Activity_Title as 'ActivityName',b.t_Activity_Fee as 'Money',b.t_Activity_sDate as 'ActivityDate',b.t_Activity_Street as 'Addr',b.t_Activity_Holder as 'Holder',c.t_User_RealName as 'ApplyUserName',c.t_User_Mobile as 'Phone' from T_Activity_Apply as a left join T_Activity as b on a.t_Activity_Guid=b.guid left join T_Users as c on a.t_User_Guid=c.guid where a.guid=@0 and a.t_DelState=0";
                return rp1.Get(sql, new object[] { guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有报名信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<ActivityApplyModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_Activity_Apply where t_DelState=0 order by t_Date desc";
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
        public ActivityApplyModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_Activity_Apply where guid=@0";
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
        public bool Add(ActivityApplyModel model)
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
        public bool Edit(ActivityApplyModel model)
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
        public bool Del(ActivityApplyModel model)
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
