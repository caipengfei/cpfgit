using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace qch.Infrastructure
{
    #region section
    /// <summary>
    /// 配置文件中的FTP节点
    /// </summary>
    public class FtpServer : ConfigurationSection
    {
        [ConfigurationProperty("ftp")]
        public FtpItemCollection FtpItems { get { return this["ftp"] as FtpItemCollection; } }
    }
    #endregion

    /// <summary>
    /// 单个子节点
    /// </summary>
    public class FtpItem : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name { get { return this["name"].ToString(); } }
        [ConfigurationProperty("server")]
        public string Server { get { return this["server"].ToString(); } }
        [ConfigurationProperty("port")]
        public string Port { get { return this["port"].ToString(); } }
        [ConfigurationProperty("user")]
        public string User { get { return this["user"].ToString(); } }
        [ConfigurationProperty("pwd")]
        public string Pwd { get { return this["pwd"].ToString(); } }
    }

    public class FtpItemCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FtpItem();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FtpItem)element).Name;
        }
    }

    /// <summary>
    /// FTP服务
    /// </summary>
    public class FTPService
    {
        //用户信息类图片存放路径 /user/userid/年月日/guid.jpg
        //商品图片类  /goods/种类Id/年月日/guid.jpg


        /// <summary>
        /// 获取指定的FTP服务器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public qch.Infrastructure.FtpItem GetFtpConfig(string name)
        {
            try
            {
                qch.Infrastructure.FtpItem target = new FtpItem();
                FtpServer ftp = (FtpServer)ConfigurationManager.GetSection("FtpServer");
                if (ftp != null)
                {
                    foreach (var item in ftp.FtpItems)
                    {
                        target = (FtpItem)item;
                        if (target.Name.ToLower() == name.ToLower())
                        {
                            break;
                        }
                    }
                }
                return target;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
