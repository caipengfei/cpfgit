using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 动态圈
    /// </summary>
    public class TopicsModel
    {
        public string CodeMsg { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int TalkCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public string t_User_Guid { get; set; }
        /// <summary>
        /// 发布人类型
        /// </summary>
        public int t_User_Style { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int t_UserStyleAudit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int t_Topic_Top { get; set; }
        /// <summary>
        /// 活动前缀
        /// </summary>
        public string Prefix
        {
            get
            {
                if (t_Topic_Top == 999)
                    return "#寄语中国创客有奖互动#";
                else
                    return "";
            }
        }
        /// <summary>
        /// 是否点赞
        /// </summary>
        public bool IfPraise { get; set; }
        /// <summary>
        /// 动态发布时间
        /// </summary>
        public DateTime t_Date { get; set; }
        /// <summary>
        /// 动态内容
        /// </summary>
        public string t_Topic_Contents { get; set; }
        /// <summary>
        /// 动态城市
        /// </summary>
        public string t_Topic_City { get; set; }
        /// <summary>
        /// 动态地址
        /// </summary>
        public string t_Topic_Address { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public string t_User_RealName { get; set; }
        /// <summary>
        /// 发布人头像
        /// </summary>
        public string t_User_Pic { get; set; }
        /// <summary>
        /// 创业意向
        /// </summary>
        public string t_User_Intention { get; set; }
        /// <summary>
        /// 现阶段需求
        /// </summary>
        public string t_User_NowNeed { get; set; }
        /// <summary>
        /// 我最擅长
        /// </summary>
        public string t_User_Best { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string t_User_Position { get; set; }
        /// <summary>
        /// 所在公司
        /// </summary>
        public string t_User_Commpany { get; set; }
        public IEnumerable<UserPraise> UserPraise { get; set; }
        public IEnumerable<PicModel> Pics { get; set; }
        public IEnumerable<TopicTalkModel> talk { get; set; }
        /// <summary>
        /// 创业意向
        /// </summary>
        public IEnumerable<SelectStyle> t_User_Intention_Text { get; set; }
        /// <summary>
        /// 现阶段需求
        /// </summary>
        public IEnumerable<SelectStyle> t_User_NowNeed_Text { get; set; }
        /// <summary>
        /// 我最擅长
        /// </summary>
        public IEnumerable<SelectStyle> t_User_Best_Text { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public IEnumerable<SelectStyle> t_User_Position_Text { get; set; }
    }
}
