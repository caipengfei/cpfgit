using qch.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Maticsoft.Common;
using Maticsoft.Model;

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
