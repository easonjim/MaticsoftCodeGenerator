using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Maticsoft.Utility;
using Maticsoft.CodeHelper;
namespace Maticsoft.BuilderModel
{
    /// <summary>
    /// Model�����������
    /// </summary>
    public class BuilderModel : IBuilder.IBuilderModel
    {        
        #region ��������
        protected string _modelname=""; //model����
        protected string _namespace = "Maticsoft"; //���������ռ���
        protected string _modelpath="";//ʵ����������ռ�
        protected string _tabledescription="";
        protected List<ColumnInfo> _fieldlist;
        
        /// <summary>
        /// ���������ռ��� 
        /// </summary>        
        public string NameSpace
        {
            set { _namespace = value; }
            get { return _namespace; }
        }
        /// <summary>
        /// ʵ����������ռ�
        /// </summary>
        public string Modelpath
        {
            set { _modelpath = value; }
            get { return _modelpath; }
        }
        /// <summary>
        /// model����
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// ���������Ϣ
        /// </summary>
        public string TableDescription
        {
            set { _tabledescription = value; }
            get { return _tabledescription; }
        }
        /// <summary>
        /// ѡ����ֶμ���
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        //�����ļ�
        public Hashtable Languagelist
        {
            get
            {
                return Maticsoft.CodeHelper.Language.LoadFromCfg("BuilderModel.lan");
            }
        }
        #endregion
                
       

        public BuilderModel()
        {            
        }        

        #region ��������Model��
        /// <summary>
        /// ��������Model��
        /// </summary>		
        public string CreatModel()
        {           
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("namespace " + Modelpath);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            if (TableDescription.Length > 0)
            {
                strclass.AppendSpaceLine(1, "/// ��Model��: " + TableDescription.Replace("\r\n", "\r\n\t///"));
            }
            else
            {
                strclass.AppendSpaceLine(1, "/// ��Model��: " + _modelname /*+ ":" + Languagelist["summary"].ToString()*/);
            }            
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "[Serializable]");
            strclass.AppendSpaceLine(1, "public partial class " + _modelname);
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "public " + _modelname + "()");
            strclass.AppendSpaceLine(2, "{}");
            strclass.AppendLine(CreatModelMethod());
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            if (Modelpath.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.ToString();
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.ToString().Replace("Maticsoft", Modelpath.Split('.')[0]);
            }
        }
        #endregion

        #region ����Model���Բ���
        /// <summary>
        /// ����ʵ���������
        /// </summary>
        /// <returns></returns>
        public string CreatModelMethod()
        {

            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(2, "#region Model");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnTypedb = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                bool ispk = field.IsPrimaryKey;
                bool cisnull = field.Nullable;
                //string defValue=field.DefaultVal;
                string deText = field.Description;
                string columnType = CodeCommon.DbTypeToCS(columnTypedb);
                string isnull = "";
                if (CodeCommon.isValueType(columnType))
                {
                    if ((!IsIdentity) && (!ispk) && (cisnull))
                    {
                        isnull = "?";//����ɿ�����
                    }
                }
                
                strclass1.AppendSpace(2, "private " + columnType + isnull + " _" + columnName.ToLower());//˽�б���
                if (field.DefaultVal.Length > 0)
                {
                    switch (columnType.ToLower())
                    {                        
                        case "int":
                        case "long":
                            strclass1.Append("=" + field.DefaultVal.Trim().Replace("'", ""));  
                            break;
                        case "bool":
                        case "bit":
                            {
                                string val=field.DefaultVal.Trim().Replace("'", "").ToLower();
                                if(val=="1"||val=="true")
                                {
                                    strclass1.Append("= true" );
                                }
                                else
                                {
                                    strclass1.Append("= false");
                                }
                                
                            }
                            break;
                        case "nchar":
                        case "ntext":
                        case "nvarchar":                          
                        case "char":
                        case "text":
                        case "varchar":
                        case "string":
                            if (field.DefaultVal.Trim().StartsWith("N'"))
                            {
                                strclass1.Append("=" + field.DefaultVal.Trim().Remove(0, 1).Replace("'", "\""));  
                            }
                            else
                            {
                                if (field.DefaultVal.Trim().IndexOf("'") > -1)
                                {
                                    strclass1.Append("=" + field.DefaultVal.Trim().Replace("'", "\""));
                                }
                                else
                                {
                                    strclass1.Append("= \"" + field.DefaultVal.Trim().Replace("(", "").Replace(")", "") + "\"");
                                }
                            }                            
                            break;
                        case "datetime":
                            if (field.DefaultVal == "getdate"||
                                field.DefaultVal == "Now()"||
                                field.DefaultVal == "Now"||
                                field.DefaultVal == "CURRENT_TIME" ||
                                field.DefaultVal == "CURRENT_DATE"
                                )
                            {
                                strclass1.Append("= DateTime.Now");                                
                            }
                            else
                            {
                                strclass1.Append("= Convert.ToDateTime(" + field.DefaultVal.Trim().Replace("'", "\"") + ")");
                            }
                            break;
                        case "uniqueidentifier":
                            {
                                //if (field.DefaultVal == "newid")
                                //{
                                //    strclass1.Append("=" + field.DefaultVal.Trim().Replace("'", ""));
                                //}                                
                            }
                            break;
                        case "decimal":
                        case "double":
                        case "float":
                            {
                                strclass1.Append("=" + field.DefaultVal.Replace("'", "").Replace("(", "").Replace(")", "").ToLower() + "M");                                
                            }
                            break;
                        //case "sys_guid()":
                        //    break;
                        default:                            
                        //    strclass1.Append("=" + field.DefaultVal);
                            break;

                    }                    
                }                
                strclass1.AppendLine(";");

                strclass2.AppendSpaceLine(2, "/// <summary>");
                strclass2.AppendSpaceLine(2, "/// " + deText);
                strclass2.AppendSpaceLine(2, "/// </summary>");
                strclass2.AppendSpaceLine(2, "public " + columnType + isnull + " " + columnName);//����
                strclass2.AppendSpaceLine(2, "{");
                strclass2.AppendSpaceLine(3, "set{" + " _" + columnName.ToLower() + "=value;}");
                strclass2.AppendSpaceLine(3, "get{return " + "_" + columnName.ToLower() + ";}");
                strclass2.AppendSpaceLine(2, "}");
            }
            strclass.Append(strclass1.Value);
            strclass.Append(strclass2.Value);
            strclass.AppendSpaceLine(2, "#endregion Model");

            return strclass.ToString();
        }

        #endregion
    }
}
