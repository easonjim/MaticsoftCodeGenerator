using System;

namespace Maticsoft.Web.SysManage
{
	/// <summary>
	/// delete ��ժҪ˵����
	/// </summary>
	public partial class delete : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();
				string id=Request.Params["id"];
				sm.DelTreeNode(int.Parse(id));				
				Response.Redirect("treelist.aspx");
			}
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
