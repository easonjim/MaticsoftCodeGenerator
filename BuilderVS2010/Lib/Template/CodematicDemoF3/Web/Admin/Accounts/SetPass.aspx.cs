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

namespace Maticsoft.Web.Accounts
{
	/// <summary>
	/// SetPass ��ժҪ˵����
	/// </summary>
	public partial class SetPass : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
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

		protected void btnUpdate_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid) 
			{	
				string username=this.txtUserName.Text;
				string passward=this.txtPassword.Text;
				
				LTP.Accounts.Bus.User currentUser=new LTP.Accounts.Bus.User();				
			
				if (!currentUser.SetPassword(username,passward))
				{
					this.lblMsg.ForeColor=Color.Red;
					this.lblMsg.Text = "�����û���Ϣ��������";
				}
				else 
				{
					this.lblMsg.ForeColor=Color.Blue;
					this.lblMsg.Text = "�û���Ϣ���³ɹ���";
				}
				
			}
		
		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.txtPassword.Text="";
			this.txtPassword1.Text="";
		}
	}
}
