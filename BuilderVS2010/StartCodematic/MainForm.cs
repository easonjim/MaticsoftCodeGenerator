using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Maticsoft.Utility;

namespace StartCodematic
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //启动动软代码生成器
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Codematic.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show("当前目录无Codematic.exe文件，无法启动！");
            }
        }

        //启动数据库连接字符串加密
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\DEncryptTool.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show("当前目录无DEncryptTool.exe文件，无法启动！");
            }
        }

        //手动更新
        private void button3_Click(object sender, EventArgs e)
        {
            InitCheckUpdate(true);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitCheckUpdate();
        }
        void InitCheckUpdate(bool isCheck=false)
        {
            //调用外部更新程序执行更新
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Updater.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show("当前目录无Updater.exe文件，无法启动！");
            }
        }

        //快速创建分部类
        private void button4_Click(object sender, EventArgs e)
        {
            List<string> al=new List<string>();//文件夹
            List<string> f = new List<string>();//文件路径+文件名
            List<string> fname = new List<string>();//文件名

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)//选择文件夹
            {
                string foldPath = dialog.SelectedPath;

                //拆分子文件夹
                {
                    GetAllDirList(foldPath, al, f, fname); //递归得到文件夹和文件名集合
                    al.ForEach(obj =>
                        {
                            var tempPath = obj.Replace(".", "/");
                            if (!Directory.Exists(tempPath))
                            {
                                Directory.CreateDirectory(tempPath);
                            }
                        });
                    for (int i = 0; i < f.Count; i++)
                    {
                        if (f[i].Contains(".cs") || f[i].Contains(".aspx"))
                        {
                            //替换cs/aspx后缀的，最后再替换回来
                            string resultString = null;
                            try
                            {
                                resultString = Regex.Replace(f[i], @"(\.aspx$)", "$aspx", RegexOptions.Multiline);
                                resultString = Regex.Replace(resultString, @"(\.aspx\.designer\.cs$)",
                                                             "$aspx$designer$cs", RegexOptions.Multiline);
                                resultString = Regex.Replace(resultString, @"(\.aspx\.cs$)", "$aspx$cs",
                                                             RegexOptions.Multiline);
                                resultString = Regex.Replace(resultString, @"(\.cs$)", "$cs", RegexOptions.Multiline);
                                //转换.为/,得到转换后的路径
                                resultString = Regex.Replace(resultString, @"\.", "\\", RegexOptions.Multiline);
                                resultString = Regex.Replace(resultString, @"(\$aspx$)", ".aspx", RegexOptions.Multiline);
                                resultString = Regex.Replace(resultString, @"(\$aspx\$designer\$cs$)",
                                                             ".aspx.designer.cs", RegexOptions.Multiline);
                                resultString = Regex.Replace(resultString, @"(\$aspx\$cs$)", ".aspx.cs",
                                                             RegexOptions.Multiline);
                                resultString = Regex.Replace(resultString, @"(\$cs$)", ".cs", RegexOptions.Multiline);
                                var tempPath = resultString;
                                //进行文件移动
                                if (f[i]!=tempPath)
                                {
                                    File.Move(f[i], tempPath);
                                    f[i] = tempPath;//复制新路径
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                // Syntax error in the regular expression
                                MessageBox.Show("拆分子文件夹错误！" + ex.Message);
                            }
                        }
                    }
                    //清除带.的文件夹
                    al.ForEach(obj =>
                        {
                            try
                            {
                                if (obj.Contains(".") && Directory.Exists(obj))
                                {
                                    Directory.Delete(obj, true);
                                }
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show("拆分子文件夹错误！删除原有文件夹错误！" + ex.Message);
                            }
                        });
                }

                if (f.Where(obj => obj.Contains(".cs")).ToList().Count > 0)//判断是否有cs文件
                {
                    try
                    {
                        string DataAccessFactoryName = "DataAccess";
                        for (int i = 0; i < f.Count; i++)
                        {
                            if (f[i].Contains(".cs") && !f[i].Contains(".aspx.cs") && !f[i].Contains(".designer.cs") /*&& !f[i].Contains("_Ext.cs")*/)
                            {
                                string csContent = File.ReadAllText(f[i], Encoding.UTF8); //得到文件内容
                                StringPlus strclass = new StringPlus();
                                {//获取版权信息
                                    StringCollection resultList = new StringCollection();
                                    try
                                    {
                                        Regex regexObj = new Regex(@"/\*\*(.*?)\*/", RegexOptions.Singleline);
                                        Match matchResult = regexObj.Match(csContent);
                                        while (matchResult.Success)
                                        {
                                            resultList.Add(matchResult.Value);
                                            matchResult = matchResult.NextMatch();
                                        }
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        // Syntax error in the regular expression
                                    }
                                    foreach (string str in resultList)
                                    {
                                        strclass.AppendSpaceLine(0, str.ToString());
                                    }

                                }
                                //通过正则匹配想要的内容
                                {//引用
                                    Regex reg = new Regex(@"using .*;", RegexOptions.Multiline);
                                    var match = reg.Match(csContent);
                                    while (match.Success)
                                    {
                                        strclass.AppendSpaceLine(0, match.ToString());
                                        match = match.NextMatch();
                                    }

                                    strclass.AppendSpaceLine(0, "");
                                }
                                {//命名空间
                                    Regex reg = new Regex(@"namespace .*");
                                    var match = reg.Match(csContent);
                                    if (match.Success)
                                    {
                                        strclass.AppendSpaceLine(0, match.ToString().Replace("\r", ""));
                                    }
                                    strclass.AppendSpaceLine(0, "{");
                                }
                                {//注释
                                    Regex reg = new Regex(@"/// .*", RegexOptions.Multiline);
                                    var match = reg.Match(csContent);
                                    while (match.Success)
                                    {
                                        if (!match.ToString().Contains("/// </summary>"))
                                        {
                                            strclass.AppendSpaceLine(1, match.ToString().Replace("\r", ""));
                                            match = match.NextMatch();
                                        }
                                        else
                                        {
                                            strclass.AppendSpaceLine(1, "/// </summary>");
                                            break;
                                        }
                                    }
                                }
                                {//类名
                                    Regex reg = new Regex(@"public partial .*");
                                    var match = reg.Match(csContent);
                                    if (match.Success)
                                    {
                                        strclass.AppendSpaceLine(1, match.ToString().Replace("\r", ""));
                                        strclass.AppendSpaceLine(1, "{");
                                        strclass.AppendSpaceLine(1, "}");
                                    }
                                    else
                                    {
                                        //工厂类特殊处理
                                        if (new Regex(@"public sealed .*").Match(csContent).Success && fname[i].Equals("DataAccess.cs"))
                                        {
                                            {// 创建文件
                                                var r = new Regex(@"(.*)\\(.*).cs");
                                                //创建和查找工厂类名称
                                                string resultString = null;
                                                try
                                                {
                                                    resultString = Regex.Replace(Regex.Match(csContent, @"value=\""(.*)\.SQLServerDAL\.(.*)\""", RegexOptions.Multiline).Value, @"value=\""(.*)\.SQLServerDAL\.(.*)\""", "DA$2", RegexOptions.Multiline);
                                                    resultString = Regex.Replace(resultString, @"\.", "", RegexOptions.Multiline);
                                                    DataAccessFactoryName = resultString;
                                                    if (DataAccessFactoryName.Trim().Length == 0)//没有子文件夹时
                                                    {
                                                        var r2 = new Regex(@"[^//]string ClassNamespace = AssemblyPath \+""\.(.*?)"";");
                                                        DataAccessFactoryName = "DA" + r2.Match(csContent).Groups[1].ToString().Split('.')[r2.Match(csContent).Groups[1].ToString().Split('.').Length - 1];
                                                    }
                                                }
                                                catch (ArgumentException ex)
                                                {
                                                    // Syntax error in the regular expression
                                                }

                                                FileStream fs = new FileStream(r.Match(f[i]).Groups[1] + "\\"+DataAccessFactoryName + ".cs", FileMode.Create, FileAccess.ReadWrite);
                                                StreamWriter sw = new StreamWriter(fs);
                                                sw.Write(csContent.ToString().Replace("DataAccess//<t>\r\n\t{\r\n\t\tprivate static readonly string AssemblyPath = ConfigurationManager.AppSettings[\"DAL\"];\r\n\t\t/// <summary>\r\n\t\t/// 创建对象或从缓存获取\r\n\t\t/// </summary>\r\n\t\tpublic static object CreateObject(string AssemblyPath,string ClassNamespace)\r\n\t\t{\r\n\t\t\tobject objType = DataCache.GetCache(ClassNamespace);//从缓存读取\r\n\t\t\tif (objType == null)\r\n\t\t\t{\r\n\t\t\t\ttry\r\n\t\t\t\t{\r\n\t\t\t\t\tobjType = Assembly.Load(AssemblyPath).CreateInstance(ClassNamespace);//反射创建\r\n\t\t\t\t\tDataCache.SetCache(ClassNamespace, objType);// 写入缓存\r\n\t\t\t\t}\r\n\t\t\t\tcatch\r\n\t\t\t\t{}\r\n\t\t\t}\r\n\t\t\treturn objType;\r\n\t\t}\r\n\t\t/// <summary>\r\n\t\t/// 创建数据层接口\r\n\t\t/// </summary>\r\n\t\t//public static t Create(string ClassName)\r\n\t\t//{\r\n\t\t\t//string ClassNamespace = AssemblyPath +\".\"+ ClassName;\r\n\t\t\t//object objType = CreateObject(AssemblyPath, ClassNamespace);\r\n\t\t\t//return (t)objType;\r\n\t\t//}\r\n\t\t", DataAccessFactoryName + ":DataAccessBase\r\n\t{\r\n\t\t"));
                                                sw.Close();
                                                fs.Close();
                                            }
                                            {//删除原有工厂类
                                                if (File.Exists(f[i]))
                                                    File.Delete(f[i]);
                                            }
                                            {//修改BLL
                                                for (int j = 0; j < f.Count; j++)
                                                {
                                                    if (f[j].Contains(".cs") && !f[j].Contains(".aspx.cs") && !f[j].Contains(".designer.cs") /*&& !f[j].Contains("_Ext.cs")*/)
                                                    {
                                                        Regex reg1 = new Regex(@"(.*).cs");
                                                        var match1 = reg1.Match(f[j]);
                                                        if (match1.Success)
                                                        {
                                                            if (match1.Groups[1].ToString().Contains("BLL"))//如果BLL层则修改DataAccess的默认值，修改datafactory的名称
                                                            {
                                                                var tempContent = File.ReadAllText(f[j], Encoding.UTF8).Replace("DataAccess", DataAccessFactoryName);
                                                                var fs1 = new FileStream(f[j], FileMode.Create,FileAccess.Write);
                                                                StreamWriter sw1 = new StreamWriter(fs1, Encoding.UTF8);
                                                                sw1.Write(tempContent);
                                                                sw1.Close();
                                                                fs1.Close();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        continue;
                                    }
                                    strclass.AppendSpaceLine(0, "}");
                                }

                                //MessageBox.Show(strclass.ToString());
                                {// 创建文件
                                    Regex reg = new Regex(@"(.*).cs");
                                    var match = reg.Match(f[i]);
                                    if (match.Success)
                                    {
                                        FileStream fs = new FileStream(match.Groups[1] + "_Ext.cs", FileMode.Create,FileAccess.ReadWrite);
                                        StreamWriter sw = new StreamWriter(fs,Encoding.UTF8);
                                        sw.Write(strclass);
                                        sw.Close();
                                        fs.Close();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("生成分部类文件失败！" + ex.Message);
                        return;
                    }
                    MessageBox.Show("成功生成分部类文件！");
                }
                else//没有
                {
                    MessageBox.Show("当前路径下面没有CS文件！");
                }
            }

        }

        public void GetAllDirList(string strBaseDir,List<string> al,List<string> f,List<string> fname)
        {
            DirectoryInfo di = new DirectoryInfo(strBaseDir);
            {//获取当前目录
                al.Add(di.FullName);

                FileInfo[] fileInfo = di.GetFiles();
                foreach (FileInfo NextFile in fileInfo) //遍历文件
                {
                    if (!f.Contains(di.FullName + "\\" + NextFile.Name))//不存在的才加
                    {
                        f.Add(di.FullName + "\\" + NextFile.Name); //得到全文件路径文件名
                        fname.Add(NextFile.Name);
                    }
                }
            }
            DirectoryInfo[] diA = di.GetDirectories();
            for (int i = 0; i < diA.Length; i++)//获取下一级
            {
                al.Add(diA[i].FullName);

                FileInfo[] fileInfo = diA[i].GetFiles();
                foreach (FileInfo NextFile in fileInfo)  //遍历文件
                {
                    if (!f.Contains(diA[i].FullName + "\\" + NextFile.Name))//不存在的才加
                    {
                        f.Add(diA[i].FullName + "\\" + NextFile.Name);//得到全文件路径文件名
                        fname.Add(NextFile.Name);
                    }
                }

                GetAllDirList(diA[i].FullName, al, f, fname);
            }

        }
    }
}
