using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Xml;
//����
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;

namespace Maticsoft.BuilderDALIBatisNetMapXML
{
    /// <summary>
    /// ���ݷ��ʲ���빹������IBatisNet��ʽXMLӳ���ļ���
    /// </summary>
    public class BuilderDAL : Maticsoft.IBuilder.IBuilderDAL
    {
        #region ˽�б���
        protected string _key = "ID";//��ʶ�У��������ֶ�		
        protected string _keyType = "int";//��ʶ�У��������ֶ�����        
        #endregion

        #region ��������
        IDbObject dbobj;
        private string _dbname;
        private string _tablename;
        private string _modelname; //model����
        private string _dalname;//dal����    
        private List<ColumnInfo> _fieldlist;
        private List<ColumnInfo> _keys; // �����������ֶ��б�        
        private string _namespace; //���������ռ���
        private string _folder; //�����ļ���
        private string _dbhelperName;//���ݿ��������           
        private string _modelpath;
        private string _dalpath;
        private string _idalpath;
        private string _iclass;
        private string _procprefix;

        public IDbObject DbObject
        {
            set { dbobj = value; }
            get { return dbobj; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string DbName
        {
            set { _dbname = value; }
            get { return _dbname; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string TableName
        {
            set { _tablename = value; }
            get { return _tablename; }
        }

        /// <summary>
        /// ѡ��Ҫ���ɵ��ֶμ���
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        /// <summary>
        /// �����������ֶεļ���
        /// </summary>
        public List<ColumnInfo> Keys
        {
            set { _keys = value; }
            get { return _keys; }
        }
        /// <summary>
        /// ���������ռ���
        /// </summary>
        public string NameSpace
        {
            set { _namespace = value; }
            get { return _namespace; }
        }
        /// <summary>
        /// �����ļ���
        /// </summary>
        public string Folder
        {
            set { _folder = value; }
            get { return _folder; }
        }

        /*============================*/

        /// <summary>
        /// ʵ����������ռ�
        /// </summary>
        public string Modelpath
        {
            set { _modelpath = value; }
            get { return _modelpath; }
        }
        /// <summary>
        /// ʵ������
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// ʵ��������������ռ� + ������������ Modelpath+ModelName
        /// </summary>
        public string ModelSpace
        {
            get { return Modelpath + "." + ModelName; }
        }
        /// <summary>
        /// ʵ����ĳ���
        /// </summary>
        public string ModelAssembly
        {
            get
            {
                string _modelspace = _namespace + "." + "Model";
                return _modelspace;
            }
        }
        /*============================*/

        /// <summary>
        /// ���ݲ�������ռ�
        /// </summary>
        public string DALpath
        {
            set { _dalpath = value; }
            get
            {
                return _dalpath;
            }
        }
        public string DALName
        {
            set { _dalname = value; }
            get { return _dalname; }
        }
        /*============================*/


        /// <summary>
        /// �ӿڵ������ռ�
        /// </summary>
        public string IDALpath
        {
            set { _idalpath = value; }
            get
            {
                return _idalpath;
            }
        }
        /// <summary>
        /// �ӿ�����
        /// </summary>
        public string IClass
        {
            set { _iclass = value; }
            get { return _iclass; }
        }
        /*============================*/

        /// <summary>
        /// ���ݿ��������
        /// </summary>
        public string DbHelperName
        {
            set { _dbhelperName = value; }
            get { return _dbhelperName; }
        }
        /// <summary>
        /// �洢����ǰ׺ 
        /// </summary>       
        public string ProcPrefix
        {
            set { _procprefix = value; }
            get { return _procprefix; }
        }

        //�����ļ�
        public Hashtable Languagelist
        {
            get
            {
                return Maticsoft.CodeHelper.Language.LoadFromCfg("BuilderDALIBatisNetMapXML.lan");
            }
        }
        #endregion

        #region ��������

        /// <summary>
        /// ��ѡ�ֶε� select �б�
        /// </summary>
        public string Fieldstrlist
        {
            get
            {
                StringPlus _fields = new StringPlus();
                foreach (ColumnInfo obj in Fieldlist)
                {
                    _fields.Append(obj.ColumnName + ",");
                }
                _fields.DelLastComma();
                return _fields.Value;
            }
        }

        /// <summary>
        /// ��ͬ���ݿ����ǰ׺
        /// </summary>
        public string DbParaHead
        {
            get
            {
                switch (dbobj.DbType)
                {
                    case "SQL2000":
                    case "SQL2005":
                    case "SQL2008":
                        return "Sql";
                    case "Oracle":
                        return "Oracle";
                    case "MySQL":
                        return "MySql";
                    case "OleDb":
                        return "OleDb";
                    default:
                        return "Sql";
                }
            }

        }
        /// <summary>
        ///  ��ͬ���ݿ��ֶ�����
        /// </summary>
        public string DbParaDbType
        {
            get
            {
                switch (dbobj.DbType)
                {
                    case "SQL2000":
                    case "SQL2005":
                    case "SQL2008":
                        return "SqlDbType";
                    case "Oracle":
                        return "OracleType";
                    case "OleDb":
                        return "OleDbType";
                    case "MySQL":
                        return "MySqlDbType";
                    default:
                        return "SqlDbType";
                }
            }
        }

        /// <summary>
        /// �洢���̲��� ���÷���@
        /// </summary>
        public string preParameter
        {
            get
            {
                switch (dbobj.DbType)
                {
                    case "SQL2000":
                    case "SQL2005":
                    case "SQL2008":
                        return "@";
                    case "Oracle":
                        return ":";
                    //case "OleDb":
                    // break;
                    default:
                        return "@";

                }
            }
        }
        /// <summary>
        /// �����������ֶ����Ƿ��б�ʶ��
        /// </summary>
        public bool IsHasIdentity
        {
            get
            {
                bool isid = false;
                if (_keys.Count > 0)
                {
                    foreach (ColumnInfo key in _keys)
                    {
                        if (key.IsIdentity)
                        {
                            isid = true;
                        }
                    }
                }
                return isid;
            }
        }

        private string KeysNullTip
        {
            get
            {
                if (_keys.Count == 0)
                {
                    return "//�ñ���������Ϣ�����Զ�������/�����ֶ�";
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region ���캯��

        public BuilderDAL()
        {
        }
        public BuilderDAL(IDbObject idbobj)
        {
            dbobj = idbobj;
        }

        public BuilderDAL(IDbObject idbobj, string dbname, string tablename, string modelname, string dalName,
            List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string namepace,
            string folder, string dbherlpername, string modelpath,
            string dalpath, string idalpath, string iclass)
        {
            dbobj = idbobj;
            _dbname = dbname;
            _tablename = tablename;
            _modelname = modelname;
            _dalname = dalName;
            _namespace = namepace;
            _folder = folder;
            _dbhelperName = dbherlpername;
            _modelpath = modelpath;
            _dalpath = dalpath;
            _idalpath = idalpath;
            _iclass = iclass;
            Fieldlist = fieldlist;
            Keys = keys;
            foreach (ColumnInfo key in _keys)
            {
                _key = key.ColumnName;
                _keyType = key.TypeName;
                if (key.IsIdentity)
                {
                    _key = key.ColumnName;
                    _keyType = CodeCommon.DbTypeToCS(key.TypeName);
                    break;
                }
            }
        }

        #endregion
        
        #region  ��������Ϣ �õ��������б�

        /// <summary>
        /// �õ�Where������� - Parameter��ʽ (���磺����Exists  Delete  GetModel ��where)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string GetWhereExpression(List<ColumnInfo> keys)
        {
            StringPlus strclass = new StringPlus();
            foreach (ColumnInfo key in keys)
            {
                strclass.Append(key.ColumnName + "=" + preParameter + key.ColumnName + " and ");
            }
            strclass.DelLastChar("and");
            return strclass.Value;
        }

        /// <summary>
        /// ����sql����еĲ����б�(���磺����Add  Exists  Update Delete  GetModel �Ĳ�������)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string GetPreParameter(List<ColumnInfo> keys)
        {
            StringPlus strclass = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            int n = 0;
            foreach (ColumnInfo key in keys)
            {
                strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "" + key.ColumnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, key.TypeName, "") + "),");
                strclass2.AppendSpaceLine(3, "parameters[" + n.ToString() + "].Value = " + key.ColumnName + ";");
                n++;
            }
            strclass.DelLastComma();
            strclass.AppendLine("};");
            strclass.Append(strclass2.Value);
            return strclass.Value;
        }

        #endregion


        #region ���ݲ�(������)

        /// <summary>
        /// �õ�������Ĵ���
        /// </summary>     
        public string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            StringPlus strclass = new StringPlus();            
            strclass.AppendLine(GetMapXMLs());
            return strclass.ToString();
        }

        #endregion

        #region ���ݲ�(ʹ��IBatisNetʵ��)

        /// <summary>
        /// �õ����ID�ķ�������
        /// </summary>
        /// <param name="TabName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatGetMaxID()
        {
            StringPlus strclass = new StringPlus();
            if (_keys.Count > 0)
            {
                string keyname = "";
                foreach (ColumnInfo obj in _keys)
                {
                    if (CodeCommon.DbTypeToCS(obj.TypeName) == "int")
                    {
                        keyname = obj.ColumnName;
                        if (obj.IsPrimaryKey)
                        {
                            strclass.AppendLine("");
                            strclass.AppendSpaceLine(2, "/// <summary>");
                            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetMaxId"].ToString());
                            strclass.AppendSpaceLine(2, "/// </summary>");
                            strclass.AppendSpaceLine(2, "public int GetMaxID()");
                            strclass.AppendSpaceLine(2, "{");
                            strclass.AppendSpaceLine(2, "return ExecuteGetMaxID(\"GetMaxID\"); ");
                            strclass.AppendSpaceLine(2, "}");
                            break;
                        }
                    }
                }
            }
            return strclass.ToString();
        }

        /// <summary>
        /// �õ�Exists�����Ĵ���
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatExists()
        {
            StringPlus strclass = new StringPlus();
            if (_keys.Count > 0)
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryExists"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool Exists(object Id)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "return ExecuteExists(\"Exists\", Id);");               
                strclass.AppendSpaceLine(2, "}");
            }
            return strclass.Value;
        }

        /// <summary>
        /// �õ�����Add()�Ĵ���
        /// </summary>        
        public string CreatAdd()
        {           
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            StringPlus strclass3 = new StringPlus();
            StringPlus strclass4 = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryadd"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            string strretu = "void";
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005" || dbobj.DbType == "SQL2008") && (IsHasIdentity))
            {
                strretu = "int";
            }
            //��������ͷ
            string strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)";
            strclass.AppendLine(strFun);
            strclass.AppendSpaceLine(2, "{");            
            //���¶��巽��ͷ
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005" || dbobj.DbType == "SQL2008") && (IsHasIdentity))
            {
                strclass.AppendSpaceLine(3, "return ExecuteInsert(\"Insert" + ModelName + "\", model);");

            }
            else
            {
                strclass.AppendSpaceLine(3, "ExecuteInsert(\"Insert" + ModelName + "\", model);");
            }
            strclass.AppendSpace(2, "}");
            return strclass.ToString();
        }

        /// <summary>
        /// �õ�Update�����Ĵ���
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public string CreatUpdate()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryUpdate"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void Update(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "ExecuteUpdate(\"Update"+ModelName+"\", model);");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        /// <summary>
        /// �õ�Delete�Ĵ���
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatDelete()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void Delete(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "ExecuteDelete(\"Delete"+ModelName+"\", model);");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }

        /// <summary>
        /// �õ�GetModel()�Ĵ���
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public string CreatGetModel()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModel(object Id)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, ModelSpace + " model = ExecuteQueryForObject<" + ModelSpace + ">(\"SelectById\", Id);");
            strclass.AppendSpaceLine(3, "return model;");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }

