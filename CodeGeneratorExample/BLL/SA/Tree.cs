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
using System.Collections.Generic;
using JSoft.Common;
using JSoft.Model.SA;
using JSoft.DALFactory;
using JSoft.IDAL.SA;
namespace JSoft.BLL.SA
{
	/// <summary>
	/// 【BLL】: Tree
	/// </summary>
	public partial class Tree
	{
		private readonly ITree dal=DASA.CreateTree();
		public Tree()
		{}
		#region  BasicMethod

		#region Maxid

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}


		/// <summary>
		/// 根据条件得到最大ID
		/// </summary>
		public object GetMaxId(string fieldName, string strWhere)
		{
			return dal.GetMaxId(fieldName, strWhere);
		}

		#endregion

		#region Exists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int NodeID)
		{
			return dal.Exists(NodeID);
		}

		/// <summary>
		/// 是否存在该记录
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public bool ExistsWhere(string strWhere)
		{
			return dal.ExistsWhere(strWhere);
		}

		#endregion

		#region Add
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(JSoft.Model.SA.Tree model, bool resetCache = false)
		{
			int  flag = dal.Add(model);
			if (resetCache && (flag>0)) { JSoft.Common.DataCache.RemoveByPattern("TreeModel(.*)");/*清除缓存，通过正则删除缓存*/ }
			return flag;
		}

		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(JSoft.Model.SA.Tree model, bool resetCache = false)
		{
			bool flag = dal.Update(model);
			if (resetCache && (flag)) { JSoft.Common.DataCache.RemoveByPattern("TreeModel(.*)");/*清除缓存，通过正则删除缓存*/ }
			return flag;
		}

