﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Updater
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             var form=new UpdateForm() ;
        }
    }
}