        /// <summary>
        /// �õ�GetList()�Ĵ���
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetList()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "IList<" + ModelSpace + "> list = null; ");
            strclass.AppendSpaceLine(3, "list = ExecuteQueryForList<" + ModelSpace + ">(\"Select" + ModelName + "\", model); ");
            strclass.AppendSpaceLine(3, "return list; ");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
            
        }

        /// <summary>
        /// �õ�GetList()�Ĵ���
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetListByPageProc()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/*");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(int PageSize,int PageIndex,string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "tblName\", " + DbParaDbType + ".VarChar, 255),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "fldName\", " + DbParaDbType + ".VarChar, 255),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "PageSize\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "int") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "PageIndex\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "int") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "IsReCount\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "bit") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "OrderType\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "bit") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "strWhere\", " + DbParaDbType + ".VarChar,1000),");
            strclass.AppendSpaceLine(5, "};");
            strclass.AppendSpaceLine(3, "parameters[0].Value = \"" + this.TableName + "\";");
            strclass.AppendSpaceLine(3, "parameters[1].Value = \"" + this._key + "\";");
            strclass.AppendSpaceLine(3, "parameters[2].Value = PageSize;");
            strclass.AppendSpaceLine(3, "parameters[3].Value = PageIndex;");
            strclass.AppendSpaceLine(3, "parameters[4].Value = 0;");
            strclass.AppendSpaceLine(3, "parameters[5].Value = 0;");
            strclass.AppendSpaceLine(3, "parameters[6].Value = strWhere;	");
            strclass.AppendSpaceLine(3, "return " + DbHelperName + ".RunProcedure(\"UP_GetRecordByPage\",parameters,\"ds\");");
            strclass.AppendSpaceLine(2, "}*/");
            return strclass.Value;
        }

        #endregion


        #region  IBatisNetӳ���ļ�
        /// <summary>
        /// �õ�IBatisNetӳ���ļ�
        /// </summary>
        /// <returns></returns>
        public string GetMapXMLs()
        {
            //1����Ҫ����һ���յ�XML�ĵ�
            XmlDocument xmldoc = new XmlDocument();

            //2��XML���ĵ�����ͷ������XML����������
            XmlNode xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmldoc.AppendChild(xmlnode);

            #region ����һ����Ԫ��
            XmlElement xmlelem = xmldoc.CreateElement("", "sqlMap", "");
            XmlAttribute xmlAttr = xmldoc.CreateAttribute("xmlns");
            xmlAttr.Value = "http://ibatis.apache.org/mapping";
            xmlelem.Attributes.Append(xmlAttr);

            xmlAttr = xmldoc.CreateAttribute("xmlns:xsi");
            xmlAttr.Value = "http://www.w3.org/2001/XMLSchema-instance";
            xmlelem.Attributes.Append(xmlAttr);

            xmlAttr = xmldoc.CreateAttribute("namespace");
            xmlAttr.Value = ModelName;
            xmlelem.Attributes.Append(xmlAttr);

            xmldoc.AppendChild(xmlelem);

            #endregion


            #region  ������Ԫ�� alias

            XmlElement xmlelem2 = xmldoc.CreateElement("alias");
            XmlElement xmlelem3 = xmldoc.CreateElement("typeAlias");
            XmlAttribute xmlAttr3 = xmldoc.CreateAttribute("alias");
            xmlAttr3.Value = ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("type");
            xmlAttr3.Value = ModelSpace + "," + ModelAssembly;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlelem2.AppendChild(xmlelem3);
            xmlelem.AppendChild(xmlelem2);

            #endregion


            #region ������Ԫ�� resultMaps

            xmlelem2 = xmldoc.CreateElement("resultMaps");
            xmlelem3 = xmldoc.CreateElement("resultMap");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "SelectAllResult";
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("class");
            xmlAttr3.Value = ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);
            XmlElement xmlelem4;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                xmlelem4 = xmldoc.CreateElement("result");
                XmlAttribute xmlAttr4 = xmldoc.CreateAttribute("property");
                xmlAttr4.Value = columnName;
                xmlelem4.Attributes.Append(xmlAttr4);

                xmlAttr4 = xmldoc.CreateAttribute("column");
                xmlAttr4.Value = columnName;
                xmlelem4.Attributes.Append(xmlAttr4);
                xmlelem3.AppendChild(xmlelem4);
            }

            xmlelem2.AppendChild(xmlelem3);
            xmlelem.AppendChild(xmlelem2);

            #endregion


            #region  ������Ԫ�� statements

            xmlelem2 = xmldoc.CreateElement("statements");

            #region GetMaxID
            xmlelem3 = xmldoc.CreateElement("select");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "GetMaxID";
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("resultClass");
            xmlAttr3.Value = "int";
            xmlelem3.Attributes.Append(xmlAttr3);

            XmlText xmltext = xmldoc.CreateTextNode("select max(" + _key + ") from " + TableName);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion

            #region Exists
            xmlelem3 = xmldoc.CreateElement("select");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "Exists";
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("resultClass");
            xmlAttr3.Value = "int";
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("parameterclass");
            xmlAttr3.Value = _keyType;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmltext = xmldoc.CreateTextNode("select count(1) from  " + TableName + " where " + _key + " = #value#");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion

            #region Insert
            xmlelem3 = xmldoc.CreateElement("insert");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "Insert" + ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            //xmlAttr3 = xmldoc.CreateAttribute("resultClass");
            //xmlAttr3.Value = "int";
            //xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("parameterclass");
            xmlAttr3.Value = ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            StringBuilder sqlinsert1 = new StringBuilder();
            StringBuilder sqlinsert2 = new StringBuilder();
            
            #region ������ʶ
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005" || dbobj.DbType == "SQL2008") && (IsHasIdentity))
            {
                xmlelem4 = xmldoc.CreateElement("selectKey");
                XmlAttribute xmlAttr4 = xmldoc.CreateAttribute("property");
                xmlAttr4.Value = _key;
                xmlelem4.Attributes.Append(xmlAttr4);

                xmlAttr4 = xmldoc.CreateAttribute("type");
                xmlAttr4.Value = "post";
                xmlelem4.Attributes.Append(xmlAttr4);

                xmlAttr4 = xmldoc.CreateAttribute("resultClass");
                xmlAttr4.Value = "int";
                xmlelem4.Attributes.Append(xmlAttr4);

                xmltext = xmldoc.CreateTextNode("${selectKey}");
                xmlelem4.AppendChild(xmltext);

                xmlelem3.AppendChild(xmlelem4);
            }
           
            #endregion

            StringBuilder sqlInsert = new StringBuilder();
            StringPlus sql1 = new StringPlus();
            StringPlus sql2 = new StringPlus();
            sqlInsert.Append("insert into " + TableName + "(");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                string Length = field.Length;
                if (field.IsIdentity)
                {
                    continue;
                }
                sql1.Append(columnName + ",");
                sql2.Append("#" + columnName + "#,");                                
            }
            sql1.DelLastComma();
            sql2.DelLastComma();
            sqlInsert.Append(sql1.Value);
            sqlInsert.Append(") values (");
            sqlInsert.Append(sql2.Value+")");
            xmltext = xmldoc.CreateTextNode(sqlInsert.ToString());
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion


            #region  update

            #endregion

            #region  delete
            xmlelem3 = xmldoc.CreateElement("delete");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "Delete"+ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            //xmlAttr3 = xmldoc.CreateAttribute("resultClass");
            //xmlAttr3.Value = "int";
            //xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("parameterclass");
            xmlAttr3.Value = _keyType;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmltext = xmldoc.CreateTextNode("delete from  " + TableName + " where " + _key + " = #value#");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion

            #region  SelectAll
            xmlelem3 = xmldoc.CreateElement("select");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "SelectAll" + ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("resultMap");
            xmlAttr3.Value = "SelectAllResult";
            xmlelem3.Attributes.Append(xmlAttr3);

            xmltext = xmldoc.CreateTextNode("select * from  " + TableName );
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion

            #region  SelectByID
            xmlelem3 = xmldoc.CreateElement("select");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "SelectBy" + _key;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("resultMap");
            xmlAttr3.Value = "SelectAllResult";
            xmlelem3.Attributes.Append(xmlAttr3);


            xmlAttr3 = xmldoc.CreateAttribute("resultClass");
            xmlAttr3.Value = ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("parameterclass");
            xmlAttr3.Value = _keyType;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmltext = xmldoc.CreateTextNode("select * from " + TableName + " where " + _key + " = #value#");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion


            xmlelem.AppendChild(xmlelem2);
            #endregion
                       
            return xmldoc.OuterXml;


        }
        #endregion
    }
}
