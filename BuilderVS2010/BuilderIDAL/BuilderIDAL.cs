using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.BuilderIDAL
{
    /// <summary>
    /// 接口层代码组件
    /// </summary>
    public class BuilderIDAL : IBuilder.IBuilderIDAL
    {
        #region 私有变量

        /// <summary>
        /// 标识列，或主键字段	
        /// </summary>
        protected string _IdentityKey = "";
        /// <summary>
        /// 标识列，或主键字段类型 
        /// </summary>
        protected string _IdentityKeyType = "int";

        #endregion

        #region 公有属性
        private string dbType;
        private string _modelname; //model类名        
        private List<ColumnInfo> _fieldlist;
        private List<ColumnInfo> _keys; // 主键或条件字段列表        
        private string _namespace; //顶级命名空间名
        private string _folder; //所在文件夹           
        private string _modelpath;
        private string _idalpath;
        private string _iclass;
        protected string _tabledescription = "";
        private bool isHasIdentity;


        public string DbType
        {
            set { dbType = value; }
            get { return dbType; }
        }
        /// <summary>
        /// 选择要生成的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        /// <summary>
        /// 主键或条件字段的集合
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
        /// 顶级命名空间名
        /// </summary>
        public string NameSpace
        {
            set { _namespace = value; }
            get { return _namespace; }
        }
        /// <summary>
        /// 所在文件夹
        /// </summary>
        public string Folder
        {
            set { _folder = value; }
            get { return _folder; }
        }
        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        public string Modelpath
        {
            set { _modelpath = value; }
            get { return _modelpath; }
        }
        /// <summary>
        /// 实体类名
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// 实体类的整个命名空间 + 类名，即等于 Modelpath+ModelName
        /// </summary>
        public string ModelSpace
        {
            get { return Modelpath + "." + ModelName; }
        }
        /// <summary>
        /// 接口的命名空间
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
        /// 接口类名
        /// </summary>
        public string IClass
        {
            set { _iclass = value; }
            get { return _iclass; }
        }
        /*============================*/



        /// <summary>
        /// 表的描述信息
        /// </summary>
        public string TableDescription
        {
            set { _tabledescription = value; }
            get { return _tabledescription; }
        }
        /// <summary>
        /// 是否有自动增长标识列
        /// </summary>
        public bool IsHasIdentity
        {
            set { isHasIdentity = value; }
            get
            {
                return isHasIdentity;
            }
        }
        #endregion

        #region 接口代码
        public string GetIDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("namespace " + IDALpath);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            //strclass.AppendSpaceLine(1, "/// 【IDAL】: 接口层" + TableDescription);
            if (TableDescription.Length > 0)
            {
                strclass.AppendSpaceLine(1, "/// 【IDAL】: " + TableDescription.Replace("\r\n", "\r\n\t///"));
            }
            else
            {
                strclass.AppendSpaceLine(1, "/// 【IDAL】: " + _modelname /*+ ":" + Languagelist["summary"].ToString()*/);
            }           
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "public partial interface " + IClass);
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "#region  成员方法");

            if (Maxid)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region Maxid");
                strclass.Append(CreatGetMaxID());
                strclass.Append(CreatGetMaxID2());//新增方法
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (Exists)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region Exists");
                strclass.Append(CreatExists());
                strclass.AppendLine(CreatExists2());//新增方法
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (Add)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region Add");
                strclass.Append(CreatAdd());
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (Update)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region Update");
                strclass.Append(CreatUpdate());
                strclass.AppendLine(CreatUpdate2());//新增方法
                //strclass.AppendLine(CreatAddOrUpdate());//新增方法
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (Delete)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region Delete");
                strclass.Append(CreatDelete());
                strclass.AppendLine(CreatDelete2());//新增方法
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (GetModel)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region GetModel");
                strclass.Append(CreatGetModel());
                strclass.Append(CreatGetModel2());//新增方法
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (List)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region List");
                strclass.Append(CreatGetList());
                //strclass.AppendLine(CreatGetList2());//新增方法，其实返回list是合理的，同样是数据，只是这个是强类型的数据，如同返回实体的信息一样
                strclass.AppendSpaceLine(2, "#endregion");
                strclass.AppendLine();
            }
            strclass.AppendSpaceLine(2, "#endregion  成员方法");


            strclass.AppendSpaceLine(2, "#region  MethodEx");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "#endregion  MethodEx");


            strclass.AppendLine("	} ");
            strclass.AppendLine("}");

            if (IDALpath.Contains("Maticsoft"))//如果为默认命名空间直接返回
            {
                return strclass.ToString();
            }
            else//否则直接替换原始命名空间
            {
                return strclass.ToString().Replace("Maticsoft", IDALpath.Split('.')[0]);
            }

        }
        #endregion

        #region 方法代码
        public string CreatGetMaxID()
        {
            StringPlus strclass = new StringPlus();
            if (Keys.Count > 0)
            {
                foreach (ColumnInfo obj in Keys)
                {
                    if (CodeCommon.DbTypeToCS(obj.TypeName) == "int" || CodeCommon.DbTypeToCS(obj.TypeName) == "long")
                    {
                        if (obj.IsPrimaryKey)
                        {
                            strclass.AppendSpaceLine(2, "/// <summary>");
                            strclass.AppendSpaceLine(2, "/// 得到最大ID");
                            strclass.AppendSpaceLine(2, "/// </summary>");
                            strclass.AppendLine("		" + CodeCommon.DbTypeToCS(obj.TypeName) + " GetMaxId();");
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
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 根据条件得到最大ID");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendLine("		object GetMaxId(string fieldName, string strWhere);");
            return strclass.ToString();
        }
        public string CreatExists()
        {
            StringPlus strclass = new StringPlus();
            if (Keys.Count > 0)
            {
                string strInparam = Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false);
                if (!string.IsNullOrEmpty(strInparam))
                {
                    strclass.AppendSpaceLine(2, "/// <summary>");
                    strclass.AppendSpaceLine(2, "/// 是否存在该记录");
                    strclass.AppendSpaceLine(2, "/// </summary>");
                    strclass.AppendSpaceLine(2, "bool Exists(" + strInparam + ");");
                }
            }
            return strclass.ToString();
        }

        public string CreatExists2()
        {
            StringPlus strclass = new StringPlus();
            if (Keys.Count > 0)
            {
                string strInparam = Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false);
                if (!string.IsNullOrEmpty(strInparam))
                {
                    strclass.AppendSpaceLine(2, "/// <summary>");
                    strclass.AppendSpaceLine(2, "/// 是否存在该记录");
                    strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
                    strclass.AppendSpaceLine(2, "/// </summary>");
                    strclass.AppendSpaceLine(2, "bool ExistsWhere(string strWhere);");
                }
            }
            return strclass.ToString();
        }

        public string CreatAdd()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 增加一条数据");
            strclass.AppendSpaceLine(2, "/// </summary>");

            string strretu = "bool";
            if ((DbType == "SQL2000" || DbType == "SQL2005" || DbType == "SQL2008" || DbType == "SQL2012" || DbType == "SQLite" || DbType == "MySQL") && (IsHasIdentity))            
            {
                strretu = "int";
                if (_IdentityKeyType != "int")
                {
                    strretu = _IdentityKeyType;
                }
            }
            strclass.AppendSpaceLine(2, strretu + " Add(" + ModelSpace + " model);");
            return strclass.ToString();
        }

        public string CreatUpdate()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 更新一条数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "bool Update(" + ModelSpace + " model);");
            return strclass.ToString();
        }
        public string CreatUpdate2()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 更新一条数据");
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "bool Update(string strWhere,string[] fields,object[] values);");
            return strclass.ToString();
        }

        /*public string CreatAddOrUpdate()//此方法必须要有自增长列，或者主键列才能使用
        {
            StringPlus strclass = new StringPlus();

            if (Keys.Count > 0)
            {
                string strInparam = Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false);
                if (!string.IsNullOrEmpty(strInparam))
                {
                    strclass.AppendSpaceLine(2, "/// <summary>");
                    strclass.AppendSpaceLine(2, "/// " + "增加或更新一条数据");
                    strclass.AppendSpaceLine(2, "/// </summary>");

                    string strretu = "int"; //这里我只要两种类型
                    if ((DbType == "SQL2000" || DbType == "SQL2005" || DbType == "SQL2008" || DbType == "SQL2012" ||
                         DbType == "SQLite" || DbType == "MySQL") && (IsHasIdentity))
                    {
                        if (_IdentityKeyType == "long")
                        {
                            strretu = _IdentityKeyType;
                        }
                    }
                    strclass.AppendSpaceLine(2, strretu + " AddOrUpdate(" + ModelSpace + " model);");
                }
            }
            return strclass.ToString();
        }*/

        public string CreatDelete()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 删除一条数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ");");

            #region 联合主键优先的删除(既有标识字段，又有非标识主键字段)

            if ((Maticsoft.CodeHelper.CodeCommon.HasNoIdentityKey(Keys)) && (Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(Keys) != null))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// 删除一条数据");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false) + ");");
            }

            #endregion

            #region 批量删除方法
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
                strclass.AppendSpaceLine(2, "/// 删除一条数据");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "bool DeleteList(string " + keyField + "list );");
            }

            #endregion

            return strclass.ToString();
        }
        public string CreatDelete2()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 删除一条数据");
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "bool DeleteWhere(string strWhere);");

            return strclass.ToString();
        }

        public string CreatGetModel()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 得到一个对象实体");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, ModelSpace + " GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ");");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 转换实体");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, ModelSpace + " DataRowToModel(DataRow row);");
            return strclass.ToString();
        }
        public string CreatGetModel2()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 得到一个对象实体");
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, ModelSpace + " GetModelWhere(string strWhere);");
            return strclass.ToString();
        }

        public string CreatGetList()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 自由查询");
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "DataSet SelectList(string filed, string where, string table=\"\");");

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 获得数据列表");
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "DataSet GetList(string strWhere);");

            if ((DbType == "SQL2000") ||
            (DbType == "SQL2005") ||
            (DbType == "SQL2008") ||
            (DbType == "SQL2012") || 
            (DbType == "MySQL"))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// 获得前几行数据");
                strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "DataSet GetList(int Top,string strWhere,string filedOrder);");
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// 得到总行数");
                strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "int GetRecordCount(string strWhere);");
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// 根据分页获得数据列表");
                strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex);");
            }

            return strclass.ToString();
        }

        public string CreatGetList2()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 获得数据列表");
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, " List<" + ModelSpace + "> GetListModel(string strWhere);");

            if ((DbType == "SQL2000") ||
            (DbType == "SQL2005") ||
            (DbType == "SQL2008") ||
            (DbType == "SQL2012") ||
            (DbType == "MySQL"))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// 获得前几行数据");
                strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, " List<" + ModelSpace + "> GetListModel(int Top,string strWhere,string filedOrder);");

                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// 根据分页获得数据列表");
                strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, " List<" + ModelSpace + "> GetListModelByPage(string strWhere, string orderby, int startIndex, int endIndex);");
            }

            return strclass.ToString();
        }
        #endregion
    }
}
