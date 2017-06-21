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
using System.Data.SqlClient;
using LTP.Accounts.Bus;

namespace Maticsoft.Web.Accounts.Admin
{
	/// <summary>
	/// UserAdmin ��ժҪ˵����
	/// </summary>
	public partial class UserAdmin : System.Web.UI.Page//Maticsoft.Web.Accounts.MoviePage
	{
		protected System.Web.UI.WebControls.ImageButton ImageButton1;
//		protected int currentPage;
        //Maticsoft.BLL.ADManage.AdSupplier supp=new Maticsoft.BLL.ADManage.AdSupplier();
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!Page.IsPostBack)
			{
				DataGrid1.BorderWidth=Unit.Pixel(1);
				DataGrid1.CellPadding=3;
				DataGrid1.CellSpacing=0;
				DataGrid1.BorderColor=ColorTranslator.FromHtml(Application[Session["Style"].ToString()+"xtable_bordercolorlight"].ToString());
				DataGrid1.HeaderStyle.BackColor=ColorTranslator.FromHtml(Application[Session["Style"].ToString()+"xtable_titlebgcolor"].ToString());
						
				
				dataBind();
			}
		}
		protected void dataBind()
		{
            string usertype = DropUserType.SelectedValue;
			string key=this.TextBox1.Text.Trim();
			User userAdmin=new User();
			DataSet ds=new DataSet();
            if (usertype != "")
            {
                ds = userAdmin.GetUsersByType(usertype, key);
            }
            else
            {
                ds = userAdmin.GetAllUsers(key);
            }
			int pageIndex=this.DataGrid1.CurrentPageIndex;
			DataGrid1.DataSource=ds.Tables[0].DefaultView;
			int record_Count=ds.Tables[0].Rows.Count;
			int page_Size=DataGrid1.PageSize;
			int totalPages = int.Parse(Math.Ceiling((double)record_Count/page_Size).ToString());
			if(totalPages>0)
			{
				if (pageIndex>totalPages-1) 
					pageIndex = totalPages-1;				
			}
			else
			{
				pageIndex=0;
			}
			DataGrid1.CurrentPageIndex=pageIndex;
			DataGrid1.DataBind();						
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
		protected void InitializeComponent()
		{    
			this.BtnSearch.Click += new System.Web.UI.ImageClickEventHandler(this.BtnSearch_Click);
			this.DataGrid1.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemCreated);
			this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
			this.DataGrid1.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGrid1_PageIndexChanged);
			this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);

		}
		#endregion

		protected void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
		
			switch(e.Item.ItemType)
			{
				case ListItemType.Item:
				case ListItemType.AlternatingItem:
					ImageButton btn = (ImageButton)e.Item.FindControl("BtnDel");
					btn.Attributes.Add("onclick", "return confirm('���Ƿ�ȷ��ɾ��������¼��');");
                    string DepartmentID = (string)DataBinder.Eval(e.Item.DataItem, "DepartmentID");
                    if (DepartmentID == "-1")
                    {
                        string herosoftmana = Maticsoft.Common.ConfigHelper.GetConfigString("AdManager");
                        e.Item.Cells[6].Text = herosoftmana;
                    }
                    else
                    {
                        //if (Maticsoft.Common.PageValidate.IsNumber(DepartmentID))
                        //{
                        //    e.Item.Cells[6].Text = supp.GetName(int.Parse(DepartmentID));
                        //}
                    }
					break;    
			}
			
		}

		protected void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			string btn=e.CommandName;
			switch(btn)
			{
				case "BtnEdit":
					int userID1=int.Parse(e.Item.Cells[9].Text.Trim());
					Response.Redirect("../userupdate.aspx?userid="+userID1);					
					break;
				case "BtnDel":
					int userID2=int.Parse(e.Item.Cells[9].Text.Trim());
					User currentUser2 = new User(userID2);
					currentUser2.Delete();
					break;
			}
			dataBind();		
		}

		protected void BtnSearch_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			dataBind();
		}

		protected void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.DataGrid1.CurrentPageIndex=e.NewPageIndex;
			dataBind();

		}

		protected void DataGrid1_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Pager)
			{				
				foreach (Control c in e.Item.Cells[0].Controls)
				{
					if (c is Label)
					{
						Label lblpage=(Label)c;
						lblpage.ForeColor= System.Drawing.ColorTranslator.FromHtml("#FF0000");						
						lblpage.Font.Bold=true;
						//lblpage.Text="["+lblpage.Text+"]";//ֻ�����������ҳ������֣�����ǰҳ��
						//((Label)c).ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
						//((Label)c).ForeColor = System.Drawing.Color.Green;						
						break;
					}
				}

				
			}
		}

       

	

		

		
	}
}
