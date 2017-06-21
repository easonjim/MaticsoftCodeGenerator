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
using System.Configuration;
namespace Maticsoft.Web.Accounts
{
	/// <summary>
	/// usermodify ��ժҪ˵����
	/// </summary>
	public partial class usermodify : System.Web.UI.Page
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
					txtTrueName.Text=currentUser.TrueName;
					if(currentUser.Sex=="��")
						RadioButton1.Checked=true;
					else
						RadioButton2.Checked=true;
					this.txtPhone.Text=currentUser.Phone;
					txtEmail.Text=currentUser.Email;

//					for(int i=0;i<this.Dropdepart.Items.Count;i++)
//					{
//						if(this.Dropdepart.Items[i].Value==currentUser.DepartmentID)
//						{
//							this.Dropdepart.Items[i].Selected=true;
//						}
//					}

                    //for (int i = 0; i < this.dropUserType.Items.Count; i++)
                    //{
                    //    if (this.dropUserType.Items[i].Value == currentUser.UserType)
                    //    {
                    //        this.dropUserType.Items[i].Selected = true;
                    //    }
                    //}

					this.dropStyle.SelectedIndex=currentUser.Style-1;

//					BindRoles(user);					
				}
			}

		}

		private void BindRoles(AccountsPrincipal user)
		{
			if(user.Permissions.Count>0)
			{
				RoleList.Visible = true;
				ArrayList Permissions = user.Permissions;
				RoleList.Text = "Ȩ���б�<ul>";
				for(int i=0;i<Permissions.Count;i++)
				{
					RoleList.Text+="<li>" + Permissions[i] + "</li>";
				}
				RoleList.Text += "</ul>";
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
				string username=this.lblName.Text.Trim();				
				AccountsPrincipal user=new AccountsPrincipal(username);
				User currentUser=new LTP.Accounts.Bus.User(user);
				currentUser.UserName=username;
				currentUser.TrueName=txtTrueName.Text.Trim();
				if(RadioButton1.Checked)
					currentUser.Sex="��";
				else
					currentUser.Sex="Ů";
				currentUser.Phone=this.txtPhone.Text.Trim();
				currentUser.Email=txtEmail.Text.Trim();
                //currentUser.UserType = dropUserType.SelectedValue;
				int style=int.Parse(this.dropStyle.SelectedValue);
				currentUser.Style=style;
				if (!currentUser.Update())
				{
					this.lblMsg.ForeColor=Color.Red;
					this.lblMsg.Text = "�����û���Ϣ��������";
				}
				else 
				{
					this.lblMsg.ForeColor=Color.Blue;
					this.lblMsg.Text = "�û���Ϣ���³ɹ���";
				}
				string virtualPath=ConfigurationManager.AppSettings.Get("VirtualPath");
				Session["Style"]=style;
				Response.Clear();
				Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">\n");
				Response.Write("<!--\n");
				Response.Write("parent.topFrame.location=\""+virtualPath+"/Admin/top.aspx\";\n");
				Response.Write("parent.leftFrame.location=\""+virtualPath+"/Admin/left.aspx\";\n");
				Response.Write("parent.spliterFrame.location=\""+virtualPath+"/Admin/spliter.aspx\";\n");
				Response.Write("parent.mainFrame.location=\"userinfo.aspx\";\n");
//				Response.Write("parent.mainFrame.location=\"userinfo.aspx?id="+userName+"\";\n");
				Response.Write("//-->\n");
				Response.Write("</SCRIPT>");
				Response.End();

				

			}
		}

	





	}
}
