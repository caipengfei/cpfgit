using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Maticsoft.Common;

namespace Maticsoft.Web.QCH_User
{
    /// <summary>
    /// Ajax_User 的摘要说明
    /// </summary>
    public class Ajax_User : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        Maticsoft.BLL.T_User_Talk talkService = new BLL.T_User_Talk();
        Maticsoft.BLL.T_User_Foucs foucsService = new BLL.T_User_Foucs();
        private readonly BLL.T_Users bll = new BLL.T_Users();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            string view = context.Request["Action"];

            switch (view)
            {
                case "List":
                    GetData(context);
                    break;
                case "IsExist":
                    IsExist(context);
                    break;
                case "GetUser":
                    GetUser(context);
                    break;
                case "getModel":
                    GetModel(context);
                    break;
                case "AddOrModify":
                    AddOrModify(context);
                    break;
                case "UserData":
                    UserData(context);
                    break;
                case "Attention":
                    GetAttention(context);
                    break;
                case "MyFans":
                    GetFans(context);
                    break;
                case "GetTalk":
                    GetTalk(context);
                    break;
            }
            context.Response.End();
        }

        /// <summary>
        /// 获取我关注的人
        /// </summary>
        /// <param name="context"></param>
        private void GetAttention(HttpContext context)
        {
            string guid = context.Request["guid"];
            DataSet _Ds = foucsService.GetList(0, "t_User_Guid='" + guid + "' and t_Focus_Guid!='' and t_Focus_Guid is not null", "t_Date");
            int _TotalCount = 0;
            if (_Ds != null && _Ds.Tables[0].Rows.Count > 0)
            {
                _TotalCount = _Ds.Tables[0].Rows.Count;
                DataTable _Dt = _Ds.Tables[0];
                string _JsonData = new DataTableJSONSerializer().Serialize(_Dt);
                string _Json = @"{""Rows"":" + _JsonData + @",""Total"":""" + _TotalCount + @"""}";
                context.Response.Write(_Json);
                context.Response.End();
            }
        }
        /// <summary>
        /// 获取我的粉丝
        /// </summary>
        /// <param name="context"></param>
        private void GetFans(HttpContext context)
        {
            string guid = context.Request["guid"];
            DataSet _Ds = foucsService.GetList(0, "t_Focus_Guid='" + guid + "' and t_User_Guid!='' and t_User_Guid is not null", "t_Date");
            int _TotalCount = 0;
            if (_Ds != null && _Ds.Tables[0].Rows.Count > 0)
            {
                _TotalCount = _Ds.Tables[0].Rows.Count;
                DataTable _Dt = _Ds.Tables[0];
                string _JsonData = new DataTableJSONSerializer().Serialize(_Dt);
                string _Json = @"{""Rows"":" + _JsonData + @",""Total"":""" + _TotalCount + @"""}";
                context.Response.Write(_Json);
                context.Response.End();
            }
        }
        /// <summary>
        /// 获取用户结构数据
        /// </summary>
        /// <param name="syncDataRegionCode"></param>
        /// <param name="currentzw"></param>
        public void UserData(HttpContext context)
        {
            string loginid = context.Request["LoginId"];
            string strWhere = string.Format("t_DelState =0 and t_User_LoginId like '%{0}%'", loginid);
            DataTable dt = bll.GetList(0, strWhere, "t_User_LoginId ").Tables[0];
            string json = string.Empty;
            json = GetTree(dt);
            context.Response.Write(json);
            context.Response.Flush();
        }
        //返回json串
        private string GetTree(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder rs = new StringBuilder();
                rs.Append("[");
                foreach (DataRow dr in dt.Rows)
                {
                    rs.Append("{\"id\":\"" + dr["Guid"] + "\",\"text\":\"" + dr["t_User_LoginId"] + "\"");
                    rs.Append("},");
                }
                rs = rs.Remove(rs.Length - 1, 1);
                rs.Append("]");
                return rs.ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 获取对象实体
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public void GetModel(HttpContext context)
        {
            string guid = context.Request["Guid"];
            context.Response.Write(bll.GetModel(guid).ToJson());
            context.Response.Flush();
        }
        /// <summary>
        /// 添加或修改
        /// </summary>
        private void AddOrModify(HttpContext context)
        {
            Maticsoft.Model.T_Users model = new Maticsoft.Model.T_Users();
            string LoginId = context.Request["LoginId"];
            model.t_User_LoginId = LoginId;
            model.t_User_Style = 0;
            string MethodType = context.Request["methodType"];
            if (MethodType == "add")//添加
            {
                if (!bll.IsExist(LoginId))
                {
                    model.t_DelState = 0;
                    model.t_User_Date = DateTime.Now;
                    model.t_User_Pwd = "1D4D2BC318872FEB";//默认密码 123456
                    string guid = Guid.NewGuid().ToString();
                    model.Guid = guid;
                    if (bll.Add(model))
                    {
                        context.Response.Write("ok");
                        context.Response.Flush();
                    }
                    else
                    {
                        context.Response.Write("fail");
                        context.Response.Flush();
                    }
                }
                else
                {
                    context.Response.Write("该手机号已注册！");
                    context.Response.Flush();
                }
            }
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="context"></param>
        private void IsExist(HttpContext context)
        {
            string _exist = context.Request["Exist"];
            bool ifSuc = bll.IsExist(_exist);
            if (ifSuc)
            {
                context.Response.Write("ok");
                context.Response.Flush();
            }
            else
            {
                context.Response.Write("fail");
                context.Response.Flush();
            }
        }
        /// <summary>
        /// 获取用户
        /// </summary>
        DataTable dt = new BLL.T_Users().GetList("").Tables[0];
        public void GetUser(HttpContext context)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat("(t_User_LoginId like '%{0}%' or t_User_RealName  like '%{0}%') and t_DelState=0", context.Request["LoginID"]);
            DataRow[] dr = dt.Select(strWhere.ToString());
            StringBuilder rs = new StringBuilder();
            rs.Append(" <option value=\"\">--请选择--</option>");
            if (dr != null && dr.Length > 0)
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    rs.Append(" <option value=\"" + dr[i]["Guid"] + "\">" + dr[i]["t_User_LoginId"] + "-" + dr[i]["t_User_RealName"] + "</option>");
                }
            }
            context.Response.Write(rs.ToString());
            context.Response.Flush();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="context"></param>
        private void GetData(HttpContext context)
        {
            string name = context.Request["name"];
            string loginid = context.Request["loginid"];
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append("t_DelState =0 ");
            if (!string.IsNullOrEmpty(loginid))
            {
                strWhere.AppendFormat(" and t_User_LoginId = '{0}'", loginid);
            }
            if (!string.IsNullOrEmpty(name))
            {
                strWhere.AppendFormat(" and t_User_RealName = '{0}'", name);
            }
            DataSet _Ds = bll.GetList(0, strWhere.ToString(), "t_User_Date desc");
            int _TotalCount = 0;
            if (_Ds != null && _Ds.Tables[0].Rows.Count > 0)
            {
                _TotalCount = _Ds.Tables[0].Rows.Count;
                DataTable _Dt = _Ds.Tables[0];

                string _JsonData = new DataTableJSONSerializer().Serialize(_Dt);
                string _Json = @"{""Rows"":" + _JsonData + @",""Total"":""" + _TotalCount + @"""}";
                context.Response.Write(_Json);
                context.Response.End();

            }
        }
        /// <summary>
        /// 相关评论
        /// </summary>
        /// <param name="context"></param>
        public void GetTalk(HttpContext context)
        {
            StringBuilder strJson = new StringBuilder();
            string guid = context.Request["Guid"];
            string strWhere = string.Format("t_Associate_Guid='{0}' and t_DelState=0", guid);
            DataSet ds = talkService.GetListViewByPage(1, 10, strWhere, "t_Talk_FromDate desc ");

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                strJson.Append("[");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    #region 获取回复
                    DataSet dsReply = talkService.GetListView(0, "t_DelState=0 and t_Talk_ToUserGuid='" + dr["t_Talk_FromUserGuid"] + "'", "t_Talk_ToDate desc ");
                    string strReply = "";
                    if (dsReply != null && dsReply.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strReplyJson = new StringBuilder();
                        foreach (DataRow drReply in dsReply.Tables[0].Rows)
                        {
                            strReplyJson.Append("{\"Guid\":\"" + drReply["Guid"] + "\",\"t_Associate_Guid\":\"" + drReply["t_Associate_Guid"] + "\",\"t_Talk_FromUserGuid\":\"" + drReply["t_Talk_FromUserGuid"] + "\",\"t_Talk_FromContent\":\"" + Filter(drReply["t_Talk_FromContent"].ToString()) + "\",\"t_Talk_FromDate\":\"" + drReply["t_Talk_FromDate"] + "\",\"t_Talk_ToUserGuid\":\"" + drReply["t_Talk_ToUserGuid"] + "\",\"t_Talk_ToContent\":\"" + Filter(drReply["t_Talk_ToContent"].ToString()) + "\",\"t_Talk_ToDate\":\"" + drReply["t_Talk_ToDate"] + "\",\"t_Talk_Good\":\"" + drReply["t_Talk_Good"] + "\",\"t_Talk_Bad\":\"" + drReply["t_Talk_Bad"] + "\",\"t_Talk_Audit\":\"" + drReply["t_Talk_Audit"] + "\",\"t_DelState\":\"" + drReply["t_DelState"] + "\",\"t_User_LoginId\":\"" + drReply["t_User_LoginId"] + "\",\"t_User_RealName\":\"" + drReply["t_User_RealName"] + "\",\"t_User_Pic\":\"" + drReply["t_User_Pic"] + "\",\"toUserLoginId\":\"" + drReply["toUserLoginId"] + "\",\"toUserRealName\":\"" + drReply["toUserRealName"] + "\",\"toUserPic\":\"" + drReply["toUserPic"] + "\"},");
                        }
                        strReply = strReplyJson.ToString().Substring(0, strReplyJson.Length - 1);
                    }
                    #endregion
                    strJson.Append("{\"Guid\":\"" + dr["Guid"] + "\",\"t_Associate_Guid\":\"" + dr["t_Associate_Guid"] + "\",\"t_Talk_FromUserGuid\":\"" + dr["t_Talk_FromUserGuid"] + "\",\"t_Talk_FromContent\":\"" + Filter(dr["t_Talk_FromContent"].ToString()) + "\",\"t_Talk_FromDate\":\"" + dr["t_Talk_FromDate"] + "\",\"t_Talk_ToUserGuid\":\"" + dr["t_Talk_ToUserGuid"] + "\",\"t_Talk_ToContent\":\"" + Filter(dr["t_Talk_ToContent"].ToString()) + "\",\"t_Talk_ToDate\":\"" + dr["t_Talk_ToDate"] + "\",\"t_Talk_Good\":\"" + dr["t_Talk_Good"] + "\",\"t_Talk_Bad\":\"" + dr["t_Talk_Bad"] + "\",\"t_Talk_Audit\":\"" + dr["t_Talk_Audit"] + "\",\"t_DelState\":\"" + dr["t_DelState"] + "\",\"t_User_LoginId\":\"" + dr["t_User_LoginId"] + "\",\"t_User_RealName\":\"" + dr["t_User_RealName"] + "\",\"t_User_Pic\":\"" + dr["t_User_Pic"] + "\",\"toUserLoginId\":\"" + dr["toUserLoginId"] + "\",\"toUserRealName\":\"" + dr["toUserRealName"] + "\",\"toUserPic\":\"" + dr["toUserPic"] + "\",\"strReply\":[" + strReply + "]},");
                }
                string str = strJson.ToString().Substring(0, strJson.Length - 1) + "]";
                 
                context.Response.Write(str);
            }
            else
            {
                context.Response.Write("[{\"state\":\"false\",\"result\":\"暂无数据\"}]");
            }
        }
        //过滤敏感词
        private string Filter(string word)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("../bad.txt");
            FilterWord filter = new FilterWord(path);
            filter.SourctText = word;
            return filter.Filter('*');
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}