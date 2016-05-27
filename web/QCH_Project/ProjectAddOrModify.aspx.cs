using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.QCH_Project
{
    public partial class ProjectAddOrModify : System.Web.UI.Page
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
        private BLL.T_Project bll = new BLL.T_Project();
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
                    txtTitle.Value = entity.t_Project_Name;
                    content1.Value = entity.t_Project_Instruction;
                    txtOneWord.Value = entity.t_Project_OneWord.Replace("===", "\n");
                    txtField.Value = entity.t_Project_Field.ToString();
                    txtPhase.Value = entity.t_Project_Phase.ToString();
                    txtFinance.Value = entity.t_Project_Finance.Replace("===", "\n");
                    txtFinancePhase.Value = entity.t_Project_FinancePhase.ToString();
                    txtFinanceUse.Value = entity.t_Project_FinanceUse.Replace("===", "\n");
                    txtParterWant.Value = entity.t_Project_ParterWant;
                    //selIsIn.Value = entity.t_Project_In.ToString();
                    //selRoadShow.Value = entity.t_Project_RoadShow.ToString();
                    hidFileName.Value = entity.t_Project_ConverPic;
                    hdId.Value = entity.Guid.ToString();
                }
            }
        }
    }
}