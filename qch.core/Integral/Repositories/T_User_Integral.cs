using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 积分映射表
    /// </summary>
    [TableName("T_User_Integral")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_User_Integral
    {
        #region Model
        private string _guid;
        private string _t_user_guid;
        private string _t_integralmanager_guid;
        private string _t_integralmanager_pinyin;
        private int? _t_userintergral_addreward;
        private int? _t_userintegral_reducereward;
        private int? _t_userintegral_reward;
        private string _t_remark;
        private DateTime? _t_adddate;
        private int? _t_delstate = 0;
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public string Guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 用户主键
        /// </summary>
        /// 
        [Column]
        public string t_User_Guid
        {
            set { _t_user_guid = value; }
            get { return _t_user_guid; }
        }
        /// <summary>
        /// 关联主键
        /// </summary>
        /// 
        [Column]
        public string t_IntegralManager_Guid
        {
            set { _t_integralmanager_guid = value; }
            get { return _t_integralmanager_guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public string t_IntegralManager_PinYin
        {
            set { _t_integralmanager_pinyin = value; }
            get { return _t_integralmanager_pinyin; }
        }
        /// <summary>
        /// 积分增加
        /// </summary>
        /// 
        [Column]
        public int? t_UserIntergral_AddReward
        {
            set { _t_userintergral_addreward = value; }
            get { return _t_userintergral_addreward; }
        }
        /// <summary>
        /// 积分减少
        /// </summary>
        /// 
        [Column]
        public int? t_UserIntegral_ReduceReward
        {
            set { _t_userintegral_reducereward = value; }
            get { return _t_userintegral_reducereward; }
        }
        /// <summary>
        /// 积分余额
        /// </summary>
        /// 
        [Column]
        public int? t_UserIntegral_Reward
        {
            set { _t_userintegral_reward = value; }
            get { return _t_userintegral_reward; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        /// 
        [Column]
        public string t_Remark
        {
            set { _t_remark = value; }
            get { return _t_remark; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        /// 
        [Column]
        public DateTime? t_AddDate
        {
            set { _t_adddate = value; }
            get { return _t_adddate; }
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        /// 
        [Column]
        public int? t_DelState
        {
            set { _t_delstate = value; }
            get { return _t_delstate; }
        }
        #endregion Model
    }
}
