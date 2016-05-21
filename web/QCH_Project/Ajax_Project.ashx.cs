using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Maticsoft.Common;

namespace Maticsoft.Web.QCH_Project
{
    /// <summary>
    /// Ajax_Project 的摘要说明
    /// </summary>
    public class Ajax_Project : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        private BLL.T_Project bll = new BLL.T_Project();
        private BLL.T_User_Talk talkbll = new BLL.T_User_Talk();
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
                case "AddOrModify":
                    AddOrModify(context);
                    break;
                case "Recommend":
                    Recommend(context);
                    break;
                case "Audit":
                    Audit(context);
                    break;
                case "GetUserTalk": //获取评论
                    GetUserTalk(context);
                    break;
                case "DelUserTalk"://删除评论
                    DelUserTalk(context);
                    break;
                case "getModel":
                    GetModel(context);
                    break;
            }
            context.Response.End();
        }
  
        /// <summary>
        /// 获取评论数据
        /// </summary>
        /// <param name="context"></param>
        private void GetUserTalk(HttpContext context)
        {
            StringBuilder strWhere = new StringBuilder();
            string name = context.Request["name"];
            string guid = context.Request["Guid"];
            strWhere.AppendFormat(" t_DelState =0 and t_User_LoginId like '%{0}%'", name);
            strWhere.AppendFormat(" and t_Associate_Guid ='{0}'", guid);
            DataSet _Ds = talkbll.GetListView(0, strWhere.ToString(), "t_AddDate desc");
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
        /// 删除项目评价
        /// </summary>
        /// <param name="context"></param>
        private void DelUserTalk(HttpContext context)
        {
            string guid = context.Request["Guid"];
            bool ifSuc = talkbll.Delete(guid);
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
        /// 推荐
        /// </summary>
        /// <param name="context"></param>
        private void Recommend(HttpContext context)
        {
            string tmpguids = context.Request["GUID"];
            string[] guidList = tmpguids.Split(',');
            string guids = "";
            foreach (string guid in guidList)
            {
                guids = guids + "'" + guid + "',";
            }
            guids = guids.Substring(0, guids.Length - 1);
            bool ifSuc = bll.Recommend(guids);
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
        /// 审核
        /// </summary>
        /// <param name="context"></param>
        private void Audit(HttpContext context)
        {
            string tmpguids = context.Request["GUID"];
            string[] guidList = tmpguids.Split(',');
            string guids = "";
            foreach (string guid in guidList)
            {
                guids = guids + "'" + guid + "',";
            }
            guids = guids.Substring(0, guids.Length - 1);
            bool ifSuc = bll.Audit(guids);
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
        /// 获取数据
        /// </summary>
        /// <param name="context"></param>
        private void GetData(HttpContext context)
        {
            StringBuilder strWhere = new StringBuilder();
            string name = context.Request["name"];
            strWhere.AppendFormat(" t_DelState =0 and t_Project_Name like '%{0}%'", name);
            DataSet _Ds = bll.GetListView(0, strWhere.ToString(), "t_Project_Recommend desc,t_AddDate desc");
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

        /// <summary>
        /// 添加或修改
        /// </summary>
        private void AddOrModify(HttpContext context)
        {
            Maticsoft.Model.T_Project model = new Maticsoft.Model.T_Project();
            string userguid = context.Request["UserGuid"];
            string placeguid = context.Request["PlaceGuid"];
            string title = context.Request["Title"];
            string coverpic = context.Request["CoverPic"];
            string oneword = context.Request["OneWord"].Replace("\n", "===").Replace("\"", "“"); 
            string field = context.Request["Field"];
            string phase = context.Request["Phase"];
            string finance = context.Request["Finance"].Replace("\n", "===").Replace("\"", "“");
            string financeuse = context.Request["FinanceUse"].Replace("\n", "===").Replace("\"", "“");
            string financephase = context.Request["FinancePhase"];
            string parterwant = context.Request["ParterWant"];
            string province = context.Request["Province"];
            string city = context.Request["City"];
            string cityname = context.Request["CityName"];
            string instruction = context.Request["Instruction"];
            string isin = context.Request["IsIn"];
            string isroadshow = context.Request["IsRoadShow"];
            string MethodType = context.Request["MethodType"];
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(instruction))
            {
                context.Response.Write("标题或活动介绍不能为空！");
                return;
            }
            model.t_User_Guid = userguid;
            model.t_Place_Guid = placeguid;
            model.t_Project_Name = title;
            model.t_Project_ConverPic = coverpic;
            model.t_Project_OneWord = oneword;
            if (!string.IsNullOrEmpty(field))
            {
                model.t_Project_Field = int.Parse(field);
            }
            if (!string.IsNullOrEmpty(field))
            {
                model.t_Project_Phase = int.Parse(phase);
            }
            if (!string.IsNullOrEmpty(financephase))
            {
                model.t_Project_FinancePhase = int.Parse(financephase);
            }
            model.t_Project_Finance = finance;
            model.t_Project_FinanceUse = financeuse;
            model.t_Project_ParterWant = parterwant;
            if (!string.IsNullOrEmpty(province))
            {
                model.t_Project_Province = int.Parse(province);
            }
            if (!string.IsNullOrEmpty(city))
            {
                model.t_Project_City = int.Parse(city);
            }
            model.t_Project_CityName = cityname;
            model.t_Project_Instruction = instruction;
            if (!string.IsNullOrEmpty(isin))
            {
                model.t_Project_In = int.Parse(isin);
            }
            else
            {
                model.t_Project_In = 0;
            }
            if (!string.IsNullOrEmpty(isroadshow))
            {
                model.t_Project_RoadShow = int.Parse(isroadshow);
            }
            else
            {
                model.t_Project_RoadShow = 0;
            }
            if (MethodType == "add")
            {
                model.t_DelState = 0;
                model.t_Project_Recommend = 0;
                model.t_Project_Audit = 0;
                model.Guid = Guid.NewGuid().ToString();
                model.t_AddBy = context.Session["CurrentUserID"].ToString();
                model.t_AddDate = DateTime.Now;
                bool result = bll.Add(model);
                if (result)
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
            else if (MethodType == "modify")
            {
                model.Guid = context.Request["Guid"];
                model.t_ModifyBy = context.Session["CurrentUserID"].ToString();
                model.t_ModifydDate = DateTime.Now;
                bool result = bll.Update(model);
                if (result)
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