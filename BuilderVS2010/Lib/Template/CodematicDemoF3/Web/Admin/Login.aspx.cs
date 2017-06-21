using System;
using LTP.Accounts.Bus;
using System.Web.Security;

namespace Maticsoft.Web.Admin
{
	/// <summary>
	/// Login ��ժҪ˵����
	/// </summary>
	public partial class Login : System.Web.UI.Page
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
			this.btnLogin.Click += new System.Web.UI.ImageClickEventHandler(this.btnLogin_Click);

		}
		#endregion

		private void btnLogin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
            if ((Session["PassErrorCountAdmin"] != null) && (Session["PassErrorCountAdmin"].ToString() != ""))
            {
                int PassErroeCount = Convert.ToInt32(Session["PassErrorCountAdmin"]);
                if (PassErroeCount > 3)
                {
                    txtUsername.Disabled = true;
                    txtPass.Disabled = true;
                    btnLogin.Enabled = false;
                    this.lblMsg.Text = "�Բ���������¼�����Σ�ϵͳ��¼������";
                    return;
                }

            }

            if ((Session["CheckCode"] != null) && (Session["CheckCode"].ToString() != ""))
            {
                if (Session["CheckCode"].ToString().ToLower() != this.CheckCode.Value.ToLower())
                {
                    this.lblMsg.Text = "����д����֤���������Ĳ��� !";
                    Session["CheckCode"] = null;
                    return;
                }
                else
                {
                    Session["CheckCode"] = null;
                }
            }
            else
            {
                Response.Redirect("login.aspx");
            }

			string userName=Maticsoft.Common.PageValidate.InputText(txtUsername.Value.Trim(),30);
			string Password=Maticsoft.Common.PageValidate.InputText(txtPass.Value.Trim(),30);			

			AccountsPrincipal newUser = AccountsPrincipal.ValidateLogin(userName,Password);			
			if (newUser == null)
			{				
				this.lblMsg.Text = "��½ʧ�ܣ� " + userName;
                if ((Session["PassErrorCountAdmin"] != null) && (Session["PassErrorCountAdmin"].ToString() != ""))
                {
                    int PassErroeCount = Convert.ToInt32(Session["PassErrorCountAdmin"]);
                    Session["PassErrorCountAdmin"] = PassErroeCount + 1;
                }
                else
                {
                    Session["PassErrorCountAdmin"] = 1;
                }
			}
			else 
			{				
				User currentUser=new LTP.Accounts.Bus.User(newUser);
                //if (currentUser.UserType != "AA")
                //{
                //    this.lblMsg.Text = "��ǹ���Ա�û�����û��Ȩ�޵�¼��̨ϵͳ��";
                //    return;
                //}
				Context.User = newUser;
				if(((SiteIdentity)User.Identity).TestPassword( Password) == 0)
				{
					this.lblMsg.Text = "���������Ч��";
                    if ((Session["PassErrorCountAdmin"] != null) && (Session["PassErrorCountAdmin"].ToString() != ""))
                    {
                        int PassErroeCount = Convert.ToInt32(Session["PassErrorCountAdmin"]);
                        Session["PassErrorCountAdmin"] = PassErroeCount + 1;
                    }
                    else
                    {
                        Session["PassErrorCountAdmin"] = 1;
                    }
				}
				else
				{					
					FormsAuthentication.SetAuthCookie( userName,false );
                    //��־
                    //UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "��¼�ɹ�");
					
					Session["UserInfo"]=currentUser;
					Session["Style"]=currentUser.Style;
					if(Session["returnPage"]!=null)
					{
						string returnpage=Session["returnPage"].ToString();
						Session["returnPage"]=null;
						Response.Redirect(returnpage);
					}
					else
					{
						Response.Redirect("main.htm");
					}
				}
			}		
		}

       
	}
}
