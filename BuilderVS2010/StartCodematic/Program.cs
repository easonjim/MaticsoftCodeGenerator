using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace StartCodematic
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool initiallyOwned = true;  
            bool isCreated;  
            Mutex m = new Mutex( initiallyOwned, "StartCodematic", out isCreated);
            if (!(initiallyOwned && isCreated))
            {
                MessageBox.Show("抱歉，程序只能在一台机上运行一个实例！", "提示");
                Application.Exit();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
