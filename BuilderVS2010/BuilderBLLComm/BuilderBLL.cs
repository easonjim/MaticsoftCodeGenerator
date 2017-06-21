using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.BuilderBLLComm
{
    /// <summary>
    /// 业务层代码组件
    /// </summary>
    public class BuilderBLL : IBuilder.IBuilderBLL
    {
        #region 私有变量
        protected string _key = "ID";//默认第一个主键字段		
        protected string _keyType = "int";//默认第一个主键类型        
        #endregion

        #region 公有属性
        private List<ColumnInfo> _fieldlist;
        private List<ColumnInfo> _keys;
        private string _namespace; //顶级命名空间名
        private string _folder;//所在文件夹
        private string _modelspace;
        private string _modelname;//model类名 
        protected string _tabledescription = "";
        private string _bllname;//bll类名    
        private string _dalname;//dal类名    
        private string _modelpath;
        private string _bllpath;
        private string _factorypath;
        private string _idalpath;
        private string _iclass;
        private string _dalpath;
        private string _dalspace;
        private bool isHasIdentity;
        private string dbType;

        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        /// <summary>
        /// 主键或条件字段列表 
        /// </summary>
        public List<ColumnInfo> Keys
        {
            set 
            { 
                _keys = value;
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
            //set { _keys = value; }
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
        /// 所在文件夹，二级命名空间名
        /// </summary>
        public string Folder
        {
            set { _folder = value; }
            get { return _folder; }
        }
        /*============================*/

        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        public string Modelpath
        {
            set { _modelpath = value; }
            get { return _modelpath; }
        }
        /// <summary>
        /// Model类名
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
        /// 表的描述信息
        /// </summary>
        public string TableDescription
        {
            set { _tabledescription = value; }
            get { return _tabledescription; }
        }
        /*============================*/

        /// <summary>
        /// 业务逻辑层的命名空间
        /// </summary>
        public string BLLpath
        {
            set { _bllpath = value; }
            get { return _bllpath; }
        }
        /// <summary>
        /// BLL类名
        /// </summary>
        public string BLLName
        {
            set { _bllname = value; }
            get { return _bllname; }
        }

        /*============================*/

        /// <summary>
        /// 数据层的命名空间
        /// </summary>
        public string DALpath
        {
            set { _dalpath = value; }
            get { return _dalpath; }
        }
        /// <summary>
        /// DAL类名
        /// </summary>
        public string DALName
        {
            set { _dalname = value; }
            get { return _dalname; }
        }

        /// <summary>
        /// 数据层的命名空间+ 类名，即等于 DALpath + DALName
        /// </summary>
        public string DALSpace
        {
            get { return DALpath + "." + DALName; }
        }

        /*============================*/
        /// <summary>
        /// 工厂类的命名空间
        /// </summary>
        public string Factorypath
        {
            set { _factorypath = value; }
            get { return _factorypath; }
        }
        /// <summary>
        /// 接口的命名空间
        /// </summary>
        public string IDALpath
        {
            set { _idalpath = value; }
            get { return _idalpath; }
        }
        /// <summary>
        /// 接口名
        /// </summary>
        public string IClass
        {
            set { _iclass = value; }
            get { return _iclass; }
        }

        /*============================*/

        /// <summary>
        /// 是否有自动增长标识列
        /// </summary>
        public bool IsHasIdentity
        {
            set { isHasIdentity = value; }
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
        public string DbType
        {
            set { dbType = value; }
            get { return dbType; }
        }
        /// <summary>
        /// 主键标识字段
        /// </summary>
        public string Key
        {
            get
            {
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
                return _key;
            }
        }
        private string KeysNullTip
        {
            get
            {
                if (_keys.Count == 0)
                {
                    return "//该表无主键信息，请自定义主键/条件字段";
                }
                else
                {
                    return "";
                }
            }
        }

        //语言文件
        public Hashtable Languagelist
        {
            get
            {
                return Maticsoft.CodeHelper.Language.LoadFromCfg("BuilderBLLComm.lan");
            }
        }
        #endregion

        #region  构造函数
        public BuilderBLL()
        {
            
        }
        public BuilderBLL(List<ColumnInfo> keys, string modelspace)
        {
            _modelspace = modelspace;
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

        #region 业务层方法
        /// <summary>
        /// 得到整个类的代码
        /// </summary>      
        public string GetBLLCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Collections.Generic;");
            if (GetModelByCache)
            {
                strclass.AppendLine("using Maticsoft.Common;");
            }
            strclass.AppendLine("using " + Modelpath + ";");
            if ((Factorypath != "") && (Factorypath != null))
            {
                strclass.AppendLine("using " + Factorypath + ";");
            }
            if ((IDALpath != "") && (IDALpath != null))
            {
                strclass.AppendLine("using " + IDALpath + ";");
            }
            strclass.AppendLine("namespace " + BLLpath);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            if (TableDescription.Length > 0)
            {
                strclass.AppendSpaceLine(1, "/// 【BLL】: " + TableDescription.Replace("\r\n", "\r\n\t///"));
            }
            else
            {
                strclass.AppendSpaceLine(1, "/// 【BLL】: " + BLLName /*+ ":" + Languagelist["summary"].ToString()*/);
            }
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "public partial class " + BLLName);
            strclass.AppendSpaceLine(1, "{");

            if ((IClass != "") && (IClass != null))
            {
                strclass.AppendSpaceLine(2, "private readonly " + IClass + " dal=" + "DataAccess.Create" + DALName + "();");
                //if (Folder != "")
                //{
                //    strclass.AppendSpaceLine(2, "private readonly " + IClass + " dal=" + "DataAccess<" + IClass + ">.Create(\"" + Folder + "." + DALName + "\");");
                //}
                //else
                //{                    
                //    strclass.AppendSpaceLine(2, "private readonly " + IClass + " dal=" + "DataAccess<" + IClass + ">.Create(\"" + DALName + "\");");
                //}                
            }
            else
            {
                strclass.AppendSpaceLine(2, "private readonly " + DALSpace + " dal=" + "new " + DALSpace + "();");
            }
            strclass.AppendSpaceLine(2, "public " + BLLName + "()");
            strclass.AppendSpaceLine(2, "{}");
            strclass.AppendSpaceLine(2, "#region  BasicMethod");

            #region  方法代码
            if (Maxid)
            {
                if (Keys.Count > 0)
                {
                    strclass.AppendLine();
                    strclass.AppendSpaceLine(2,"#region Maxid");
                    foreach (ColumnInfo obj in Keys)
                    {
                        if (CodeCommon.DbTypeToCS(obj.TypeName) == "int" || CodeCommon.DbTypeToCS(obj.TypeName) == "long")
                        {
                            if (obj.IsPrimaryKey)
                            {
                                strclass.AppendLine(CreatBLLGetMaxID());
                                break;
                            }
                        }
                    }
                    strclass.AppendLine(CreatBLLGetMaxID2());//新增方法
                    strclass.AppendSpaceLine(2,"#endregion");
                }
            }
            if (Exists)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region Exists");
                strclass.AppendLine(CreatBLLExists());
                strclass.AppendLine(CreatBLLExists2());//新增方法
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (Add)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region Add");
                strclass.AppendLine(CreatBLLADD());
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (Update)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region Update");
                strclass.AppendLine(CreatBLLUpdate());
                strclass.AppendLine(CreatBLLUpdate2());//新增方法
                //strclass.AppendLine(CreatBLLAddOrUpdate());//新增方法
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (Delete)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region Delete");
                strclass.AppendLine(CreatBLLDelete());
                strclass.AppendLine(CreatBLLDelete2());//新增方法
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (GetModel)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region GetModel");
                strclass.AppendLine(CreatBLLGetModel());
                strclass.AppendLine(CreatBLLGetModel2());//新增方法
                strclass.AppendLine(CreatBLLDataRowToModel());//新增方法
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (GetModelByCache)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region GetModelByCache");
                strclass.AppendLine(CreatBLLGetModelByCache(ModelName));
                strclass.AppendLine(CreatBLLGetModelByCache2(ModelName));//新增方法
                strclass.AppendLine(CreatBLLGetListByCache());//新增方法
                strclass.AppendLine(CreatBLLGetList2ByCache());//新增方法
                strclass.AppendLine(CreatBLLGetListByPage2ByCache());//新增方法
                strclass.AppendLine(CreatBLLSelectListByPage2ByCache());//新增方法
                strclass.AppendLine(CreatBLLRemoveCache(ModelName));//新增方法
                strclass.AppendSpaceLine(2,"#endregion");
            }
            if (List)
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2,"#region List");
                strclass.AppendLine(CreatBLLGetList());
                strclass.AppendLine(CreatBLLGetList2());//新增方法
                strclass.AppendLine(CreatBLLGetAllList());
                strclass.AppendLine(CreatBLLGetListByPage2());//新增方法
                strclass.AppendLine(CreatBLLGetListByPage());
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

            if (BLLpath.Contains("Maticsoft"))//如果为默认命名空间直接返回
            {
                return strclass.ToString();
            }
            else//否则直接替换原始命名空间
            {
                return strclass.ToString().Replace("Maticsoft", BLLpath.Split('.')[0]);
            }
        }

        #endregion

        #region 具体方法代码

        public string CreatBLLGetMaxID()
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
                            strclass.AppendSpaceLine(3, "return dal.GetMaxId();");
                            strclass.AppendSpaceLine(2, "}");
                            break;
                        }
                    }
                }
            }
            return strclass.ToString();

        }
        public string CreatBLLGetMaxID2()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 根据条件得到最大ID");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public object GetMaxId(string fieldName, string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return dal.GetMaxId(fieldName, strWhere);");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();

        }
        public string CreatBLLExists()
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
                    strclass.AppendSpaceLine(3, "return dal.Exists(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, false) + ");");
                    strclass.AppendSpaceLine(2, "}");
                }
                
            }
            return strclass.ToString();
        }
        public string CreatBLLExists2()
        {
            StringPlus strclass = new StringPlus();
            if (_keys.Count > 0)
            {
                string strInparam = Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false);
                if (!string.IsNullOrEmpty(strInparam))
                {
                    strclass.AppendSpaceLine(2, "/// <summary>");
                    strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryExists"].ToString());
                    strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
                    strclass.AppendSpaceLine(2, "/// </summary>");
                    strclass.AppendSpaceLine(2, "public bool ExistsWhere(string strWhere)");
                    strclass.AppendSpaceLine(2, "{");
                    strclass.AppendSpaceLine(3, "return dal.ExistsWhere(strWhere);");
                    strclass.AppendSpaceLine(2, "}");
                }

            }
            return strclass.ToString();
        }
        public string CreatBLLADD()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryadd"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            string strretu = "bool";
            if ((DbType == "SQL2000" || DbType == "SQL2005" || DbType == "SQL2008" || DbType == "SQL2012" || DbType == "SQLite" || DbType == "MySQL") && (IsHasIdentity))    
            {
                strretu = "int ";
                if (_keyType != "int")
                {
                    strretu = _keyType;
                }  
            }
            strclass.AppendSpaceLine(2, "public " + strretu + " Add(" + ModelSpace + " model, bool resetCache = false)");
            strclass.AppendSpaceLine(2, "{");
            if (strretu == "void")
            {
                strclass.AppendSpaceLine(3, "dal.Add(model);");
                //加入清楚缓存
                strclass.AppendSpaceLine(3, "if (resetCache) { Maticsoft.Common.DataCache.RemoveByPattern(\"" + ModelName + "Model(.*)\");/*清除缓存，通过正则删除缓存*/ }");
            }
            else
            {
                //strclass.AppendSpaceLine(3, "return dal.Add(model);");

                strclass.AppendSpaceLine(3,strretu + " flag = dal.Add(model);");
                //加入清楚缓存
                strclass.AppendSpaceLine(3, "if (resetCache && (flag" + (strretu != "bool" ? ">0" : "") + ")) { Maticsoft.Common.DataCache.RemoveByPattern(\"" + ModelName + "Model(.*)\");/*清除缓存，通过正则删除缓存*/ }");
                strclass.AppendSpaceLine(3, "return flag;");

            }
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        public string CreatBLLUpdate()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryUpdate"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Update(" + ModelSpace + " model, bool resetCache = false)");
            strclass.AppendSpaceLine(2, "{");
            //strclass.AppendSpaceLine(3, "return dal.Update(model);");

            strclass.AppendSpaceLine(3, "bool flag = dal.Update(model);");
            //加入清楚缓存
            strclass.AppendSpaceLine(3, "if (resetCache && (flag)) { Maticsoft.Common.DataCache.RemoveByPattern(\"" + ModelName + "Model(.*)\");/*清除缓存，通过正则删除缓存*/ }");
            strclass.AppendSpaceLine(3, "return flag;");

            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        public string CreatBLLUpdate2()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryUpdate"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Update(string strWhere,string[] fields,object[] values, bool resetCache = false)");
            strclass.AppendSpaceLine(2, "{");
            //strclass.AppendSpaceLine(3, "return dal.Update(strWhere,fields,values);");

            strclass.AppendSpaceLine(3, "bool flag = dal.Update(strWhere,fields,values);");
            //加入清楚缓存
            strclass.AppendSpaceLine(3, "if (resetCache && (flag)) { Maticsoft.Common.DataCache.RemoveByPattern(\"" + ModelName + "Model(.*)\");/*清除缓存，通过正则删除缓存*/ }");
            strclass.AppendSpaceLine(3, "return flag;");

            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        /*public string CreatBLLAddOrUpdate()
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
                    string strretu = "int";
                    if ((DbType == "SQL2000" || DbType == "SQL2005" || DbType == "SQL2008" || DbType == "SQL2012" || DbType == "SQLite" || DbType == "MySQL") && (IsHasIdentity))
                    {
                        strretu = "int ";
                        if (_keyType == "int" || _keyType == "long")
                        {
                            strretu = _keyType;
                        }
                    }
                    strclass.AppendSpaceLine(2, "public " + strretu + " AddOrUpdate(" + ModelSpace + " model, bool resetCache = false)");
                    strclass.AppendSpaceLine(2, "{");
                    //strclass.AppendSpaceLine(3, "return dal.AddOrUpdate(model);");
                    strclass.AppendSpaceLine(3, strretu + " flag = dal.AddOrUpdate(model);");
                    //加入清楚缓存
                    strclass.AppendSpaceLine(3, "if (resetCache && (flag" + (strretu != "bool" ? ">0" : "") + ")) { Maticsoft.Common.DataCache.RemoveByPattern(\"" + ModelName + "Model(.*)\");/*清除缓存，通过正则删除缓存#1# }");
                    strclass.AppendSpaceLine(3, "return flag;");
                    strclass.AppendSpaceLine(2, "}");
                }
            }
            return strclass.ToString();
        }*/
        public string CreatBLLDelete()
        {
            StringPlus strclass = new StringPlus();

            #region 标识字段优先的删除
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Delete(" + (Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true).Length>0?Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true)+",":"") + " bool resetCache = false)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            //strclass.AppendSpaceLine(3, "return dal.Delete(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");

            strclass.AppendSpaceLine(3, "bool flag = dal.Delete(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
            //加入清楚缓存
            strclass.AppendSpaceLine(3, "if (resetCache && (flag)) { Maticsoft.Common.DataCache.RemoveByPattern(\"" + ModelName + "Model(.*)\");/*清除缓存，通过正则删除缓存*/ }");
            strclass.AppendSpaceLine(3, "return flag;");

            strclass.AppendSpaceLine(2, "}");

            #endregion

            #region 联合主键优先的删除(既有标识字段，又有非标识主键字段)

            if ((Maticsoft.CodeHelper.CodeCommon.HasNoIdentityKey(Keys)) && (Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(Keys) != null))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false) + ", bool resetCache = false)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, KeysNullTip);
                //strclass.AppendSpaceLine(3, "return dal.Delete(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, false) + ");");

                strclass.AppendSpaceLine(3, "bool flag = dal.Delete(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, false) + ");");
                //加入清楚缓存
                strclass.AppendSpaceLine(3, "if (resetCache && (flag)) { Maticsoft.Common.DataCache.RemoveByPattern(\"" + ModelName + "Model(.*)\");/*清除缓存，通过正则删除缓存*/ }");
                strclass.AppendSpaceLine(3, "return flag;");

                strclass.AppendSpaceLine(2, "}");
            }

            #endregion


            #region 批量删除

            //批量删除方法
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
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool DeleteList(string " + keyField + "list , bool resetCache = false)");
                strclass.AppendSpaceLine(2, "{");
                //strclass.AppendSpaceLine(3, "return dal.DeleteList(" + keyField + "list );");

                strclass.AppendSpaceLine(3, "bool flag = dal.DeleteList(" + keyField + "list );");
                //加入清楚缓存
                strclass.AppendSpaceLine(3, "if (resetCache && (flag)) { Maticsoft.Common.DataCache.RemoveByPattern(\"" + ModelName + "Model(.*)\");/*清除缓存，通过正则删除缓存*/ }");
                strclass.AppendSpaceLine(3, "return flag;");

                strclass.AppendSpaceLine(2, "}");
            }
            #endregion

            return strclass.ToString();
        }
        public string CreatBLLDelete2()
        {
            StringPlus strclass = new StringPlus();

            #region 标识字段优先的删除
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool DeleteWhere(string strWhere, bool resetCache = false)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            //strclass.AppendSpaceLine(3, "return dal.Delete(strWhere);");

            strclass.AppendSpaceLine(3, "bool flag = dal.DeleteWhere(strWhere);");
            //加入清楚缓存
            strclass.AppendSpaceLine(3, "if (resetCache && (flag)) { Maticsoft.Common.DataCache.RemoveByPattern(\"" + ModelName + "Model(.*)\");/*清除缓存，通过正则删除缓存*/ }");
            strclass.AppendSpaceLine(3, "return flag;");

            strclass.AppendSpaceLine(2, "}");

            #endregion

            

            return strclass.ToString();
        }
        public string CreatBLLGetModel()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            strclass.AppendSpaceLine(3, "return dal.GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();

        }
        public string CreatBLLGetModel2()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModelWhere(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            strclass.AppendSpaceLine(3, "return dal.GetModelWhere(strWhere);");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();

        }
        public string CreatBLLDataRowToModel()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2,"/// <summary>");
            strclass.AppendSpaceLine(2,"/// 获得数据实体");
            strclass.AppendSpaceLine(2,"/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " DataRowToModel(DataRow row)");
            strclass.AppendSpaceLine(2,"{");
            strclass.AppendSpaceLine(3,"return dal.DataRowToModel(row);");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();

        }
        public string CreatBLLGetModelByCache(string ModelName)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModelByCache"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModelByCache(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + (string.IsNullOrEmpty(Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true)) ? "" : ",") + "bool resetCache=false,DateTime? cacheTime = null)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            string para = "";
            if (Keys.Count > 0)
            {
                para = "+ " + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlistAdd(Keys, true);
            }
            strclass.AppendSpaceLine(3, "string CacheKey = \"" + ModelName + "Model-\" " + para + ";");
            strclass.AppendSpaceLine(3, "if (resetCache) { Maticsoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存");
            strclass.AppendSpaceLine(3, "object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);");
            strclass.AppendSpaceLine(3, "if (objModel == null)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "try");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "objModel = dal.GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
            strclass.AppendSpaceLine(5, "if (objModel != null)");
            strclass.AppendSpaceLine(5, "{");
            strclass.AppendSpaceLine(6, "int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt(\"ModelCache\");");
            //strclass.AppendSpaceLine(6, "int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache(\"CacheTime\"), 30);");
            strclass.AppendSpaceLine(6, "Maticsoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
            strclass.AppendSpaceLine(5, "}");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(4, "catch{}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return (" + ModelSpace + ")objModel;");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;

        }
        public string CreatBLLGetModelByCache2(string ModelName)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModelByCache"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModelWhereByCache(string strWhere,bool resetCache=false,DateTime? cacheTime = null)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            string para = "";
            if (Keys.Count > 0)
            {
                para = "+ " + "strWhere";
            }
            strclass.AppendSpaceLine(3, "string CacheKey = \"" + ModelName + "Model-\" " + para + ";");
            strclass.AppendSpaceLine(3, "if (resetCache) { Maticsoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存");
            strclass.AppendSpaceLine(3, "object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);");
            strclass.AppendSpaceLine(3, "if (objModel == null)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "try");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "objModel = dal.GetModelWhere(strWhere);");
            strclass.AppendSpaceLine(5, "if (objModel != null)");
            strclass.AppendSpaceLine(5, "{");
            strclass.AppendSpaceLine(6, "int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt(\"ModelCache\");");
            //strclass.AppendSpaceLine(6, "int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache(\"CacheTime\"), 30);");
            strclass.AppendSpaceLine(6, "Maticsoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
            strclass.AppendSpaceLine(5, "}");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(4, "catch{}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return (" + ModelSpace + ")objModel;");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;

        }
        public string CreatBLLRemoveCache(string ModelName)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 清除当前业务逻辑所有缓存");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void RemoveCache(string key = null)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "Maticsoft.Common.DataCache.RemoveByPattern(key ?? \"" + ModelName + "Model(.*)\");/*清除缓存，通过正则删除缓存*/");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;

        }
        public string CreatBLLGetList()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + "自由查询");
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet SelectList(string filed=\"*\", string where=\"\", string table=\"\")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return dal.SelectList(filed, where, table);");
            strclass.AppendSpaceLine(2, "}");
            //返回DataSet
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return dal.GetList(strWhere);");
            strclass.AppendSpaceLine(2, "}");



            //根据权限返回DataSet
            string param = "";            
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                               
            }
                       

            if (param.Length > 1)//至少有其中一个字段
            {

                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere" + param + ")");
                strclass.AppendSpaceLine(2, "{");

                //个性定制-特殊字段处理
                //if (fieldValid.Length > 0)
                //{
                //    strclass.AppendSpaceLine(3, "if(!ShowInvalid)");
                //    strclass.AppendSpaceLine(3, "{");
                //    strclass.AppendSpaceLine(4, "if (strWhere.Length > 1)");
                //    strclass.AppendSpaceLine(4, "{");
                //    strclass.AppendSpaceLine(5, "strWhere += \" and \";");
                //    strclass.AppendSpaceLine(4, "}");
                //    strclass.AppendSpaceLine(4, "strWhere += \" " + fieldValid + "=1 \";");
                //    strclass.AppendSpaceLine(3, "}");
                //}
                //if (fieldAuthority.Length > 0)
                //{
                //    strclass.AppendSpaceLine(3, "if (UserPermissions.Count > 0)");
                //    strclass.AppendSpaceLine(3, "{");
                //    strclass.AppendSpaceLine(4, "if (strWhere.Length > 1)");
                //    strclass.AppendSpaceLine(4, "{");
                //    strclass.AppendSpaceLine(5, "strWhere += \" and \";");
                //    strclass.AppendSpaceLine(4, "}");
                //    strclass.AppendSpaceLine(4, "strWhere += \" " + fieldAuthority + " in (\" + StringPlus.GetArrayStr(UserPermissions) + \")\";");
                //    strclass.AppendSpaceLine(3, "}");
                //}


                strclass.AppendSpaceLine(3, "return dal.GetList(strWhere);");
                strclass.AppendSpaceLine(2, "}");
            }


            if ((DbType == "SQL2000") ||
                (DbType == "SQL2005") ||
                (DbType == "SQL2008") || (DbType == "SQL2012" || DbType == "MySQL"))
            {
                //返回DataSet
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList2"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public DataSet GetList(int Top,string strWhere,string filedOrder)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "return dal.GetList(Top,strWhere,filedOrder);");
                strclass.AppendSpaceLine(2, "}");
            }


            //返回List<>
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetModelList(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "DataSet ds = dal.GetList(strWhere);");
            strclass.AppendSpaceLine(3, "return DataTableToList(ds.Tables[0]);");
            strclass.AppendSpaceLine(2, "}");


            //返回List<>
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> DataTableToList(DataTable dt)");
            strclass.AppendSpaceLine(2, "{");            
            strclass.AppendSpaceLine(3, "List<" + ModelSpace + "> modelList = new List<" + ModelSpace + ">();");
            strclass.AppendSpaceLine(3, "int rowsCount = dt.Rows.Count;");
            strclass.AppendSpaceLine(3, "if (rowsCount > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, ModelSpace + " model;");
            strclass.AppendSpaceLine(4, "for (int n = 0; n < rowsCount; n++)");
            strclass.AppendSpaceLine(4, "{");

            #region 字段赋值
            /*
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;

                strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"]!=null && dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                strclass.AppendSpaceLine(5, "{");
                #region
                switch (CodeCommon.DbTypeToCS(columnType))
                {
                    case "int":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=int.Parse(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "long":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=long.Parse(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "decimal":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=decimal.Parse(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "float":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=float.Parse(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "DateTime":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=DateTime.Parse(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "string":
                        {
                            strclass.AppendSpaceLine(5, "model." + columnName + "=dt.Rows[n][\"" + columnName + "\"].ToString();");
                        }
                        break;
                    case "bool":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "if((dt.Rows[n][\"" + columnName + "\"].ToString()==\"1\")||(dt.Rows[n][\"" + columnName + "\"].ToString().ToLower()==\"true\"))");
                            strclass.AppendSpaceLine(6, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=true;");
                            strclass.AppendSpaceLine(6, "}");
                            strclass.AppendSpaceLine(6, "else");
                            strclass.AppendSpaceLine(6, "{");
                            strclass.AppendSpaceLine(7, "model." + columnName + "=false;");
                            strclass.AppendSpaceLine(6, "}");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "byte[]":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=(byte[])dt.Rows[n][\"" + columnName + "\"];");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "uniqueIdentifier":
                    case "Guid":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=new Guid(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    default:
                        strclass.AppendSpaceLine(5, "//model." + columnName + "=dt.Rows[n][\"" + columnName + "\"].ToString();");
                        break;
                }
                #endregion

                strclass.AppendSpaceLine(5, "}");


            }
*/
            #endregion

            strclass.AppendSpaceLine(5, "model = dal.DataRowToModel(dt.Rows[n]);");
            strclass.AppendSpaceLine(5, "if (model != null)");
            strclass.AppendSpaceLine(5, "{");
            strclass.AppendSpaceLine(6, "modelList.Add(model);");
            strclass.AppendSpaceLine(5, "}");

            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return modelList;");
            strclass.AppendSpaceLine(2, "}");



            return strclass.ToString();

        }
        public string CreatBLLGetListByCache()
        {
            StringPlus strclass = new StringPlus();
            //返回DataSet





            //返回List<>
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString()+",从缓存中");
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetModelListByCache(string strWhere,bool resetCache=false,DateTime? cacheTime = null)");
            strclass.AppendSpaceLine(2, "{");

            strclass.AppendSpaceLine(3, KeysNullTip);
            string para = "";
            if (Keys.Count > 0)
            {
                para = "+ " + "strWhere";
            }
            strclass.AppendSpaceLine(3, "string CacheKey = \"" + ModelName + "Model-GetModelListByCache-\" " + para + ";");
            strclass.AppendSpaceLine(3, "if (resetCache) { Maticsoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存");
            strclass.AppendSpaceLine(3, "object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);");
            strclass.AppendSpaceLine(3, "if (objModel == null)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "try");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "DataSet ds = dal.GetList(strWhere);");
            strclass.AppendSpaceLine(5, "objModel = DataTableToList(ds.Tables[0]);");
            strclass.AppendSpaceLine(5, "if (objModel != null)");
            strclass.AppendSpaceLine(5, "{");
            strclass.AppendSpaceLine(6, "int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt(\"ModelCache\");");
            //strclass.AppendSpaceLine(6, "int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache(\"CacheTime\"), 30);");
            strclass.AppendSpaceLine(6, "Maticsoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
            strclass.AppendSpaceLine(5, "}");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(4, "catch{}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return (List<" + ModelSpace + ">)objModel;");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();

        }
        public string CreatBLLGetList2()
        {
            StringPlus strclass = new StringPlus();
            
            if ((DbType == "SQL2000") ||
                (DbType == "SQL2005") ||
                (DbType == "SQL2008") || (DbType == "SQL2012" || DbType == "MySQL"))
            {
                //返回DataSet
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList2"].ToString());
                strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetModelList(int Top,string strWhere,string filedOrder)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "DataSet ds = dal.GetList(Top, strWhere, filedOrder);");
                strclass.AppendSpaceLine(3, "return DataTableToList(ds.Tables[0]);");
                strclass.AppendSpaceLine(2, "}");
            }

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetAllModelList()");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return GetModelList(\"\");");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();

        }
        public string CreatBLLGetList2ByCache()
        {
            StringPlus strclass = new StringPlus();

            if ((DbType == "SQL2000") ||
                (DbType == "SQL2005") ||
                (DbType == "SQL2008") || (DbType == "SQL2012" || DbType == "MySQL"))
            {
                //返回DataSet
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList2"].ToString() + ",从缓存中");
                strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetModelListByCache(int Top,string strWhere,string filedOrder,bool resetCache=false,DateTime? cacheTime = null)");
                strclass.AppendSpaceLine(2, "{");

                string para = "";
                if (Keys.Count > 0)
                {
                    para = "+ " + "Top + strWhere + filedOrder";
                }
                strclass.AppendSpaceLine(3, "string CacheKey = \"" + ModelName + "Model-GetModelListByCache-\" " + para + ";");
                strclass.AppendSpaceLine(3, "if (resetCache) { Maticsoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存");
                strclass.AppendSpaceLine(3, "object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);");
                strclass.AppendSpaceLine(3, "if (objModel == null)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "try");
                strclass.AppendSpaceLine(4, "{");
                strclass.AppendSpaceLine(5, "DataSet ds = dal.GetList(Top, strWhere, filedOrder);");
                strclass.AppendSpaceLine(5, "objModel = DataTableToList(ds.Tables[0]);");
                strclass.AppendSpaceLine(5, "if (objModel != null)");
                strclass.AppendSpaceLine(5, "{");
                strclass.AppendSpaceLine(6, "int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt(\"ModelCache\");");
                //strclass.AppendSpaceLine(6, "int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache(\"CacheTime\"), 30);");
                strclass.AppendSpaceLine(6, "Maticsoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
                strclass.AppendSpaceLine(5, "}");
                strclass.AppendSpaceLine(4, "}");
                strclass.AppendSpaceLine(4, "catch{}");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "return (List<" + ModelSpace + ">)objModel;");


                strclass.AppendSpaceLine(2, "}");
            }

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString() + ",从缓存中");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetAllModelListByCache(bool resetCache=false,DateTime? cacheTime = null)");
            strclass.AppendSpaceLine(2, "{");

            strclass.AppendSpaceLine(3, "string CacheKey = \"" + ModelName + "Model-GetAllModelListByCache-\" " + "" + ";");
            strclass.AppendSpaceLine(3, "if (resetCache) { Maticsoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存");
            strclass.AppendSpaceLine(3, "object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);");
            strclass.AppendSpaceLine(3, "if (objModel == null)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "try");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "objModel = GetModelList(\"\");");
            strclass.AppendSpaceLine(5, "if (objModel != null)");
            strclass.AppendSpaceLine(5, "{");
            strclass.AppendSpaceLine(6, "int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt(\"ModelCache\");");
            //strclass.AppendSpaceLine(6, "int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache(\"CacheTime\"), 30);");
            strclass.AppendSpaceLine(6, "Maticsoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
            strclass.AppendSpaceLine(5, "}");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(4, "catch{}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return (List<" + ModelSpace + ">)objModel;");


            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();

        }
        public string CreatBLLGetAllList()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetAllList()");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return GetList(\"\");");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        public string CreatBLLGetListByPage()
        {
            StringPlus strclass = new StringPlus();
            
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public int GetRecordCount(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return dal.GetRecordCount(strWhere);");
            strclass.AppendSpaceLine(2, "}");

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);");
            strclass.AppendSpaceLine(2, "}");


            
            return strclass.ToString();
        }
        public string CreatBLLGetListByPage2()
        {
            StringPlus strclass = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetModelListByPage(string strWhere, string orderby, int startIndex, int endIndex)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "DataSet ds = dal.GetListByPage(strWhere, orderby, startIndex, endIndex);");
            strclass.AppendSpaceLine(3, "return DataTableToList(ds.Tables[0]);");
            strclass.AppendSpaceLine(2, "}");



            return strclass.ToString();
        }
        public string CreatBLLGetListByPage2ByCache()
        {
            StringPlus strclass = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString() + ",从缓存中");
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetModelListByPageByCache(string strWhere, string orderby, int startIndex, int endIndex,bool resetCache=false,DateTime? cacheTime = null)");
            strclass.AppendSpaceLine(2, "{");

            string para = "";
            if (Keys.Count > 0)
            {
                para = "+ " + "orderby + strWhere + startIndex + endIndex";
            }
            strclass.AppendSpaceLine(3, "string CacheKey = \"" + ModelName + "Model-GetModelListByPageByCache-\" " + para + ";");
            strclass.AppendSpaceLine(3, "if (resetCache) { Maticsoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存");
            strclass.AppendSpaceLine(3, "object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);");
            strclass.AppendSpaceLine(3, "if (objModel == null)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "try");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "DataSet ds = dal.GetListByPage(strWhere, orderby, startIndex, endIndex);");
            strclass.AppendSpaceLine(5, "objModel = DataTableToList(ds.Tables[0]);");
            strclass.AppendSpaceLine(5, "if (objModel != null)");
            strclass.AppendSpaceLine(5, "{");
            strclass.AppendSpaceLine(6, "int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt(\"ModelCache\");");
            //strclass.AppendSpaceLine(6, "int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache(\"CacheTime\"), 30);");
            strclass.AppendSpaceLine(6, "Maticsoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
            strclass.AppendSpaceLine(5, "}");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(4, "catch{}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return (List<" + ModelSpace + ">)objModel;");

            strclass.AppendSpaceLine(2, "}");

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString() + ",从缓存中");
            strclass.AppendSpaceLine(2, "/// " + "注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public int GetRecordCountByCache(string strWhere,bool resetCache=false,DateTime? cacheTime = null)");
            strclass.AppendSpaceLine(2, "{");

            string para1 = "";
            if (Keys.Count > 0)
            {
                para1 = "+ " + "strWhere";
            }
            strclass.AppendSpaceLine(3, "string CacheKey = \"" + ModelName + "Model-GetRecordCountByCache-\" " + para1 + ";");
            strclass.AppendSpaceLine(3, "if (resetCache) { Maticsoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存");
            strclass.AppendSpaceLine(3, "object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);");
            strclass.AppendSpaceLine(3, "if (objModel == null)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "try");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "objModel = dal.GetRecordCount(strWhere);");
            strclass.AppendSpaceLine(5, "if (objModel != null)");
            strclass.AppendSpaceLine(5, "{");
            strclass.AppendSpaceLine(6, "int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt(\"ModelCache\");");
            //strclass.AppendSpaceLine(6, "int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache(\"CacheTime\"), 30);");
            strclass.AppendSpaceLine(6, "Maticsoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
            strclass.AppendSpaceLine(5, "}");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(4, "catch{}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return (int)objModel;");


            strclass.AppendSpaceLine(2, "}");


            return strclass.ToString();
        }
        public string CreatBLLSelectListByPage2ByCache()
        {
            StringPlus strclass = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 自由查询,从缓存中");
            strclass.AppendSpaceLine(2, "/// 注意：参数要使用防SQL注入过滤");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet SelectListByCache(string filed = \"*\", string where = \"\", string table = \"\",bool resetCache=false,DateTime? cacheTime = null)");
            strclass.AppendSpaceLine(2, "{");

            strclass.AppendSpaceLine(3, "string CacheKey = \"" + ModelName + "Model-SelectListByCache-\" + where + table;");
            strclass.AppendSpaceLine(3, "if (resetCache) { Maticsoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存");
            strclass.AppendSpaceLine(3, "object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);");
            strclass.AppendSpaceLine(3, "if (objModel == null)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "try");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "objModel = dal.SelectList(filed, where, table);");
            strclass.AppendSpaceLine(5, "if (objModel != null)");
            strclass.AppendSpaceLine(5, "{");
            strclass.AppendSpaceLine(6, "int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt(\"ModelCache\");");
            //strclass.AppendSpaceLine(6, "int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache(\"CacheTime\"), 30);");
            strclass.AppendSpaceLine(6, "Maticsoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
            strclass.AppendSpaceLine(5, "}");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(4, "catch{}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return (DataSet)objModel;");


            strclass.AppendSpaceLine(2, "}");


            return strclass.ToString();
        }
        #endregion


    }
}
