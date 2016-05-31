using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 优惠券业务层
    /// </summary>
    public class VoucherService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        VoucherRepository rp = new VoucherRepository();

        #region 用户优惠券业务
        /// <summary>
        /// 获取某用户的某种类型的优惠券
        /// </summary>
        /// <param name="Guid"></param>
        /// <param name="voucherType"></param>
        /// <returns></returns>
        public VoucherTypeModel GetVoucherByUser(string Guid, int voucherType)
        {
            try
            {
                string xy = "";
                int xy2 = 0;
                using (var db = new PetaPoco.Database(DbConfig.qch))
                {
                    string sql = "select a.T_Voucher_Type as VoucherTypeText from T_Voucher as a left join T_User_Voucher as b on a.guid=b.T_Voucher_Guid where b.T_User_Guid=@0 and a.T_Voucher_Type=@1";
                    var list = db.Query<VoucherModel>(sql, new object[] { Guid, voucherType });
                    if (list != null && list.Count() > 0)
                    {
                        foreach (var item in list.ToList())
                        {
                            xy = item.VoucherTypeText;
                            if (!string.IsNullOrWhiteSpace(xy))
                                break;
                        }
                        xy2 = list.Count();
                    }
                }
                var target = new VoucherTypeModel
                {
                    VoucherType = xy,
                    VoucherCount = xy2
                };
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        #endregion
        #region 优惠券基础业务
        /// <summary>
        /// 根据全拼获取
        /// </summary>
        /// <param name="Pinyin"></param>
        /// <returns></returns>
        public VoucherModel GetByAction(string Pinyin)
        {
            try
            {
                return rp.GetByAction(Pinyin);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// getbyid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public VoucherModel GetById(string Guid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Guid))
                    return null;
                return rp.GetById(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Msg Save(VoucherModel model)
        {
            Msg msg = new Msg();
            msg.type = "error";
            msg.Data = "操作失败";
            try
            {
                if (model == null)
                    return msg;
                var tt = GetById(model.Guid);
                if (tt != null)
                {
                    //model.t_News_Date = DateTime.Now;
                    if (rp.Edit(model))
                    {
                        msg.type = "success";
                        msg.Data = "保存成功";
                    }
                }
                else
                {
                    model.Guid = Guid.NewGuid().ToString();
                    model.T_CreateDate = DateTime.Now;
                    model.T_sDate = DateTime.Now;
                    if (rp.Add(model))
                    {
                        msg.type = "success";
                        msg.Data = "新增成功";
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return msg;
            }
        }
        /// <summary>
        /// 更改删除状态
        /// </summary>
        /// <param name="Guid"></param>
        /// <param name="DelState"></param>
        /// <returns></returns>
        public bool EditStyle(string Guid, int DelState)
        {
            try
            {
                var model = GetById(Guid);
                if (model == null)
                    return false;
                model.T_DelState = DelState;
                return rp.Edit(model);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        #endregion
    }
}
