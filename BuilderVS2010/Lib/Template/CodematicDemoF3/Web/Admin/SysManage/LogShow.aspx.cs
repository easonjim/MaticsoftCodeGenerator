using System;
using System.Data;
namespace Maticsoft.Web.SysManage
{
	/// <summary>
	/// LogShow ��ժҪ˵����
	/// </summary>
	public partial class LogShow : System.Web.UI.Page
	{
	
		public string strtime,errmsg,Particular;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{				
				if(Request.Params["id"]!=null && Request.Params["id"].Trim()!="")
				{
					string id=Request.Params["id"];
					Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();
					DataRow row=sm.GetLog(id);
					strtime=row["datetime"].ToString();
					errmsg=row["loginfo"].ToString();
					Particular=row["Particular"].ToString().Replace("\r\n","<br>");	
					
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
