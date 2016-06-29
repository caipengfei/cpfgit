using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 投资相关资源层
    /// </summary>
    public class InvestRepository
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Repository<InvestCaseModel> rp = new Repository<InvestCaseModel>();
        Repository<InvestPlaceModel> placeRp = new Repository<InvestPlaceModel>();
        Repository<InvestPlaceCase> placeCaseRp = new Repository<InvestPlaceCase>();
        Repository<InvestPlaceMember> placMemberRp = new Repository<InvestPlaceMember>();

        #region 投资案例资源
        /// <summary>
        /// 获取某人所有未删除的投资案例
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public IEnumerable<InvestCaseModel> GetByUser(string UserGuid)
        {
            try
            {
                string sql = "select * from T_Invest_Case where t_DelState=0 and t_User_Guid=@0";
                return rp.GetAll(sql, new object[] { UserGuid });
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
        public PetaPoco.Page<InvestCaseModel> GetAll(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_Invest_Case where t_DelState=0 order by t_Date desc";
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
        public InvestCaseModel GetById(string Guid)
        {
            try
            {
                string sql = "select * from T_Invest_Case where guid=@0";
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
        public bool Add(InvestCaseModel model)
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
        public bool Edit(InvestCaseModel model)
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
        public bool Del(InvestCaseModel model)
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

        #region 投资机构资源
        /// <summary>
        /// 分页获取所有
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PetaPoco.Page<InvestPlaceModel> GetAllPlace(int page, int pagesize)
        {
            try
            {
                string sql = "select * from T_Invest_Place where t_DelState=0";
                return placeRp.GetPageData(page, pagesize, sql);
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
        public InvestPlaceModel GetPlaceById(string Guid)
        {
            try
            {
                string sql = "select * from T_Invest_Place where guid=@0";
                return placeRp.Get(sql, new object[] { Guid });
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
        public bool AddPlace(InvestPlaceModel model)
        {
            try
            {
                return placeRp.Insert(model) == null ? false : true;
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
        public bool EditPlace(InvestPlaceModel model)
        {
            try
            {
                return (int)placeRp.Update(model) > 0 ? true : false;
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
        public bool DelPlace(InvestPlaceModel model)
        {
            try
            {
                return (int)placeRp.Delete(model) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 投资机构入驻成员
        /// <summary>
        /// 投资机构入驻成员
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<InvestPlaceMember> GetPlaceMember(string Guid)
        {
            try
            {
                string sql = "select a.guid as Guid,b.t_User_Pic as UserPic,b.t_User_RealName as UserName,b.Guid as UserGuid,c.t_Style_Name as UserPosition from T_InvestPlace_Member as a left join T_Users as b on a.t_User_Guid=b.guid left join T_Style as c on b.t_User_Position=c.id where a.[t_InvestPlace_Guid]=@0";
                return placMemberRp.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion

        #region 投资机构接收项目
        /// <summary>
        /// 投资机构接收的项目
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<InvestPlaceCase> GetPlaceCase(string Guid)
        {
            try
            {
                string sql = "select a.guid as Guid,b.t_Project_ConverPic as ProjectPic,b.t_Project_Name as ProjectName,b.Guid as ProjectGuid from T_InvestPlace_Case as a left join T_Project as b on a.t_Project_Guid=b.guid where a.[t_InvestPlace_Guid]=@0";
                return placeCaseRp.GetAll(sql, new object[] { Guid });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
