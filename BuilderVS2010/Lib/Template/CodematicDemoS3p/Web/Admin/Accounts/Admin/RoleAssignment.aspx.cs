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
/// Ϊ�û������ɫ
/// </summary>
public partial class RoleAssignment : System.Web.UI.Page 
{
	private int userID;
	User currentUser;

	protected void Page_Load(object sender, System.EventArgs e)
	{			
		userID=int.Parse(Request.Params["UserID"]);
		currentUser = new User(userID);

		Label1.Text="Ϊ�û�: "+currentUser.UserName+" �����ɫ";
		if(!Page.IsPostBack)
		{
            //��ȡ����ʾ���н�ɫ
			DataSet dsRole=AccountsTool.GetRoleList();
			CheckBoxList1.DataSource=dsRole.Tables[0].DefaultView;
			CheckBoxList1.DataTextField="Description";
			CheckBoxList1.DataValueField="RoleID";
			CheckBoxList1.DataBind();

            //��ȡ�û��Ѿ�ӵ�еĽ�ɫ������CheckBoxList�ؼ�����ѡ��״̬
			AccountsPrincipal newUser = new AccountsPrincipal(currentUser.UserName);
			if (newUser.Roles.Count > 0 )
			{
				ArrayList roles = newUser.Roles;
				for(int i=0; i<roles.Count; i++)
				{
					foreach(ListItem item in CheckBoxList1.Items)
					{
						if(item.Text==roles[i].ToString())
                            item.Selected=true;
					}
				}
			}

            if (newUser.Permissions.Count > 0)
            {
                RoleList.Visible = true;
                ArrayList Permissions = newUser.Permissions;
                RoleList.Text = "�û�ӵ�е�Ȩ���б�<ul>";
                for (int i = 0; i < Permissions.Count; i++)
                {
                    RoleList.Text += "<li>" + Permissions[i] + "</li>";
                }
                RoleList.Text += "</ul>";
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
		this.BtnOk.Click += new System.Web.UI.ImageClickEventHandler(this.BtnOk_Click);
		this.Btnback.Click += new System.Web.UI.ImageClickEventHandler(this.Btnback_Click);

	}
	#endregion


    //ȷ����ť����
	private void BtnOk_Click(object sender, System.Web.UI.ImageClickEventArgs e)
	{
        //����CheckBoxList������ѡ�еĽ�ɫ��Ϣ���Ƴ�δѡ�еĽ�ɫ
		foreach(ListItem item in CheckBoxList1.Items)
		{
			if(item.Selected==true)
			{
				currentUser.AddToRole(Convert.ToInt32(item.Value));
			}
			else
			{
				currentUser.RemoveRole(Convert.ToInt32(item.Value));
			}
		}
		Response.Redirect("UserAdmin.aspx?PageIndex="+Request.Params["PageIndex"]);		
	}
    //�����û��б�
	private void Btnback_Click(object sender, System.Web.UI.ImageClickEventArgs e)
	{
		Response.Redirect("UserAdmin.aspx?PageIndex="+Request.Params["PageIndex"]);	
	}


		

	}
}
