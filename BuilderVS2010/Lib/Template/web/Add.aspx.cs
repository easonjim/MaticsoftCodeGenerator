/**  ģ��������Create by Jim
* Add.cs
*
* �� �ܣ� [N/A]
* �� ���� Add.cs
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
using <$$namespace$$>.Common;
using <$$namespace$$>.Accounts.Bus;

namespace <$$namespace$$>.Web.Demo
{
    public partial class Add : PageBaseAdmin
    {
        /*Ȩ�����ÿ�ʼ*/
        /*���ڡ�������Ϊ���������Ӧ������ȡ�÷��ص�ID����̨���ã�����ɫ����Ӧ->��Ȩ�޹�����Ӧ->��������Ϊ����*/
        protected new int Act_AddData = -1;//������ݣ��磺CMS_���ݹ���_�������
        protected override int Act_PageLoad { get { return Act_AddData; } } //ҳ��Ȩ�ޣ��磺CMS_���ݹ���_�б�ҳ��ע�⣺����ֱ��ʹ�ð�ťȨ�޽������ã������ʵ���������
        /*Ȩ�����ý���*/

        protected void Page_Load(object sender, EventArgs e)
        {
                       
        }

        <$$AddAspxCs$$>

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
