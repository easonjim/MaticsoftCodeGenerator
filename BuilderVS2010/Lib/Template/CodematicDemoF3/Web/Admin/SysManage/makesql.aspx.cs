using System;

namespace Maticsoft.Web.SysManage
{
	/// <summary>
	/// makesql ��ժҪ˵����
	/// </summary>
	public partial class makesql : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
//			Session["Modulesql"]="";
			Session["strWheresys"]="";
			Response.Redirect("treelist.aspx?page=1");
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
