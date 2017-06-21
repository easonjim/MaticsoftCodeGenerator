using System;
using Maticsoft.Model;

namespace Maticsoft.Web.SysManage
{
	/// <summary>
	/// show ��ժҪ˵����
	/// </summary>
	public partial class show : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblModuledept;
		protected System.Web.UI.WebControls.Label lblModule;
		protected System.Web.UI.WebControls.Label lblKeshiPublic;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				string id=Request.Params["id"];
				if(id==null || id.Trim()=="")
				{
					Response.Redirect("treelist.aspx");
					Response.End();
				}

				Navigation011.Para_Str="id="+id;
				Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();				
				SysNode node=sm.GetNode(int.Parse(id));
				lblID.Text=id;
				this.lblOrderid.Text=node.OrderID.ToString();
				lblName.Text=node.Text;			
				if(node.ParentID==0)
				{
					this.lblTarget.Text="��Ŀ¼";
				}
				else
				{
					lblTarget.Text=sm.GetNode(node.ParentID).Text;			
				}
				lblUrl.Text=node.Url;
				lblImgUrl.Text=node.ImageUrl;
				LTP.Accounts.Bus.Permissions perm=new LTP.Accounts.Bus.Permissions();
				if(node.PermissionID==-1)
				{
					this.lblPermission.Text="û��Ȩ������";
				}
				else
				{
					this.lblPermission.Text=perm.GetPermissionName(node.PermissionID);
				}
				
				lblDescription.Text=node.Comment;
//				if(node.ModuleID!=-1)
//				{
//					this.lblModule.Text=sm.GetModuleName(node.ModuleID);
//				}
//				else
//				{
//					this.lblModule.Text="δ�����κ�ģ��";
//				}
//
//				if(node.KeShiDM!=-1)
//				{
//					this.lblModuledept.Text=Maticsoft.BLL.PubConstant.GetKeshiName(node.KeShiDM);
//				}
//				else
//				{
//					this.lblModuledept.Text="δ�����κβ���";
//				}
//				if(node.KeshiPublic=="true")
//				{
//					this.lblKeshiPublic.Text="��Ϊ�����ڲ����в��ֳ���";
//				}

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
