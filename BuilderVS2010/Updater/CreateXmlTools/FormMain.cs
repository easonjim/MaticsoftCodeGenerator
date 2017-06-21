using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using AutoUpdaterCreateXmlTools.Properties;
using System.Security.Cryptography;

namespace CreateXmlTools
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            txtWebUrl.Text = "http://localhost:8011";
            txtWebUrl.ForeColor = Color.Gray;
        }

        //获取当前目录
        //string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string currentDirectory = System.Environment.CurrentDirectory;
        //服务端xml文件名称
        string serverXmlName = "AutoupdateService.xml";
        //更新文件URL前缀
        string url = string.Empty;

        List<XmlNode> needDelNodeList =null;//需要删除的xmlNode
        void CreateXml()
        {
            needDelNodeList = new List<XmlNode>();
            //创建文档对象
            XmlDocument doc = initialXml();
            XmlElement root = null;
            var isNewDoc = true;
            if (doc == null)
            {
                doc = new XmlDocument();
                //创建根节点
                 root = doc.CreateElement("updateFiles");
                //头声明
                XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(xmldecl);
            }
            else
            {
                root = doc.DocumentElement;
                isNewDoc = false;
                if(root.ChildNodes.Count>0)
                {
                 foreach(var node in root.ChildNodes)
                 {
                    needDelNodeList.Add((XmlNode)node);
                 }
                }
                
            }
            DirectoryInfo dicInfo = new DirectoryInfo(currentDirectory);
            
            //调用递归方法组装xml文件
            PopuAllDirectory(doc, root, dicInfo);
            if (isNewDoc)
            {
                //追加节点
                doc.AppendChild(root);
            }
            //删除不在该列表中，已被删除的文档
            if (needDelNodeList != null && needDelNodeList.Count>0)
            {
                foreach(var node in needDelNodeList)
                    root.RemoveChild(node);
            }
            //保存文档
            doc.Save(serverXmlName);
            if (root != null)
            {
                this.label2.Text = "总文件数为："+root.ChildNodes.Count.ToString();
            }
        }

        //递归组装xml文件方法
        private void PopuAllDirectory(XmlDocument doc, XmlElement root, DirectoryInfo dicInfo)
        {
            foreach (FileInfo f in dicInfo.GetFiles())
            {
                //排除当前目录中生成xml文件的工具文件
                if (f.Name != "CreateXmlTools.exe" && f.Name != "AutoupdateService.xml" && !f.Name.Contains("CreateXmlTools"))
                {
                    string path = dicInfo.FullName.Replace(currentDirectory, "").Replace("\\", "/");
                    string folderPath=string.Empty;
                    if (path != string.Empty)
                    {
                        folderPath = path.TrimStart('/') + "/";
                    }
                    var fullFilePath = folderPath + f.Name;
                    var fullUrl=url + path + "/" + f.Name;
                    var curChildElem = doc.SelectSingleNode(string.Format("//file[@path='{0}'and @url='{1}']", fullFilePath, fullUrl));
                    if (curChildElem == null)
                    {
                        XmlElement child = doc.CreateElement("file");
                        child.SetAttribute("path", fullFilePath);
                        child.SetAttribute("url", fullUrl);
                        child.SetAttribute("lastver", FileVersionInfo.GetVersionInfo(f.FullName).FileVersion);
                        child.SetAttribute("size", f.Length.ToString());
                        child.SetAttribute("needRestart", "false");
                        child.SetAttribute("version", Guid.NewGuid().ToString());
                        child.SetAttribute("hash", GetFileHash(f.FullName));
                        child.SetAttribute("updateTime", DateTime.Now.ToString());
                        
                        root.AppendChild(child);
                    }
                    else //已经存在需要判断是否需要新增
                    {
                        
                        var size = GetAttribute(curChildElem,"size");
                        var hashCode=GetAttribute(curChildElem,"hash");
                        var curFileHash = GetFileHash(f.FullName);
                        if (f.Length.ToString() != size || curFileHash != hashCode)
                        {
                            SetAttribute(curChildElem, "size", f.Length.ToString());
                            SetAttribute(curChildElem, "version", Guid.NewGuid().ToString());
                            SetAttribute(curChildElem, "updateTime", DateTime.Now.ToString());
                            SetAttribute(curChildElem, "hash", curFileHash);
                        }
                        needDelNodeList.Remove(curChildElem);
                    }
                }
            }

            foreach (DirectoryInfo di in dicInfo.GetDirectories())
                PopuAllDirectory(doc, root, di);
        }

        public static bool isValidFileContent(string filePath1, string filePath2)
        {//创建一个哈希算法对象
            using (HashAlgorithm hash = HashAlgorithm.Create())
            {
                using (FileStream file1 = new FileStream(filePath1, FileMode.Open),
                   file2=new FileStream(filePath2,FileMode.Open)){
                   byte[] hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组
                   byte[] hashByte2 = hash.ComputeHash(file2);
                   string str1 = BitConverter.ToString(hashByte1);//将字节数组装换为字符串
                   string str2 = BitConverter.ToString(hashByte2);
                   return (str1==str2);//比较哈希码
                  }
            }
        }
        /// <summary>
        /// 获取文件hash
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileHash(string filePath)
        {
            HashAlgorithm hash = HashAlgorithm.Create();
            FileStream file = new FileStream(filePath, FileMode.Open);
            byte[] hashByte = hash.ComputeHash(file);//哈希算法根据文本得到哈希码的字节数组
            string str1 = BitConverter.ToString(hashByte);//将字节数组装换为字符串
            return str1;
        }

        private string GetAttribute(XmlNode xe,string name)
        {
            if (xe.Attributes[name] != null)
            {
                return xe.Attributes[name].Value;
            }
            else
            {
                return string.Empty;
            }
        }
        private void SetAttribute(XmlNode xe, string name,string value)
        {
            if (xe.Attributes[name] != null)
            {
                  xe.Attributes[name].Value=value;
            }
            
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtWebUrl.Text.ToLower().Contains("http") || txtWebUrl.Text.ToLower().Contains("ftp"))
            {
                url = txtWebUrl.Text.Trim();
            }
            else
            {
                url = "http://" + txtWebUrl.Text.Trim();
            }

           
            CreateXml();
            ReadXml();
           
            if (!string.IsNullOrEmpty(txtWebUrl.Text))
            {
                Settings.Default.httpUrl = txtWebUrl.Text;
                Settings.Default.Save();
            }
        }

        private void ReadXml()
        {
            string path="AutoupdateService.xml";
            rtbXml.ReadOnly = true;
            if (File.Exists(path))
            {
                rtbXml.Text = File.ReadAllText(path);
            }
        }

        /// <summary>
        /// 初始化XmlDoc
        /// </summary>
        /// <returns></returns>
        private XmlDocument initialXml()
        {
            string path = "AutoupdateService.xml";

            if (File.Exists(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                return doc;
            }
            else
            {
                return null;
            }
        }

       

        private void txtWebUrl_Enter(object sender, EventArgs e)
        {
            txtWebUrl.ForeColor = Color.Black;
            if (txtWebUrl.Text.Trim() == "localhost:8011")
            {
                txtWebUrl.Text = string.Empty;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(Settings.Default.httpUrl))
            {
              txtWebUrl.Text = Settings.Default.httpUrl;
            }
 
        }
        
    }
}
