using qch.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Maticsoft.Common;
using System.Text;
using System.Data;

namespace api.Controllers
{
    public class UserController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Maticsoft.BLL.T_Users bll = new Maticsoft.BLL.T_Users();
        UserService userService = new UserService();
        FoucsService foucsService = new FoucsService();
        StyleService styleService = new StyleService();
        InvestService investService = new InvestService();
        AccountService accountService = new AccountService();
        VoucherService voucherService = new VoucherService();
        IntegralService integralService = new IntegralService();
        TopicService topicService = new TopicService();
        ProjectService projectService = new ProjectService();

        private readonly Maticsoft.BLL.T_Users bll = new Maticsoft.BLL.T_Users();
        private readonly Maticsoft.BLL.T_HistoryWork hwbll = new Maticsoft.BLL.T_HistoryWork();
        private readonly Maticsoft.BLL.T_User_Foucs foucsbll = new Maticsoft.BLL.T_User_Foucs();
        private readonly Maticsoft.BLL.T_Topic topicbll = new Maticsoft.BLL.T_Topic();
        private readonly Maticsoft.BLL.T_Project projectbll = new Maticsoft.BLL.T_Project();
        private readonly Maticsoft.BLL.T_Invest_Case icbll = new Maticsoft.BLL.T_Invest_Case();

        //获取类型
        private DataTable dtStyle = new Maticsoft.BLL.T_Style().GetList("").Tables[0];

