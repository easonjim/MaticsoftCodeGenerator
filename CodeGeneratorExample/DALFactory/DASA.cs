using System;
using System.Reflection;
using System.Configuration;
using JSoft.IDAL.SA;
namespace JSoft.DALFactory
{
	/// <summary>
	/// 抽象工厂模式创建DAL。
	/// web.config 需要加入配置：(利用工厂模式+反射机制+缓存机制,实现动态创建不同的数据层对象接口) 
	/// DataCache类在导出代码的文件夹里
	/// <appSettings> 
	/// <add key="DAL" value="JSoft.SQLServerDAL.SA" /> (这里的命名空间根据实际情况更改为自己项目的命名空间)
	/// </appSettings> 
	/// </summary>
	public sealed class DASA:DataAccessBase
	{
		/// <summary>
		/// 创建Tree数据层接口。
		/// </summary>
		public static JSoft.IDAL.SA.ITree CreateTree()
		{

			string ClassNamespace = AssemblyPath +".SA.Tree";
			object objType=CreateObject(AssemblyPath,ClassNamespace);
			return (JSoft.IDAL.SA.ITree)objType;
		}

	}
}
