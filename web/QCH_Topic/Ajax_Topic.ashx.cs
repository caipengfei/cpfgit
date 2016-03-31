using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Maticsoft.Common;

namespace Maticsoft.Web.QCH_Topic
{
    /// <summary>
    /// Ajax_Topic 的摘要说明
    /// </summary>
    public class Ajax_Topic : IHttpHandler
    {

        private BLL.T_Topic bll = new BLL.T_Topic();
        private BLL.T_Topic_Report reportbll = new BLL.T_Topic_Report();
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
                case "Delete":
                    DeleteData(context);
                    break;
                case "getModel":
                    GetModel(context);
                    break;
                case "GetTopicReport": //获取举报信息
                    GetTopicReport(context);
                    break;
                case "DelTopicReport"://删除举报信息
                    DelTopicReport(context);
                    break;
                case "GetTopicView"://删除举报信息
                    GetTopicView(context);
                    break;
            }
            context.Response.End();
        }
        public void GetTopicView(HttpContext context)
        {
            string guid = context.Request["Guid"];
            string strWhere = string.Format(" Guid='{0}' and t_DelState=0", guid);
            DataSet ds = bll.GetListView(0, strWhere, "t_Topic_Top,t_Date desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                context.Response.Write(GetTopicJson(ds));
            }
            else
            {
                context.Response.Write("[{\"state\":\"false\",\"result\":\"暂无数据！\"}]");
            }
        }
        //获取图片数据源
        private DataTable dtPic = new BLL.T_Associate_Pic().GetList("").Tables[0];
        //获取点赞数据源
        private DataTable dtPraise = new BLL.T_Praise().GetListView(0, "t_DelState=0", "t_Date desc").Tables[0];
        /// <summary>
        /// 获取json
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private string GetTopicJson(DataSet ds)
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder strJson = new StringBuilder();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    #region  获取关联图片
                    string pic = "";
                    DataRow[] drPic = dtPic.Select("t_Associate_Guid='" + dr["Guid"] + "'");
                    if (drPic != null && drPic.Length > 0)
                    {
                        StringBuilder strPicJson = new StringBuilder();
                        foreach (DataRow tmpPic in drPic)
                        {
                            strPicJson.Append("{\"t_Pic_Url\":\"" + tmpPic["t_Pic_Url"] + "\",\"t_Pic_Remark\":\"" + tmpPic["t_Pic_Remark"] + "\"},");
                        }
                        pic = strPicJson.ToString().Substring(0, strPicJson.Length - 1);
                    }
                    #endregion
                    #region 获取点赞人员信息
                    string strUserPraise = string.Format("t_Associate_Guid='{0}'", dr["Guid"].ToString());
                    DataRow[] drUserPraise = dtPraise.Select(strUserPraise);
                    //点赞人数
                    int PraiseCount = 0;
                    //点赞人员信息json
                    string PraiseUsers = "";
                    if (drUserPraise != null && drUserPraise.Length > 0)
                    {
                        StringBuilder strPraiseJson = new StringBuilder();
                        PraiseCount = drUserPraise.Length;
                        foreach (DataRow drtmp in drUserPraise)
                        {
                            strPraiseJson.Append("{\"PraiseUserGuid\":\"" + drtmp["t_User_Guid"] + "\",\"PraiseUserLoginId\":\"" + drtmp["t_User_LoginId"] + "\",\"PraiseUserRealName\":\"" + drtmp["t_User_RealName"] + "\",\"PraiseUserPic\":\"" + drtmp["t_User_Pic"] + "\",\"PraiseUserStyle\":\"" + drtmp["t_User_Style"] + "\"},");
                        }
                        PraiseUsers = strPraiseJson.ToString().Substring(0, strPraiseJson.Length - 1);
                    }
                    #endregion
                    strJson.Append("{\"Guid\":\"" + dr["Guid"] + "\",\"t_User_Guid\":\"" + dr["t_User_Guid"] + "\",\"t_Topic_Contents\":\"" + dr["t_Topic_Contents"] + "\",\"t_Topic_City\":\"" + dr["t_Topic_City"] + "\",\"t_Topic_Longitude\":\"" + dr["t_Topic_Longitude"] + "\",\"t_Topic_Latitude\":\"" + dr["t_Topic_Latitude"] + "\",\"t_Topic_Address\":\"" + dr["t_Topic_Address"] + "\",\"t_Topic_Top\":\"" + dr["t_Topic_Top"] + "\",\"t_Date\":\"" + dr["t_Date"] + "\",\"t_User_LoginId\":\"" + dr["t_User_LoginId"] + "\",\"t_User_RealName\":\"" + dr["t_User_RealName"] + "\",\"t_User_Pic\":\"" + dr["t_User_Pic"] + "\",\"t_User_Commpany\":\"" + dr["t_User_Commpany"] + "\",\"t_User_Position\":\"" + dr["t_User_Position"] + "\",\"PositionName\":\"" + dr["PositionName"] + "\",\"t_User_Style\":\"" + dr["t_User_Style"] + "\",\"PraiseCount\":\"" + PraiseCount + "\",\"PraiseUsers\":[" + PraiseUsers + "],\"Pic\":[" + pic + "]},");
                }
                return strJson.ToString().Substring(0, strJson.Length - 1);
            }
            else
            {
                return "[{\"state\":\"false\",\"result\":\"暂无数据！\"}]";
            }
        }
        /// <summary>
        /// 获取举报信息
        /// </summary>
        /// <param name="context"></param>
        private void GetTopicReport(HttpContext context)
        {
            StringBuilder strWhere = new StringBuilder();
            string name = context.Request["name"];
            string guid = context.Request["Guid"];
            strWhere.AppendFormat(" t_DelState =0 and t_User_LoginId like '%{0}%'", name);
            strWhere.AppendFormat(" and t_Topic_Guid ='{0}'", guid);
            DataSet _Ds = reportbll.GetListView(0, strWhere.ToString(), "t_Date desc");
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
        /// 删除活动评价
        /// </summary>
        /// <param name="context"></param>
        private void DelTopicReport(HttpContext context)
        {
            string guid = context.Request["Guid"];
            bool ifSuc = reportbll.Delete(guid);
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
        /// 获取数据
        /// </summary>
        /// <param name="context"></param>
        private void GetData(HttpContext context)
        {
            StringBuilder strWhere = new StringBuilder();
            string name = context.Request["name"];
            strWhere.AppendFormat(" t_DelState =0 and t_Topic_Contents like '%{0}%'", name);
            DataSet _Ds = bll.GetListView(0, strWhere.ToString(), "t_Date desc");
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
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        private void DeleteData(HttpContext context)
        {
            string tmpguids = context.Request["GUID"];
            string[] guidList = tmpguids.Split(',');
            string guids = "";
            foreach (string guid in guidList)
            {
                guids = guids + "'" + guid + "',";
            }
            guids = guids.Substring(0, guids.Length - 1);
            bool ifSuc = bll.Del(guids);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}