		/// <summary>
		/// 更新一条数据
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public bool Update(string strWhere,string[] fields,object[] values, bool resetCache = false)
		{
			bool flag = dal.Update(strWhere,fields,values);
			if (resetCache && (flag)) { JSoft.Common.DataCache.RemoveByPattern("TreeModel(.*)");/*清除缓存，通过正则删除缓存*/ }
			return flag;
		}

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int NodeID, bool resetCache = false)
		{
			
			bool flag = dal.Delete(NodeID);
			if (resetCache && (flag)) { JSoft.Common.DataCache.RemoveByPattern("TreeModel(.*)");/*清除缓存，通过正则删除缓存*/ }
			return flag;
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string NodeIDlist , bool resetCache = false)
		{
			bool flag = dal.DeleteList(NodeIDlist );
			if (resetCache && (flag)) { JSoft.Common.DataCache.RemoveByPattern("TreeModel(.*)");/*清除缓存，通过正则删除缓存*/ }
			return flag;
		}

		/// <summary>
		/// 删除一条数据
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public bool DeleteWhere(string strWhere, bool resetCache = false)
		{
			
			bool flag = dal.DeleteWhere(strWhere);
			if (resetCache && (flag)) { JSoft.Common.DataCache.RemoveByPattern("TreeModel(.*)");/*清除缓存，通过正则删除缓存*/ }
			return flag;
		}

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public JSoft.Model.SA.Tree GetModel(int NodeID)
		{
			
			return dal.GetModel(NodeID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public JSoft.Model.SA.Tree GetModelWhere(string strWhere)
		{
			
			return dal.GetModelWhere(strWhere);
		}

		/// <summary>
		/// 获得数据实体
		/// </summary>
		public JSoft.Model.SA.Tree DataRowToModel(DataRow row)
		{
			return dal.DataRowToModel(row);
		}

		#endregion

		#region GetModelByCache
		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public JSoft.Model.SA.Tree GetModelByCache(int NodeID,bool resetCache=false,DateTime? cacheTime = null)
		{
			
			string CacheKey = "TreeModel-" + NodeID;
			if (resetCache) { JSoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存
			object objModel = JSoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(NodeID);
					if (objModel != null)
					{
						int ModelCache = JSoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						JSoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (JSoft.Model.SA.Tree)objModel;
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public JSoft.Model.SA.Tree GetModelWhereByCache(string strWhere,bool resetCache=false,DateTime? cacheTime = null)
		{
			
			string CacheKey = "TreeModel-" + strWhere;
			if (resetCache) { JSoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存
			object objModel = JSoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModelWhere(strWhere);
					if (objModel != null)
					{
						int ModelCache = JSoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						JSoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (JSoft.Model.SA.Tree)objModel;
		}

		/// <summary>
		/// 获得数据列表,从缓存中
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public List<JSoft.Model.SA.Tree> GetModelListByCache(string strWhere,bool resetCache=false,DateTime? cacheTime = null)
		{
			
			string CacheKey = "TreeModel-GetModelListByCache-" + strWhere;
			if (resetCache) { JSoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存
			object objModel = JSoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					DataSet ds = dal.GetList(strWhere);
					objModel = DataTableToList(ds.Tables[0]);
					if (objModel != null)
					{
						int ModelCache = JSoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						JSoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (List<JSoft.Model.SA.Tree>)objModel;
		}

		/// <summary>
		/// 获得前几行数据,从缓存中
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public List<JSoft.Model.SA.Tree> GetModelListByCache(int Top,string strWhere,string filedOrder,bool resetCache=false,DateTime? cacheTime = null)
		{
			string CacheKey = "TreeModel-GetModelListByCache-" + Top + strWhere + filedOrder;
			if (resetCache) { JSoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存
			object objModel = JSoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					DataSet ds = dal.GetList(Top, strWhere, filedOrder);
					objModel = DataTableToList(ds.Tables[0]);
					if (objModel != null)
					{
						int ModelCache = JSoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						JSoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (List<JSoft.Model.SA.Tree>)objModel;
		}
		/// <summary>
		/// 获得数据列表,从缓存中
		/// </summary>
		public List<JSoft.Model.SA.Tree> GetAllModelListByCache(bool resetCache=false,DateTime? cacheTime = null)
		{
			string CacheKey = "TreeModel-GetAllModelListByCache-" ;
			if (resetCache) { JSoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存
			object objModel = JSoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = GetModelList("");
					if (objModel != null)
					{
						int ModelCache = JSoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						JSoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (List<JSoft.Model.SA.Tree>)objModel;
		}

		/// <summary>
		/// 分页获取数据列表,从缓存中
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public List<JSoft.Model.SA.Tree> GetModelListByPageByCache(string strWhere, string orderby, int startIndex, int endIndex,bool resetCache=false,DateTime? cacheTime = null)
		{
			string CacheKey = "TreeModel-GetModelListByPageByCache-" + orderby + strWhere + startIndex + endIndex;
			if (resetCache) { JSoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存
			object objModel = JSoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					DataSet ds = dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
					objModel = DataTableToList(ds.Tables[0]);
					if (objModel != null)
					{
						int ModelCache = JSoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						JSoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (List<JSoft.Model.SA.Tree>)objModel;
		}
		/// <summary>
		/// 分页获取数据列表,从缓存中
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public int GetRecordCountByCache(string strWhere,bool resetCache=false,DateTime? cacheTime = null)
		{
			string CacheKey = "TreeModel-GetRecordCountByCache-" + strWhere;
			if (resetCache) { JSoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存
			object objModel = JSoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetRecordCount(strWhere);
					if (objModel != null)
					{
						int ModelCache = JSoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						JSoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (int)objModel;
		}

		/// <summary>
		/// 自由查询,从缓存中
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public DataSet SelectListByCache(string filed = "*", string where = "", string table = "",bool resetCache=false,DateTime? cacheTime = null)
		{
			string CacheKey = "TreeModel-SelectListByCache-" + where + table;
			if (resetCache) { JSoft.Common.DataCache.DeleteCache(CacheKey); }//清除缓存
			object objModel = JSoft.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.SelectList(filed, where, table);
					if (objModel != null)
					{
						int ModelCache = JSoft.Common.ConfigHelper.GetConfigInt("ModelCache");
						JSoft.Common.DataCache.SetCache(CacheKey, objModel,cacheTime ?? DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (DataSet)objModel;
		}

		/// <summary>
		/// 清除当前业务逻辑所有缓存
		/// </summary>
		public void RemoveCache(string key = null)
		{
			JSoft.Common.DataCache.RemoveByPattern(key ?? "TreeModel(.*)");/*清除缓存，通过正则删除缓存*/
		}

		#endregion

		#region List
		/// <summary>
		/// 自由查询
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public DataSet SelectList(string filed="*", string where="", string table="")
		{
			return dal.SelectList(filed, where, table);
		}
		/// <summary>
		/// 获得数据列表
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public List<JSoft.Model.SA.Tree> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<JSoft.Model.SA.Tree> DataTableToList(DataTable dt)
		{
			List<JSoft.Model.SA.Tree> modelList = new List<JSoft.Model.SA.Tree>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				JSoft.Model.SA.Tree model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得前几行数据
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public List<JSoft.Model.SA.Tree> GetModelList(int Top,string strWhere,string filedOrder)
		{
			DataSet ds = dal.GetList(Top, strWhere, filedOrder);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<JSoft.Model.SA.Tree> GetAllModelList()
		{
			return GetModelList("");
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public List<JSoft.Model.SA.Tree> GetModelListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			DataSet ds = dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
			return DataTableToList(ds.Tables[0]);
		}

		/// <summary>
		/// 分页获取数据列表
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}

		#endregion

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

