using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 商品业务层
    /// </summary>
    public class GoodsService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        GoodsRepository rp = new GoodsRepository();

        #region 商品资源
        /// <summary>
        /// 获取所有积分兑换商品
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T_Goods_Convert> GetAll()
        {
            try
            {
                return rp.GetAll();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public T_Goods_Convert GetDetail(string Guid)
        {
            try
            {
                return rp.GetDetail(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion
        #region 商品兑换记录资源
        /// <summary>
        /// 商品兑换
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="GoodsCode"></param>
        /// <param name="Cnee"></param>
        /// <returns></returns>
        public Msg CreateRecord(string UserGuid, string GoodsCode, string Cnee)
        {
            /*
             * 1、查询用户信息
             * 2、查询商品信息
             * 3、查询用户积分是否够用
             * 4、增加商品的兑换次数
             * 5、生成用户兑换记录
             * 6、扣除用户积分
             * 7、生成用户积分明细
             */
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "兑换失败";
            try
            {
                log.Info("-----------------------------------------商品兑换开始--------------------------------");
                log.Info("兑换人UserGuid=" + UserGuid);
                log.Info("兑换商品编码/guid=" + GoodsCode);
                log.Info("收货信息=" + Cnee);
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    db.BeginTransaction();
                    #region 验证
                    //查询用户信息
                    var user = db.SingleOrDefault<T_Users>(" where guid=@0 and t_delstate=0", new object[] { UserGuid });
                    if (user == null)
                    {
                        msg.Data = "未能获取到用户信息";
                        log.Info(msg.Data);
                        return msg;
                    }
                    //查询商品信息
                    var goods = db.SingleOrDefault<T_Goods_Convert>(" where guid=@0 or goodscode=@0 and t_delstate=0", new object[] { GoodsCode });
                    if (goods == null)
                    {
                        msg.Data = "未能获取到商品信息";
                        log.Info(msg.Data);
                        return msg;
                    }
                    //查询用户积分
                    var integral = db.SingleOrDefault<T_User_Integral>("select top 1 * from T_User_Integral where t_User_Guid=@0 and t_delstate=0 order by t_AddDate desc", new object[] { user.Guid });
                    if (integral == null)
                    {
                        msg.Data = "未能获取到商品信息";
                        log.Info(msg.Data);
                        return msg;
                    }
                    if (integral.t_UserIntegral_Reward < goods.t_Need_Integral)
                    {
                        msg.Data = "积分余额不足";
                        log.Info(msg.Data);
                        return msg;
                    }
                    var logs = db.SingleOrDefault<t_User_Cnee>(" where guid=@0 and t_delstate=0", new object[] { Cnee });
                    if (logs == null)
                    {
                        msg.Data = "未能获取到用户的收货地址";
                        log.Info(msg.Data);
                        return msg;
                    }
                    #endregion
                    //增加商品兑换次数
                    goods.t_Convert_Count += 1;
                    db.Save(goods);
                    //生成用户兑换记录                    
                    T_Goods_Convert_List list = new T_Goods_Convert_List
                    {
                        Guid = Guid.NewGuid().ToString(),
                        t_Cnee_Guid = Cnee,
                        t_Convert_CreateDate = DateTime.Now,
                        t_Convert_OrderNo = "",
                        t_DelState = 0,
                        t_Goods_Guid = GoodsCode,
                        t_Logistics_Company = "",
                        t_Logistics_Status = 0,
                        t_Logistics_WaybillNo = "",
                        t_User_Guid = UserGuid
                    };
                    db.Insert(list);
                    //扣积分，生成积分交易记录
                    T_User_Integral ui = new T_User_Integral
                    {
                        t_UserIntegral_Reward = integral.t_UserIntegral_Reward - goods.t_Need_Integral,
                        Guid = Guid.NewGuid().ToString(),
                        t_AddDate = DateTime.Now,
                        t_DelState = 0,
                        t_IntegralManager_Guid = "",
                        t_IntegralManager_PinYin = "duihuanshangpin",
                        t_Remark = "兑换商品减少",
                        t_User_Guid = UserGuid,
                        t_UserIntegral_ReduceReward = goods.t_Need_Integral,
                        t_UserIntergral_AddReward = 0
                    };
                    db.Insert(ui);
                    db.CompleteTransaction();
                }
                log.Info("-----------------------------------------商品兑换成功结束--------------------------------");
                msg.type = "success";
                msg.Data = "兑换成功";
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
        /// <summary>
        /// 获取某用户的兑换商品记录
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public IEnumerable<T_Goods_Convert_List> GetList(string UserGuid)
        {
            try
            {
                return rp.GetList(UserGuid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
