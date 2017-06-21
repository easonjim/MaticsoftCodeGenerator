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
namespace JSoft.Model.SA
{
	/// <summary>
	/// 【Model】: Tree
	/// </summary>
	[Serializable]
	public partial class Tree
	{
		public Tree()
		{}
		#region Model
		private int _nodeid;
		private string _treetext;
		private int _parentid;
		private string _parentpath;
		private string _location;
		private int? _orderid;
		private string _comment;
		private string _url;
		private int? _permissionid;
		private string _imageurl;
		private int? _moduleid;
		private int? _keshidm;
		private string _keshipublic;
		private int _treetype=0;
		private bool _enabled= true;
		/// <summary>
		/// 
		/// </summary>
		public int NodeID
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TreeText
		{
			set{ _treetext=value;}
			get{return _treetext;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ParentID
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ParentPath
		{
			set{ _parentpath=value;}
			get{return _parentpath;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Location
		{
			set{ _location=value;}
			get{return _location;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Comment
		{
			set{ _comment=value;}
			get{return _comment;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Url
		{
			set{ _url=value;}
			get{return _url;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? PermissionID
		{
			set{ _permissionid=value;}
			get{return _permissionid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ImageUrl
		{
			set{ _imageurl=value;}
			get{return _imageurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ModuleID
		{
			set{ _moduleid=value;}
			get{return _moduleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? KeShiDM
		{
			set{ _keshidm=value;}
			get{return _keshidm;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string KeshiPublic
		{
			set{ _keshipublic=value;}
			get{return _keshipublic;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int TreeType
		{
			set{ _treetype=value;}
			get{return _treetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Enabled
		{
			set{ _enabled=value;}
			get{return _enabled;}
		}
		#endregion Model

	}
}