        [HttpGet]
        public object GetUserView(string guid, string userGuid)
        {
            try
            {
                string strUser = string.Format("Guid='{0}'", guid);
                DataSet ds = bll.GetListView(0, strUser, "t_User_Date desc");
                return getUserViewJson(ds, userGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取用户json
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private string getUserViewJson(DataSet ds, string userGuid)
        {
            StringBuilder strJson = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                //获取关注数据源
                DataTable dtFoucs = new Maticsoft.BLL.T_User_Foucs().GetList(0, "t_DelState=0", "t_Date desc").Tables[0];
                strJson.Append("[{\"state\":\"true\",\"result\":[");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    #region 是否关注
                    string strFoucs = string.Format("t_DelState=0 and t_Focus_Guid='{0}' and t_User_Guid='{1}'", dr["Guid"].ToString(), userGuid);
                    DataSet dsFoucs = new Maticsoft.BLL.T_User_Foucs().GetList(strFoucs);
                    //默认未关注
                    string ifFoucs = "0";
                    if (dsFoucs != null && dsFoucs.Tables[0].Rows.Count > 0)
                    {
                        ifFoucs = "1";
                    }
                    #endregion
                    #region 粉丝
                    string strFCount = string.Format("select count(Guid) as fcount FROM T_User_Foucs where t_DelState=0 and t_Focus_Guid='{0}'", dr["Guid"].ToString());
                    DataSet dsfCount = new Maticsoft.BLL.CommonSql().GetDataBySql(strFCount);
                    //粉丝
                    int FCount = Maticsoft.Common.ExtendMethod.ToInt(dsfCount.Tables[0].Rows[0]["fcount"].ToString());
                    #endregion
                    #region 我关注的人
                    string strPCount = string.Format("select count(Guid) as pcount FROM T_User_Foucs where t_DelState=0 and t_User_Guid='{0}'", dr["Guid"].ToString());
                    DataSet dspCount = new Maticsoft.BLL.CommonSql().GetDataBySql(strPCount);
                    //我关注的其他
                    string strPCount1 = string.Format("select count(Guid) as pcount FROM T_Praise where t_User_Guid='{0}' and t_DelState=0 and t_Remark<>'创业圈动态关注'", dr["Guid"].ToString());
                    DataSet dspCount1 = new Maticsoft.BLL.CommonSql().GetDataBySql(strPCount1);

                    //我关注的人
                    int PCount = Maticsoft.Common.ExtendMethod.ToInt(dspCount.Tables[0].Rows[0]["pcount"].ToString()) + Maticsoft.Common.ExtendMethod.ToInt(dspCount1.Tables[0].Rows[0]["pcount"].ToString());


                    #endregion
                    #region  我最擅长
                    string strBest = "";
                    if (!string.IsNullOrEmpty(dr["t_User_Best"].ToString()))
                    {
                        string[] arrBest = dr["t_User_Best"].ToString().Split(';');

                        if (arrBest != null && arrBest.Length > 0)
                        {
                            StringBuilder strBestJson = new StringBuilder();
                            foreach (var tmp in arrBest)
                            {
                                if (!string.IsNullOrEmpty(tmp))
                                {
                                    strBestJson.Append("{\"BestID\":\"" + tmp + "\",\"BestName\":\"" + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString() + "\"},");
                                }
                            }
                            strBest = strBestJson.ToString().Substring(0, strBestJson.Length - 1);
                        }
                    }
                    #endregion
                    #region  关注领域
                    string strFoucsArea = "";
                    if (dr["t_User_Style"].ToString() == "3")
                    {
                        if (!string.IsNullOrEmpty(dr["t_User_FocusArea"].ToString()))
                        {
                            string[] arrFoucs = dr["t_User_FocusArea"].ToString().Split(';');
                            if (arrFoucs != null && arrFoucs.Length > 0)
                            {
                                StringBuilder strFoucsJson = new StringBuilder();
                                foreach (var tmp in arrFoucs)
                                {
                                    if (!string.IsNullOrEmpty(tmp))
                                    {
                                        strFoucsJson.Append("{\"FoucsID\":\"" + tmp + "\",\"FoucsName\":\"" + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString() + "\"},");
                                    }
                                }
                                strFoucsArea = strFoucsJson.ToString().Substring(0, strFoucsJson.Length - 1);
                            }
                        }
                    }
                    #endregion
                    #region  工作经历
                    string strHistoryWork = "";
                    if (dr["t_User_Style"].ToString() == "3")
                    {
                        string strHWWhere = string.Format("t_User_Guid='{0}' and t_DelState=0", dr["Guid"].ToString());
                        DataSet dsHW = hwbll.GetList(strHWWhere);
                        StringBuilder strHWJson = new StringBuilder();
                        if (dsHW != null && dsHW.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow drHW in dsHW.Tables[0].Rows)
                            {
                                string tmp = "";
                                if (!string.IsNullOrEmpty(drHW["t_eDate"].ToString()))
                                {
                                    tmp = drHW["t_eDate"].ToString();
                                }
                                strHWJson.Append("{\"Guid\":\"" + drHW["Guid"] + "\",\"t_User_Guid\":\"" + drHW["t_User_Guid"] + "\",\"t_sDate\":\"" + drHW["t_sDate"] + "\",\"t_eDate\":\"" + tmp + "\",\"t_Commpany\":\"" + drHW["t_Commpany"] + "\",\"t_Position\":\"" + drHW["t_Position"] + "\"},");
                            }
                            strHistoryWork = strHWJson.ToString().Substring(0, strHWJson.Length - 1);
                        }
                    }
                    #endregion
                    #region  关注项目
                    string strPriaseProject = "";
                    if (dr["t_User_Style"].ToString() == "2")
                    {
                        string strPriaseProjectWhere = string.Format(" t_DelState=0 and Guid in (select t_Associate_Guid from T_Praise where t_Remark='项目关注' and t_User_Guid='{0}')", dr["Guid"].ToString());
                        DataSet dsPriaseProject = projectbll.GetList(0, strPriaseProjectWhere, "t_AddDate desc");
                        StringBuilder strPPJson = new StringBuilder();
                        if (dsPriaseProject != null && dsPriaseProject.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow drPP in dsPriaseProject.Tables[0].Rows)
                            {
                                strPPJson.Append("{\"Guid\":\"" + drPP["Guid"] + "\",\"t_User_Guid\":\"" + drPP["t_User_Guid"] + "\",\"t_Project_ConverPic\":\"" + drPP["t_Project_ConverPic"] + "\"},");
                            }
                            strPriaseProject = strPPJson.ToString().Substring(0, strPPJson.Length - 1);
                        }
                    }
                    #endregion
                    #region  投资案例
                    string strInvestCase = "";
                    if (dr["t_User_Style"].ToString() == "2")
                    {
                        string strICWhere = string.Format("t_User_Guid='{0}' and t_DelState=0", dr["Guid"].ToString());
                        DataSet dsIC = icbll.GetList(strICWhere);
                        StringBuilder strICJson = new StringBuilder();
                        if (dsIC != null && dsIC.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow drIC in dsIC.Tables[0].Rows)
                            {
                                #region  投资阶段
                                string strCaseInvestPhase = "";
                                if (!string.IsNullOrEmpty(drIC["t_Invest_Phase"].ToString()))
                                {
                                    string[] arrCaseInvestPhase = drIC["t_Invest_Phase"].ToString().Split(';');

                                    if (arrCaseInvestPhase != null && arrCaseInvestPhase.Length > 0)
                                    {
                                        StringBuilder strCaseInvestPhaseJson = new StringBuilder();
                                        foreach (var tmp in arrCaseInvestPhase)
                                        {
                                            strCaseInvestPhaseJson.Append("{\"InvestPhaseID\":\"" + tmp + "\",\"InvestPhaseName\":\"" + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString() + "\"},");
                                        }
                                        strCaseInvestPhase = strCaseInvestPhaseJson.ToString().Substring(0, strCaseInvestPhaseJson.Length - 1);
                                    }
                                }
                                #endregion
                                strICJson.Append("{\"Guid\":\"" + drIC["Guid"] + "\",\"t_User_Guid\":\"" + drIC["t_User_Guid"] + "\",\"t_Invest_Project\":\"" + drIC["t_Invest_Project"] + "\",\"t_Invest_Phase\":\"" + drIC["t_Invest_Phase"] + "\",\"t_Invest_Date\":\"" + drIC["t_Invest_Date"] + "\",\"t_Invest_Pic\":\"" + drIC["t_Invest_Pic"] + "\",\"InvestPhase\":[" + strCaseInvestPhase + "]},");
                            }
                            strInvestCase = strICJson.ToString().Substring(0, strICJson.Length - 1);
                        }
                    }
                    #endregion
                    #region  投资领域
                    string strInvestArea = "";
                    if (dr["t_User_Style"].ToString() == "2")
                    {
                        if (!string.IsNullOrEmpty(dr["t_User_InvestArea"].ToString()))
                        {
                            string[] arrInvestArea = dr["t_User_InvestArea"].ToString().Split(';');
                            if (arrInvestArea != null && arrInvestArea.Length > 0)
                            {
                                StringBuilder strInvestAreaJson = new StringBuilder();
                                foreach (var tmp in arrInvestArea)
                                {
                                    strInvestAreaJson.Append("{\"InvestAreaID\":\"" + tmp + "\",\"InvestAreaName\":\"" + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString() + "\"},");
                                }
                                strInvestArea = strInvestAreaJson.ToString().Substring(0, strInvestAreaJson.Length - 1);
                            }
                        }
                    }
                    #endregion
                    #region  投资阶段
                    string strInvestPhase = "";
                    if (dr["t_User_Style"].ToString() == "2")
                    {
                        if (!string.IsNullOrEmpty(dr["t_User_InvestPhase"].ToString()))
                        {
                            string[] arrInvestPhase = dr["t_User_InvestPhase"].ToString().Split(';');
                            if (arrInvestPhase != null && arrInvestPhase.Length > 0)
                            {
                                StringBuilder strInvestPhaseJson = new StringBuilder();
                                foreach (var tmp in arrInvestPhase)
                                {
                                    if (!string.IsNullOrEmpty(tmp))
                                    {
                                        strInvestPhaseJson.Append("{\"InvestPhaseID\":\"" + tmp + "\",\"InvestPhaseName\":\"" + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString() + "\"},");
                                    }
                                }
                                strInvestPhase = strInvestPhaseJson.ToString().Substring(0, strInvestPhaseJson.Length - 1);
                            }
                        }
                    }
                    #endregion
                    #region  现阶段需求
                    string strNowNeed = "";
                    if (!string.IsNullOrEmpty(dr["t_User_NowNeed"].ToString()))
                    {
                        string[] arrNowNeed = dr["t_User_NowNeed"].ToString().Split(';');
                        if (arrNowNeed != null && arrNowNeed.Length > 0)
                        {
                            StringBuilder strNowNeedJson = new StringBuilder();
                            foreach (var tmp in arrNowNeed)
                            {
                                if (!string.IsNullOrEmpty(tmp))
                                {
                                    strNowNeedJson.Append("{\"NowNeedID\":\"" + tmp + "\",\"NowNeedName\":\"" + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString() + "\"},");
                                }
                            }
                            strNowNeed = strNowNeedJson.ToString().Substring(0, strNowNeedJson.Length - 1);
                        }
                    }
                    #endregion
                    #region  创业意向
                    string strIntention = "";
                    if (!string.IsNullOrEmpty(dr["t_User_Intention"].ToString()))
                    {
                        string[] arrIntention = dr["t_User_Intention"].ToString().Split(';');
                        if (arrIntention != null && arrIntention.Length > 0)
                        {
                            StringBuilder strIntentionJson = new StringBuilder();
                            foreach (var tmp in arrIntention)
                            {
                                if (!string.IsNullOrEmpty(tmp))
                                {
                                    strIntentionJson.Append("{\"IntentionID\":\"" + tmp + "\",\"IntentionName\":\"" + dtStyle.Select("Id=" + tmp)[0]["t_Style_Name"].ToString() + "\"},");
                                }
                            }
                            strIntention = strIntentionJson.ToString().Substring(0, strIntentionJson.Length - 1);
                        }
                    }
                    #endregion
                    #region  动态
                    string strTopic = "";

