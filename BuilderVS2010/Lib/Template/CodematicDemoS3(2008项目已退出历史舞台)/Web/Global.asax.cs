using System;
using System.Web;
using System.Collections;
using System.ComponentModel;
using System.Web.SessionState;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.Security;
using LTP.Accounts.Bus;
namespace <$$namespace$$>.Web 
{
	/// <summary>
	/// Global ��ժҪ˵����
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{		
           #region Ĭ����
            						
			Application["1xtop1_bgimage"]="images/top-1.gif"; //���򱳾�ͼƬ
			Application["1xtop2_bgimage"]="images/top-2.gif"; //���򱳾�ͼƬ
			Application["1xtop3_bgimage"]="images/top-3.gif"; //���򱳾�ͼƬ
			Application["1xtop4_bgimage"]="images/top-4.gif"; //���򱳾�ͼƬ
			Application["1xtop5_bgimage"]="images/top-5.gif"; //���򱳾�ͼƬ
			Application["1xtopbj_bgimage"]="images/top-bj.gif"; //���򱳾�ͼƬ

			Application["1xtopbar_bgimage"]="images/topbar_01.jpg"; //���򹤾�������ͼƬ
			Application["1xfirstpage_bgimage"]="images/dbsx_01.gif"; //��ҳ����ͼƬ
			Application["1xforumcolor"]="#f0f4fb";
			Application["1xleft_width"]="204"; //���ܿ��
			
			Application["1xtree_bgcolor"]="#e3eeff"; //����������ɫ
			Application["1xleft1_bgimage"]="images/left-1.gif"; 
			Application["1xleft2_bgimage"]="images/left-2.gif"; 
			Application["1xleft3_bgimage"]="images/left-3.gif"; 
			Application["1xleftbj_bgimage"]="images/left-bj.gif"; 

			Application["1xspliter_color"]="#6B7DDE"; //�ָ���ɫ

			Application["1xdesktop_bj"]="";//images/right-bj.gif
			Application["1xdesktop_bgimage"]="images/desktop_01.gif";//right.gif

			Application["1xtable_bgcolor"]="#F5F9FF"; //������񱳾�
			Application["1xtable_bordercolorlight"]="#4F7FC9"; //�в������߿�
			Application["1xtable_bordercolordark"]="#D3D8E0"; //�в��񰵱߿�
			Application["1xtable_titlebgcolor"]="#E3EFFF"; //�в��������

			Application["1xform_requestcolor"]="#E78A29"; //���б����ֶ�*��ɫ
			Application["1xfirstpage_topimage"]="images/top_01.gif";
			Application["1xfirstpage_bottomimage"]="images/bottom_01.gif";
			Application["1xfirstpage_middleimage"]="images/bg_01.gif";
			#endregion 		

		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
			Session["Style"]=1;
		}
		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
		}
		protected void Application_EndRequest(Object sender, EventArgs e)
		{
		}
		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
		}
		protected void Application_Error(Object sender, EventArgs e)
		{
			
		}
		protected void Session_End(Object sender, EventArgs e)
		{		
			
		}
		protected void Application_End(Object sender, EventArgs e)
		{
		}
			
		#region Web ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

