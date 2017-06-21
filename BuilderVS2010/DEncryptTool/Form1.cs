using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace DEncryptTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Encrypt_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("config.ini", false);
            sw.Write(this.txtString.Text);
            sw.Close();
            this.txtEnString.Text = Maticsoft.Accounts.DESEncrypt.Encrypt(this.txtString.Text);
        }

        private void btn_Decrypt_Click(object sender, EventArgs e)
        {
            this.txtString.Text = Maticsoft.Accounts.DESEncrypt.Decrypt(this.txtEnString.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("config.ini"))
            {
                StreamReader srFile = new StreamReader("config.ini", Encoding.Default);
                string Contents = srFile.ReadToEnd();
                srFile.Close();
                this.txtString.Text = Contents;
            }
        }
    }
}
