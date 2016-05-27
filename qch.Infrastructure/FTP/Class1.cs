using EnterpriseDT.Net.Ftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xftwl.Infrastructure.FTP
{
    class Class1
    {
        private const string ftpServerIP = "xx.xx.xx.xx";
        private const string ftpRemotePath = "web";
        private const string ftpUserID = "user";
        private const string ftpPassword = "123";

        private void btnCreateAndUpload_Click(object sender, EventArgs e)
        {
            using (var conn = new FTPConnection
            {
                ServerAddress = ftpServerIP,
                ServerDirectory = ftpRemotePath,
                UserName = ftpUserID,
                Password = ftpPassword,
                CommandEncoding = Encoding.GetEncoding("GBK")
            })
            {
                conn.Connect();

                //创建目录L11
                conn.CreateDirectory("L11");
                //============切换当前工作目录到L11============
                conn.ChangeWorkingDirectory("L11");
                //上行代码也可以用conn.ServerDirectory = "L11";
                //上传"文档.txt"
                conn.UploadFile("文档.txt", "文档.txt");
                //创建子目录L21,L22      
                conn.CreateDirectory("L21");
                conn.CreateDirectory("L22");
                //在当前工作目录L11上传文件"L11.htm"
                conn.UploadFile("11.htm","11.htm");

                //============切换当前工作目录到L21============
                conn.ChangeWorkingDirectory("L21");

                //上传文件21.htm
                conn.UploadFile("21.htm", "21.htm");
                //创建目录L31
                conn.CreateDirectory("L31");

                //============切换当前工作目录到 L31============
                conn.ChangeWorkingDirectory("L31");

                //上传文件31.htm
                conn.UploadFile("31.htm", "31.htm");
            }
        }

    }
}
