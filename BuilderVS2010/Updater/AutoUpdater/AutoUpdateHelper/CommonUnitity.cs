using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Autoupdater
{
    class CommonUnitity
    {
        public static string SystemBinUrl = AppDomain.CurrentDomain.BaseDirectory;

        public static void RestartApplication()
        {
            Process.Start(Application.ExecutablePath);
            Environment.Exit(0);
        }

        public static void CloseApplication()
        {
            OperProcess op = new OperProcess();
            //启动进程
            op.StartProcess();
        }

        #region 原有模式
        //public static string GetFolderUrl(DownloadFileInfo file)
        //{
        //    string folderPathUrl = string.Empty;
        //    int folderPathPoint = file.DownloadUrl.IndexOf("/", 15) + 1;
        //    string filepathstring = file.DownloadUrl.Substring(folderPathPoint);
        //    int folderPathPoint1 = filepathstring.IndexOf("/");
        //    string filepathstring1 = filepathstring.Substring(folderPathPoint1 + 1);
        //    if (filepathstring1.IndexOf("/") != -1)
        //    {
        //        string[] ExeGroup = filepathstring1.Split('/');
        //        for (int i = 0; i < ExeGroup.Length - 1; i++)
        //        {
        //            folderPathUrl += "\\" + ExeGroup[i];
        //        }
        //        if (!Directory.Exists(SystemBinUrl + ConstFile.TEMPFOLDERNAME + folderPathUrl))
        //        {
        //            Directory.CreateDirectory(SystemBinUrl + ConstFile.TEMPFOLDERNAME + folderPathUrl);
        //        }
        //    }
        //    return folderPathUrl;
        //}
        #endregion

        public static string GetFolderUrl(DownloadFileInfo file,string tempFolderUrl="")
        {
            string folderPathUrl = string.Empty;
            int folderPathPoint = file.DownloadUrl.IndexOf("/", 15) + 1;
            string filepathstring = file.DownloadUrl.Substring(folderPathPoint);
            //int folderPathPoint1 = filepathstring.IndexOf("/");
            //string filepathstring1 = filepathstring.Substring(folderPathPoint1 + 1);
            //if(filepathstring1.IndexOf("/") != -1)
            if (filepathstring.IndexOf("/") != -1)
            {
                //string[] ExeGroup = filepathstring1.Split('/');
                string[] ExeGroup = filepathstring.Split('/');
                for (int i = 0; i < ExeGroup.Length - 1; i++)
                {
                    folderPathUrl += "\\" + ExeGroup[i];
                }
                if (string.IsNullOrEmpty(tempFolderUrl))
                {
                    tempFolderUrl = ConstFile.TEMPFOLDERNAME;
                    if (!Directory.Exists(SystemBinUrl + tempFolderUrl + folderPathUrl))
                    {
                        Directory.CreateDirectory(SystemBinUrl + tempFolderUrl + folderPathUrl);
                    }
                }
                else
                {
                    if (!Directory.Exists(SystemBinUrl + tempFolderUrl))
                    {
                        Directory.CreateDirectory(SystemBinUrl + tempFolderUrl);
                    }
                }
               
            }
            return folderPathUrl;
        }
    }
}
