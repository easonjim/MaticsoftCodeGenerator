using System;
using Maticsoft.Common;
namespace Maticsoft.Web.Admin
{
	/// <summary>
	/// Main ��ժҪ˵����
	/// </summary>
	public partial class Main : System.Web.UI.Page
	{
		
		protected void Page_Load(object sender, System.EventArgs e)
		{			
			if(!Page.IsPostBack)
			{
				if (!Context.User.Identity.IsAuthenticated )
				{					
					Response.Clear();
					Response.Write("<script language=javascript>window.alert('��û��Ȩ�޽��뱾ҳ��\\n���¼�������Ա��ϵ��');history.back();</script>");
					Response.End();
                   
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
