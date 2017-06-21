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
	/// Add ��ժҪ˵����
	/// </summary>
	public partial class Add : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlInputButton btnCancel;
		public string adminname="������";
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{		
                //BindSuppData();				
			}
		}
        //private void BindSuppData()
        //{
        //    Maticsoft.BLL.ADManage.AdSupplier adsupp=new Maticsoft.BLL.ADManage.AdSupplier();
        //    this.Dropdepart.DataSource=adsupp.GetNameList();
        //    this.Dropdepart.DataTextField="SupplierName";
        //    this.Dropdepart.DataValueField="SupplierID";
        //    this.Dropdepart.DataBind();
        //    adminname=Maticsoft.Common.ConfigHelper.GetConfigString("AdManager");
        //    this.Dropdepart.Items.Insert(0,new ListItem(adminname,"-1"));
        //}

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
			User newUser = new User();
			string strErr="";
//			if(this.Dropdepart.SelectedIndex==0)
//			{
//				strErr+="��ѡ����!";				
//			}
			if(newUser.HasUser(txtUserName.Text))
			{
				strErr+="���û����Ѵ��ڣ�";
			}

			if(strErr!="")
			{
				Maticsoft.Common.MessageBox.Show(this,strErr);
				return;
			}			
			newUser.UserName=txtUserName.Text;
			newUser.Password=AccountsPrincipal.EncryptPassword(txtPassword.Text);
			newUser.TrueName=txtTrueName.Text;
			if(RadioButton1.Checked)
				newUser.Sex="��";
			else
				newUser.Sex="Ů";

			newUser.Phone=this.txtPhone.Text.Trim();
			newUser.Email=txtEmail.Text;
			newUser.EmployeeID=0;
            //newUser.DepartmentID=this.Dropdepart.SelectedValue;
			newUser.Activity=true;
            newUser.UserType = "AA";
			newUser.Style=1;
			int userid=newUser.Create();		
			if (userid == -100)
			{
				this.lblMsg.Text = "���û����Ѵ��ڣ�";
				this.lblMsg.Visible = true;
			}
			else 
			{
				Response.Redirect("Admin/RoleAssignment.aspx?UserID="+userid);
			}
		
		}

		
	}
}
