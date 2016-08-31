using pili_sdk_csharp.pili;
using pili_sdk_csharp.pili_qiniu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    decimal amount = 100m;
        //    decimal yifan = 0m;
        //    decimal shengyu = 10000m;
        //    int day = 0;

        //    for (int i = 1; i <= 9999; i++)
        //    {

        //        yifan = shengyu * 0.0005m;
        //        shengyu = shengyu - yifan;
        //        shengyu = Math.Round(shengyu, 2);
        //        yifan = Math.Round(yifan, 2);

        //        day++;

        //        if (shengyu <= 10)
        //        {
        //            break;
        //        }

        //        Console.WriteLine("第" + day + "天返还了" + yifan + "元，剩余" + shengyu + "元");
        //    }
        //    Console.ReadLine();
        //}
        private const string ACCESS_KEY = "X2s2-Q7qFqhEr8mpCz4k_ZBlh7SX0Jp3jgubdd_V";
        private const string SECRET_KEY = "-pCRmWeh5oAYi9dqfSwWJK0uxVQaIm3CyvzdAPvi";

        // Replace with your hub name
        private const string HUB_NAME = "qch-lvie";
        static void Main(string[] args)
        {
            testCreatStream();
            //测试推流后才能执行操作的函数，需要填写生成的流的id。
            // testTuiStream("z1.liuhanlin.561f62c5fb16df53010003ed");
            Console.ReadKey();
        }
        public static void testCreatStream()
        {
            //////////////////////////////////////////////////////////////////////////////////////////
            // Hub begin
            //////////////////////////////////////////////////////////////////////////////////////////

            // Instantiate an Hub object
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
