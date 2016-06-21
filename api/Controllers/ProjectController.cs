using qch.core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace api.Controllers
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        StyleService styleService = new StyleService();
        ProjectService service = new ProjectService();
        private readonly Maticsoft.BLL.T_Project bll = new Maticsoft.BLL.T_Project();
        private DataTable dtStyle = new Maticsoft.BLL.T_Style().GetList("").Tables[0];
        private DataTable dtPraise = new Maticsoft.BLL.T_Praise().GetListView(0, "t_DelState=0", "t_Date desc").Tables[0];
        private DataTable dtPic = new Maticsoft.BLL.T_Associate_Pic().GetList(0, " t_DelState=0", "t_Date desc").Tables[0];


        /// <summary>
        /// 获取项目团队
        /// </summary>
        /// <param name="ProjectGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetTeam(string ProjectGuid)
        {
            try
            {
                return service.GetTeam(ProjectGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        [HttpGet]
        public object GetProjectInfo(string guid, string userGuid)
        {
            try
            {
                string strWhere = string.Format(" Guid='{0}' and t_DelState=0", guid);
                DataSet ds = bll.GetListView(0, strWhere, "t_AddDate desc");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return GetProjectJson(ds, userGuid);
                }
                else
                {
                    return "[{\"state\":\"false\",\"result\":\"暂无数据！\"}]";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取json
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private string GetProjectJson(DataSet ds, string userGuid)
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder strJson = new StringBuilder();
                strJson.Append("[{\"state\":\"true\",\"result\":[");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    #region 是否点赞
                    string strPraise = string.Format("t_Associate_Guid='{0}' and t_User_Guid='{1}'", dr["Guid"].ToString(), userGuid);
                    DataRow[] drPraise = dtPraise.Select(strPraise);
                    //默认未赞
                    string ifPraise = "0";
                    if (drPraise != null && drPraise.Length > 0)
                    {
                        ifPraise = "1";
                    }
                    #endregion
                    #region 获取点赞人员信息
                    string strUserPraise = string.Format("t_Associate_Guid='{0}'", dr["Guid"].ToString());
                    DataRow[] drUserPraise = dtPraise.Select(strUserPraise);
                    //点赞人数
                    int PraiseCount = 0;
                    //点赞人员信息json
                    string PraiseUsers = "";
                    if (drUserPraise != null && drUserPraise.Length > 0)
                    {
                        StringBuilder strPraiseJson = new StringBuilder();
                        PraiseCount = drUserPraise.Length;
                        foreach (DataRow drtmp in drUserPraise)
                        {
                            strPraiseJson.Append("{\"PraiseUserGuid\":\"" + drtmp["t_User_Guid"] + "\",\"PraiseUserLoginId\":\"" + drtmp["t_User_LoginId"] + "\",\"PraiseUserRealName\":\"" + drtmp["t_User_RealName"] + "\",\"PraiseUserPic\":\"" + drtmp["t_User_Pic"] + "\"},");
                        }
                        PraiseUsers = strPraiseJson.ToString().Substring(0, strPraiseJson.Length - 1);
                    }
                    #endregion
                    #region  合伙人需求
                    string strParterWant = "";
                    if (!string.IsNullOrEmpty(dr["t_Project_ParterWant"].ToString()))
                    {
                        string[] arrParterWant = dr["t_Project_ParterWant"].ToString().Split(';');
                        if (arrParterWant != null && arrParterWant.Length > 0)
                        {
                            StringBuilder strParterWantJson = new StringBuilder();
                            foreach (var tmp in arrParterWant)
                            {
                                strParterWantJson.Append("{\"ParterWant\":\"" + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString() + "\"},");
                            }
                            strParterWant = strParterWantJson.ToString().Substring(0, strParterWantJson.Length - 1);
                        }
                    }
                    #endregion
                    #region  获取关联图片
                    string pic = "";
                    DataRow[] drPic = dtPic.Select("t_Associate_Guid='" + dr["Guid"] + "'");
                    if (drPic != null && drPic.Length > 0)
                    {
                        StringBuilder strPicJson = new StringBuilder();
                        foreach (DataRow tmpPic in drPic)
                        {
                            strPicJson.Append("{\"t_Pic_Url\":\"" + tmpPic["t_Pic_Url"] + "\",\"t_Pic_Remark\":\"" + tmpPic["t_Pic_Remark"] + "\"},");
                        }
                        pic = strPicJson.ToString().Substring(0, strPicJson.Length - 1);
                    }
                    #endregion
                    //#region  获取团队成员
                    //string team = "";
                    //DataRow[] drTeam = dtTeam.Select("t_Project_Guid='" + dr["Guid"] + "'");
                    //if (drTeam != null && drTeam.Length > 0)
                    //{
                    //    StringBuilder strTeamJson = new StringBuilder();
                    //    foreach (DataRow tmpTeam in drTeam)
                    //    {
                    //        if (dr["t_User_Guid"].ToString() == tmpTeam["t_User_Guid"])
                    //        {
                    //            strTeamJson.Append("{\"t_User_LoginId\":\"" + tmpTeam["t_User_LoginId"] + "\",\"t_User_RealName\":\"" + tmpTeam["t_User_RealName"] + "\",\"t_User_Pic\":\"" + tmpTeam["t_User_Pic"] + "\",\"t_User_Position\":\"" + tmpTeam["t_User_Position"] + "\",\"t_User_Remark\":\"" + tmpTeam["t_User_Remark"] + "\"},");
                    //        }
                    //        else if (dr["t_Audit"].ToString() == "1")
                    //        { 

                    //        }
                    //    }
                    //    team = strTeamJson.ToString().Substring(0, strTeamJson.Length - 1);
                    //}
                    //#endregion
                    strJson.Append("{\"Guid\":\"" + dr["Guid"] + "\",\"t_User_Guid\":\"" + dr["t_User_Guid"] + "\",\"t_Project_Name\":\"" + dr["t_Project_Name"] + "\",\"t_Project_OneWord\":\"" + dr["t_Project_OneWord"] + "\",\"t_Project_Instruction\":\"" + Maticsoft.Common.StringPlus.HtmlToTxt(dr["t_Project_Instruction"].ToString().Trim()) + "\",\"t_Project_CityName\":\"" + dr["t_Project_CityName"] + "\",\"t_Project_Field\":\"" + dr["t_Project_Field"] + "\",\"t_Project_Phase\":\"" + dr["t_Project_Phase"] + "\",\"t_Project_Finance\":\"" + dr["t_Project_Finance"] + "\",\"t_Project_FinanceUse\":\"" + dr["t_Project_FinanceUse"] + "\",\"t_Project_FinancePhase\":\"" + dr["t_Project_FinancePhase"] + "\",\"t_Project_ParterWant\":\"" + dr["t_Project_ParterWant"] + "\",\"t_Project_ConverPic\":\"" + dr["t_Project_ConverPic"] + "\",\"t_Project_Recommend\":\"" + dr["t_Project_Recommend"] + "\",\"t_Project_Audit\":\"" + dr["t_Project_Audit"] + "\",\"t_AddDate\":\"" + dr["t_AddDate"] + "\",\"t_User_LoginId\":\"" + dr["t_User_LoginId"] + "\",\"t_User_RealName\":\"" + dr["t_User_RealName"] + "\",\"t_User_Pic\":\"" + dr["t_User_Pic"] + "\",\"t_User_Commpany\":\"" + dr["t_User_Commpany"] + "\",\"t_User_Position\":\"" + dr["t_User_Position"] + "\",\"PositionName\":\"" + dr["PositionName"] + "\",\"t_User_Style\":\"" + dr["t_User_Style"] + "\",\"ifPraise\":\"" + ifPraise + "\",\"PraiseCount\":\"" + PraiseCount + "\",\"FiledName\":\"" + dr["FiledName"] + "\",\"PhaseName\":\"" + dr["PhaseName"] + "\",\"FinancePhaseName\":\"" + dr["FinancePhaseName"] + "\",\"t_Project_ProfitWay\":\"" + dr["t_Project_ProfitWay"] + "\",\"t_Project_Perfer\":\"" + dr["t_Project_Perfer"] + "\",\"t_Project_Client\":\"" + dr["t_Project_Client"] + "\",\"t_Project_Website\":\"" + dr["t_Project_Website"] + "\",\"t_Project_Link\":\"" + dr["t_Project_Link"] + "\",\"t_Project_Weixin\":\"" + dr["t_Project_Weixin"] + "\",\"t_Project_Audit\":\"" + dr["t_Project_Audit"] + "\",\"ParterWant\":[" + strParterWant + "],\"PraiseUsers\":[" + PraiseUsers + "],\"Pic\":[" + pic + "]},");
                }
                return strJson.ToString().Substring(0, strJson.Length - 1) + "]}]";
            }
            else
            {
                return "[{\"state\":\"false\",\"result\":\"暂无数据！\"}]";
            }
        }
        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetProject(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                var model = service.GetById(Guid);
                if (model == null)
                    return null;
                var target = new
                {
                    logo = model.t_Project_ConverPic,
                    projectName = model.t_Project_Name,
                    projectDescription = model.t_Project_OneWord,
                    city = model.t_Project_CityName,
                    projectField = styleService.GetById(model.t_Project_Field),
                    projectStage = styleService.GetById(model.t_Project_Phase),
                    financingState = styleService.GetById(model.t_Project_FinancePhase),
                    financingMoney = model.t_Project_Finance,
                    UseOfFunds = model.t_Project_FinanceUse,
                    aboutProject = model.t_Project_Instruction,
                    partnerDemand = model.t_Project_ParterWant
                };
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
