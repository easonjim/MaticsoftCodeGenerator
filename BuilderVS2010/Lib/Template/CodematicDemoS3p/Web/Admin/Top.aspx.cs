using System;

namespace Maticsoft.Web.Admin
{
	/// <summary>
	/// Top ��ժҪ˵����
	/// </summary>
	public partial class Top : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblMember;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!Page.IsPostBack)
			{
				
				if (!Context.User.Identity.IsAuthenticated )
				{
//					Session["message"]="��û��ͨ��Ȩ����ˣ�";
//					Session["returnPage"]="main.htm";
//					Response.Redirect("Login.aspx",true);
					Response.Clear();
					Response.Write("<script language=javascript>window.alert('��û��Ȩ�޽��뱾ҳ��\\n���¼�������Ա��ϵ��');history.back();</script>");
					Response.End();
				}
				else
				{
//					AccountsPrincipal user=new AccountsPrincipal(Context.User.Identity.Name);
//					LTP.Accounts.Bus.User usern=new LTP.Accounts.Bus.User(user);
					if(Session["UserInfo"]==null)
					{
						return ;
					}
					LTP.Accounts.Bus.User currentUser=(LTP.Accounts.Bus.User)Session["UserInfo"];
					this.lblSignIn.Text=currentUser.TrueName;
				}

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
