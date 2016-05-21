using qch.core;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    /// <summary>
    /// 活动相关
    /// </summary>
    public class ActivityController : ApiController
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ActivityApplyService service = new ActivityApplyService();
        ActivityService activityService = new ActivityService();
        UserTalkService talkService = new UserTalkService();

        /// <summary>
        /// 活动详情
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object ApplyInfo(string guid)
        {
            try
            {
                var model = activityService.GetById(guid);
                if (model != null)
                {
                    //获取评论
                    var talk = talkService.GetAll(1, 999, model.Guid);
                    //报名列表
                    var bm = service.GetByActivityGuid(model.Guid);
                    //报名人数
                    int xy = 0;
                    if (bm != null)
                        xy = bm.Count();
                    var target = new
                    {
                        activityImg = model.t_Activity_CoverPic,
                        activityTitle = model.t_Activity_Title,
                        sponsor = model.t_Activity_Holder,
                        activityCost = model.t_Activity_Fee,
                        Telephone = model.t_Activity_Tel,
                        activityTime = model.t_Activity_sDate,
                        activityAddress = model.t_Activity_Street,
                        activityDetails = model.t_Activity_Instruction,
                        numTotal = model.t_Activity_LimitPerson,
                        numSigns = xy,
                        avatar = bm,
                        comments = talk
                    };
                    return target;
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// app端活动报名
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpPost]
        public Msg Apply(string guid, string phone)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "报名失败";
            try
            {
                log.Info("app活动报名开始------------------------------------");
                //生成报名凭证
                string proof = str.Ran(12);
                //生成凭证二维码
                string qrcode = qch.Infrastructure.QRcode.CreateCode_Simple(proof, "D:\\QCH2.0\\Attach\\Images\\");
                msg = service.Apply("", guid, phone, "", "", "", proof, qrcode);
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }

        /// <summary>
        /// 活动报名凭证
        /// </summary>
        /// <param name="guid">活动报名guid</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ProofModel ApplyProof(string guid)
        {
            var model = service.GetProof(guid);
            return model;
        }
        /// <summary>
        /// 报名凭证列表
        /// </summary>
        /// <param name="guid">用户guid</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IEnumerable<ProofModel> ProofList(string guid)
        {
            var model = service.GetProofList(guid);
            return model;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public PetaPoco.Page<ProofModel> ProofList(int page, int pagesize, string guid)
        {
            var model = service.GetProofList(page, pagesize, guid);
            return model;
        }
    }
}
