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
	/// userPass ��ժҪ˵����
	/// </summary>
	public partial class userPass : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
				if (Context.User.Identity.IsAuthenticated)
				{					
					AccountsPrincipal user=new AccountsPrincipal(Context.User.Identity.Name);
					User currentUser=new LTP.Accounts.Bus.User(user);
					this.lblName.Text=currentUser.UserName;					
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

		protected void btnAdd_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid) 
			{			
				SiteIdentity SID=new SiteIdentity(User.Identity.Name);
				if(SID.TestPassword(txtOldPassword.Text)==0)					
				{			
					this.lblMsg.ForeColor=Color.Red;
					this.lblMsg.Text = "ԭ�����������";
				}
				else
					if(this.txtPassword.Text.Trim()!=this.txtPassword1.Text.Trim())
				{
					this.lblMsg.ForeColor=Color.Red;
					this.lblMsg.Text="��������Ĳ�һ�£������ԣ�";
				}
				else
				{
					AccountsPrincipal user=new AccountsPrincipal(Context.User.Identity.Name);
					User currentUser=new LTP.Accounts.Bus.User(user);
				
					currentUser.Password=AccountsPrincipal.EncryptPassword(txtPassword.Text);					

					if (!currentUser.Update())
					{
						this.lblMsg.ForeColor=Color.Red;
						this.lblMsg.Text = "�����û���Ϣ��������";
                        //��־
                        //UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "�û��������ʧ��");
					}
					else 
					{
						this.lblMsg.ForeColor=Color.Blue;
						this.lblMsg.Text = "�û���Ϣ���³ɹ���";
                        //��־
                        //UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "�û�������³ɹ�");
					}
                    
				}
			}

		
		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.txtOldPassword.Text="";
			this.txtPassword.Text="";
			this.txtPassword1.Text="";
		}
	}
}