                    string strTopicWhere = string.Format("t_User_Guid='{0}' and t_DelState=0", dr["Guid"].ToString());
                    DataSet dsTopic = topicbll.GetList(0, strTopicWhere, "t_Date desc");
                    StringBuilder strTopicJson = new StringBuilder();
                    if (dsTopic != null && dsTopic.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drTopic in dsTopic.Tables[0].Rows)
                        {
                            #region  获取关联图片
                            string pic = "";
                            DataRow[] drPic = Maticsoft.Web.AssociatePic.DTPics().Select("t_Associate_Guid='" + drTopic["Guid"] + "'");
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
                            strTopicJson.Append("{\"Guid\":\"" + drTopic["Guid"] + "\",\"t_Topic_Contents\":\"" + drTopic["t_Topic_Contents"] + "\",\"t_Topic_City\":\"" + drTopic["t_Topic_City"] + "\",\"t_Topic_Address\":\"" + drTopic["t_Topic_Address"] + "\",\"t_Date\":\"" + drTopic["t_Date"] + "\",\"pic\":[" + pic + "]},");
                        }
                        strTopic = strTopicJson.ToString().Substring(0, strTopicJson.Length - 1);
                    }
                    #endregion
                    #region 获取他的项目
                    string strUserProject = string.Format("t_User_Guid='{0}' and t_DelState=0", dr["Guid"].ToString());
                    DataSet dsUserProject = projectbll.GetList(0, strUserProject, "t_AddDate desc");

