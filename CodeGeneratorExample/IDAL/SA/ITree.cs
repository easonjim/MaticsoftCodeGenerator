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
namespace JSoft.IDAL.SA
{
	/// <summary>
	/// 【IDAL】: Tree
	/// </summary>
	public partial interface ITree
	{
		#region  成员方法

		#region Maxid
		/// <summary>
		/// 得到最大ID
		/// </summary>
		int GetMaxId();
		/// <summary>
		/// 根据条件得到最大ID
		/// </summary>
		object GetMaxId(string fieldName, string strWhere);
		#endregion

		#region Exists
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(int NodeID);
		/// <summary>
		/// 是否存在该记录
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		bool ExistsWhere(string strWhere);

		#endregion

		#region Add
		/// <summary>
		/// 增加一条数据
		/// </summary>
		int Add(JSoft.Model.SA.Tree model);
		#endregion

		#region Update
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(JSoft.Model.SA.Tree model);
		/// <summary>
		/// 更新一条数据
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		bool Update(string strWhere,string[] fields,object[] values);

		#endregion

		#region Delete
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(int NodeID);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool DeleteList(string NodeIDlist );
		/// <summary>
		/// 删除一条数据
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		bool DeleteWhere(string strWhere);

		#endregion

		#region GetModel
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		JSoft.Model.SA.Tree GetModel(int NodeID);
		/// <summary>
		/// 转换实体
		/// </summary>
		JSoft.Model.SA.Tree DataRowToModel(DataRow row);
		/// <summary>
		/// 得到一个对象实体
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		JSoft.Model.SA.Tree GetModelWhere(string strWhere);
		#endregion

		#region List
		/// <summary>
		/// 自由查询
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		DataSet SelectList(string filed, string where, string table="");
		/// <summary>
		/// 获得数据列表
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		DataSet GetList(string strWhere);
		/// <summary>
		/// 获得前几行数据
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		DataSet GetList(int Top,string strWhere,string filedOrder);
		/// <summary>
		/// 得到总行数
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		int GetRecordCount(string strWhere);
		/// <summary>
		/// 根据分页获得数据列表
		/// 注意：参数要使用防SQL注入过滤
		/// </summary>
		DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex);
		#endregion

		#endregion  成员方法
		#region  MethodEx

		#endregion  MethodEx
	} 
}
