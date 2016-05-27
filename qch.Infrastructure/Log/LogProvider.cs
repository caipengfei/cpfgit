using System;
using System.IO;
using System.Web;

namespace qch.Infrastructure
{
	/// <summary>
	/// 日志提供者
	/// </summary>
	public static class LogProvider
	{
		public static object _LockObject;

		static LogProvider()
		{
			LogProvider._LockObject = new object();
		}

		public static void WriterLog(Type errorSource, string errorMethod, string param, string message, string stackTrace)
		{
			lock (LogProvider._LockObject)
			{
				string str = HttpContext.Current.Server.MapPath("Web.config");
				str = str.Substring(0, str.LastIndexOf("\\"));
				str = str.Substring(0, str.LastIndexOf("\\"));
				str = string.Concat(str, "\\Logs");
				if (!Directory.Exists(str))
				{
					Directory.CreateDirectory(str);
				}
				DateTime today = DateTime.Today;
				string str1 = Path.Combine(str, string.Concat("error_", today.ToString("yyyy-MM-dd"), ".log"));
				using (FileStream fileStream = new FileStream(str1, FileMode.OpenOrCreate, FileAccess.Write))
				{
					fileStream.Seek((long)0, SeekOrigin.End);
					using (StreamWriter streamWriter = new StreamWriter(fileStream))
					{
						streamWriter.WriteLine(string.Empty);
						DateTime now = DateTime.Now;
						streamWriter.WriteLine(string.Format("************************{0}************************", now.ToString("yyyy-MM-dd HH:mm:ss")));
						streamWriter.WriteLine(string.Format("错 误 源: {0}", errorSource.FullName));
						streamWriter.WriteLine(string.Format("错误方法: {0}", errorMethod));
						streamWriter.WriteLine(string.Format("参    数: {0}", param));
						streamWriter.WriteLine(string.Format("错误消息: {0}", message));
						streamWriter.WriteLine(string.Format("调用堆栈: {0}", stackTrace));
						DateTime dateTime = DateTime.Now;
						streamWriter.WriteLine(string.Format("************************{0}************************", dateTime.ToString("yyyy-MM-dd HH:mm:ss")));
						streamWriter.WriteLine(string.Empty);
						streamWriter.Flush();
						fileStream.Flush();
					}
				}
			}
		}
		/// <summary>
		/// 记录日志
		/// </summary>
		/// <param name="RightSource"></param>
		/// <param name="RightMethod"></param>
		/// <param name="param"></param>
		/// <param name="returnParam"></param>
		/// <param name="Info">错误信息</param>
		public static void WriterRightLog(Type RightSource, string RightMethod, string param, string returnParam,string Info)
		{
			lock (LogProvider._LockObject)
			{
				string str = HttpContext.Current.Server.MapPath("Web.config");
				str = str.Substring(0, str.LastIndexOf("\\"));
				str = str.Substring(0, str.LastIndexOf("\\"));
				str = string.Concat(str, "\\Logs");
				if (!Directory.Exists(str))
				{
					Directory.CreateDirectory(str);
				}
				DateTime today = DateTime.Today;
				string str1 = Path.Combine(str, string.Concat("work_", today.ToString("yyyy-MM-dd"), ".log"));
				using (FileStream fileStream = new FileStream(str1, FileMode.OpenOrCreate, FileAccess.Write))
				{
					fileStream.Seek((long)0, SeekOrigin.End);
					using (StreamWriter streamWriter = new StreamWriter(fileStream))
					{
						streamWriter.WriteLine(string.Empty);
						DateTime now = DateTime.Now;
						streamWriter.WriteLine(string.Format("************************{0}************************", now.ToString("yyyy-MM-dd HH:mm:ss")));
						streamWriter.WriteLine(string.Format("成 功 源: {0}", RightSource.FullName));
						streamWriter.WriteLine(string.Format("成功方法: {0}", RightMethod));
						streamWriter.WriteLine(string.Format("录入参数: {0}", param));
						streamWriter.WriteLine(string.Format("返回参数: {0}", returnParam));
						streamWriter.WriteLine(string.Format("返回信息: {0}", Info));
						DateTime dateTime = DateTime.Now;
						streamWriter.WriteLine(string.Format("************************{0}************************", dateTime.ToString("yyyy-MM-dd HH:mm:ss")));
						streamWriter.WriteLine(string.Empty);
						streamWriter.Flush();
						fileStream.Flush();
					}
				}
			}
		}
 
	 

	}
}