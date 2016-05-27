using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.QCH_Activity
{
    public partial class ActivityView : System.Web.UI.Page
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
        private DataTable dtCity = new BLL.S_City().GetList("").Tables[0];
        /// <summary>
        /// 加载数据
        /// </summary>
        private void IniteData()
        {
            string strWhere = string.Format("Guid='{0}'", Request["Guid"]);
            DataSet ds = new BLL.T_Activity().GetListView(1, strWhere, "t_AddDate desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                spTitle.InnerText = ds.Tables[0].Rows[0]["t_Activity_Title"].ToString();
                spContents.InnerHtml = ds.Tables[0].Rows[0]["t_Activity_Instruction"].ToString();
                spUser.InnerText = ds.Tables[0].Rows[0]["t_User_LoginId"].ToString();
                spsDate.InnerText = ds.Tables[0].Rows[0]["t_Activity_sDate"].ToString();
                speDate.InnerText = ds.Tables[0].Rows[0]["t_Activity_eDate"].ToString();
                spLongitude.InnerText = ds.Tables[0].Rows[0]["t_Activity_Longitude"].ToString();
                spLatitude.InnerText = ds.Tables[0].Rows[0]["t_Activity_Latitude"].ToString();
                spAddress.InnerText = ds.Tables[0].Rows[0]["t_Activity_CityName"].ToString() + ds.Tables[0].Rows[0]["t_Activity_Street"].ToString();
               
                spFeeType.InnerText = ds.Tables[0].Rows[0]["t_Activity_FeeType"].ToString();
                spFee.InnerText = ds.Tables[0].Rows[0]["t_Activity_Fee"].ToString();
                spTel.InnerText = ds.Tables[0].Rows[0]["t_Activity_Tel"].ToString();
                spHolder.InnerText = ds.Tables[0].Rows[0]["t_Activity_Holder"].ToString();
                spAddDate.InnerText = DateTime.Parse(ds.Tables[0].Rows[0]["t_AddDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string ifaudit = "否";
                if (ds.Tables[0].Rows[0]["t_Activity_Audit"].ToString() == "1")
                {
                    ifaudit = "是";
                }
                spAudit.InnerText = ifaudit;
                string ifre = "否";
                if (ds.Tables[0].Rows[0]["t_Activity_Recommand"].ToString() == "1")
                {
                    ifre = "是";
                }
                spRecommend.InnerText = ifre;
                #region 获取报名人数
                string applyCount = "0";
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["applyCount"].ToString()))
                {
                    applyCount = ds.Tables[0].Rows[0]["applyCount"].ToString();
                }
                #endregion
                spLimitPerson.InnerText = applyCount + "/" + ds.Tables[0].Rows[0]["t_Activity_LimitPerson"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Guid"].ToString()))
                {
                    #region
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<ul  class=\"box01\">");
                    sb.Append("<li> <div class=\"in\"><img style=\"width:180px;height:120px\"  id=\"" + ds.Tables[0].Rows[0]["Guid"].ToString() + "\" src=\"../Attach/Images/" + ds.Tables[0].Rows[0]["t_Activity_CoverPic"].ToString() + "\"/><p style=\"text-align:center\"><b>" + ds.Tables[0].Rows[0]["t_Activity_Title"] + "</b></p></li>");
                    sb.Append("</ul>");
                    imgtest.InnerHtml = sb.ToString();
                    #endregion
                }
            }
        }
    }
}