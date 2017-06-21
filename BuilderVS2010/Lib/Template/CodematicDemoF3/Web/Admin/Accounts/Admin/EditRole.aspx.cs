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
	/// EditRole ��ժҪ˵����
	/// </summary>
	public partial class EditRole : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button Button1;
	
		private Role currentRole;



		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
				Button btn = (Button)Page.FindControl("RemoveRoleButton");
				btn.Attributes.Add("onclick", "return confirm('���Ƿ�ȷ��ɾ���ý�ɫ��');");
				DoInitialDataBind();
				CategoryDownList_SelectedIndexChanged(sender,e);
			}
		}

		//������б�
		private void DoInitialDataBind()
		{
			currentRole = new Role(Convert.ToInt32(Request["RoleID"]));
			RoleLabel.Text = "��ǰ��ɫ�� " + currentRole.Description;
			this.TxtNewname.Text=currentRole.Description;
		
			DataSet allCategories = AccountsTool.GetAllCategories();
			CategoryDownList.DataSource = allCategories.Tables[0];
			CategoryDownList.DataTextField = "Description";
			CategoryDownList.DataValueField = "CategoryID";
			CategoryDownList.DataBind();
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
			this.BtnUpName.Click += new System.Web.UI.ImageClickEventHandler(this.BtnUpName_Click);

		}
		#endregion

		//ѡ��������2��listbox
		protected void CategoryDownList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int categoryID=Convert.ToInt32(CategoryDownList.SelectedItem.Value);	
			FillCategoryList(categoryID);		
			SelectCategory( categoryID, false );			
		}


		//����Ȩ���б�
		private void FillCategoryList(int categoryId)
		{		
			currentRole = new Role(Convert.ToInt32(Request["RoleID"]));
			DataTable categories = currentRole.NoPermissions.Tables["Categories"];
			DataRow currentCategory = categories.Rows.Find( categoryId );

			if (currentCategory != null) 
			{
				DataRow[] permissions = currentCategory.GetChildRows("PermissionCategories");

				CategoryList.Items.Clear();
				foreach (DataRow currentRow in permissions)
				{
					CategoryList.Items.Add(
						new ListItem( (string)currentRow["Description"], Convert.ToString(currentRow["PermissionID"])) );
				}

			}
		}


		//�������Ȩ��listbox
		private void SelectCategory(int categoryId, bool forceSelection)
		{
			currentRole = new Role(Convert.ToInt32(Request["RoleID"]));
			DataTable categories = currentRole.Permissions.Tables["Categories"];
			DataRow currentCategory = categories.Rows.Find( categoryId );

			if (currentCategory != null) 
			{
				DataRow[] permissions = currentCategory.GetChildRows("PermissionCategories");

				PermissionList.Items.Clear();
				foreach (DataRow currentRow in permissions)
				{
					PermissionList.Items.Add(
						new ListItem( (string)currentRow["Description"], Convert.ToString(currentRow["PermissionID"])) );
				}
			}


		}

		//����Ȩ��
		protected void AddPermissionButton_Click(object sender, System.EventArgs e)
		{
			if(this.CategoryList.SelectedIndex>-1)
			{
				int currentRole = Convert.ToInt32(Request["RoleID"]);
				Role bizRole = new Role(currentRole);
				bizRole.AddPermission( Convert.ToInt32(this.CategoryList.SelectedValue) );			
				CategoryDownList_SelectedIndexChanged(sender,e);
			}
		
		}

		//�Ƴ�Ȩ��
		protected void RemovePermissionButton_Click(object sender, System.EventArgs e)
		{
			if(this.PermissionList.SelectedIndex>-1)
			{
				int currentRole = Convert.ToInt32(Request["RoleID"]);
				Role bizRole = new Role(currentRole);
				bizRole.RemovePermission( Convert.ToInt32(this.PermissionList.SelectedValue) );
				CategoryDownList_SelectedIndexChanged(sender,e);
			}
		}

	

		protected void RemoveRoleButton_Click(object sender, System.EventArgs e)
		{
			int currentRole = Convert.ToInt32(Request["RoleID"]);
			Role bizRole = new Role(currentRole);
			bizRole.Delete();
			Server.Transfer("RoleAdmin.aspx");
		}

		private void BtnUpName_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			string newname=this.TxtNewname.Text.Trim();
			currentRole = new Role(Convert.ToInt32(Request["RoleID"]));
			currentRole.Description=newname;
			currentRole.Update();
			DoInitialDataBind();
		}

		protected void Button2_ServerClick(object sender, System.EventArgs e)
		{
			Response.Redirect("RoleAdmin.aspx");
		}





	}
}
