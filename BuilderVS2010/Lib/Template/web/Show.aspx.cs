/**  ģ��������Create by Jim
* Show.cs
*
* �� �ܣ� [N/A]
* �� ���� Show.cs
*
* Ver    �������             ������  �������
* ����������������������������������������������������������������������
* V0.01  
*
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.Script.Serialization;

namespace <$$namespace$$>.Web.Demo
{
    public partial class Show : PageBaseAdmin
    {        
        /*Ȩ�����ÿ�ʼ*/
        /*���ڡ�������Ϊ���������Ӧ������ȡ�÷��ص�ID����̨���ã�����ɫ����Ӧ->��Ȩ�޹�����Ӧ->��������Ϊ����*/
        protected override int Act_PageLoad { get { return -1; } } //ҳ��Ȩ�ޣ��磺CMS_���ݹ���_�б�ҳ��ע�⣺�����ID��Listҳ��IDһ���������ʵ���������
        /*Ȩ�����ý���*/

        <$$ShowAspxCs$$>
        	
	    public void btnCancle_Click(object sender, EventArgs e)
	    {
	        Response.Redirect("list.aspx");
	    }
    }

}
