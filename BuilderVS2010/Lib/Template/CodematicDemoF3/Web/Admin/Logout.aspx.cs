using System;
using System.Web.Security;
namespace Maticsoft.Web.Admin
{
	/// <summary>
	/// Logout ��ժҪ˵����
	/// </summary>
	public partial class Logout : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			FormsAuthentication.SignOut();	
			Session.Clear();
			Session.Abandon();
			Response.Clear();
			Response.Write("<script language=javascript>window.close();</script>");
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
