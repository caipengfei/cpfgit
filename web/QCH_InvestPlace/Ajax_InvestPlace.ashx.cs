using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Maticsoft.Common;

namespace Maticsoft.Web.QCH_InvestPlace
{
    /// <summary>
    /// Ajax_InvestPlace 的摘要说明
    /// </summary>
    public class Ajax_InvestPlace : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        private BLL.T_Invest_Place bll = new BLL.T_Invest_Place();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            string view = context.Request["Action"];
            switch (view)
            {
                case "List":
                    GetData(context);
                    break;
                case "Delete":
                    DeleteData(context);
                    break;
                case "AddOrModify":
                    AddOrModify(context);
                    break;
                case "Up"://置顶
                    Up(context);
                    break;
                case "getModel"://获取数据模型
                    GetModel(context);
                    break;
                case "DelInvestPlaceCase"://删除孵化项目
                    DelInvestPlaceCase(context);
                    break;
                case "DelInvestPlaceMember"://删除入驻成员
                    DelInvestPlaceMember(context);
                    break;
                case "IsAdd"://
                    IsAdd(context);
                    break;
                case "AddCase"://添加孵化案例
                    AddCase(context);
                    break;
                case "AddMember"://添加入驻成员
                    AddMember(context);
                    break;
                case "GetInvestPlaceView":
                    GetInvestPlaceView(context);
                    break;
            }
            context.Response.End();
        }
        //获取类型
        private DataTable dtStyle = new BLL.T_Style().GetList(0, "", "t_AddDate desc").Tables[0];
        Maticsoft.BLL.T_Invest_Place investService = new BLL.T_Invest_Place();
        /// <summary>
        /// 获取投资空间明细
        /// </summary>
        /// <returns></returns>
        public void GetInvestPlaceView(HttpContext context)
        {
            string guid = context.Request["Guid"];
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat("t_DelState=0 and Guid='{0}'", guid);
            DataSet ds = investService.GetList(0, strWhere.ToString(), "t_AddDate desc ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                context.Response.Write(GetJson(ds));
                context.Response.Flush();
            }
        }
        /// <summary>
        /// json
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private string GetJson(DataSet ds)
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder strJson = new StringBuilder();
             
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    #region  投资领域
                    string strInvestArea = "";

                    if (!string.IsNullOrEmpty(dr["t_InvestPlace_Area"].ToString()))
                    {
                        string[] arrInvestArea = dr["t_InvestPlace_Area"].ToString().Split(';');
                        if (arrInvestArea != null && arrInvestArea.Length > 0)
                        {
                            StringBuilder strInvestAreaJson = new StringBuilder();
                            foreach (var tmp in arrInvestArea)
                            {
                                strInvestAreaJson.Append("{\"InvestAreaID\":\"" + tmp + "\",\"InvestAreaName\":\"" + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString() + "\"},");
                            }
                            strInvestArea = strInvestAreaJson.ToString().Substring(0, strInvestAreaJson.Length - 1);
                        }
                    }
                    #endregion
                    #region  投资阶段
                    string strInvestPhase = "";

                    if (!string.IsNullOrEmpty(dr["t_InvestPlace_Phase"].ToString()))
                    {
                        string[] arrInvestPhase = dr["t_InvestPlace_Phase"].ToString().Split(';');
                        if (arrInvestPhase != null && arrInvestPhase.Length > 0)
                        {
                            StringBuilder strInvestPhaseJson = new StringBuilder();
                            foreach (var tmp in arrInvestPhase)
                            {
                                strInvestPhaseJson.Append("{\"InvestPhaseID\":\"" + tmp + "\",\"InvestPhaseName\":\"" + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString() + "\"},");
                            }
                            strInvestPhase = strInvestPhaseJson.ToString().Substring(0, strInvestPhaseJson.Length - 1);
                        }
                    }
                    #endregion
                    #region  孵化案例
                    string strCase = "";
                    int CaseCount = 0;
                    DataSet dsCase = new BLL.T_InvestPlace_Case().GetListView(0, "t_InvestPlace_Guid='" + dr["Guid"] + "'", "");
                    if (dsCase != null && dsCase.Tables[0].Rows.Count > 0)
                    {
                        CaseCount = dsCase.Tables[0].Rows.Count;
                        StringBuilder strCaseJson = new StringBuilder();
                        foreach (DataRow drcase in dsCase.Tables[0].Rows)
                        {
                            strCaseJson.Append("{\"CaseGuid\":\"" + drcase["t_Project_Guid"] + "\",\"CaseName\":\"" + drcase["t_Project_Name"].ToString() + "\",\"CasePic\":\"" + drcase["t_Project_ConverPic"].ToString() + "\"},");
                        }
                        strCase = strCaseJson.ToString().Substring(0, strCaseJson.Length - 1);
                    }
                    #endregion
                    #region  入驻成员
                    string strMember = "";
                    int MemberCount = 0;
                    DataSet dsMember = new BLL.T_InvestPlace_Member().GetListView(0, "t_InvestPlace_Guid='" + dr["Guid"] + "'", "");
                    if (dsMember != null && dsMember.Tables[0].Rows.Count > 0)
                    {
                        MemberCount = dsMember.Tables[0].Rows.Count;
                        StringBuilder strMemberJson = new StringBuilder();
                        foreach (DataRow drmember in dsMember.Tables[0].Rows)
                        {
                            strMemberJson.Append("{\"UserGuid\":\"" + drmember["t_User_Guid"] + "\",\"UserName\":\"" + drmember["t_User_RealName"].ToString() + "\",\"OneWord\":\"" + drmember["t_User_Remark"].ToString() + "\",\"UserPic\":\"" + drmember["t_User_Pic"].ToString() + "\",\"UserStyle\":\"" + drmember["t_User_Style"].ToString() + "\"},");
                        }
                        strMember = strMemberJson.ToString().Substring(0, strMemberJson.Length - 1);
                    }
                    #endregion
                    strJson.Append("{\"Guid\":\"" + dr["Guid"] + "\",\"t_InvestPlace_Title\":\"" + dr["t_InvestPlace_Title"] + "\",\"t_InvestPlace_ConverPic\":\"" + dr["t_InvestPlace_ConverPic"] + "\",\"t_InvestPlace_Phase\":\"" + dr["t_InvestPlace_Phase"] + "\",\"t_InvestPlace_Area\":\"" + dr["t_InvestPlace_Area"] + "\",\"t_InvestPlace_Money\":\"" + dr["t_InvestPlace_Money"] + "\",\"t_InvestPlace_Instruction\":\"" + dr["t_InvestPlace_Instruction"] + "\",\"t_InvestPlace_Case\":\"" + dr["t_InvestPlace_Case"] + "\",\"t_InvestPlace_Member\":\"" + dr["t_InvestPlace_Member"] + "\",\"t_InvestPlace_Recommend\":\"" + dr["t_InvestPlace_Recommend"] + "\",\"t_AddDate\":\"" + dr["t_AddDate"] + "\",\"CaseCount\":\"" + CaseCount + "\",\"MemberCount\":\"" + MemberCount + "\",\"InvestArea\":[" + strInvestArea + "],\"InvestPhase\":[" + strInvestPhase + "],\"Cases\":[" + strCase + "],\"Members\":[" + strMember + "]},");
                }
                return strJson.ToString().Substring(0, strJson.Length - 1);
            }
            else
            {
                return "[{\"state\":\"false\",\"result\":\"暂无数据！\"}]";
            }
        }
        /// <summary>
        /// AddMember
        /// </summary>
        /// <param name="context"></param>
        private void AddMember(HttpContext context)
        {
            string tmpguids = context.Request["GUID"];
            string investplaceguid = context.Request["InvestPlaceGuid"];
            string[] guidList = tmpguids.Split(',');
            List<string> sqlList = new List<string>();
            DataTable dttmp = new BLL.T_InvestPlace_Member().GetList("").Tables[0];
            foreach (string guid in guidList)
            {
                DataRow[] dr = dttmp.Select("t_InvestPlace_Guid='" + investplaceguid + "' and t_User_Guid='" + guid + "'");
                if (dr != null && dr.Length > 0)
                { }
                else
                {
                    string strMember = string.Format(" insert into T_InvestPlace_Member(Guid,t_InvestPlace_Guid,t_User_Guid) values('{0}','{1}','{2}')", Guid.NewGuid().ToString(), investplaceguid, guid);
                    sqlList.Add(strMember);
                }
            }
            if (new BLL.Tran().SqlTran(sqlList))
            {
                context.Response.Write("ok");
                context.Response.Flush();
            }
            else
            {
                context.Response.Write("fail");
                context.Response.Flush();
            }
        }
        /// <summary>
        /// AddCase
        /// </summary>
        /// <param name="context"></param>
        private void AddCase(HttpContext context)
        {
            string tmpguids = context.Request["GUID"];
            string investplaceguid = context.Request["InvestPlaceGuid"];
            string[] guidList = tmpguids.Split(',');
            List<string> sqlList = new List<string>();
            DataTable dttmp = new BLL.T_InvestPlace_Case().GetList("").Tables[0];
            foreach (string guid in guidList)
            {
                DataRow[] dr = dttmp.Select("t_InvestPlace_Guid='" + investplaceguid + "' and t_Project_Guid='" + guid + "'");
                if (dr != null && dr.Length > 0)
                { }
                else
                {
                    string strCase = string.Format(" insert into T_InvestPlace_Case(Guid,t_InvestPlace_Guid,t_Project_Guid) values('{0}','{1}','{2}')", Guid.NewGuid().ToString(), investplaceguid, guid);
                    sqlList.Add(strCase);
                }
            }
            if (new BLL.Tran().SqlTran(sqlList))
            {
                context.Response.Write("ok");
                context.Response.Flush();
            }
            else
            {
                context.Response.Write("fail");
                context.Response.Flush();
            }
        }
        /// <summary>
        /// 判断是否已添加
        /// </summary>
        /// <param name="context"></param>
        private void IsAdd(HttpContext context)
        {
            string investplaceguid = context.Request["InvestPlaceGuid"];
            string projectguid = context.Request["ProjectGuid"];
            string strWhere = string.Format("t_InvestPlace_Guid='{0}' and t_Project_Guid='{1}'", investplaceguid, projectguid);
            DataSet ds = new BLL.T_InvestPlace_Case().GetList(strWhere);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                context.Response.Write("ok");
                context.Response.Flush();
            }
            else
            {
                context.Response.Write("fail");
                context.Response.Flush();
            }
        }
        /// <summary>
        /// 删除孵化案例
        /// </summary>
        /// <param name="context"></param>
        private void DelInvestPlaceCase(HttpContext context)
        {
            string investplaceguid = context.Request["InvestPlaceGuid"];
            string projectguid = context.Request["ProjectGuid"];
            bool ifSuc =new BLL.T_InvestPlace_Case().Del(investplaceguid, projectguid);
            if (ifSuc)
            {
                context.Response.Write("ok");
                context.Response.Flush();
            }
            else
            {
                context.Response.Write("fail");
                context.Response.Flush();
            }
        }
        /// <summary>
        /// 删除入驻成员
        /// </summary>
        /// <param name="context"></param>
        private void DelInvestPlaceMember(HttpContext context)
        {
            string investplaceguid = context.Request["InvestPlaceGuid"];
            string userguid = context.Request["UserGuid"];
            bool ifSuc = new BLL.T_InvestPlace_Member().Del(investplaceguid, userguid);
            if (ifSuc)
            {
                context.Response.Write("ok");
                context.Response.Flush();
            }
            else
            {
                context.Response.Write("fail");
                context.Response.Flush();
            }
        }
        /// <summary>
        /// 获取数据模型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public void GetModel(HttpContext context)
        {
            string guid = context.Request["Guid"];
            context.Response.Write(bll.GetModel(guid).ToJson());
            context.Response.Flush();
        }
        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="context"></param>
        private void Up(HttpContext context)
        {
            string tmpguids = context.Request["GUID"];
            string[] guidList = tmpguids.Split(',');
            string guids = "";
            foreach (string guid in guidList)
            {
                guids = guids + "'" + guid + "',";
            }
            guids = guids.Substring(0, guids.Length - 1);
            bool ifSuc = bll.Up(guids);
            if (ifSuc)
            {
                context.Response.Write("ok");
                context.Response.Flush();
            }
            else
            {
                context.Response.Write("fail");
                context.Response.Flush();
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="context"></param>
        private void GetData(HttpContext context)
        {
            StringBuilder strWhere = new StringBuilder();
            string name = context.Request["name"];
            strWhere.AppendFormat(" t_DelState =0 and t_InvestPlace_Title like '%{0}%'", name);
            DataSet _Ds = bll.GetList(0, strWhere.ToString(), "t_InvestPlace_Recommend desc");
            int _TotalCount = 0;
            if (_Ds != null && _Ds.Tables[0].Rows.Count > 0)
            {
                _TotalCount = _Ds.Tables[0].Rows.Count;
                DataTable _Dt = _Ds.Tables[0];
                string _JsonData = new DataTableJSONSerializer().Serialize(_Dt);
                string _Json = @"{""Rows"":" + _JsonData + @",""Total"":""" + _TotalCount + @"""}";
                context.Response.Write(_Json);
                context.Response.End();
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        private void DeleteData(HttpContext context)
        {
            string tmpguids = context.Request["GUID"];
            string[] guidList = tmpguids.Split(',');
            string guids = "";
            foreach (string guid in guidList)
            {
                guids = guids + "'" + guid + "',";
            }
            guids = guids.Substring(0, guids.Length - 1);
            bool ifSuc = bll.Del(guids);
            if (ifSuc)
            {
                context.Response.Write("ok");
                context.Response.Flush();
            }
            else
            {
                context.Response.Write("fail");
                context.Response.Flush();
            }
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        private void AddOrModify(HttpContext context)
        {
            Maticsoft.Model.T_Invest_Place model = new Maticsoft.Model.T_Invest_Place();
            string title = context.Request["Title"];
            string phase = context.Request["Phase"];
            string area = context.Request["Area"];
            string money = context.Request["Money"];
            string instruction = context.Request["Instruction"].Replace("\n", "===").Replace("\"", "“"); ;
            string cases ="";
            string member = "";
            string pic = context.Request["Pic"];
            string MethodType = context.Request["MethodType"];
            if (string.IsNullOrEmpty(title))
                return;
            model.t_InvestPlace_Title = title;
            model.t_InvestPlace_Phase = phase;
            model.t_InvestPlace_Area = area;
            model.t_InvestPlace_Money = money;
            model.t_InvestPlace_Instruction = instruction;
            model.t_InvestPlace_Member = member;
            model.t_InvestPlace_Money = money;
            model.t_InvestPlace_Case = cases;
            model.t_InvestPlace_ConverPic = pic;
            if (MethodType == "add")
            {
                model.t_DelState = 0;
                model.t_InvestPlace_Recommend = 0;
                model.Guid = Guid.NewGuid().ToString();
                bool result = bll.Add(model);
                if (result)
                {
                    context.Response.Write("ok");
                    context.Response.Flush();
                }
                else
                {
                    context.Response.Write("fail");
                    context.Response.Flush();
                }
            }
            else if (MethodType == "modify")
            {
                model.Guid = context.Request["Guid"];
                bool result = bll.Update(model);
                if (result)
                {
                    context.Response.Write("ok");
                    context.Response.Flush();
                }
                else
                {
                    context.Response.Write("fail");
                    context.Response.Flush();
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}