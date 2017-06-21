using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace Autoupdater
{
    public partial class DownloadProgress : Form
    {
        #region The private fields
        private bool isFinished = false;
        private List<DownloadFileInfo> downloadFileList = null;
        private List<DownloadFileInfo> allFileList = null;
        private ManualResetEvent evtDownload = null;
        private ManualResetEvent evtPerDonwload = null;
        private WebClient clientDownload = null;
        private string curBakFolderName = ConstFile.TEMPFOLDERNAME+"\\"+DateTime.Now.ToString("yyyy-MM-dd");
        Config config = new Config();
        int tryTimes = 0;//尝试下载次数;
        NLog.Logger _log = null;
        #endregion

        #region The constructor of DownloadProgress
        public DownloadProgress(List<DownloadFileInfo> downloadFileListTemp,Config _config)
        {
            InitializeComponent();
            config = _config;
            int.TryParse(config.TryTimes, out tryTimes);
            this.downloadFileList = downloadFileListTemp;
            allFileList = new List<DownloadFileInfo>();
            foreach (DownloadFileInfo file in downloadFileListTemp)
            {
                allFileList.Add(file);
            }
            //初始化日志选择器
            try
            {
                _log = NLog.LogManager.GetCurrentClassLogger();

            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region The method and event
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isFinished && DialogResult.No == MessageBox.Show(ConstFile.CANCELORNOT, ConstFile.MESSAGETITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                e.Cancel = true;
                return;
            }
            else
            {
                if (clientDownload != null)
                    clientDownload.CancelAsync();

                evtDownload.Set();
                evtPerDonwload.Set();
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            //config = Config.LoadConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstFile.FILENAME));
            evtDownload = new ManualResetEvent(true);
            evtDownload.Reset();
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcDownload));
        }

        long total = 0;
        long nDownloadedTotal = 0;

        private void ProcDownload(object o)
        {
            var errorMessageStr = new StringBuilder();
            string tempFolderPath = Path.Combine(CommonUnitity.SystemBinUrl, curBakFolderName);
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }


            evtPerDonwload = new ManualResetEvent(false);

            foreach (DownloadFileInfo file in this.downloadFileList)
            {
                total += file.Size;
            }
            try
            {
                while (!evtDownload.WaitOne(0, false))
                {
                    if (this.downloadFileList.Count == 0)
                        break;

                    DownloadFileInfo file = this.downloadFileList[0];


                    //Debug.WriteLine(String.Format("Start Download:{0}", file.FileName));

                    this.ShowCurrentDownloadFileName(file.FileName);

                    //Download
                    clientDownload = new WebClient();

                    //Added the function to support proxy
                    // clientDownload.Proxy = System.Net.WebProxy.GetDefaultProxy();
                    clientDownload.Proxy = WebRequest.GetSystemWebProxy();
                    clientDownload.Proxy.Credentials = CredentialCache.DefaultCredentials;
                    //clientDownload.Credentials = System.Net.CredentialCache.DefaultCredentials;//ftp可能不可用
                    if (!string.IsNullOrEmpty(config.PassWord) && !string.IsNullOrEmpty(config.UserName))
                    {
                        clientDownload.Credentials = new NetworkCredential(config.UserName, config.PassWord);
                    }
                    else
                    {
                        clientDownload.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    }
                    //End added

                    clientDownload.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                    {
                        try
                        {
                            this.SetProcessBar(e.ProgressPercentage, (int)((nDownloadedTotal + e.BytesReceived) * 100 / total));
                        }
                        catch
                        {
                            //log the error message,you can use the application's log code
                        }

                    };

                    clientDownload.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
                    {
                        try
                        {
                            DealWithDownloadErrors();
                            DownloadFileInfo dfile = e.UserState as DownloadFileInfo;
                            nDownloadedTotal += dfile.Size;
                            this.SetProcessBar(0, (int)(nDownloadedTotal * 100 / total));
                            evtPerDonwload.Set();
                        }
                        catch (Exception ex)
                        {
                            //log the error message,you can use the application's log code
                            MessageBox.Show(ex.Message);
                        }

                    };

                    evtPerDonwload.Reset();

                    //Download the folder file
                    string tempFolderPath1 = CommonUnitity.GetFolderUrl(file, curBakFolderName);
                    if (!string.IsNullOrEmpty(tempFolderPath1))
                    {
                        tempFolderPath = Path.Combine(CommonUnitity.SystemBinUrl, curBakFolderName);
                        tempFolderPath += tempFolderPath1;
                    }
                    else
                    {
                        tempFolderPath = Path.Combine(CommonUnitity.SystemBinUrl, curBakFolderName);
                    }

                    if (!Directory.Exists(tempFolderPath))
                    {
                        Directory.CreateDirectory(tempFolderPath);
                    }
                        
                    clientDownload.DownloadFileAsync(new Uri(file.DownloadUrl), Path.Combine(tempFolderPath, file.FileName), file);
                    //Wait for the download complete
                    evtPerDonwload.WaitOne();

                    clientDownload.Dispose();
                    clientDownload = null;

                    #region 可能会进行下载失败，进行多次重新下载
                    string tempUrlPath = CommonUnitity.GetFolderUrl(file, curBakFolderName);
                     var newPath = Path.Combine(CommonUnitity.SystemBinUrl + curBakFolderName + tempUrlPath, file.FileName);
                     System.IO.FileInfo f = new FileInfo(newPath);
  
                     if (file.TryTimes < tryTimes && !file.Size.ToString().Equals(f.Length.ToString()) && !file.FileName.ToString().EndsWith(".xml"))
                     {
                        //下载出错，进行重试
                         file.TryTimes += 1;//尝试次数递增
                         var curItem = config.UpdateFileList.Where(c => c.Version == file.Version).FirstOrDefault();
                         if (curItem != null)
                         {
                             curItem.TryTimes += 1;//失败的文件不保存,用于下次重启
                         }
                         if (_log != null)
                             _log.Info(string.Format("文件{0}:{1}下载失败后进行了第{2}次重试下载\n\r", file.DownloadUrl, file.Version, file.TryTimes));
                     }
                     else
                     {
                         //Remove the downloaded files
                         this.downloadFileList.Remove(file);
                     }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _log.Info(string.Format("Message:{0},StackTrace:{1},Source:{2}", ex.Message, ex.StackTrace, ex.Source));
                ShowErrorAndRestartApplication();
                //throw;
            }

            //When the files have not downloaded,return.
            if (downloadFileList.Count > 0)
            {
                return;
            }

            //Test network and deal with errors if there have 
            DealWithDownloadErrors();

            //Debug.WriteLine("All Downloaded");
            foreach (DownloadFileInfo file in this.allFileList)
            {
                string tempUrlPath = CommonUnitity.GetFolderUrl(file, curBakFolderName);
                string oldPath = string.Empty;
                string newPath = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(tempUrlPath))
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1), file.FileName);
                        newPath = Path.Combine(CommonUnitity.SystemBinUrl + curBakFolderName + tempUrlPath, file.FileName);
                    }
                    else
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl, file.FileName);
                        newPath = Path.Combine(CommonUnitity.SystemBinUrl + curBakFolderName, file.FileName);
                    }

                    //just deal with the problem which the files EndsWith xml can not download
                    System.IO.FileInfo f = new FileInfo(newPath);
                    //errorMessageStr.AppendFormat("{0},", file.FileFullName);
                    //2015.5.11文件不存在可以进行拷贝xml
                    if (!file.Size.ToString().Equals(f.Length.ToString()) && !file.FileName.ToString().EndsWith(".xml"))
                    {
                        //<LocalFile path="packages.config" lastver="" size="370" version="e0d3579f-44ba-4e99-b557-de2b37d9f588" />
                        var errorMsg = string.Format("<LocalFile path=\"{0}\"  lastver=\"\" size=\"{1}\" version=\"{2}\" downLoadUrl=\"{3}\"/>", file.FileFullName, file.Size, file.Version,file.DownloadUrl);
                        errorMessageStr.AppendLine(file.DownloadUrl);
                        var curItem = config.UpdateFileList.Where(c => c.Version == file.Version).FirstOrDefault();
                        if (curItem != null)
                        {
                            config.UpdateFileList.Remove(curItem);//失败的文件不保存,用于下次重启
                        }
                        
                       continue;
                      //  ShowErrorAndRestartApplication();
                    }

                    //Added for dealing with the config file download errors
                    string newfilepath = string.Empty;
                    if (newPath.Substring(newPath.LastIndexOf(".") + 1).Equals(ConstFile.CONFIGFILEKEY))
                    {
                        if (System.IO.File.Exists(newPath))
                        {
                            if (newPath.EndsWith("_"))
                            {
                                newfilepath = newPath;
                                newPath = newPath.Substring(0, newPath.Length - 1);
                                oldPath = oldPath.Substring(0, oldPath.Length - 1);
                            }
                            File.Copy(newfilepath, newPath,true);
                        }
                    }
                    //End added

                    if (File.Exists(oldPath))//文件存在
                    {
                        MoveFolderToOld(oldPath, newPath);
                    }
                    else
                    {
                        //Edit for config_ file
                        if (!string.IsNullOrEmpty(tempUrlPath))
                        {
                            if (!Directory.Exists(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1)))
                            {
                                Directory.CreateDirectory(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1));


                                MoveFolderToOld(oldPath, newPath);
                            }
                            else
                            {
                                MoveFolderToOld(oldPath, newPath);
                            }
                        }
                        else
                        {
                            MoveFolderToOld(oldPath, newPath);
                        }

                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                    //log the error message,you can use the application's log code
                }

            }

            //After dealed with all files, clear the data
            this.allFileList.Clear();

            if (!string.IsNullOrEmpty(errorMessageStr.ToString()))
            {
                if (_log!=null)
                _log.Info(string.Format("更新内容出错，无大小或者下载出错文件如下，可尝试手动更新，并配置IIS为可访问下载{0}", errorMessageStr.ToString()));
                //CommonUnitity.RestartApplication();
            }
            else
            {
                if (_log != null)
                _log.Info(string.Format("本次更新成功个数为:{0}", config.UpdateFileList.Count()));
            }


            if (this.downloadFileList.Count == 0)
                Exit(true);
            else
                Exit(false);
          
            evtDownload.Set();
            
        }

        //To delete or move to old files
        void MoveFolderToOld(string oldPath, string newPath)
        {
            //2015.5.12修改不添加Old字段
           if (File.Exists(oldPath + ".old"))
                File.Delete(oldPath + ".old");

            //if (File.Exists(oldPath))
            //    File.Move(oldPath, oldPath + ".old");
            //
            //如果.config文件存在则不进行复制
           if (File.Exists(oldPath) && oldPath.Substring(oldPath.LastIndexOf(".") + 1).Equals(ConstFile.CONFIGFILE))
            {

            }
            else
            {
                File.Copy(newPath, oldPath,true);
            }
            //File.Delete(oldPath + ".old");
        }

        delegate void ShowCurrentDownloadFileNameCallBack(string name);
        private void ShowCurrentDownloadFileName(string name)
        {
            if (this.labelCurrentItem.InvokeRequired)
            {
                ShowCurrentDownloadFileNameCallBack cb = new ShowCurrentDownloadFileNameCallBack(ShowCurrentDownloadFileName);
                this.Invoke(cb, new object[] { name });
            }
            else
            {
                this.labelCurrentItem.Text = name;
            }
        }

        delegate void SetProcessBarCallBack(int current, int total);
        private void SetProcessBar(int current, int total)
        {
            if (this.progressBarCurrent.InvokeRequired)
            {
                SetProcessBarCallBack cb = new SetProcessBarCallBack(SetProcessBar);
                this.Invoke(cb, new object[] { current, total });
            }
            else
            {
                if (current > 100)
                {
                    current = 100;
                }
                this.progressBarCurrent.Value = current;
                if (total > 100)
                { 
                total=100;
                }
                this.progressBarTotal.Value = total;
            }
        }

        delegate void ExitCallBack(bool success);
        private void Exit(bool success)
        {
            if (this.InvokeRequired)
            {
                ExitCallBack cb = new ExitCallBack(Exit);
                this.Invoke(cb, new object[] { success });
            }
            else
            {
                this.isFinished = success;
                this.DialogResult = success ? DialogResult.OK : DialogResult.Cancel;
                this.Close();
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            //bCancel = true;
             // evtDownload.Set();
             // evtPerDonwload.Set();
            ShowErrorAndRestartApplication();
        }

      

        private void DealWithDownloadErrors()
        {
            try
            {
                //Test Network is OK or not.
              
                WebClient client = new WebClient();
                if (!string.IsNullOrEmpty(config.PassWord) && !string.IsNullOrEmpty(config.UserName))
                {
                    client.Credentials = new NetworkCredential(config.UserName, config.PassWord);
                }
                else
                {
                    client.Credentials = new NetworkCredential();
                }
                client.DownloadString(config.ServerUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //log the error message,you can use the application's log code
                ShowErrorAndRestartApplication();
            }
        }

        private void ShowErrorAndRestartApplication()
        {
            Exit(false);
            MessageBox.Show(ConstFile.NOTNETWORK, ConstFile.MESSAGETITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //CommonUnitity.RestartApplication();
           
            
            // CommonUnitity.CloseApplication();
        }

        #endregion
    }
}