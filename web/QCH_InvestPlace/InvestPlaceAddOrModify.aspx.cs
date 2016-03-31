using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.QCH_InvestPlace
{
    public partial class InvestPlaceAddOrModify : System.Web.UI.Page
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
        private BLL.T_Invest_Place bll = new BLL.T_Invest_Place();
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
                    txtTitle.Value = entity.t_InvestPlace_Title.ToString();
                    txtPhase.Value = entity.t_InvestPlace_Phase;
                    txtArea.Value = entity.t_InvestPlace_Area;
                    txtMoney.Value = entity.t_InvestPlace_Money;
                    txtInstruction.Value = entity.t_InvestPlace_Instruction.Replace("===", "\n");
                    //txtCases.Value = entity.t_InvestPlace_Case;
                    //txtMember.Value = entity.t_InvestPlace_Member;
                    hidFileName.Value = entity.t_InvestPlace_ConverPic;
                    hdId.Value = entity.Guid;
                }
            }
        }
    }
}