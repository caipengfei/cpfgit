using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.QCH_Topic
{
    public partial class TopicView : System.Web.UI.Page
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
            string strWhere = string.Format("Guid='{0}'", Request["Guid"]);
            DataSet ds = new BLL.T_Topic().GetListView(1, strWhere, "t_Date desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                spContents.InnerHtml = ds.Tables[0].Rows[0]["t_Topic_Contents"].ToString();
                spUser.InnerText = "【" + ds.Tables[0].Rows[0]["t_User_LoginId"].ToString() + "】" + ds.Tables[0].Rows[0]["t_User_RealName"].ToString();
                spDate.InnerText = DateTime.Parse(ds.Tables[0].Rows[0]["t_Date"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"); 
                spLongitude.InnerText = ds.Tables[0].Rows[0]["t_Topic_Longitude"].ToString();
                spLatitude.InnerText = ds.Tables[0].Rows[0]["t_Topic_Latitude"].ToString();
                spAddress.InnerText = ds.Tables[0].Rows[0]["t_Topic_City"].ToString() + ds.Tables[0].Rows[0]["t_Topic_Address"].ToString();
                DataSet dspic = new BLL.T_Associate_Pic().GetList("t_Associate_Guid='" + ds.Tables[0].Rows[0]["Guid"].ToString() + "'");
                if (dspic!=null&&dspic.Tables[0].Rows.Count>0)
                {
                    #region
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<ul  class=\"box01\">");
                    foreach (DataRow dr in dspic.Tables[0].Rows)
                    {
                        sb.Append("<li> <div class=\"in\"><img style=\"width:180px;height:120px\"  id=\"" + dr["Guid"].ToString() + "\" src=\"../Attach/Images/" + dr["t_Pic_Url"].ToString() + "\"/><p style=\"text-align:center\"><b>" + dr["t_Pic_Remark"] + "</b></p></li>");
                    }
                    sb.Append("</ul>");
                    imgtest.InnerHtml = sb.ToString();
                    #endregion
                }
            }
        }
    }
}