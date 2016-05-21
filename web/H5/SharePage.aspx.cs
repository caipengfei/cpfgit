using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.H5
{
    public partial class SharePage : System.Web.UI.Page
    {
        Maticsoft.BLL.T_Users userService = new BLL.T_Users();
        Maticsoft.BLL.T_HistoryWork workService = new BLL.T_HistoryWork();
        Maticsoft.BLL.T_Topic topicService = new BLL.T_Topic();
        Maticsoft.BLL.T_Project projectService = new BLL.T_Project();
        Maticsoft.BLL.T_Associate_Pic picService = new BLL.T_Associate_Pic();
        Maticsoft.BLL.T_User_Foucs foucsService = new BLL.T_User_Foucs();
        protected void Page_Load(object sender, EventArgs e)
        {
            var userguid = "5aaa0d2b-8204-4994-82c1-686299491b91";
            //获取用户信息
            var user = userService.GetModel(userguid);
            if (user != null)
            {
                this.spUserName.InnerText = user.t_User_RealName;
                this.txtMyAtte.Value = user.t_User_FocusArea;
                this.txtMyGood.Value = user.t_User_Best;
                this.spUserCity.InnerText = user.t_User_City;
                this.spPosition.InnerText = user.t_User_Position;
                this.spCom.InnerText = user.t_User_Commpany;
            }
            //关注的人
            var guanzhu = foucsService.GetList(0, "t_User_Guid='" + userguid + "' and t_Focus_Guid!='' and t_Focus_Guid is not null", "t_Date");
            if (guanzhu != null)
            {
                int count = guanzhu.Tables[0].Rows.Count;
                this.spAttention.InnerText = count.ToString();
            }
            //粉丝
            var fensi = foucsService.GetList(0, "t_Focus_Guid='" + userguid + "' and t_User_Guid!='' and t_User_Guid is not null", "t_Date");
            if (fensi != null)
            {
                int count = fensi.Tables[0].Rows.Count;
                this.spFans.InnerText = count.ToString();
            }
            //工作经历
            var work = workService.GetList(3, "t_User_Guid='" + userguid + "'", "t_Date");
            if (work != null)
            {
                this.repWorkList.DataSource = work;
                this.repWorkList.DataBind();
            }
            //动态
            List<object> list = new List<object>();
            var topic = topicService.GetList(3, "t_User_Guid='" + userguid + "'", "t_Date");
            if (topic != null)
            {

                foreach (DataRow item in topic.Tables[0].Rows)
                {
                    //获取动态图片
                    var userid = item[0].ToString();
                    string picUrl = "";
                    if (!string.IsNullOrWhiteSpace(userid))
                    {
                        var pic = picService.GetList("t_Associate_Guid='" + userid + "'");
                        if (pic != null)
                        {
                            foreach (DataRow tt in pic.Tables[0].Rows)
                            {
                                picUrl = tt[2].ToString();
                            }
                        }
                    }
                    //重新封装
                    var target = new
                    {
                        City = item[3],
                        Contents = item[2],
                        ImgUrl = picUrl
                    };
                    list.Add(target);
                }
                this.repTopic.DataSource = list;
                this.repTopic.DataBind();
            }
            //项目
            var project = projectService.GetList(3, "t_User_Guid='" + userguid + "'", "t_AddDate");
            if (project != null)
            {
                this.repProject.DataSource = project;
                this.repProject.DataBind();
            }
        }
    }
}