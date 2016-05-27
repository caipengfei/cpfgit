using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.QCH_Style
{
    public partial class StyleView : System.Web.UI.Page
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
        /// <summary>
        /// 加载数据
        /// </summary>
        private void IniteData()
        {
            string strWhere = string.Format("Id={0}", Request["Id"]);
            DataSet ds = new BLL.T_Style().GetListView(1, strWhere, "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                spIndex.InnerText = ds.Tables[0].Rows[0]["t_SortIndex"].ToString();
                spName.InnerText = ds.Tables[0].Rows[0]["t_Style_Name"].ToString();
                spfName.InnerText = ds.Tables[0].Rows[0]["fName"].ToString();
                spRemark.InnerHtml = ds.Tables[0].Rows[0]["t_Style_Remark"].ToString().Replace("===", "<br/>");
                spDate.InnerText = DateTime.Parse(ds.Tables[0].Rows[0]["t_AddDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["t_Style_Pic"].ToString()))
                {
                    #region
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<ul  class=\"box01\">");
                    sb.Append("<li> <div class=\"in\"><img  id=\"" + ds.Tables[0].Rows[0]["Id"].ToString() + "\" src=\"../Attach/" + ds.Tables[0].Rows[0]["t_Style_Pic"].ToString() + "\"/><p style=\"text-align:center\"><b>" + ds.Tables[0].Rows[0]["t_Style_Name"] + "</b></p></li>");
                    sb.Append("</ul>");
                    imgtest.InnerHtml = sb.ToString();
                    #endregion
                }
            }
        }
    }
}