using PetaPoco;
using qch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qch.Repositories
{
    #region weixinjsapi
     
    [TableName("T_Weixin_JsApi")]
    [PrimaryKey("Id")]
    [ExplicitColumns]
    public partial class T_Weixin_JsApi
    {
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public int Id { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        [Column]
        public string Timestamp { get; set; }
        /// <summary>
        /// 随机数
        /// </summary>
        [Column]
        public string Noncestr { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [Column]
        public string Signature { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Jsapi_ticket { get; set; }
        /// <summary>
        /// 微信公众平台Id
        /// </summary>
        [Column]
        public string AppId { get; set; }
        /// <summary>
        /// 授权token
        /// </summary>
        [Column]
        public string access_token { get; set; }
        /// <summary>
        /// 调用jsapi的页面
        /// </summary>
        [Column]
        public string PageName { get; set; }
    }
    #endregion
    public class JsapiRepository : Repository<T_Weixin_JsApi>
    {
        Repository<JsapiModel> rp = new Repository<JsapiModel>();
        /// <summary>
        /// 写入缓存记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(JsapiModel model)
        {
            try
            {
                return (int)rp.Insert(model) > 0 ? true : false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool Del(JsapiModel model)
        {
            try
            {
                return (int)rp.Delete(model) > 0 ? true : false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        /// <summary>
        /// 编辑缓存记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(JsapiModel model)
        {
            try
            {
                return (int)rp.Update(model) > 0 ? true : false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        /// <summary>
        /// 编辑时用，根据Id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsapiModel GetById(int id)
        {
            try
            {
                string sql = "select top 1 *  from xft_weixin_jsapi where id=@0 order by id desc";
                return rp.Get(sql, new object[] { id });
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// 读取缓存信息
        /// </summary>
        /// <returns></returns>
        public JsapiModel Get(string PageName)
        {
            try
            {
                string sql = "select top 1 *  from xft_weixin_jsapi where PageName=@0 order by id desc";
                return rp.Get(sql, new object[] { PageName });
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}