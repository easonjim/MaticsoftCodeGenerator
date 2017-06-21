using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
//����
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;

namespace Maticsoft.BuilderDALParam
{
    /// <summary>
    /// ���ݷ��ʲ���빹������Parameter��ʽ��
    /// </summary>
    public class BuilderDAL : Maticsoft.IBuilder.IBuilderDAL
    {

        #region ˽�б���

        /// <summary>
        /// ��ʶ�У��������ֶ�	
        /// </summary>
        protected string _IdentityKey = "";
        /// <summary>
        /// ��ʶ�У��������ֶ����� 
        /// </summary>
        protected string _IdentityKeyType = "int";

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
        protected string _tabledescription = "";

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        public string TableDescription
        {
            set { _tabledescription = value; }
            get { return _tabledescription; }
        }

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
            set
            {
                _keys = value;
                foreach (ColumnInfo key in _keys)
                {
                    _IdentityKey = key.ColumnName;
                    _IdentityKeyType = key.TypeName;
                    if (key.IsIdentity)
                    {
                        _IdentityKey = key.ColumnName;
                        _IdentityKeyType = CodeCommon.DbTypeToCS(key.TypeName);
                        break;
                    }
                }
            }
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
                return CodeCommon.DbParaHead(dbobj.DbType);
            }

        }
        /// <summary>
        ///  ��ͬ���ݿ��ֶ�����
        /// </summary>
        public string DbParaDbType
        {
            get
            {
                return CodeCommon.DbParaDbType(dbobj.DbType);
            }
        }

        /// <summary>
        /// �洢���̲��� ���÷���@
        /// </summary>
        public string preParameter
        {
            get
            {
                return CodeCommon.preParameter(dbobj.DbType);
            }
        }
        /// <summary>
        /// �����������ֶ����Ƿ��б�ʶ��
        /// </summary>
        public bool IsHasIdentity
        {
            get
            {
                return CodeCommon.IsHasIdentity(_keys);
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

        //�����ļ�
        public Hashtable Languagelist
        {
            get
            {
                return Maticsoft.CodeHelper.Language.LoadFromCfg("BuilderDALParam.lan");
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
                _IdentityKey = key.ColumnName;
                _IdentityKeyType = key.TypeName;
                if (key.IsIdentity)
                {
                    _IdentityKey = key.ColumnName;
                    _IdentityKeyType = CodeCommon.DbTypeToCS(key.TypeName);
                    break;
                }
            }
        }

        #endregion


        #region  ��������Ϣ �õ��������б�

        ///// <summary>
        ///// �õ�Where������� - Parameter��ʽ (���磺����Exists  Delete  GetModel ��where)
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <returns></returns>
        //public string GetWhereExpression(List<ColumnInfo> keys, bool IdentityisPrior)
        //{
        //    StringPlus strClass = new StringPlus();
        //    ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(keys);
        //    if ((IdentityisPrior) && (field != null)) //�б�ʶ�ֶ�
        //    {
        //        strClass.Append(field.ColumnName + "=" + preParameter + field.ColumnName);
        //    }
        //    else
        //    {
        //        foreach (ColumnInfo key in keys)
        //        {
        //            if (key.IsPrimaryKey || !key.IsIdentity)// if (key.IsPK)
        //            {
        //                strClass.Append(key.ColumnName + "=" + preParameter + key.ColumnName + " and ");
        //            }
        //        }
        //        strClass.DelLastChar("and");
        //    }
        //    return strClass.Value;
        //}

        ///// <summary>
        ///// ����sql����еĲ����б�(���磺���� Exists  Delete  GetModel ��where������ֵ)
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <returns></returns>
        //public string GetPreParameter(List<ColumnInfo> keys, bool IdentityisPrior)
        //{
        //    StringPlus strclass = new StringPlus();
        //    StringPlus strclass2 = new StringPlus();
        //    strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");

        //    ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(keys);
        //    if ((IdentityisPrior) && (field != null)) //�б�ʶ�ֶ�
        //    {
        //        strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "" + field.ColumnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, field.TypeName, "") + ")");
        //        strclass2.AppendSpaceLine(3, "parameters[0].Value = " + field.ColumnName + ";");
        //    }
        //    else
        //    {
        //        int n = 0;
        //        foreach (ColumnInfo key in keys)
        //        {
        //            if (key.IsPrimaryKey || !key.IsIdentity) //if (key.IsPK)
        //            {
        //                strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "" + key.ColumnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, key.TypeName, "") + "),");
        //                strclass2.AppendSpaceLine(3, "parameters[" + n.ToString() + "].Value = " + key.ColumnName + ";");
        //                n++;
        //            }
        //        }
        //        strclass.DelLastComma();
        //    }
        //    strclass.AppendLine("};");
        //    strclass.Append(strclass2.Value);
        //    return strclass.Value;

        //}

        #endregion

        #region ���ݲ�(������)

        /// <summary>
        /// �õ�������Ĵ���
        /// </summary>     
        public string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Text;");
            strclass.AppendLine("using System.Collections.Generic;");
            switch (dbobj.DbType)
            {
                case "SQL2008":
                case "SQL2005":
                case "SQL2000":
                case "SQL2012":
                    strclass.AppendLine("using System.Data.SqlClient;");
                    break;
                case "Oracle":
                    strclass.AppendLine("using System.Data.OracleClient;");
                    break;
                case "MySQL":
                    strclass.AppendLine("using MySql.Data.MySqlClient;");
                    break;
                case "OleDb":
                    strclass.AppendLine("using System.Data.OleDb;");
                    break;
                case "SQLite":
                    strclass.AppendLine("using System.Data.SQLite;");
                    break;
            }
            if (IDALpath != "")
            {
                strclass.AppendLine("using " + IDALpath + ";");
            }
            strclass.AppendLine("using Maticsoft.DBUtility;//Please add references");
            strclass.AppendLine("namespace " + DALpath);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            //strclass.AppendSpaceLine(1, "/// ��DAL��: " + Languagelist["summary"].ToString() + ":" + DALName);
            if (TableDescription.Length > 0)
            {
                strclass.AppendSpaceLine(1, "/// ��DAL��: " + TableDescription.Replace("\r\n", "\r\n\t///"));
            }
            else
            {
                strclass.AppendSpaceLine(1, "/// ��DAL��: " + _modelname /*+ ":" + Languagelist["summary"].ToString()*/);
            }           
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpace(1, "public partial class " + DALName);
            if (IClass != "")
            {
                strclass.Append(":" + IClass);
            }
            strclass.AppendLine("");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "public " + DALName + "()");
            strclass.AppendSpaceLine(2, "{}");
            strclass.AppendSpaceLine(2, "#region  BasicMethod");

            #region  ��������

            if (Maxid)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "#region Maxid");
                strclass.AppendLine(CreatGetMaxID());
                strclass.AppendLine(CreatGetMaxID2());//��������
                strclass.AppendSpaceLine(2, "#endregion");
            }
            if (Exists)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "#region Exists");
                strclass.AppendLine(CreatExists());
                strclass.AppendLine(CreatExists2());//��������
                strclass.AppendSpaceLine(2, "#endregion");
            }
            if (Add)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "#region Add");
                strclass.AppendLine(CreatAdd());
                strclass.AppendSpaceLine(2, "#endregion");
            }
            if (Update)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "#region Update");
                strclass.AppendLine(CreatUpdate());
                strclass.AppendLine(CreatUpdate2());//��������
                //strclass.AppendLine(CreatAddOrUpdate());//��������
                strclass.AppendSpaceLine(2, "#endregion");
            }
            if (Delete)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "#region Delete");
                strclass.AppendLine(CreatDelete());
                strclass.AppendLine(CreatDelete2());//��������
                strclass.AppendSpaceLine(2, "#endregion");
            }
            if (GetModel)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "#region GetModel");
                strclass.AppendLine(CreatGetModel());
                strclass.AppendLine(CreatGetModel2());//��������
                strclass.AppendLine(CreatDataRowToModel());
                strclass.AppendSpaceLine(2, "#endregion");
            }
            if (List)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "#region List");
                strclass.AppendLine(CreatGetList());
                //strclass.AppendLine(CreatGetList2());//��������
                strclass.AppendLine(CreatGetListByPage());
                //strclass.AppendLine(CreatGetListByPage2());//��������
                //strclass.AppendLine(CreatGetListByPageProc());
                strclass.AppendSpaceLine(2, "#endregion");
                strclass.AppendLine();
            }
            #endregion

            strclass.AppendSpaceLine(2, "#endregion  BasicMethod");

            strclass.AppendSpaceLine(2, "#region  ExtensionMethod");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "#endregion  ExtensionMethod");

            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            if (DALpath.Contains("Maticsoft"))//���ΪĬ�������ռ�ֱ�ӷ���
            {
                return strclass.Value;
            }
            else//����ֱ���滻ԭʼ�����ռ�
            {
                return strclass.Value.Replace("Maticsoft", DALpath.Split('.')[0]);
            }
        }

        #endregion

        #region ���ݲ�(ʹ��Parameterʵ��)

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
                    if (CodeCommon.DbTypeToCS(obj.TypeName) == "int" || CodeCommon.DbTypeToCS(obj.TypeName) == "long")
                    {
                        keyname = obj.ColumnName;
                        if (obj.IsPrimaryKey)
                        {
                            strclass.AppendLine("");
                            strclass.AppendSpaceLine(2, "/// <summary>");
                            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetMaxId"].ToString());
                            strclass.AppendSpaceLine(2, "/// </summary>");
                            strclass.AppendSpaceLine(2, "public " + CodeCommon.DbTypeToCS(obj.TypeName) + " GetMaxId()");
                            strclass.AppendSpaceLine(2, "{");
                            strclass.AppendSpaceLine(3,"string strsql = \"select max(\" + \""+keyname+"\" + \")+1 from \" + \""+_tablename+"\";");
                            strclass.AppendSpaceLine(3,"object obj = DbHelperSQL.GetSingle(strsql);");
                            strclass.AppendSpaceLine(3,"if (obj == null)");
                            strclass.AppendSpaceLine(3,"{");
                            strclass.AppendSpaceLine(4,"return 1;");
                            strclass.AppendSpaceLine(3,"}");
                            strclass.AppendSpaceLine(3,"else");
                            strclass.AppendSpaceLine(3,"{");

                            switch (CodeCommon.DbTypeToCS(obj.TypeName))//ֻ�ж���������
                            {
                                case "int":
                                    strclass.AppendSpaceLine(4, "return Convert.ToInt32(obj.ToString());");
                                    break;
                                case "long":
                                    strclass.AppendSpaceLine(4, "return Convert.ToInt64(obj.ToString());");
                                    break;
                            }

                            strclass.AppendSpaceLine(3,"}");
                            strclass.AppendSpaceLine(2, "}");
                            break;
                        }
                    }
                }
            }
            return strclass.ToString();
        }

        public string CreatGetMaxID2()
        {
            StringPlus strclass = new StringPlus();

            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// ���������õ����ID");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public object GetMaxId(string fieldName, string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql = new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"" + _tablename + "\");");
            strclass.AppendSpaceLine(3, "if (strWhere.Trim() != \"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \" + strWhere);");
            strclass.AppendSpaceLine(3, "}");

            strclass.AppendSpaceLine(3, "string strsql = \"select max(\" + fieldName + \")+1 from \" + strSql.ToString();");
            strclass.AppendSpaceLine(3, "object obj = DbHelperSQL.GetSingle(strsql);");
            strclass.AppendSpaceLine(3, "if (obj == null)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return 1;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return obj;");
            strclass.AppendSpaceLine(3, "}");

            strclass.AppendSpaceLine(2, "}");
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
                string strInparam = Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false);
                if (!string.IsNullOrEmpty(strInparam))
                {
                    strclass.AppendSpaceLine(2, "/// <summary>");
                    strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryExists"].ToString());
                    strclass.AppendSpaceLine(2, "/// </summary>");
                    strclass.AppendSpaceLine(2, "public bool Exists(" + strInparam + ")");
                    strclass.AppendSpaceLine(2, "{");
                    strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                    strclass.AppendSpaceLine(3, "strSql.Append(\"select count(1) from " + _tablename + "\");");
                    strclass.AppendSpaceLine(3, "strSql.Append(\" where " + Maticsoft.CodeHelper.CodeCommon.GetWhereParameterExpression(Keys, false, dbobj.DbType) + "\");");

                    strclass.AppendLine(CodeCommon.GetPreParameter(Keys, false, dbobj.DbType));

                    strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Exists(strSql.ToString(),parameters);");
                    strclass.AppendSpaceLine(2, "}");
                }
            }
            return strclass.Value;
        }

        /// <summary>
        /// �õ�Exists�����Ĵ���2
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatExists2()
        {
            StringPlus strclass = new StringPlus();
            if (_keys.Count > 0)
            {
                string strInparam = Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false);
                if (!string.IsNullOrEmpty(strInparam))
                {
                    strclass.AppendSpaceLine(2, "/// <summary>");
                    strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryExists"].ToString());
                    strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                    strclass.AppendSpaceLine(2, "/// </summary>");
                    strclass.AppendSpaceLine(2, "public bool ExistsWhere(string strWhere)");
                    strclass.AppendSpaceLine(2, "{");
                    strclass.AppendSpaceLine(3, "if (string.IsNullOrEmpty(strWhere))");
                    strclass.AppendSpaceLine(3, "{");
                    strclass.AppendSpaceLine(4, "return false;");
                    strclass.AppendSpaceLine(3, "}");
                    strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                    strclass.AppendSpaceLine(3, "strSql.Append(\"select count(1) from " + _tablename + "\");");
                    strclass.AppendSpaceLine(3, "strSql.Append(\" where \"+strWhere);");
                    
                    strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Exists(strSql.ToString());");
                    strclass.AppendSpaceLine(2, "}");
                }
            }
            return strclass.Value;
        }

        /// <summary>
        /// �õ�Add()�Ĵ���
        /// </summary>        
        public string CreatAdd()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            StringPlus strclass3 = new StringPlus();
            StringPlus strclass4 = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryadd"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            string strretu = "bool";
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005" ||
                dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012" || dbobj.DbType == "SQLite" || dbobj.DbType == "MySQL") && (IsHasIdentity))
            {
                strretu = "int";
                if (_IdentityKeyType != "int")
                {
                    strretu = _IdentityKeyType;
                }
            }
            
            //��������ͷ
            string strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)";
            strclass.AppendLine(strFun);
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"insert into " + _tablename + "(\");");
            strclass1.AppendSpace(3, "strSql.Append(\"");
            int n = 0;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                string Length = field.Length;
                bool nullable = field.Nullable;

                if (field.IsIdentity)
                {
                    continue;
                }
                strclass3.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + columnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, columnType, Length) + "),");
                strclass1.Append(columnName + ",");
                strclass2.Append(preParameter + columnName + ",");
                if ("uniqueidentifier" == columnType.ToLower())
                {
                    strclass4.AppendSpaceLine(3, "parameters[" + n + "].Value = Guid.NewGuid();");
                }
                else
                {
                    strclass4.AppendSpaceLine(3, "parameters[" + n + "].Value = model." + columnName + ";");
                }
                n++;
            }

            //ȥ�����Ķ���
            strclass1.DelLastComma();
            strclass2.DelLastComma();
            strclass3.DelLastComma();
            strclass1.AppendLine(")\");");
            strclass.Append(strclass1.ToString());
            strclass.AppendSpaceLine(3, "strSql.Append(\" values (\");");
            strclass.AppendSpaceLine(3, "strSql.Append(\"" + strclass2.ToString() + ")\");");
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005"
                || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012" || dbobj.DbType == "MySQL") && (IsHasIdentity))
            {
                strclass.AppendSpaceLine(3, "strSql.Append(\";select @@IDENTITY\");");
            }
            if ((dbobj.DbType == "SQLite") && (IsHasIdentity))
            {
                strclass.AppendSpaceLine(3, "strSql.Append(\";select LAST_INSERT_ROWID()\");");
            }
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass.Append(strclass3.Value);
            strclass.AppendLine("};");
            strclass.AppendLine(strclass4.Value);


            //���¶��巽��ͷ            
            if (strretu == "void")
            {
                strclass.AppendSpaceLine(3, "" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");
            }
            else
                if (strretu == "bool")
                {
                    strclass.AppendSpaceLine(3, "int rows=" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");
                    strclass.AppendSpaceLine(3, "if (rows > 0)");
                    strclass.AppendSpaceLine(3, "{");
                    strclass.AppendSpaceLine(4, "return true;");
                    strclass.AppendSpaceLine(3, "}");
                    strclass.AppendSpaceLine(3, "else");
                    strclass.AppendSpaceLine(3, "{");
                    strclass.AppendSpaceLine(4, "return false;");
                    strclass.AppendSpaceLine(3, "}");
                }
                else
                {
                    strclass.AppendSpaceLine(3, "object obj = " + DbHelperName + ".GetSingle(strSql.ToString(),parameters);");
                    strclass.AppendSpaceLine(3, "if (obj == null)");
                    strclass.AppendSpaceLine(3, "{");
                    strclass.AppendSpaceLine(4, "return 0;");
                    strclass.AppendSpaceLine(3, "}");
                    strclass.AppendSpaceLine(3, "else");
                    strclass.AppendSpaceLine(3, "{");
                    switch (strretu)
                    {
                        case "int":
                            strclass.AppendSpaceLine(4, "return Convert.ToInt32(obj);");
                            break;
                        case "long":
                            strclass.AppendSpaceLine(4, "return Convert.ToInt64(obj);");
                            break;
                        case "decimal":
                            strclass.AppendSpaceLine(4, "return Convert.ToDecimal(obj);");
                            break;
                    }

                    strclass.AppendSpaceLine(3, "}");
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
            StringPlus strclass3 = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryUpdate"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Update(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"update " + _tablename + " set \");");
            int n = 0;

            if (Fieldlist.Count == 0)
            {
                Fieldlist = Keys;
            }

            //�������ֶ���䣬��ʱ����
            List<ColumnInfo> fieldpk = new List<ColumnInfo>();

            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string Length = field.Length;
                bool IsIdentity = field.IsIdentity;
                bool isPK = field.IsPrimaryKey;

                if (field.IsIdentity || field.IsPrimaryKey || (Keys.Contains(field)))
                {
                    fieldpk.Add(field);
                    continue;
                }
                if (columnType.ToLower() == "timestamp")
                {
                    continue;
                }
                strclass1.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + columnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, columnType, Length) + "),");
                strclass2.AppendSpaceLine(3, "parameters[" + n + "].Value = model." + columnName + ";");
                n++;

                strclass3.AppendSpaceLine(3, "strSql.Append(\"" + columnName + "=" + preParameter + columnName + ",\");");
            }
            foreach (ColumnInfo field in fieldpk)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string Length = field.Length;
                bool IsIdentity = field.IsIdentity;
                bool isPK = field.IsPrimaryKey;

                strclass1.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + columnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, columnType, Length) + "),");
                strclass2.AppendSpaceLine(3, "parameters[" + n + "].Value = model." + columnName + ";");
                n++;
            }


            if (strclass3.Value.Length > 0)
            {
                //ȥ�����Ķ���			
                strclass3.DelLastComma();
                strclass3.AppendLine("\");");
            }
            else
            {
                strclass3.AppendLine("#warning ϵͳ����ȱ�ٸ��µ��ֶΣ����ֹ�ȷ����˸����Ƿ���ȷ�� ");
                foreach (ColumnInfo field in Fieldlist)
                {
                    string columnName = field.ColumnName;
                    strclass3.AppendSpaceLine(3, "strSql.Append(\"" + columnName + "=" + preParameter + columnName + ",\");");
                }
                if (Fieldlist.Count > 0)
                {
                    strclass3.DelLastComma();
                    strclass3.AppendLine("\");");
                }

            }

            strclass.Append(strclass3.Value);
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetWhereParameterExpression(Keys, true, dbobj.DbType) + "\");");

            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass1.DelLastComma();
            strclass.Append(strclass1.Value);
            strclass.AppendLine("};");
            strclass.AppendLine(strclass2.Value);
            strclass.AppendSpaceLine(3, "int rows=" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");

            strclass.AppendSpaceLine(3, "if (rows > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return true;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return false;");
            strclass.AppendSpaceLine(3, "}");

            strclass.AppendSpaceLine(2, "}");
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
        public string CreatUpdate2()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            StringPlus strclass3 = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + "������������");
            strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Update(string strWhere,string[] fields,object[] values)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3,"if (string.IsNullOrEmpty(strWhere) || fields.Length!= values.Length || fields.Length==0 || values.Length==0)");
            strclass.AppendSpaceLine(3,"{");
            strclass.AppendSpaceLine(4,"return false;");
            strclass.AppendSpaceLine(3,"}");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"update " + _tablename + " set \");");
            strclass.AppendSpaceLine(3,"for (int i = 0; i < fields.Length; i++)");
            strclass.AppendSpaceLine(3,"{");
            strclass.AppendSpaceLine(4,"strSql.Append(fields[i]+\"=\"+values[i]+\",\");");
            strclass.AppendSpaceLine(3,"}");
            strclass.AppendSpaceLine(3,"strSql.Remove(strSql.Length - 1, 1);");
            strclass.AppendSpaceLine(3,"strSql.Append(\" where \"+strWhere);");

            strclass.AppendSpaceLine(3, "int rows=" + DbHelperName + ".ExecuteSql(strSql.ToString());");

            strclass.AppendSpaceLine(3, "if (rows > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return true;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return false;");
            strclass.AppendSpaceLine(3, "}");

            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }

        /*/// <summary>
        /// �õ�AddOrUpdate�����Ĵ���
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public string CreatAddOrUpdate()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            StringPlus strclass3 = new StringPlus();

            if (Keys.Count > 0)
            {
                string strInparam = Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false);
                if (!string.IsNullOrEmpty(strInparam))
                {
                    strclass.AppendSpaceLine(2, "/// <summary>");
                    strclass.AppendSpaceLine(2, "/// " + "���ӻ����һ������");
                    strclass.AppendSpaceLine(2, "/// </summary>");

                    string strretu = "int"; //������ֻҪ��������
                    if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005" ||
                         dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012" || dbobj.DbType == "SQLite" ||
                         dbobj.DbType == "MySQL") && (IsHasIdentity))
                    {
                        if (_IdentityKeyType == "long")
                        {
                            strretu = _IdentityKeyType;
                        }
                    }
                    strclass.AppendSpaceLine(2, "public " + strretu + " AddOrUpdate(" + ModelSpace + " model)");
                    strclass.AppendSpaceLine(2, "{");
                    strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                    int n = 0;

                    if (Fieldlist.Count == 0)
                    {
                        Fieldlist = Keys;
                    }

                    //�������ֶ���䣬��ʱ����
                    List<ColumnInfo> fieldpk = new List<ColumnInfo>();

                    foreach (ColumnInfo field in Fieldlist)
                    {
                        string columnName = field.ColumnName;
                        string columnType = field.TypeName;
                        string Length = field.Length;
                        bool IsIdentity = field.IsIdentity;
                        bool isPK = field.IsPrimaryKey;

                        if (field.IsIdentity || field.IsPrimaryKey || (Keys.Contains(field)))
                        {
                            fieldpk.Add(field);
                            continue;
                        }
                        if (columnType.ToLower() == "timestamp")
                        {
                            continue;
                        }

                        n++;

                    }
                    foreach (ColumnInfo field in fieldpk)
                    {
                        strclass1.Append("model." + field.ColumnName + ",");
                        n++;
                    }
                    if (strclass1.ToString().Length > 0)
                    {
                        strclass1.DelLastComma();
                    }

                    strclass.AppendSpaceLine(3, "if (this.Exists(" + strclass1 + "))");
                    strclass.AppendSpaceLine(3, "{");
                    strclass.AppendSpaceLine(4, "if (this.Update(model))");
                    strclass.AppendSpaceLine(4, "{");
                    strclass.AppendSpaceLine(5, "return model." + (fieldpk.Count > 0 ? fieldpk[0].ColumnName : "") + ";");
                    strclass.AppendSpaceLine(4, "}");
                    strclass.AppendSpaceLine(4, "return -1;");
                    strclass.AppendSpaceLine(3, "}");
                    strclass.AppendSpaceLine(3, "else");
                    strclass.AppendSpaceLine(3, "{");

                    string strretu1 = "bool";
                    if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005" ||
                        dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012" || dbobj.DbType == "SQLite" || dbobj.DbType == "MySQL") && (IsHasIdentity))
                    {
                        strretu1 = "int";
                        if (_IdentityKeyType != "int")
                        {
                            strretu1 = _IdentityKeyType;
                        }
                    }
                    switch (strretu1) //����ֻҪ��������
                    {
                        case "int":
                        case "long":
                            strclass.AppendSpaceLine(4, "if (this.Add(model)>0)//��Ϊ�����������");
                            break;
                        default:
                            strclass.AppendSpaceLine(4, "if (this.Add(model))//��Ϊ�����������");
                            break;
                    }

                    strclass.AppendSpaceLine(4, "{");
                    strclass.AppendSpaceLine(5, "return model." + (fieldpk.Count > 0 ? fieldpk[0].ColumnName : "") + ";");
                    strclass.AppendSpaceLine(4, "}");
                    strclass.AppendSpaceLine(4, "return -1;");
                    strclass.AppendSpaceLine(3, "}");
                    strclass.AppendSpaceLine(2, "}");
                }
            }
            return strclass.ToString();
        }*/



        /// <summary>
        /// �õ�Delete�Ĵ���
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatDelete()
        {
            StringPlus strclass = new StringPlus();

            #region ��ʶ�ֶ�����-ɾ��
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Delete(" + CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablename + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetWhereParameterExpression(Keys, true, dbobj.DbType) + "\");");
            strclass.AppendLine(CodeCommon.GetPreParameter(Keys, true, dbobj.DbType));
            strclass.AppendSpaceLine(3, "int rows=" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");
            strclass.AppendSpaceLine(3, "if (rows > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return true;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return false;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(2, "}");

            #endregion

            #region �����������ȵ�ɾ��(���б�ʶ�ֶΣ����зǱ�ʶ�����ֶ�)

            if ((Maticsoft.CodeHelper.CodeCommon.HasNoIdentityKey(Keys)) && (Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(Keys) != null))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false) + ")");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, KeysNullTip);
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablename + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetWhereParameterExpression(Keys, false, dbobj.DbType) + "\");");
                strclass.AppendLine(CodeCommon.GetPreParameter(Keys, false, dbobj.DbType));

                strclass.AppendSpaceLine(3, "int rows=" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");
                strclass.AppendSpaceLine(3, "if (rows > 0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return true;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return false;");
                strclass.AppendSpaceLine(3, "}");

                strclass.AppendSpaceLine(2, "}");
            }

            #endregion

            #region ����ɾ������

            string keyField = "";
            if (Keys.Count == 1)
            {
                keyField = Keys[0].ColumnName;
            }
            else
            {
                foreach (ColumnInfo field in Keys)
                {
                    if (field.IsIdentity)
                    {
                        keyField = field.ColumnName;
                        break;
                    }
                }
            }
            if (keyField.Trim().Length > 0)
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDeletelist"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool DeleteList(string " + keyField + "list )");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablename + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" where " + keyField + " in (\"+" + keyField + "list + \")  \");");
                strclass.AppendSpaceLine(3, "int rows=" + DbHelperName + ".ExecuteSql(strSql.ToString());");
                strclass.AppendSpaceLine(3, "if (rows > 0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return true;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return false;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(2, "}");
            }
            #endregion


            return strclass.Value;
        }

        /// <summary>
        /// �õ�Delete�Ĵ���
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatDelete2()
        {
            StringPlus strclass = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + "����ɾ������");
            strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool DeleteWhere(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            strclass.AppendSpaceLine(3,"if (string.IsNullOrEmpty(strWhere))");
            strclass.AppendSpaceLine(3,"{");
            strclass.AppendSpaceLine(4,"return false;");
            strclass.AppendSpaceLine(3,"}");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablename + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "int rows=" + DbHelperName + ".ExecuteSql(strSql.ToString());");
            strclass.AppendSpaceLine(3, "if (rows > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return true;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return false;");
            strclass.AppendSpaceLine(3, "}");
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
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            if (dbobj.DbType == "SQL2005" || dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012")
            {
                strclass.Append(" top 1 ");
            }
            strclass.AppendLine(Fieldstrlist + " from " + _tablename + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetWhereParameterExpression(Keys, true, dbobj.DbType) + "\");");

            strclass.AppendLine(CodeCommon.GetPreParameter(Keys, true, dbobj.DbType));

            strclass.AppendSpaceLine(3, "" + ModelSpace + " model=new " + ModelSpace + "();");
            strclass.AppendSpaceLine(3, "DataSet ds=" + DbHelperName + ".Query(strSql.ToString(),parameters);");
            strclass.AppendSpaceLine(3, "if(ds.Tables[0].Rows.Count>0)");
            strclass.AppendSpaceLine(3, "{");

            #region �ֶθ�ֵ
            /*
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;

                strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"]!=null && ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                strclass.AppendSpaceLine(4, "{");
                #region
                switch (CodeCommon.DbTypeToCS(columnType))
                {
                    case "int":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "long":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=long.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "decimal":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "float":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=float.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "DateTime":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "string":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"]!=null)");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "bool":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))");
                            strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=true;");
                            strclass.AppendSpaceLine(5, "}");
                            strclass.AppendSpaceLine(5, "else");
                            strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=false;");
                            strclass.AppendSpaceLine(5, "}");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "byte[]":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"];");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "uniqueidentifier":
                    case "Guid":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "= new Guid(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    default:
                        strclass.AppendSpaceLine(5, "//model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
                        break;
                }
                #endregion
                strclass.AppendSpaceLine(4, "}");
            }*/
            #endregion

            strclass.AppendSpaceLine(4, "return DataRowToModel(ds.Tables[0].Rows[0]);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return null;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }

        /// <summary>
        /// �õ�GetModel()�Ĵ���
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public string CreatGetModel2()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModelWhere(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            strclass.AppendSpaceLine(3, "if (string.IsNullOrEmpty(strWhere))");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return null;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            if (dbobj.DbType == "SQL2005" || dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012")
            {
                strclass.Append(" top 1 ");
            }
            strclass.AppendLine(Fieldstrlist + " from " + _tablename + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where \"+strWhere);");

            strclass.AppendSpaceLine(3, "" + ModelSpace + " model=new " + ModelSpace + "();");
            strclass.AppendSpaceLine(3, "DataSet ds=" + DbHelperName + ".Query(strSql.ToString());");
            strclass.AppendSpaceLine(3, "if(ds.Tables[0].Rows.Count>0)");
            strclass.AppendSpaceLine(3, "{");

            

            strclass.AppendSpaceLine(4, "return DataRowToModel(ds.Tables[0].Rows[0]);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return null;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }


        /// <summary>
        /// DataRowToModel�Ĵ���
        /// </summary>      
        public string CreatDataRowToModel()
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
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " DataRowToModel(DataRow row)" );
            strclass.AppendSpaceLine(2, "{");          
            strclass.AppendSpaceLine(3, "" + ModelSpace + " model=new " + ModelSpace + "();");

            strclass.AppendSpaceLine(3, "if (row != null && row.Table!=null)");
            strclass.AppendSpaceLine(3, "{");

            #region �ֶθ�ֵ
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;

                //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                //strclass.AppendSpaceLine(4, "{");
                #region
                switch (CodeCommon.DbTypeToCS(columnType))
                {
                    case "int":
                        {
                            strclass.AppendSpaceLine(4, "if(row.Table.Columns.Contains(\"" + columnName + "\")==true && row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=int.Parse(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "long":
                        {
                            strclass.AppendSpaceLine(4, "if(row.Table.Columns.Contains(\"" + columnName + "\")==true && row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=long.Parse(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "decimal":
                        {
                            strclass.AppendSpaceLine(4, "if(row.Table.Columns.Contains(\"" + columnName + "\")==true && row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=decimal.Parse(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "float":
                        {
                            strclass.AppendSpaceLine(4, "if(row.Table.Columns.Contains(\"" + columnName + "\")==true && row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=float.Parse(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "DateTime":
                        {
                            strclass.AppendSpaceLine(4, "if(row.Table.Columns.Contains(\"" + columnName + "\")==true && row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=DateTime.Parse(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "string":
                        {
                            strclass.AppendSpaceLine(4, "if(row.Table.Columns.Contains(\"" + columnName + "\")==true && row[\"" + columnName + "\"]!=null)");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=row[\"" + columnName + "\"].ToString();");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "bool":
                        {
                            strclass.AppendSpaceLine(4, "if(row.Table.Columns.Contains(\"" + columnName + "\")==true && row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "if((row[\"" + columnName + "\"].ToString()==\"1\")||(row[\"" + columnName + "\"].ToString().ToLower()==\"true\"))");
                            strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=true;");
                            strclass.AppendSpaceLine(5, "}");
                            strclass.AppendSpaceLine(5, "else");
                            strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=false;");
                            strclass.AppendSpaceLine(5, "}");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "byte[]":
                        {
                            strclass.AppendSpaceLine(4, "if(row.Table.Columns.Contains(\"" + columnName + "\")==true && row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=(byte[])row[\"" + columnName + "\"];");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "uniqueidentifier":
                    case "Guid":
                        {
                            strclass.AppendSpaceLine(4, "if(row.Table.Columns.Contains(\"" + columnName + "\")==true && row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "= new Guid(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    default:
                        strclass.AppendSpaceLine(5, "//model." + columnName + "=row[\"" + columnName + "\"].ToString();");
                        break;
                }
                #endregion
                //strclass.AppendSpaceLine(4, "}");
            }
            #endregion

            strclass.AppendSpaceLine(3, "}");
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
            strclass.AppendSpaceLine(2, "/// ���ɲ�ѯ");
            strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet SelectList(string filed, string where,string table=\"\")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql = new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.AppendFormat(\"select {0} from {1} {2}\", filed, string.IsNullOrEmpty(table) ? \"" + TableName + "\" : table, where);");
            strclass.AppendSpaceLine(3, "return DbHelperSQL.Query(strSql.ToString());");
            strclass.AppendSpaceLine(2, "}");

            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            strclass.AppendLine(Fieldstrlist + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
            strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Query(strSql.ToString());");
            strclass.AppendSpaceLine(2, "}");

            if ((dbobj.DbType == "SQL2000") ||
               (dbobj.DbType == "SQL2005") ||
               (dbobj.DbType == "SQL2008") ||
               (dbobj.DbType == "SQL2012"))
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList2"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public DataSet GetList(int Top,string strWhere,string filedOrder)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"select \");");
                strclass.AppendSpaceLine(3, "if(Top>0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" top \"+Top.ToString());");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "strSql.Append(\" " + Fieldstrlist + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
                strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "strSql.Append(\" order by \" + filedOrder);");
                strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Query(strSql.ToString());");
                strclass.AppendSpaceLine(2, "}");
            }
            if ((dbobj.DbType =="MySQL"))
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList2"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public DataSet GetList(int Top,string strWhere,string filedOrder)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"select \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" " + Fieldstrlist + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
                strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "strSql.Append(\" order by \" + filedOrder);");
                strclass.AppendSpaceLine(3, "if(Top>0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" limit \"+Top.ToString());");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Query(strSql.ToString());");
                strclass.AppendSpaceLine(2, "}");
            }

            return strclass.Value;
        }

        /// <summary>
        /// �õ�GetList()�Ĵ���
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetList2()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetListModel(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            strclass.AppendLine(Fieldstrlist + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
            strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "DataSet ds = " + DbHelperName + ".Query(strSql.ToString());");
            strclass.AppendSpaceLine(3, "List<" + ModelSpace + "> listModel = new List<" + ModelSpace + ">();");
            strclass.AppendSpaceLine(3, "if (ds.Tables[0].Rows.Count > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "foreach (DataRow row in ds.Tables[0].Rows)");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "listModel.Add(DataRowToModel(row));");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(4, "return listModel;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return listModel;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(2, "}");

            if ((dbobj.DbType == "SQL2000") ||
               (dbobj.DbType == "SQL2005") ||
               (dbobj.DbType == "SQL2008") ||
               (dbobj.DbType == "SQL2012"))
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList2"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetListModel(int Top,string strWhere,string filedOrder)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"select \");");
                strclass.AppendSpaceLine(3, "if(Top>0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" top \"+Top.ToString());");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "strSql.Append(\" " + Fieldstrlist + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
                strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "strSql.Append(\" order by \" + filedOrder);");
                strclass.AppendSpaceLine(3, "DataSet ds = " + DbHelperName + ".Query(strSql.ToString());");
                strclass.AppendSpaceLine(3, "List<" + ModelSpace + "> listModel = new List<" + ModelSpace + ">();");
                strclass.AppendSpaceLine(3, "if (ds.Tables[0].Rows.Count > 0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "foreach (DataRow row in ds.Tables[0].Rows)");
                strclass.AppendSpaceLine(4, "{");
                strclass.AppendSpaceLine(5, "listModel.Add(DataRowToModel(row));");
                strclass.AppendSpaceLine(4, "}");
                strclass.AppendSpaceLine(4, "return listModel;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return listModel;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(2, "}");
            }
            if ((dbobj.DbType =="MySQL"))
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList2"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetListModel(int Top,string strWhere,string filedOrder)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"select \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" " + Fieldstrlist + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
                strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "strSql.Append(\" order by \" + filedOrder);");
                strclass.AppendSpaceLine(3, "if(Top>0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" limit \"+Top.ToString());");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "DataSet ds = " + DbHelperName + ".Query(strSql.ToString());");
                strclass.AppendSpaceLine(3, "List<" + ModelSpace + "> listModel = new List<" + ModelSpace + ">();");
                strclass.AppendSpaceLine(3, "if (ds.Tables[0].Rows.Count > 0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "foreach (DataRow row in ds.Tables[0].Rows)");
                strclass.AppendSpaceLine(4, "{");
                strclass.AppendSpaceLine(5, "listModel.Add(DataRowToModel(row));");
                strclass.AppendSpaceLine(4, "}");
                strclass.AppendSpaceLine(4, "return listModel;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return listModel;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(2, "}");
            }

            return strclass.Value;
        }


        /// <summary>
        /// �õ���ҳ�����Ĵ���
        /// </summary>        
        public string CreatGetListByPage()
        {
            StringPlus strclass = new StringPlus();

            if ((dbobj.DbType == "SQL2000") ||
                (dbobj.DbType == "SQL2005") ||
                (dbobj.DbType == "SQL2008") ||
                (dbobj.DbType == "SQL2012"))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["GetRecordCount"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public int GetRecordCount(string strWhere)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"select count(1) FROM " + TableName + " \");");
                strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "object obj = DbHelperSQL.GetSingle(strSql.ToString());");
                strclass.AppendSpaceLine(3, "if (obj == null)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return 0;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return Convert.ToInt32(obj);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(2, "}");



                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2,
                                         "public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"SELECT * FROM ( \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" SELECT ROW_NUMBER() OVER (\");");
                strclass.AppendSpaceLine(3, "if (!string.IsNullOrEmpty(orderby.Trim()))");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\"order by T.\" + orderby );");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\"order by T." + _IdentityKey + " desc\");");
                strclass.AppendSpaceLine(3, "}");

                strclass.AppendSpaceLine(3, "strSql.Append(\")AS Row, T.*  from " + TableName + " T \");");
                strclass.AppendSpaceLine(3, "if (!string.IsNullOrEmpty(strWhere.Trim()))");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" WHERE \" + strWhere);");
                strclass.AppendSpaceLine(3, "}");

                strclass.AppendSpaceLine(3, "strSql.Append(\" ) TT\");");
                strclass.AppendSpaceLine(3,
                                         "strSql.AppendFormat(\" WHERE TT.Row between {0} and {1}\", startIndex, endIndex);");

                strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Query(strSql.ToString());");
                strclass.AppendSpaceLine(2, "}");

            }
            if ((dbobj.DbType == "MySQL"))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["GetRecordCount"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public int GetRecordCount(string strWhere)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"select count(1) FROM " + TableName + " \");");
                strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "object obj = DbHelperSQL.GetSingle(strSql.ToString());");
                strclass.AppendSpaceLine(3, "if (obj == null)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return 0;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return Convert.ToInt32(obj);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(2, "}");



                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2,
                                         "public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"select \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" " + Fieldstrlist + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
                strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "strSql.Append(\" order by \" + orderby);");
                strclass.AppendSpaceLine(3, "if(startIndex>0 && endIndex>0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" limit \"+startIndex.ToString()+\",\"+endIndex.ToString());");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Query(strSql.ToString());");
                strclass.AppendSpaceLine(2, "}");
            }


            return strclass.Value;
        }

        /// <summary>
        /// �õ���ҳ�����Ĵ���
        /// </summary>        
        public string CreatGetListByPage2()
        {
            StringPlus strclass = new StringPlus();

            if ((dbobj.DbType == "SQL2000") ||
                (dbobj.DbType == "SQL2005") ||
                (dbobj.DbType == "SQL2008") ||
                (dbobj.DbType == "SQL2012"))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2,
                                         "public List<" + ModelSpace +"> GetListModelByPage(string strWhere, string orderby, int startIndex, int endIndex)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"SELECT * FROM ( \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" SELECT ROW_NUMBER() OVER (\");");
                strclass.AppendSpaceLine(3, "if (!string.IsNullOrEmpty(orderby.Trim()))");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\"order by T.\" + orderby );");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\"order by T." + _IdentityKey + " desc\");");
                strclass.AppendSpaceLine(3, "}");

                strclass.AppendSpaceLine(3, "strSql.Append(\")AS Row, T.*  from " + TableName + " T \");");
                strclass.AppendSpaceLine(3, "if (!string.IsNullOrEmpty(strWhere.Trim()))");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" WHERE \" + strWhere);");
                strclass.AppendSpaceLine(3, "}");

                strclass.AppendSpaceLine(3, "strSql.Append(\" ) TT\");");
                strclass.AppendSpaceLine(3,
                                         "strSql.AppendFormat(\" WHERE TT.Row between {0} and {1}\", startIndex, endIndex);");

                strclass.AppendSpaceLine(3, "DataSet ds = " + DbHelperName + ".Query(strSql.ToString());");
                strclass.AppendSpaceLine(3, "List<" + ModelSpace + "> listModel = new List<" + ModelSpace + ">();");
                strclass.AppendSpaceLine(3, "if (ds.Tables[0].Rows.Count > 0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "foreach (DataRow row in ds.Tables[0].Rows)");
                strclass.AppendSpaceLine(4, "{");
                strclass.AppendSpaceLine(5, "listModel.Add(DataRowToModel(row));");
                strclass.AppendSpaceLine(4, "}");
                strclass.AppendSpaceLine(4, "return listModel;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return listModel;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(2, "}");
            }
            if ((dbobj.DbType == "MySQL"))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "ע�⣺����Ҫʹ�÷�SQLע�����");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2,
                                         "public List<" + ModelSpace +"> GetListModelByPage(string strWhere, string orderby, int startIndex, int endIndex)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"select \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" " + Fieldstrlist + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
                strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "strSql.Append(\" order by \" + orderby);");
                strclass.AppendSpaceLine(3, "if(startIndex>0 && endIndex>0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" limit \"+startIndex.ToString()+\",\"+endIndex.ToString());");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "DataSet ds = " + DbHelperName + ".Query(strSql.ToString());");
                strclass.AppendSpaceLine(3, "List<" + ModelSpace + "> listModel = new List<" + ModelSpace + ">();");
                strclass.AppendSpaceLine(3, "if (ds.Tables[0].Rows.Count > 0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "foreach (DataRow row in ds.Tables[0].Rows)");
                strclass.AppendSpaceLine(4, "{");
                strclass.AppendSpaceLine(5, "listModel.Add(DataRowToModel(row));");
                strclass.AppendSpaceLine(4, "}");
                strclass.AppendSpaceLine(4, "return listModel;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return listModel;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(2, "}");
            }
            return strclass.Value;
        }
        

        /*/// <summary>
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
            strclass.AppendSpaceLine(3, "parameters[1].Value = \"" + this._IdentityKey + "\";");
            strclass.AppendSpaceLine(3, "parameters[2].Value = PageSize;");
            strclass.AppendSpaceLine(3, "parameters[3].Value = PageIndex;");
            strclass.AppendSpaceLine(3, "parameters[4].Value = 0;");
            strclass.AppendSpaceLine(3, "parameters[5].Value = 0;");
            strclass.AppendSpaceLine(3, "parameters[6].Value = strWhere;	");
            strclass.AppendSpaceLine(3, "return " + DbHelperName + ".RunProcedure(\"UP_GetRecordByPage\",parameters,\"ds\");");
            strclass.AppendSpaceLine(2, "}#1#");
            return strclass.Value;
        }*/

        #endregion


    }
}
