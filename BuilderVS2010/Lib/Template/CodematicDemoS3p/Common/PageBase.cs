using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
//using LTP.Accounts.Bus;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Text;
namespace Maticsoft.Common
{
	/// <summary>
	/// ҳ���(��ʾ��)����,����ҳ��̳и�ҳ��
	/// </summary>
	public class PageBase:System.Web.UI.Page
	{
        public int PermissionID = -1;//Ĭ��-1Ϊ�����ƣ������ڲ�ͬҳ��̳��������Ʋ�ͬҳ���Ȩ��
        string virtualPath = Maticsoft.Common.ConfigHelper.GetConfigString("VirtualPath");
        		
		/// <summary>
		/// ���캯��
		/// </summary>
		public PageBase()
		{
            //this.Load+=new EventHandler(PageBase_Load);
		}
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(PageBase_Load);
            this.Error += new System.EventHandler(PageBase_Error);
        }
        //������
        protected void PageBase_Error(object sender, System.EventArgs e)
        {
            string errMsg;
            Exception currentError = Server.GetLastError();
            errMsg = "<link rel=\"stylesheet\" href=\"/style.css\">";
            errMsg += "<h1>ϵͳ����</h1><hr/>ϵͳ�������� " +
                "����Ϣ�ѱ�ϵͳ��¼�����Ժ����Ի������Ա��ϵ��<br/>" +
                "�����ַ�� " + Request.Url.ToString() + "<br/>" +
                "������Ϣ�� <font class=\"ErrorMessage\">" + currentError.Message.ToString() + "</font><hr/>" +
                "<b>Stack Trace:</b><br/>" +  currentError.ToString();
            Response.Write(errMsg);
            Server.ClearError();

        }
		private void PageBase_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack )
			{

                //Ȩ����֤
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
	}
}
