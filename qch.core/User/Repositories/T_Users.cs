using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 用户映射表
    /// </summary>
    [TableName("T_Users")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_Users
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column]
        public string Guid { get; set; }
        /// <summary>
        /// 登录帐号
        /// </summary>
        [Column]
        public string t_User_LoginId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Column]
        public string t_User_Pwd { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Column]
        public string t_User_RealName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Column]
        public string t_User_NickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Column]
        public string t_User_Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        [Column]
        public DateTime t_User_Birth { get; set; }
        /// <summary>
        /// 用户手机
        /// </summary>
        [Column]
        public string t_User_Mobile { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [Column]
        public string t_User_Pic { get; set; }
        /// <summary>
        /// 用户类型
        /// 1：创客；
        /// 2：投资人；
        /// 3：合伙人；
        /// 8：微信用户
        /// </summary>
        [Column]
        public int t_User_Style { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Column]
        public DateTime t_User_Date { get; set; }
        /// <summary>
        /// 名片图片
        /// </summary>
        [Column]
        public string t_User_BusinessCard { get; set; }
        /// <summary>
        /// 所在公司
        /// </summary>
        [Column]
        public string t_User_Commpany { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        [Column]
        public string t_User_Position { get; set; }
        /// <summary>
        /// 城市区域
        /// </summary>
        [Column]
        public string t_User_City { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Column]
        public string t_User_Email { get; set; }
        /// <summary>
        /// 一句话说明
        /// </summary>
        [Column]
        public string t_User_Remark { get; set; }
        /// <summary>
        /// 投资金额
        /// </summary>
        [Column]
        public string t_User_InvestMoney { get; set; }
        /// <summary>
        /// 投资领域
        /// </summary>
        [Column]
        public string t_User_InvestArea { get; set; }
        /// <summary>
        /// 投资阶段
        /// </summary>
        [Column]
        public string t_User_InvestPhase { get; set; }
        /// <summary>
        /// 关注领域
        /// </summary>
        [Column]
        public string t_User_FocusArea { get; set; }
        /// <summary>
        /// 擅长
        /// </summary>
        [Column]
        public string t_User_Best { get; set; }
        /// <summary>
        /// 是否完善
        /// </summary>
        [Column]
        public int t_User_Complete { get; set; }
        /// <summary>
        /// 第三方登录返回值
        /// </summary>
        [Column]
        public string t_User_ThreeLogin { get; set; }
        /// <summary>
        /// 融云Token
        /// </summary>
        [Column]
        public string t_RongCloud_Token { get; set; }
        /// <summary>
        /// 安卓Rid
        /// </summary>
        [Column]
        public string t_Andriod_Rid { get; set; }
        /// <summary>
        /// IOSRid
        /// </summary>
        [Column]
        public string t_IOS_Rid { get; set; }
        /// <summary>
        /// 删除状态
        /// 0：正常；1：删除
        /// </summary>
        [Column]
        public int t_DelState { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        [Column]
        public int t_Recommend { get; set; }
        /// <summary>
        /// 推荐人
        /// </summary>
        [Column]
        public string t_ReommUser { get; set; }
    }
}
