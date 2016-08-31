using pili_sdk_csharp.pili;
using pili_sdk_csharp.pili_qiniu;
using qch.core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class qiniuController : ApiController
    {
        private const string ACCESS_KEY = "X2s2-Q7qFqhEr8mpCz4k_ZBlh7SX0Jp3jgubdd_V";
        private const string SECRET_KEY = "-pCRmWeh5oAYi9dqfSwWJK0uxVQaIm3CyvzdAPvi";
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Replace with your hub name
        private const string HUB_NAME = "qch-lvie";
        [HttpGet]
        public Msg CreateStream()
        {
            Msg msg = new Msg();
            Credentials credentials = new Credentials(ACCESS_KEY, SECRET_KEY); // Credentials Object
            Hub hub = new Hub(credentials, HUB_NAME);
            Console.WriteLine(hub.ToString());

            // Create a new Stream
            string title = null; // optional, auto-generated as default
            string publishKey = null; // optional, auto-generated as default
            string publishSecurity = null; // optional, can be "dynamic" or "static", "dynamic" as default
            Stream stream = null;
            try
            {
                stream = hub.createStream(title, publishKey, "dynamic");

                //Trace.WriteLine("hub.createStream:");
                //Console.WriteLine(stream.toJsonString());
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
            //var model = new
            //{
            //    a = stream,
            //    b = stream.toJsonString()
            //};
            string a = stream.toJsonString();
            string b = a.Replace("\r\n", "");
            msg.Data = b.Trim();
            return msg;
        }
        [HttpGet]
        public object GetList(bool liveonly)
        {
            try
            {
                Credentials credentials = new Credentials(ACCESS_KEY, SECRET_KEY); // Credentials Object
                Hub hub = new Hub(credentials, HUB_NAME);
                string marker = null; // optional
                long limit = 0; // optional
                string titlePrefix = null; // optional

                Stream.StreamList streamList = hub.listStreams(marker, limit, titlePrefix, liveonly);
                return streamList;
            }
            catch (PiliException e)
            {
                log.Info(e);
                log.Info(e.Message);
                log.Info(e.StackTrace);
                return null;
            }
        }
        [HttpGet]
        public object GetList()
        {
            try
            {
                Credentials credentials = new Credentials(ACCESS_KEY, SECRET_KEY); // Credentials Object
                Hub hub = new Hub(credentials, HUB_NAME);
                string marker = null; // optional
                long limit = 0; // optional
                string titlePrefix = null; // optional

                Stream.StreamList streamList = hub.listStreams(marker, limit, titlePrefix);
                return streamList;
            }
            catch (PiliException e)
            {
                log.Info(e);
                log.Info(e.Message);
                log.Info(e.StackTrace);
                return null;
            }
        }
        [HttpGet]
        public object GetInfo(string StreamId)
        {
            try
            {
                Credentials credentials = new Credentials(ACCESS_KEY, SECRET_KEY); // Credentials Object
                Hub hub = new Hub(credentials, HUB_NAME);
                var stream = hub.getStream(StreamId);
                return stream;
            }
            catch (PiliException e)
            {
                log.Info(e);
                log.Info(e.Message);
                log.Info(e.StackTrace);
                return null;
            }
        }
        [HttpGet]
        public void Del()
        {
            try
            {
                Credentials credentials = new Credentials(ACCESS_KEY, SECRET_KEY); // Credentials Object
                Hub hub = new Hub(credentials, HUB_NAME);
                string marker = null; // optional
                long limit = 0; // optional
                string titlePrefix = null; // optional

                Stream.StreamList streamList = hub.listStreams(marker, 100, titlePrefix);
                Console.WriteLine("hub.listStreams()");
                Console.WriteLine("marker:" + streamList.Marker);
                IList<Stream> list = streamList.Streams;
                int i = 0;
                foreach (Stream s in list)
                {
                    i++;
                    Console.WriteLine(s.StreamId);
                    string res = s.delete();
                    Console.WriteLine("Stream delete(s" + i + ")");
                    Console.WriteLine(res);
                }

                /*
                 marker:10
                 stream object
                 */
            }
            catch (PiliException e)
            {
                // TODO Auto-generated catch block
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }
    }
}
