using System;

namespace Maticsoft.Web.Admin
{
	/// <summary>
	/// toindex ��ժҪ˵����
	/// </summary>
	public partial class toindex : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{			
			Response.Clear();
			Response.Redirect("Login.aspx");
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
