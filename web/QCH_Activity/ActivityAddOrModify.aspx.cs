using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.QCH_Activity
{
    public partial class ActivityAddOrModify : System.Web.UI.Page
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
        private BLL.T_Activity bll = new BLL.T_Activity();
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
                    string guid = Request["Guid"];
                    hdType.Value = "modify";
                    var entity = bll.GetModel(guid);
                    txtTitle.Value = entity.t_Activity_Title;
                    content1.Value = entity.t_Activity_Instruction;
                    txtsDate.Value = entity.t_Activity_sDate.ToString();
                    txteDate.Value = entity.t_Activity_eDate.ToString();
                    txtStreet.Value = entity.t_Activity_Street;
                    txtLimitPerson.Value = entity.t_Activity_LimitPerson.ToString();
                    txtHolder.Value = entity.t_Activity_Holder;
                    selFeeType.Value = entity.t_Activity_FeeType;
                    txtFee.Value = entity.t_Activity_Fee.ToString();
                    txtTel.Value = entity.t_Activity_Tel.ToString();
                    hidFileName.Value = entity.t_Activity_CoverPic;
                    hdId.Value = entity.Guid.ToString();
                }
            }
        }
    }
}