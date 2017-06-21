/**  模板制作：Create by Jim
* Tree.cs
*
* 功 能： N/A
* 类 名： Tree
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/6/21 0:27:14   N/A    初版
*
*/
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using JSoft.IDAL.SA;
using JSoft.DBUtility;//Please add references
namespace JSoft.SQLServerDAL.SA
{
	/// <summary>
	/// 【DAL】: Tree
	/// </summary>
	public partial class Tree:ITree
	{
		public Tree()
		{}
		#region  BasicMethod

		#region Maxid

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			string strsql = "select max(" + "NodeID" + ")+1 from " + "SA_Tree";
			object obj = DbHelperSQL.GetSingle(strsql);
			if (obj == null)
			{
				return 1;
			}
			else
			{
				return Convert.ToInt32(obj.ToString());
			}
		}


		/// <summary>
		/// 根据条件得到最大ID
		/// </summary>
		public object GetMaxId(string fieldName, string strWhere)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SA_Tree");
			if (strWhere.Trim() != "")
			{
				strSql.Append(" where " + strWhere);
			}
			string strsql = "select max(" + fieldName + ")+1 from " + strSql.ToString();
			object obj = DbHelperSQL.GetSingle(strsql);
			if (obj == null)
			{
				return 1;
			}
			else
			{
				return obj;
			}
		}

		#endregion

		#region Exists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int NodeID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from SA_Tree");
			strSql.Append(" where NodeID=@NodeID");
			SqlParameter[] parameters = {
					new SqlParameter("@NodeID", SqlDbType.Int,4)
			};
			parameters[0].Value = NodeID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 是否存在该记录
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public bool ExistsWhere(string strWhere)
		{
			if (string.IsNullOrEmpty(strWhere))
			{
				return false;
			}
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from SA_Tree");
			strSql.Append(" where "+strWhere);
			return DbHelperSQL.Exists(strSql.ToString());
		}

		#endregion

		#region Add

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(JSoft.Model.SA.Tree model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into SA_Tree(");
			strSql.Append("TreeText,ParentID,ParentPath,Location,OrderID,Comment,Url,PermissionID,ImageUrl,ModuleID,KeShiDM,KeshiPublic,TreeType,Enabled)");
			strSql.Append(" values (");
			strSql.Append("@TreeText,@ParentID,@ParentPath,@Location,@OrderID,@Comment,@Url,@PermissionID,@ImageUrl,@ModuleID,@KeShiDM,@KeshiPublic,@TreeType,@Enabled)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@TreeText", SqlDbType.NVarChar,100),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@ParentPath", SqlDbType.NVarChar,50),
					new SqlParameter("@Location", SqlDbType.NVarChar,50),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@Comment", SqlDbType.NVarChar,50),
					new SqlParameter("@Url", SqlDbType.NVarChar,100),
					new SqlParameter("@PermissionID", SqlDbType.Int,4),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,100),
					new SqlParameter("@ModuleID", SqlDbType.Int,4),
					new SqlParameter("@KeShiDM", SqlDbType.Int,4),
					new SqlParameter("@KeshiPublic", SqlDbType.NVarChar,50),
					new SqlParameter("@TreeType", SqlDbType.SmallInt,2),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
			parameters[0].Value = model.TreeText;
			parameters[1].Value = model.ParentID;
			parameters[2].Value = model.ParentPath;
			parameters[3].Value = model.Location;
			parameters[4].Value = model.OrderID;
			parameters[5].Value = model.Comment;
			parameters[6].Value = model.Url;
			parameters[7].Value = model.PermissionID;
			parameters[8].Value = model.ImageUrl;
			parameters[9].Value = model.ModuleID;
			parameters[10].Value = model.KeShiDM;
			parameters[11].Value = model.KeshiPublic;
			parameters[12].Value = model.TreeType;
			parameters[13].Value = model.Enabled;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(JSoft.Model.SA.Tree model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update SA_Tree set ");
			strSql.Append("TreeText=@TreeText,");
			strSql.Append("ParentID=@ParentID,");
			strSql.Append("ParentPath=@ParentPath,");
			strSql.Append("Location=@Location,");
			strSql.Append("OrderID=@OrderID,");
			strSql.Append("Comment=@Comment,");
			strSql.Append("Url=@Url,");
			strSql.Append("PermissionID=@PermissionID,");
			strSql.Append("ImageUrl=@ImageUrl,");
			strSql.Append("ModuleID=@ModuleID,");
			strSql.Append("KeShiDM=@KeShiDM,");
			strSql.Append("KeshiPublic=@KeshiPublic,");
			strSql.Append("TreeType=@TreeType,");
			strSql.Append("Enabled=@Enabled");
			strSql.Append(" where NodeID=@NodeID");
			SqlParameter[] parameters = {
					new SqlParameter("@TreeText", SqlDbType.NVarChar,100),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@ParentPath", SqlDbType.NVarChar,50),
					new SqlParameter("@Location", SqlDbType.NVarChar,50),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@Comment", SqlDbType.NVarChar,50),
					new SqlParameter("@Url", SqlDbType.NVarChar,100),
					new SqlParameter("@PermissionID", SqlDbType.Int,4),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,100),
					new SqlParameter("@ModuleID", SqlDbType.Int,4),
					new SqlParameter("@KeShiDM", SqlDbType.Int,4),
					new SqlParameter("@KeshiPublic", SqlDbType.NVarChar,50),
					new SqlParameter("@TreeType", SqlDbType.SmallInt,2),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@NodeID", SqlDbType.Int,4)};
			parameters[0].Value = model.TreeText;
			parameters[1].Value = model.ParentID;
			parameters[2].Value = model.ParentPath;
			parameters[3].Value = model.Location;
			parameters[4].Value = model.OrderID;
			parameters[5].Value = model.Comment;
			parameters[6].Value = model.Url;
			parameters[7].Value = model.PermissionID;
			parameters[8].Value = model.ImageUrl;
			parameters[9].Value = model.ModuleID;
			parameters[10].Value = model.KeShiDM;
			parameters[11].Value = model.KeshiPublic;
			parameters[12].Value = model.TreeType;
			parameters[13].Value = model.Enabled;
			parameters[14].Value = model.NodeID;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 批量更新数据
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public bool Update(string strWhere,string[] fields,object[] values)
		{
			if (string.IsNullOrEmpty(strWhere) || fields.Length!= values.Length || fields.Length==0 || values.Length==0)
			{
				return false;
			}
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update SA_Tree set ");
			for (int i = 0; i < fields.Length; i++)
			{
				strSql.Append(fields[i]+"="+values[i]+",");
			}
			strSql.Remove(strSql.Length - 1, 1);
			strSql.Append(" where "+strWhere);
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int NodeID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from SA_Tree ");
			strSql.Append(" where NodeID=@NodeID");
			SqlParameter[] parameters = {
					new SqlParameter("@NodeID", SqlDbType.Int,4)
			};
			parameters[0].Value = NodeID;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string NodeIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from SA_Tree ");
			strSql.Append(" where NodeID in ("+NodeIDlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 批量删除数据
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public bool DeleteWhere(string strWhere)
		{
			
			if (string.IsNullOrEmpty(strWhere))
			{
				return false;
			}
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from SA_Tree ");
			strSql.Append(" where "+strWhere);
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region GetModel

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public JSoft.Model.SA.Tree GetModel(int NodeID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 NodeID,TreeText,ParentID,ParentPath,Location,OrderID,Comment,Url,PermissionID,ImageUrl,ModuleID,KeShiDM,KeshiPublic,TreeType,Enabled from SA_Tree ");
			strSql.Append(" where NodeID=@NodeID");
			SqlParameter[] parameters = {
					new SqlParameter("@NodeID", SqlDbType.Int,4)
			};
			parameters[0].Value = NodeID;

			JSoft.Model.SA.Tree model=new JSoft.Model.SA.Tree();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public JSoft.Model.SA.Tree GetModelWhere(string strWhere)
		{
			
			if (string.IsNullOrEmpty(strWhere))
			{
				return null;
			}
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 NodeID,TreeText,ParentID,ParentPath,Location,OrderID,Comment,Url,PermissionID,ImageUrl,ModuleID,KeShiDM,KeshiPublic,TreeType,Enabled from SA_Tree ");
			strSql.Append(" where "+strWhere);
			JSoft.Model.SA.Tree model=new JSoft.Model.SA.Tree();
			DataSet ds=DbHelperSQL.Query(strSql.ToString());
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public JSoft.Model.SA.Tree DataRowToModel(DataRow row)
		{
			JSoft.Model.SA.Tree model=new JSoft.Model.SA.Tree();
			if (row != null && row.Table!=null)
			{
				if(row.Table.Columns.Contains("NodeID")==true && row["NodeID"]!=null && row["NodeID"].ToString()!="")
				{
					model.NodeID=int.Parse(row["NodeID"].ToString());
				}
				if(row.Table.Columns.Contains("TreeText")==true && row["TreeText"]!=null)
				{
					model.TreeText=row["TreeText"].ToString();
				}
				if(row.Table.Columns.Contains("ParentID")==true && row["ParentID"]!=null && row["ParentID"].ToString()!="")
				{
					model.ParentID=int.Parse(row["ParentID"].ToString());
				}
				if(row.Table.Columns.Contains("ParentPath")==true && row["ParentPath"]!=null)
				{
					model.ParentPath=row["ParentPath"].ToString();
				}
				if(row.Table.Columns.Contains("Location")==true && row["Location"]!=null)
				{
					model.Location=row["Location"].ToString();
				}
				if(row.Table.Columns.Contains("OrderID")==true && row["OrderID"]!=null && row["OrderID"].ToString()!="")
				{
					model.OrderID=int.Parse(row["OrderID"].ToString());
				}
				if(row.Table.Columns.Contains("Comment")==true && row["Comment"]!=null)
				{
					model.Comment=row["Comment"].ToString();
				}
				if(row.Table.Columns.Contains("Url")==true && row["Url"]!=null)
				{
					model.Url=row["Url"].ToString();
				}
				if(row.Table.Columns.Contains("PermissionID")==true && row["PermissionID"]!=null && row["PermissionID"].ToString()!="")
				{
					model.PermissionID=int.Parse(row["PermissionID"].ToString());
				}
				if(row.Table.Columns.Contains("ImageUrl")==true && row["ImageUrl"]!=null)
				{
					model.ImageUrl=row["ImageUrl"].ToString();
				}
				if(row.Table.Columns.Contains("ModuleID")==true && row["ModuleID"]!=null && row["ModuleID"].ToString()!="")
				{
					model.ModuleID=int.Parse(row["ModuleID"].ToString());
				}
				if(row.Table.Columns.Contains("KeShiDM")==true && row["KeShiDM"]!=null && row["KeShiDM"].ToString()!="")
				{
					model.KeShiDM=int.Parse(row["KeShiDM"].ToString());
				}
				if(row.Table.Columns.Contains("KeshiPublic")==true && row["KeshiPublic"]!=null)
				{
					model.KeshiPublic=row["KeshiPublic"].ToString();
				}
				if(row.Table.Columns.Contains("TreeType")==true && row["TreeType"]!=null && row["TreeType"].ToString()!="")
				{
					model.TreeType=int.Parse(row["TreeType"].ToString());
				}
				if(row.Table.Columns.Contains("Enabled")==true && row["Enabled"]!=null && row["Enabled"].ToString()!="")
				{
					if((row["Enabled"].ToString()=="1")||(row["Enabled"].ToString().ToLower()=="true"))
					{
						model.Enabled=true;
					}
					else
					{
						model.Enabled=false;
					}
				}
			}
			return model;
		}

		#endregion

		#region List
		/// <summary>
		/// 自由查询
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public DataSet SelectList(string filed, string where,string table="")
		{
			StringBuilder strSql = new StringBuilder();
			strSql.AppendFormat("select {0} from {1} {2}", filed, string.IsNullOrEmpty(table) ? "SA_Tree" : table, where);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得数据列表
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select NodeID,TreeText,ParentID,ParentPath,Location,OrderID,Comment,Url,PermissionID,ImageUrl,ModuleID,KeShiDM,KeshiPublic,TreeType,Enabled ");
			strSql.Append(" FROM SA_Tree ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" NodeID,TreeText,ParentID,ParentPath,Location,OrderID,Comment,Url,PermissionID,ImageUrl,ModuleID,KeShiDM,KeshiPublic,TreeType,Enabled ");
			strSql.Append(" FROM SA_Tree ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM SA_Tree ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.NodeID desc");
			}
			strSql.Append(")AS Row, T.*  from SA_Tree T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

		#endregion

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