                    //他的项目信息json
                    string UserProject = "";
                    if (dsUserProject != null && dsUserProject.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strUserProjectJson = new StringBuilder();
                        foreach (DataRow drtmp in dsUserProject.Tables[0].Rows)
                        {
                            strUserProjectJson.Append("{\"Guid\":\"" + drtmp["Guid"] + "\",\"ProjectName\":\"" + drtmp["t_Project_Name"] + "\",\"t_Project_ConverPic\":\"" + drtmp["t_Project_ConverPic"] + "\",\"t_Project_OneWord\":\"" + drtmp["t_Project_OneWord"] + "\"},");
                        }
                        UserProject = strUserProjectJson.ToString().Substring(0, strUserProjectJson.Length - 1);
                    }
                    #endregion
                    strJson.Append("{\"Guid\":\"" + dr["Guid"] + "\",\"t_User_LoginId\":\"" + dr["t_User_LoginId"] + "\",\"t_User_RealName\":\"" + dr["t_User_RealName"] + "\",\"t_User_Mobile\":\"" + dr["t_User_Mobile"] + "\",\"t_User_Pic\":\"" + dr["t_User_Pic"] + "\",\"t_User_Sex\":\"" + dr["t_User_Sex"] + "\",\"t_User_Birth\":\"" + dr["t_User_Birth"] + "\",\"t_User_Style\":\"" + dr["t_User_Style"] + "\",\"t_User_Date\":\"" + dr["t_User_Date"] + "\",\"t_User_BusinessCard\":\"" + dr["t_User_BusinessCard"] + "\",\"t_User_Commpany\":\"" + dr["t_User_Commpany"] + "\",\"t_User_Position\":\"" + dr["t_User_Position"] + "\",\"PositionName\":\"" + dr["PositionName"] + "\",\"t_User_Complete\":\"" + dr["t_User_Complete"] + "\",\"t_User_City\":\"" + dr["t_User_City"] + "\",\"t_User_Email\":\"" + dr["t_User_Email"] + "\",\"t_User_Remark\":\"" + dr["t_User_Remark"] + "\",\"t_User_InvestArea\":\"" + dr["t_User_InvestArea"] + "\",\"t_User_InvestPhase\":\"" + dr["t_User_InvestPhase"] + "\",\"t_User_InvestMoney\":\"" + dr["t_User_InvestMoney"] + "\",\"t_User_Best\":\"" + dr["t_User_Best"] + "\",\"t_User_FocusArea\":\"" + dr["t_User_FocusArea"] + "\",\"ifFoucs\":\"" + ifFoucs + "\",\"FCount\":\"" + FCount + "\",\"PCount\":\"" + PCount + "\",\"t_BackPic\":\"" + dr["t_BackPic"] + "\",\"t_UserStyleAudit\":\"" + dr["t_UserStyleAudit"] + "\",\"Best\":[" + strBest + "],\"FoucsArea\":[" + strFoucsArea + "],\"HistoryWork\":[" + strHistoryWork + "],\"UserProject\":[" + UserProject + "],\"InvestCase\":[" + strInvestCase + "],\"PriaseProject\":[" + strPriaseProject + "],\"Topic\":[" + strTopic + "],\"InvestArea\":[" + strInvestArea + "],\"InvestPhase\":[" + strInvestPhase + "],\"NowNeed\":[" + strNowNeed + "],\"Intention\":[" + strIntention + "],\"t_RongCloud_Token\":[" + dr["t_RongCloud_Token"] + "]},");
                }
                return strJson.ToString().Substring(0, strJson.Length - 1) + "]}]";
            }
            else
            {
                strJson.Append("[{\"state\":\"false\",\"result\":\"暂无数据！\"}]");
                return strJson.ToString();
            }
        }
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object UserInfo(string UserGuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserGuid))
                    return null;
                var model = userService.GetById(UserGuid);
                if (model == null)
                    return null;
                //用户创业币
                var cyb = accountService.GetBalance(UserGuid);
                //积分
                var integral = integralService.GetIntegral(UserGuid);
                //优惠券
                long voucherCount = 0;
                var voucher = voucherService.GetAlluvByUser(1, 9999, UserGuid);
                if (voucher != null)
                    voucherCount = voucher.TotalItems;
                //直推
                var zhijie = userService.GetReferral1(UserGuid);
                //间推
                var jianjie = userService.GetReferral2(UserGuid);
                var target = new
                {
                    RegDate = model.t_User_Date, //注册日期
                    Avator = model.t_User_Pic, //头像
                    Name = model.t_User_RealName,//真实姓名
                    Referral1 = zhijie,//直接推荐人（数量）
                    Referral2 = jianjie,//简介推荐人（数字量）
                    VoucherCount = voucherCount,//优惠券数量
                    Integral = integral,//积分余额
                    Balance = cyb,//创业币余额
                    Phone = model.t_User_LoginId//用户手机号
                };
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public object GetUser(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                var model = userService.GetById(Guid);
                if (model == null)
                    return null;
                //关注人数
                int xy = 0;
                var dy = foucsService.GetFoucsFroMe(model.Guid);
                if (dy != null)
                    xy = dy.Count();
                //粉丝数量
                int xy2 = 0;
                var f = foucsService.GetFoucs(model.Guid);
                if (f != null)
                    xy2 = f.Count();
                //投资领域

                //投资阶段

                //投资案例
                var investCast = investService.GetByUser(model.Guid);
                var target = new
                {
                    username = model.t_User_RealName,
                    userAvatar = model.t_User_Pic,
                    job = model.t_User_Position,
                    company = model.t_User_Commpany,
                    city = model.t_User_City,
                    numFollow = xy,
                    fans = f,
                    dynamicThumbnail = "",
                    dynamicDate = model.t_User_Date,
                    description = model.t_User_Remark,
                    InvestArea = styleService.GetByIds(model.t_User_InvestArea),
                    InvestPhase = styleService.GetByIds(model.t_User_InvestPhase),
                    InvestMoney = model.t_User_InvestMoney,
                    InvestCase = investCast
                };
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Msg Get()
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "未获取到值";
            try
            {
                log.Info(msg.Data);
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }

        /// <summary>
        /// 获取对象实体
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //public Maticsoft.Model.T_Users GetModel(string Guid)
        //{
        //    var model = bll.GetModel(Guid);
        //    return model;
        //}
    }
}
