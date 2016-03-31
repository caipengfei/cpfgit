using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.QCH_Project
{
    public partial class ProjectView : System.Web.UI.Page
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
        private DataTable dtStyle = new BLL.T_Style().GetList("").Tables[0];
        /// <summary>
        /// 加载数据
        /// </summary>
        private void IniteData()
        {
            string strWhere = string.Format("Guid='{0}'", Request["Guid"]);
            DataSet ds = new BLL.T_Project().GetListView(1, strWhere, "t_AddDate desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                spTitle.InnerText = ds.Tables[0].Rows[0]["t_Project_Name"].ToString();
                spContents.InnerHtml = ds.Tables[0].Rows[0]["t_Project_Instruction"].ToString();
                spOneWord.InnerText = ds.Tables[0].Rows[0]["t_Project_OneWord"].ToString();

                spPhase.InnerText = ds.Tables[0].Rows[0]["PhaseName"].ToString();
                spFiled.InnerText = ds.Tables[0].Rows[0]["FiledName"].ToString();
                spCity.InnerText = ds.Tables[0].Rows[0]["t_Project_CityName"].ToString();
                spFinance.InnerText = ds.Tables[0].Rows[0]["t_Project_Finance"].ToString();
                spFinancePhase.InnerText = ds.Tables[0].Rows[0]["FinancePhaseName"].ToString();
                spFinanceUse.InnerText = ds.Tables[0].Rows[0]["t_Project_FinanceUse"].ToString();
               
                string strParterWant = "";
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["t_Project_ParterWant"].ToString()))
                {
                    string[] arrParterWant = ds.Tables[0].Rows[0]["t_Project_ParterWant"].ToString().Split(';');
                    if (arrParterWant != null && arrParterWant.Length > 0)
                    {
                        foreach (var tmp in arrParterWant)
                        {
                            if (!string.IsNullOrEmpty(strParterWant))
                            {
                                strParterWant = strParterWant + "," + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString();
                            }
                            else
                            {
                                strParterWant = dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString();
                            }
                        }
                    }
                }
                spParterWant.InnerText = strParterWant;
                spHolder.InnerText = "【" + ds.Tables[0].Rows[0]["t_User_LoginId"].ToString() + "】" + ds.Tables[0].Rows[0]["t_User_RealName"].ToString();
                spAddDate.InnerText = DateTime.Parse(ds.Tables[0].Rows[0]["t_AddDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string ifaudit = "否";
                if (ds.Tables[0].Rows[0]["t_Project_Audit"].ToString() == "1")
                {
                    ifaudit = "是";
                }
                spAudit.InnerText = ifaudit;
                string ifre = "否";
                if (ds.Tables[0].Rows[0]["t_Project_Recommend"].ToString() == "1")
                {
                    ifre = "是";
                }
                //string ifin = "否";
                //if (ds.Tables[0].Rows[0]["t_Project_In"].ToString() == "1")
                //{
                //    ifin = "是";
                //}
                //spIsIn.InnerText = ifin;
                //string ifroadshow = "否";
                //if (ds.Tables[0].Rows[0]["t_Project_RoadShow"].ToString() == "1")
                //{
                //    ifroadshow = "是";
                //}
                //spRoadShow.InnerText = ifroadshow;
                spRecommend.InnerText = ifre;
                spPlace.InnerText = ds.Tables[0].Rows[0]["t_Place_Name"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Guid"].ToString()))
                {
                    #region
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<ul  class=\"box01\">");
                    sb.Append("<li> <div class=\"in\"><img style=\"width:180px;height:120px\"  id=\"" + ds.Tables[0].Rows[0]["Guid"].ToString() + "\" src=\"../Attach/Images/" + ds.Tables[0].Rows[0]["t_Project_ConverPic"].ToString() + "\"/><p style=\"text-align:center\"><b>" + ds.Tables[0].Rows[0]["t_Project_Name"] + "</b></p></li>");
                    sb.Append("</ul>");
                    imgtest.InnerHtml = sb.ToString();
                    #endregion
                }
            }
        }
    }
}