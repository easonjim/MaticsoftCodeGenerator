namespace <$$namespace$$>.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using LTP.Accounts.Bus;
	using System.Configuration;
	using System.Web.Security;

	/// <summary>
	///	CheckRight ��ժҪ˵����
	/// </summary>
	public partial class CheckRight : System.Web.UI.UserControl
	{
		public int PermissionID=-1;
		protected void Page_Load(object sender, System.EventArgs e)
		{			
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
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            if (!Page.IsPostBack)
            {
                string virtualPath = ConfigurationManager.AppSettings.Get("VirtualPath");
                if (Context.User.Identity.IsAuthenticated)
                {
                    AccountsPrincipal user = new AccountsPrincipal(Context.User.Identity.Name);
                    if (Session["UserInfo"] == null)
                    {
                        LTP.Accounts.Bus.User currentUser = new LTP.Accounts.Bus.User(user);
                        Session["UserInfo"] = currentUser;
                        Session["Style"] = currentUser.Style;
                        Response.Write("<script defer>location.reload();</script>");
                    }
                    if ((PermissionID != -1) && (!user.HasPermissionID(PermissionID)))
                    {
                        Response.Clear();
                        Response.Write("<script defer>window.alert('��û��Ȩ�޽��뱾ҳ��\\n�����µ�¼�������Ա��ϵ');history.back();</script>");
                        Response.End();
                    }

                }
                else
                {
                    FormsAuthentication.SignOut();
                    Session.Clear();
                    Session.Abandon();
                    Response.Clear();
                    Response.Write("<script defer>window.alert('��û��Ȩ�޽��뱾ҳ��ǰ��¼�û��ѹ��ڣ�\\n�����µ�¼�������Ա��ϵ��');parent.location='" + virtualPath + "/Login.aspx';</script>");
                    Response.End();
                }

            }
		}
		#endregion
	}
}
