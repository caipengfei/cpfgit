using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Maticsoft.Common;

namespace Maticsoft.Web.QCH_Activity
{
    /// <summary>
    /// Ajax_Activity 的摘要说明
    /// </summary>
    public class Ajax_Activity : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        private BLL.T_Activity bll = new BLL.T_Activity();
        private BLL.T_Activity_Apply applybll = new BLL.T_Activity_Apply();
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
                case "GetActivityReply": //获取评论
                    GetActivityReply(context);
                    break;
                case "DelActivityReply"://删除评论
                    DelActivityReply(context);
                    break;
                case "getModel":
                    GetModel(context);
                    break;
                case "SetMap":
                    SetMap(context);
                    break;
                case "GetActivityApply"://获取报名
                    GetActivityApply(context);
                    break;
                case "DelActivityApply"://报名删除
                    DelActivityApply(context);
                    break;
                case "GetActivityView":
                    GetActivityView(context);
                    break;
            }
            context.Response.End();
        }
        public void GetActivityView(HttpContext context)
        {
            string guid = context.Request["Guid"];

            string strWhere = string.Format(" Guid='{0}' and t_DelState=0", guid);
            DataSet ds = bll.GetListView(0, strWhere, "t_AddDate desc");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                context.Response.Write(GetActivityJson(ds));
            }
            else
            {
                context.Response.Write("[{\"state\":\"false\",\"result\":\"暂无数据！\"}]");
            }
        }
        private DataTable dtApply = new BLL.T_Activity_Apply().GetListView(0, "t_DelState=0", "t_AddDate desc").Tables[0];
        /// <summary>
        /// 获取json
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private string GetActivityJson(DataSet ds)
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder strJson = new StringBuilder();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    #region 获取报名人员信息
                    string strUserApply = string.Format("t_Activity_Guid='{0}'", dr["Guid"].ToString());
                    DataRow[] drUserApply = dtApply.Select(strUserApply);
                    //报名人数
                    int ApplyCount = 0;
                    //报名人员信息json
                    string ApplyUsers = "";
                    if (drUserApply != null && drUserApply.Length > 0)
                    {
                        StringBuilder strApplyJson = new StringBuilder();
                        ApplyCount = drUserApply.Length;
                        foreach (DataRow drtmp in drUserApply)
                        {
                            strApplyJson.Append("{\"ApplyUserGuid\":\"" + drtmp["t_User_Guid"] + "\",\"ApplyUserLoginId\":\"" + drtmp["t_User_LoginId"] + "\",\"ApplyUserRealName\":\"" + drtmp["t_User_RealName"] + "\",\"ApplyUserPic\":\"" + drtmp["t_User_Pic"] + "\",\"ApplyUserStyle\":\"" + drtmp["t_User_Style"] + "\"},");
                        }
                        ApplyUsers = strApplyJson.ToString().Substring(0, strApplyJson.Length - 1);
                    }
                    #endregion
                    
                    #region 活动是否结束
                    //默认未结束
                    string ifOver = "0";
                    if (DateTime.Compare(DateTime.Parse(dr["t_Activity_eDate"].ToString()), DateTime.Now) < 0)
                    {
                        ifOver = "1";
                    }
                    #endregion
                    strJson.Append("{\"Guid\":\"" + dr["Guid"] + "\",\"t_User_Guid\":\"" + dr["t_User_Guid"] + "\",\"t_Activity_Title\":\"" + dr["t_Activity_Title"] + "\",\"t_Activity_CoverPic\":\"" + dr["t_Activity_CoverPic"] + "\",\"t_Activity_sDate\":\"" + dr["t_Activity_sDate"] + "\",\"t_Activity_eDate\":\"" + dr["t_Activity_eDate"] + "\",\"t_Activity_Street\":\"" + dr["t_Activity_Street"] + "\",\"t_Activity_Latitude\":\"" + dr["t_Activity_Latitude"] + "\",\"t_Activity_Longitude\":\"" + dr["t_Activity_Longitude"] + "\",\"t_Activity_LimitPerson\":\"" + dr["t_Activity_LimitPerson"] + "\",\"t_Activity_FeeType\":\"" + dr["t_Activity_FeeType"] + "\",\"t_Activity_Fee\":\"" + dr["t_Activity_Fee"] + "\",\"t_Activity_Tel\":\"" + dr["t_Activity_Tel"] + "\",\"t_Activity_CityName\":\"" + dr["t_Activity_CityName"] + "\",\"t_Activity_Holder\":\"" + dr["t_Activity_Holder"] + "\",\"t_AddDate\":\"" + dr["t_AddDate"] + "\",\"t_User_LoginId\":\"" + dr["t_User_LoginId"] + "\",\"t_User_RealName\":\"" + dr["t_User_RealName"] + "\",\"t_User_Pic\":\"" + dr["t_User_Pic"] + "\",\"t_User_Commpany\":\"" + dr["t_User_Commpany"] + "\",\"t_User_Position\":\"" + dr["t_User_Position"] + "\",\"PositionName\":\"" + dr["PositionName"] + "\",\"ifOver\":\"" + ifOver + "\",\"ApplyCount\":\"" + ApplyCount + "\",\"ApplyUsers\":[" + ApplyUsers + "]},");
                }
                return strJson.ToString().Substring(0, strJson.Length - 1);
            }
            else
            {
                return "[{\"state\":\"false\",\"result\":\"暂无数据！\"}]";
            }
        }
        /// <summary>
        /// 获取评论数据
        /// </summary>
        /// <param name="context"></param>
        private void GetActivityReply(HttpContext context)
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
        /// 获取报名数据
        /// </summary>
        /// <param name="context"></param>
        private void GetActivityApply(HttpContext context)
        {
            StringBuilder strWhere = new StringBuilder();
            string name = context.Request["name"];
            string guid = context.Request["Guid"];
            strWhere.AppendFormat(" t_DelState =0 and t_User_LoginId like '%{0}%'", name);
            strWhere.AppendFormat(" and t_Activity_Guid ='{0}'", guid);
            DataSet _Ds = applybll.GetListView(0, strWhere.ToString(), "t_AddDate desc");
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
        /// 删除活动评价
        /// </summary>
        /// <param name="context"></param>
        private void DelActivityReply(HttpContext context)
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
        /// 删除活动报名
        /// </summary>
        /// <param name="context"></param>
        private void DelActivityApply(HttpContext context)
        {
            string guid = context.Request["Guid"];
            bool ifSuc = new BLL.T_Activity_Apply().Delete(guid);
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
            strWhere.AppendFormat(" t_DelState =0 and t_Activity_Title like '%{0}%'", name);
            DataSet _Ds = bll.GetListView(0, strWhere.ToString(), "t_Activity_Recommand desc,t_Activity_sDate desc");
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
            Maticsoft.Model.T_Activity model = new Maticsoft.Model.T_Activity();
            string userguid = context.Request["UserGuid"];
            string title = context.Request["Title"];
            string coverpic = context.Request["CoverPic"];
            string sdate = context.Request["sDate"];
            string edate = context.Request["eDate"];
            string province = context.Request["Province"];
            string city = context.Request["City"];
            string cityname = context.Request["CityName"];
            string street = context.Request["Street"];
            string instruction = context.Request["Instruction"];
            string limitperson = context.Request["LimitPerson"];
            string feetype = context.Request["FeeType"];
            string fee = context.Request["Fee"];
            string tel = context.Request["Tel"];
            string holder = context.Request["Holder"];
            string MethodType = context.Request["MethodType"];
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(instruction))
            {
                context.Response.Write("标题或活动介绍不能为空！");
                return;
            }
            model.t_User_Guid = userguid;
            model.t_Activity_Title = title;
            if (!string.IsNullOrEmpty(sdate))
            {
                model.t_Activity_sDate = DateTime.Parse(sdate);
            }
            else
            {
                context.Response.Write("开始时间不能为空！");
                return;
            }
            if (!string.IsNullOrEmpty(edate))
            {
                if (DateTime.Parse(edate) >= DateTime.Parse(sdate))
                {
                    model.t_Activity_eDate = DateTime.Parse(edate);
                }
                else
                {
                    context.Response.Write("结束时间不能大于开始时间！");
                    return;
                }
            }
            else
            {
                context.Response.Write("结束时间不能为空！");
                return;
            }
            if (!string.IsNullOrEmpty(province))
            {
                model.t_Activity_Province = int.Parse(province);
            }
            if (!string.IsNullOrEmpty(city))
            {
                model.t_Activity_City = int.Parse(city);
            }
            model.t_Activity_CityName = cityname;
            model.t_Activity_Street = street;
            model.t_Activity_Instruction = instruction;
            if (!string.IsNullOrEmpty(limitperson.ToString()))
            {
                model.t_Activity_LimitPerson = int.Parse(limitperson);
            }
            else
            {
                context.Response.Write("限制人数不能为空！");
                return;
            }

            model.t_Activity_FeeType = feetype;
            model.t_Activity_CoverPic = coverpic;
            model.t_Activity_Tel = tel;
            model.t_Activity_Holder = holder;
            if (!string.IsNullOrEmpty(fee.ToString()))
            {
                model.t_Activity_Fee = decimal.Parse(fee);
            }
            else
            {
                model.t_Activity_Fee = 0;
            }
            if (MethodType == "add")
            {
                model.t_DelState = 0;
                model.t_Activity_Recommand = 0;
                model.t_Activity_Audit = 0;
                model.Guid = Guid.NewGuid().ToString();
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
        /// <summary>
        /// 设置地理位置
        /// </summary>
        /// <param name="context"></param>
        private void SetMap(HttpContext context)
        {
            Maticsoft.Model.T_Activity model = new Maticsoft.Model.T_Activity();
            model.Guid = context.Request["Guid"];
            model.t_Activity_Latitude = context.Request["Wei"];
            model.t_Activity_Longitude = context.Request["Jing"];
            if (bll.SetMap(model))
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