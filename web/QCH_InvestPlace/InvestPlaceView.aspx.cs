using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.QCH_InvestPlace
{
    public partial class InvestPlaceView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentUserID"] != null)
            {
                if (!IsPostBack)
                {
                    IniteData();
                }
            }
            else
            {
                Response.Redirect("../Login.aspx");
            }
        }

        //获取类型
        private DataTable dtStyle = new BLL.T_Style().GetList(0, "", "t_AddDate desc").Tables[0];
        /// <summary>
        /// 加载数据
        /// </summary>
        private void IniteData()
        {
            string strWhere = string.Format("Guid='{0}'", Request["Guid"]);
            DataSet ds = new BLL.T_Invest_Place().GetList(1, strWhere, "t_AddDate desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                spTitle.InnerText = ds.Tables[0].Rows[0]["t_InvestPlace_Title"].ToString();
                spPhase.InnerText = ds.Tables[0].Rows[0]["t_InvestPlace_Phase"].ToString();
                spArea.InnerText = ds.Tables[0].Rows[0]["t_InvestPlace_Area"].ToString();
                spMoney.InnerText = ds.Tables[0].Rows[0]["t_InvestPlace_Money"].ToString();
                spInstruction.InnerHtml = ds.Tables[0].Rows[0]["t_InvestPlace_Instruction"].ToString().Replace("===", "<br/>");
                string ifre = "否";
                if (ds.Tables[0].Rows[0]["t_InvestPlace_Recommend"].ToString() == "1")
                {
                    ifre = "是";
                }
                spRecommend.InnerText = ifre;
                #region  投资领域
                string strInvestArea = "";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["t_InvestPlace_Area"].ToString()))
                {
                    string[] arrInvestArea = ds.Tables[0].Rows[0]["t_InvestPlace_Area"].ToString().Split(';');
                    if (arrInvestArea != null && arrInvestArea.Length > 0)
                    {
                        foreach (var tmp in arrInvestArea)
                        {
                            if (!string.IsNullOrEmpty(strInvestArea))
                            {
                                strInvestArea = strInvestArea + "," + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString();
                            }
                            else
                            {
                                strInvestArea = dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString();
                            }
                        }
                    }
                }
                spArea.InnerText = strInvestArea;
                #endregion
                #region  投资阶段
                string strInvestPhase = "";

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["t_InvestPlace_Phase"].ToString()))
                {
                    string[] arrInvestPhase = ds.Tables[0].Rows[0]["t_InvestPlace_Phase"].ToString().Split(';');
                    if (arrInvestPhase != null && arrInvestPhase.Length > 0)
                    {
                        foreach (var tmp in arrInvestPhase)
                        {
                            if (!string.IsNullOrEmpty(strInvestPhase))
                            {
                                strInvestPhase = strInvestPhase + "," + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString();
                            }
                            else
                            {
                                strInvestPhase = dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString();
                            }
                        }
                    }
                }
                #endregion
                spPhase.InnerText = strInvestPhase;
                #region  孵化案例
                DataSet dsCase = new BLL.T_InvestPlace_Case().GetListView(0, "t_InvestPlace_Guid='" + ds.Tables[0].Rows[0]["Guid"] + "'", "");
                string strCase = "";
                if (dsCase != null && dsCase.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drcase in dsCase.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(strCase))
                        {
                            strCase = strCase + "【" + drcase["t_Project_Name"].ToString() + "】";
                        }
                        else
                        {
                            strCase = "【" + drcase["t_Project_Name"].ToString() + "】";
                        }
                    }
                }
                #endregion
                spCase.InnerHtml = strCase.ToString();
                #region  入驻成员
                DataSet dsMember = new BLL.T_InvestPlace_Member().GetListView(0, "t_InvestPlace_Guid='" + ds.Tables[0].Rows[0]["Guid"] + "'", "");
                string strMember = "";
                if (dsMember != null && dsMember.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drmember in dsMember.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(strMember))
                        {
                            strMember = strMember + "【" + drmember["t_User_LoginId"].ToString() + "-" + drmember["t_User_RealName"].ToString() + "】";
                        }
                        else
                        {
                            strMember = "【" + drmember["t_User_LoginId"].ToString() + "-" + drmember["t_User_RealName"].ToString() + "】";
                        }
                    }
                }
                #endregion

                spMember.InnerText = strMember;
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Guid"].ToString()))
                {
                    #region
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<ul  class=\"box01\">");
                    sb.Append("<li> <div class=\"in\"><img style=\"width:180px;height:120px\"  id=\"" + ds.Tables[0].Rows[0]["Guid"].ToString() + "\" src=\"../Attach/Images/" + ds.Tables[0].Rows[0]["t_InvestPlace_ConverPic"].ToString() + "\"/><p style=\"text-align:center\"><b>" + ds.Tables[0].Rows[0]["t_InvestPlace_Title"] + "</b></p></li>");
                    sb.Append("</ul>");
                    imgtest.InnerHtml = sb.ToString();
                    #endregion
                }
            }
        }
    }
}