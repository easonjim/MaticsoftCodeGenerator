using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using Autoupdater;

namespace Updater
{
    public partial class UpdateForm : Form
    {
      
        public UpdateForm()
        {
            InitializeComponent();
            InitCheckUpdate();
        }
        void InitCheckUpdate()
        {

            #region check and download new version program
            bool bHasError = false;
            IAutoUpdater autoUpdater = new Autoupdater.AutoUpdater();
            try
            {
                autoUpdater.CheckUpdate();
                if (autoUpdater.HasUpdate)
                {
                    DialogResult result = MessageBox.Show("发现有新版本！确定要更新吗？如选择更新，将关闭此程序，请自行重启！", "新版本提醒", MessageBoxButtons.OKCancel);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        //杀死进程
                        System.Diagnostics.Process[] process1 = System.Diagnostics.Process.GetProcessesByName("DEncryptTool");
                        foreach (var process in process1)
                        {
                            process.Kill();
                        }
                        System.Diagnostics.Process[] process2 = System.Diagnostics.Process.GetProcessesByName("Codematic");
                        foreach (var process in process2)
                        {
                            process.Kill();
                        }
                        System.Diagnostics.Process[] process3 = System.Diagnostics.Process.GetProcessesByName("StartCodematic");
                        foreach (var process in process3)
                        {
                            process.Kill();
                        }

                        //执行更新
                        autoUpdater.StartUpdate();
                    }
                }
            }
            catch (WebException exp)
            {
                MessageBox.Show("服务器连接失败");
                bHasError = true;
            }
            catch (XmlException exp)
            {
                bHasError = true;
                MessageBox.Show("下载更新文件错误");
            }
            catch (NotSupportedException exp)
            {
                bHasError = true;
                MessageBox.Show("升级文件配置错误");
            }
            catch (ArgumentException exp)
            {
                bHasError = true;
                MessageBox.Show("下载升级文件错误");
            }
            catch (Exception exp)
            {
                bHasError = true;
                MessageBox.Show("更新过程中出现错误");
            }
            finally
            {
                if (bHasError == true)
                {
                    try
                    {
                        autoUpdater.RollBack();
                    }
                    catch (Exception)
                    {
                        //Log the message to your file or database
                    }
                }
                OperProcess op = new OperProcess();
                //启动进程
                op.StartProcess();
                //  this.Close();
            }
            #endregion
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {

        }
    }
}
