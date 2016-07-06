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
        HistoryWorkService historyWork = new HistoryWorkService();
        ProjectService projectService = new ProjectService();

        /// <summary>
        /// 设置用户登录票证
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public Msg SetAuthCookie(string UserGuid)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "设置登录票证失败";
            var user = userService.GetDetail(UserGuid);
            if (user != null)
            {
                userService.SetAuthCookie(new qch.Models.UserLoginModel
                {
                    LoginName = user.t_User_LoginId,
                    LoginPwd = Senparc.Weixin.MP.Sample.CommonService.ToolHelper.createNonceStr(),
                    SafeCode = Senparc.Weixin.MP.Sample.CommonService.ToolHelper.createNonceStr()
                });
                msg.type = "success";
                msg.Data = "设置登录票证成功";
            }
            log.Info(msg.Data);
            return msg;
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
                //工作经历
                var work = historyWork.GetByUser(model.Guid);
                //关注人数
                int xy = foucsService.GetMyFoucs(model.Guid);
                //粉丝数量
                int xy2 = 0;
                var f = foucsService.GetFoucs(model.Guid);
                if (f != null)
                    xy2 = f.Count();
                //第一条动态
                var top1Topic = topicService.GetTop1(model.Guid);
                //最近发布的项目
                var project = projectService.GetTop1(model.Guid);
                //投资案例
                var investCast = investService.GetByUser(model.Guid);
                if (investCast != null)
                {
                    foreach (var item in investCast)
                    {
                        item.InvestPhase = styleService.GetByIds(item.t_Invest_Phase);
                    }
                    //investCast.Select(o => o.InvestPhase = styleService.GetByIds(o.t_Invest_Phase));
                }
                var target = new
                {
                    t_User_Style = model.t_User_Style,  //用户类型
                    t_User_RealName = model.t_User_RealName,  //姓名
                    t_User_Pic = model.t_User_Pic,   //头像
                    t_User_Position = styleService.GetByIds(model.t_User_Position),  //职位
                    t_User_Commpany = model.t_User_Commpany,  //所在公司
                    t_User_City = model.t_User_City,  //城市
                    numFollow = xy, //关注
                    fans = xy2,  //粉丝
                    dynamicThumbnail = "",
                    t_User_NowNeed = styleService.GetByIds(model.t_User_NowNeed),  //现阶段需求
                    t_User_Intention = styleService.GetByIds(model.t_User_Intention), //创业意向
                    t_User_Date = model.t_User_Date,  //注册日期
                    t_User_Remark = model.t_User_Remark,  //一句话描述
                    t_User_InvestArea = styleService.GetByIds(model.t_User_InvestArea),  //投资领域
                    t_User_InvestPhase = styleService.GetByIds(model.t_User_InvestPhase),  //投资阶段
                    t_User_InvestMoney = model.t_User_InvestMoney, //投资金额
                    t_User_FocusArea = styleService.GetByIds(model.t_User_FocusArea),  //关注领域
                    t_User_Best = styleService.GetByIds(model.t_User_Best),  //我最擅长
                    InvestCase = investCast,  //投资案例
                    topic = top1Topic,  //动态
                    project = project,  //项目
                    historyWork = work  //工作经历
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
