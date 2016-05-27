using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.QCH_Style
{
    public partial class StyleAddOrModify : System.Web.UI.Page
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
        /// 初使化数据
        /// </summary>
        private void IniteData()
        {
            string _action = Request["action"];
            if (!string.IsNullOrEmpty(_action))
            {
                if (_action == "add")//添加
                {
                    hdType.Value = "add";
                }
                else if (_action == "modify")//修改
                {
                    if (!string.IsNullOrEmpty(Request["Id"]))
                    {
                        int id = int.Parse(Request["Id"]);
                        hdType.Value = "modify";
                        var entity = new BLL.T_Style().GetModel(id);
                        txtName.Value = entity.t_Style_Name;
                        txtparentStyle.Value = entity.t_fId.ToString();
                        txtRemark.Value = entity.t_Style_Remark.Replace("===", "\n");
                        txtIndex.Value = entity.t_SortIndex.ToString();
                        hidFileName.Value = entity.t_Style_Pic;
                        hdId.Value = entity.Id.ToString();
                    }
                }
            }
        }
    }
}