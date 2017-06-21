using System;
using System.Web.Security;
namespace Maticsoft.Web.Admin
{
	/// <summary>
	/// Relogin ��ժҪ˵����
	/// </summary>
	public partial class Relogin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			FormsAuthentication.SignOut();
			Session.Clear();
			Session.Abandon();
			Response.Clear();
			Response.Redirect("Login.aspx");
			Response.End();
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
