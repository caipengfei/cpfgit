using EnterpriseDT.Net.Ftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace qch.Infrastructure
{
    /// <summary>
    /// 文件上传业务
    /// </summary>
    public class FileUpServices
    {
        #region 上传文件的保存与验证
        /// <summary>
        /// 保存上传文件到服务器
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path">要保存的路径,例如: /image/2013/1/</param>
        /// <param name="IsFtpUpload">是否保存到FTP</param>
        /// <returns>返回保存后新图片路径,失败返回空字符</returns>
        public static string SaveFile(HttpPostedFileBase file, string path, bool IsFtpUpload)
        {
            try
            {
                //文件上传后保存的路径
                string filepath = string.Empty;

                #region 验证并保存文件
                if (file != null && file.ContentLength > 0)
                {
                    // 验证文件的有效性
                    if (ValidateFile(file))
                    {
                        //判断是否存在指定的目录
                        if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~" + path)))
                        {
                            // 创建目录
                            System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~" + path));
                        }
                        //重新命名文件名
                        string newfilename = Guid.NewGuid() + System.IO.Path.GetExtension(file.FileName);

                        #region 上传至FTP服务器
                        if (IsFtpUpload)
                        {
                            FTPService ftpService = new FTPService();
                            var ftpserver = ftpService.GetFtpConfig("img1");
                            using (var conn = new FTPConnection
                            {
                                ServerAddress = ftpserver.Server,
                                ServerDirectory = "web",
                                UserName = ftpserver.User,
                                Password = ftpserver.Pwd,
                                ServerPort = int.Parse(ftpserver.Port),
                                CommandEncoding = Encoding.GetEncoding("GBK")
                            })
                            {
                                conn.Connect();

                                if (!conn.DirectoryExists(path))
                                    conn.CreateDirectory(path);
                                conn.ChangeWorkingDirectory(path);

                                conn.UploadFile(System.Web.HttpContext.Current.Server.MapPath("~" + path) + newfilename, newfilename); /* upload c:\localfile.txt to the current ftp directory as file.txt */


                            }
                        }
                        #endregion

                        #region 上传到非FTP服务器上
                        else
                        {
                            //保存文件到服务器的目录上
                            file.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~" + path) + newfilename);

                        }
                        #endregion

                        //返回服务器上的文件路径
                        filepath = path + newfilename;
                    }

                }
                #endregion

                //没有文件返回空
                return filepath;

            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 图片文件验证
        /// </summary>
        /// <returns></returns>
        public static bool ValidateFile(HttpPostedFileBase file)
        {
            bool succ = false;
            //获取文件扩展名
            string extname = System.IO.Path.GetExtension(file.FileName).ToLower();
            //判断文件的扩展名
            if (extname == ".png" || extname == ".gif" || extname == ".jpg" || extname == ".jpeg")
            {
                //判断文件大小 最大30M
                if (file.ContentLength < 31457280)
                {
                    succ = true;
                }
            }
            return succ;
        }

        #endregion


    }
}
