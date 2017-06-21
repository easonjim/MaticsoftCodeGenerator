using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LTP.Accounts.Bus;
namespace Maticsoft.Web.Accounts
{
	/// <summary>
	/// Index ��ժҪ˵����
	/// </summary>
	public partial class Index :System.Web.UI.Page// Maticsoft.Web.Accounts.MoviePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{            			
			if (!Context.User.Identity.IsAuthenticated )
			{
				Session["message"]="��û��ͨ��Ȩ����ˣ�";
				Session["returnPage"]=Request.RawUrl;
				Response.Redirect("../Login.aspx",true);
			}
            
            AccountsPrincipal user=new AccountsPrincipal(Context.User.Identity.Name);			
			if(!user.HasPermission("�ʻ�����"))
			{
				Session["message"]="��û���ʻ������Ȩ�ޣ�";
				Session["returnPage"]=Request.RawUrl;
				Response.Redirect("../Login.aspx",true);
			}

//			int i=user.Roles.Count;
//			string s=user.Roles[0].ToString();
//			bool b=user.Roles.Contains("����Ա");
//			i=user.Permissions.Count;
//			s=user.Permissions[0].ToString();
//			b=user.Permissions.Contains("�ʻ�����");



/*
			Context.User = new AccountsPrincipal(Context.User.Identity.Name);
			if(!((AccountsPrincipal)Context.User).HasPermission("�ʻ�����"))
			{
				Session["message"]="��û���ʻ������Ȩ�ޣ�";
				Session["returnPage"]=Request.RawUrl;
				Response.Redirect("../Login.aspx",true);
			}
*/



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
