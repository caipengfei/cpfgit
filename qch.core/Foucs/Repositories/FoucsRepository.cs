using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 关注关系资源层
    /// </summary>
    public class FoucsRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<FoucsModel> rp = new Repository<FoucsModel>();
        Repository<FoucsUser> rp1 = new Repository<FoucsUser>();


        /// <summary>
        /// 获取我关注的所有人
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<FoucsUser> GetFoucsFroMe(string Guid)
        {
            try
            {
                string sql = "select a.Guid,a.t_Focus_Guid,a.t_User_Guid as UserGuid,a.t_Date as FoucsDate,b.t_User_RealName as UserName,b.t_User_Pic as UserAvator from T_User_Foucs as a left join t_users as b on a.t_User_Guid=b.Guid where a.t_User_Guid=@0";
                return rp1.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取某人的所有粉丝
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<FoucsUser> GetFoucs(string Guid)
        {
            try
            {
                string sql = "select a.Guid,a.t_Focus_Guid,a.t_User_Guid as UserGuid,a.t_Date as FoucsDate,b.t_User_RealName as UserName,b.t_User_Pic as UserAvator from T_User_Foucs as a left join t_users as b on a.t_User_Guid=b.Guid where a.t_Focus_Guid=@0";
                return rp1.GetAll(sql, new object[] { Guid });
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
        public PetaPoco.Page<FoucsModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_User_Foucs where t_DelState=0 order by t_News_Date desc";
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
        public FoucsModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_User_Foucs where guid=@0";
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
        public bool Add(FoucsModel model)
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
        public bool Edit(FoucsModel model)
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
        public bool Del(FoucsModel model)
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
