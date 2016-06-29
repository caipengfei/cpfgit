using qch.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace qch.core
{
    /// <summary>
    /// 推送业务层
    /// </summary>
    public class JPushService
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //string ApiKey = "c5769e566eca15eac9f5bbc3";//Android ApiKey
        //string APIMasterSecret = "db5d2b8984d337584f343c56";//Android密码

        public static void push(string rid, string content)
        {
            try
            {
                #region 客户推送并存储推送信息
                //string strRid = "13065ffa4e0b7bf1e3b";
                //string mes = string.Format("您推荐的【{0}】已加入青创汇，您获得了{1}积分奖励！", "void", 20);
                //string message = "{\"platform\": \"all\",\"audience\" : {\"registration_id\":[" + strRid + "]},\"notification\": { \"android\": { \"alert\": \"" + mes + "\",\"title\": \"" + mes + "\",\"builder_id\": 1,\"extras\": {\"Guid\": \"" + "" + "\",\"type\": \"message\"}}, \"ios\": {\"alert\": \"" + mes + "\",\"sound\": \"default\",\"badge\": \"+1\", \"extras\": {\"Guid\": \"" + "" + "\",\"type\": \"project\"}}},\"options\": { \"time_to_live\": 259200,\"apns_production\": false}}";


                string msg = "{\"platform\": \"all\",\"audience\" : {\"registration_id\":[\"" + rid + "\"]},\"notification\": { \"android\": { \"alert\": \"" + content + "\",\"title\": \"" + content + "\",\"builder_id\": 1,\"extras\": {\"Guid\": \"98d51a31-47c5-4743-bde6-5e1a8d86f89e\",\"type\": \"activity\"}}, \"ios\": {\"alert\": \"" + content + "\",\"sound\": \"default\",\"badge\": \"+1\", \"extras\": {\"Guid\": \"98d51a31-47c5-4743-bde6-5e1a8d86f89e\",\"type\": \"activity\"}}},\"options\": { \"time_to_live\": 259200,\"apns_production\": false}}";
                //string msg = "{\"platform\": \"all\",\"audience\" : {\"registration_id\" : [\"040526ea1ef\"]},\"notification\" : {\"alert\" : \"Hi, JPush!\",\"android\" : {},\"ios\" : {\"extras\" : {\"newsid\" :321}}}}";
                JPush.PushMsg(msg);
                //存入数据库
                //T_HistoryPush modelpush = new T_HistoryPush()
                //{
                //    Guid = Guid.NewGuid().ToString(),
                //    t_Alert = “”,
                //    t_Associate_Guid = "1114a89792a99147781",
                //    t_Date = DateTime.Now,
                //    t_DelState = 0,
                //    t_Title = mes,
                //    t_Type = "message",
                //    t_User_Guid = "a26f0db6-6e39-431e-a375-d61e90a036bd"
                //};
                //db.Insert(modelpush);
                #endregion

                //[WebMethod]
                //public string JPush()
                //{
                //    string ApiKey = ConfigurationManager.AppSettings["ApiKey"].ToString();//Android ApiKey
                //    string APIMasterSecret = ConfigurationManager.AppSettings["APIMasterSecret"].ToString();//Android密码
                //    string rid = "13065ffa4e0b7bf1e3b";
                //    string msg = "{\"platform\": \"all\",\"audience\" : {\"registration_id\":[\"" + rid + "\"]},\"notification\": { \"android\": { \"alert\": \"测试推送！\",\"title\": \"测试推送！\",\"builder_id\": 1,\"extras\": {\"Guid\": \"98d51a31-47c5-4743-bde6-5e1a8d86f89e\",\"type\": \"activity\"}}, \"ios\": {\"alert\": \"测试推送\",\"sound\": \"default\",\"badge\": \"+1\", \"extras\": {\"Guid\": \"98d51a31-47c5-4743-bde6-5e1a8d86f89e\",\"type\": \"activity\"}}},\"options\": { \"time_to_live\": 259200,\"apns_production\": false}}";
                //    //string msg = "{\"platform\": \"all\",\"audience\" : {\"registration_id\" : [\"040526ea1ef\"]},\"notification\" : {\"alert\" : \"Hi, JPush!\",\"android\" : {},\"ios\" : {\"extras\" : {\"newsid\" :321}}}}";
                //    return Common.JPush.PushMsg(msg, ApiKey, APIMasterSecret);
                //}

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

    }
    public static class JPush
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string ApiKey = "c5769e566eca15eac9f5bbc3";//Android ApiKey
        static string APIMasterSecret = "db5d2b8984d337584f343c56";//Android密码
        /// <summary>
        ///  极光推送
        /// </summary>
        /// <param name="RegistrationID"></param>
        /// <param name="title"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static string PushMsg(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            string resCode = GetPostRequest(data);
            return resCode;
        }

        /// <summary>
        /// Post方式请求获取返回值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetPostRequest(byte[] data)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://api.jpush.cn/v3/push");
                byte[] bytes = Encoding.UTF8.GetBytes(ApiKey + ":" + APIMasterSecret);
                string code = Convert.ToBase64String(bytes);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/json";
                myRequest.ContentLength = data.Length;
                myRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic " + code);
                Stream newStream = myRequest.GetRequestStream();

                // Send the data.
                newStream.Write(data, 0, data.Length);
                newStream.Close();

                // Get response
                var response = (HttpWebResponse)myRequest.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                {
                    result = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                }
            }
            catch (Exception e)
            {
                log.Info(e.Message);
                return "Error:" + e;
            }
            return result;
        }

    }
}
