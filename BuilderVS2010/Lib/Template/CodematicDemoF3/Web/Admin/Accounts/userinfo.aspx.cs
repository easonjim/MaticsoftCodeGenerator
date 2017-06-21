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
	/// userinfo ��ժҪ˵����
	/// </summary>
	public partial class userinfo : System.Web.UI.Page
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
					this.lblTruename.Text=currentUser.TrueName;
					this.lblSex.Text=currentUser.Sex;
					this.lblPhone.Text=currentUser.Phone;
					this.lblEmail.Text=currentUser.Email;

                    lblUserIP.Text = Request.UserHostAddress;

                    //if(currentUser.DepartmentID=="-1")
                    //{
                    //    string herosoftmana=Maticsoft.Common.ConfigHelper.GetConfigString("AdManager");
                    //    this.lblDepart.Text=herosoftmana;
                    //}
                    //else
                    //{
						
                    //        if(Maticsoft.Common.PageValidate.IsNumber(currentUser.DepartmentID))
                    //        {
                    //            Maticsoft.BLL.ADManage.AdSupplier supp=new Maticsoft.BLL.ADManage.AdSupplier();
                    //            Maticsoft.Model.ADManage.AdSupplier suppmodel=supp.GetModel(int.Parse(currentUser.DepartmentID));
                    //            this.lblDepart.Text=suppmodel.SupplierName;
                    //            this.lblModeys.Text=suppmodel.Moneys.ToString();
                    //        }
						
						
                    //}
					switch(currentUser.Style)
					{
						case 1:
							this.lblStyle.Text="Ĭ����";
							break;
						case 2:
							this.lblStyle.Text="�����";
							break;
						case 3:
							this.lblStyle.Text="���";
							break;
						case 4:
							this.lblStyle.Text="����";
							break;
					}
					


//					if(user.Roles.Count>0)
//					{
//						RoleList.Visible = true;
//						ArrayList roles = user.Roles;
//						RoleList.Text = "��ɫ�б�<ul>";
//						for(int i=0;i<roles.Count;i++)
//						{
//							RoleList.Text+="<li>" + roles[i] + "</li>";
//						}
//						RoleList.Text += "</ul>";
//					}



//					if(user.Permissions.Count>0)
//					{
//						RoleList.Visible = true;
//						ArrayList Permissions = user.Permissions;
//						RoleList.Text = "Ȩ���б�<ul>";
//						for(int i=0;i<Permissions.Count;i++)
//						{
//							RoleList.Text+="<li>" + Permissions[i] + "</li>";
//						}
//						RoleList.Text += "</ul>";
//					}




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
