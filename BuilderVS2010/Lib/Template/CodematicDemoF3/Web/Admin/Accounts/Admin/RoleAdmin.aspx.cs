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

namespace Maticsoft.Web.Accounts.Admin
{
	/// <summary>
	/// Index ��ժҪ˵����
	/// </summary>
	public partial class RoleAdmin : System.Web.UI.Page//Maticsoft.Web.Accounts.MoviePage
	{

		private DataSet roles;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
//			AccountsPrincipal currentPrincipal=new AccountsPrincipal( Context.User.Identity.Name );

//			AccountsPrincipal currentPrincipal = (AccountsPrincipal)Context.User;
//			if (!currentPrincipal.HasPermission((int)AccountsPermissions.CreateRoles))
//			{
//				NewRoleButton.Visible = false;
//				NewRoleDescription.Visible = false;
//			}
//			else 
//			{
//				NewRoleButton.Visible = true;
//				NewRoleDescription.Visible = true;
//			}
			dataBind();
		}
		private void dataBind()
		{
			roles = AccountsTool.GetRoleList();
			RoleList.DataSource = roles.Tables["Roles"];
			RoleList.DataBind();
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			base.OnInit(e);
			InitializeComponent();
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
			this.BtnAdd.Click += new System.Web.UI.ImageClickEventHandler(this.BtnAdd_Click);

		}
		#endregion


		private void BtnAdd_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Role role=new Role();
			role.Description=TextBox1.Text;
			try
			{
				role.Create();
			}
			catch{}
			TextBox1.Text="";
			dataBind();
		
		}

	}
}
