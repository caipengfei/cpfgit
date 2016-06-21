using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 项目资源层
    /// </summary>
    public class ProjectRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<ProjectModel> rp = new Repository<ProjectModel>();
        Repository<SelectProjectTeam> rp1 = new Repository<SelectProjectTeam>();

        /// <summary>
        /// 获取项目团队
        /// </summary>
        /// <param name="ProjectGuid"></param>
        /// <returns></returns>
        public IEnumerable<SelectProjectTeam> GetTeam(string ProjectGuid)
        {
            try
            {
                string sql = "select a.guid,a.t_Project_Guid,a.t_User_Guid,b.t_User_RealName as UserName,b.t_User_Pic as UserAvator,b.t_User_Remark as UserRemark,c.t_Style_Name as UserPosition from T_Project_Team as a left join t_users as b on a.t_User_Guid=b.guid left join T_Style as c on b.t_User_Position=c.Id where a.t_Project_Guid=@0";
                return rp1.GetAll(sql, new Object[] { ProjectGuid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有项目
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<ProjectModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from t_project where t_DelState=0 order by t_AddDate desc";
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
        public ProjectModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_Project where guid=@0";
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
        public bool Add(ProjectModel model)
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
        public bool Edit(ProjectModel model)
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
        public bool Del(ProjectModel model)
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
