using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Maticsoft.Common;

namespace Maticsoft.Web.QCH_Style
{
    /// <summary>
    /// Ajax_Style 的摘要说明
    /// </summary>
    public class Ajax_Style : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        private readonly BLL.T_Style bll = new BLL.T_Style();
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
                case "StyleData":
                    StyleData(context);
                    break;
                case "StyleDataByIds":
                    StyleDataByIds(context);
                    break;
                case "ClearCache":
                    ClearCache(context);
                    break;
            }
            context.Response.End();
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="context"></param>
        private void ClearCache(HttpContext context)
        {
            Common.CacheHelper.Remove("styleCache");
            context.Response.Write("ok");
            context.Response.Flush();
        }
        /// <summary>
        /// 获取类型结构数据
        /// </summary>
        /// <param name="syncDataRegionCode"></param>
        /// <param name="currentzw"></param>
        public void StyleDataByIds(HttpContext context)
        {
            string IDs = context.Request["IDs"];
            string strWhere = string.Format("t_DelState =0 and Id in ({0})", IDs);
            DataTable dt = new BLL.T_Style().GetList(0, strWhere, "t_SortIndex ").Tables[0];
            StringBuilder rs = new StringBuilder();
            if (dt.Rows.Count > 1)
            {
                rs.Append("[");
                foreach (DataRow dr in dt.Rows)
                {
                    rs.Append("{\"id\":\"" + dr["Id"] + "\",\"text\":\"" + dr["t_Style_Name"] + "\"");
                    rs.Append("},");
                }
                rs = rs.Remove(rs.Length - 1, 1);
                rs.Append("]");
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rs.Append("{\"id\":\"" + dr["Id"] + "\",\"text\":\"" + dr["t_Style_Name"] + "\"");
                    rs.Append("},");
                }
                rs = rs.Remove(rs.Length - 1, 1);
            }
            context.Response.Write(rs);
            context.Response.Flush();
        }
        /// <summary>
        /// 获取类型结构数据
        /// </summary>
        /// <param name="syncDataRegionCode"></param>
        /// <param name="currentzw"></param>
        public void StyleData(HttpContext context)
        {
            DataTable dt = new BLL.T_Style().GetList(0, "t_DelState =0", "t_SortIndex ").Tables[0];
            string json = string.Empty;
            json = GetStyleTree(dt);
            context.Response.Write(json);
            context.Response.Flush();
        }
        //返回json串
        private string GetStyleTree(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder rs = new StringBuilder();
                rs.Append("[");
                foreach (DataRow dr in dt.Rows)
                {
                    rs.Append("{\"id\":\"" + dr["Id"] + "\",\"text\":\"" + dr["t_Style_Name"] + "\",\"pid\":\"" + dr["t_fId"] + "\"");
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
        /// 添加或修改
        /// </summary>
        private void AddOrModify(HttpContext context)
        {
            Maticsoft.Model.T_Style model = new Maticsoft.Model.T_Style();
            string name = context.Request["Name"];
            string remark = context.Request["Remark"];
            string index = context.Request["Index"];
            string typeID = context.Request["TypeID"];
            string pic = context.Request["Pic"];
            string fId = context.Request["fId"];
            string MethodType = context.Request["methodType"];
            if (bll.IsExist(name, fId))
            {
                context.Response.Write("该类型已存在！"); 
                return;
            }
            if (string.IsNullOrEmpty(name))
                return;
            if (!string.IsNullOrEmpty(index))
            {
                model.t_SortIndex = int.Parse(index);
            }
            else
            {
                model.t_SortIndex = 1;
            }
            model.t_Style_Name = name;
            model.t_Style_Remark = remark.Replace("\n", "===").Replace("\"", "“"); ;
            model.t_Style_Pic = pic;
            if (!string.IsNullOrEmpty(fId))
            {
                model.t_fId = int.Parse(fId);
            }
            else
            {
                model.t_fId = 0;
            }
            model.t_DelState = 0;
            if (MethodType == "add")//添加
            {
                model.t_AddDate = DateTime.Now;
                model.t_AddBy = context.Session["CurrentUserID"].ToString();
                if (bll.Add(model)>0)
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
            else if (MethodType == "modify")//修改 
            {
                if (!string.IsNullOrEmpty(context.Request["Id"]))
                {
                    model.Id = int.Parse(context.Request["Id"]);
                    model.t_ModifydDate = DateTime.Now;
                    model.t_ModifyBy = context.Session["CurrentUserID"].ToString();
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
                else
                {
                    context.Response.Write("fail");
                    context.Response.Flush();
                }
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
            string stylevalue = context.Request["stylevalue"];
            strWhere.AppendFormat(" t_DelState =0 and t_Style_Name like '%{0}%'", name);
            if (!string.IsNullOrEmpty(stylevalue))
            {
                strWhere.AppendFormat(" and t_fId={0}", stylevalue);
            }
            DataSet _Ds = bll.GetListView(0, strWhere.ToString(), "t_SortIndex,t_AddDate desc");
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
        private DataTable dt = new BLL.T_Style().GetList("t_DelState=0").Tables[0];
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        private void DeleteData(HttpContext context)
        {
            string tmpguids = context.Request["ID"];
            string[] idList = tmpguids.Split(',');
            string ids = "";
            foreach (string id in idList)
            {
                DataRow[] dr = dt.Select("t_fId=" + id);
                if (dr != null && dr.Length > 0)
                {

                }
                else
                {
                    ids = ids + id + ",";
                }
            }
            ids = ids.Substring(0, ids.Length - 1);
            bool ifSuc = bll.Del(ids);
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