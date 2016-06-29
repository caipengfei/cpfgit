using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 课程资源层
    /// </summary>
    public class CourseRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<CourseModel> rp = new Repository<CourseModel>();
        Repository<SelectCourse> rp1 = new Repository<SelectCourse>();
        /// <summary>
        /// 分页获取所有
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<CourseModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select a.*,b.T_Lecturer_Name as LecturerName,b.T_Lecturer_Pic as LecturerAvator from T_Course as a left join T_Lecturer as b on a.t_Lecturer_Guid=b.Guid where a.t_DelState=0 order by a.t_Course_Recommand,a.t_Add_Date desc";
                return rp.GetPageData(page, pagesize, sql);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 分页获取所有推荐
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<SelectCourse> GetRecommendCourse(int page, int pagesize)
        {
            try
            {
                string sql = "select a.*,b.T_Lecturer_Name as LecturerName,b.T_Lecturer_Pic as LecturerAvator from T_Course as a left join T_Lecturer as b on a.t_Lecturer_Guid=b.Guid where a.t_DelState=0 and a.t_Course_Recommand=1 order by a.t_Course_Recommand,a.t_Add_Date desc";
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
        public SelectCourse GetById(string Guid)
        {
            try
            {
                string sql = "select a.*,b.T_Lecturer_Name as LecturerName,b.T_Lecturer_Pic as LecturerAvator,b.T_Lecturer_Intor as LecturerRemark from T_Course as a left join T_Lecturer as b on a.t_Lecturer_Guid=b.Guid where a.guid=@0";
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
        public bool Add(CourseModel model)
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
        public bool Edit(CourseModel model)
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
        public bool Del(CourseModel model)
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
