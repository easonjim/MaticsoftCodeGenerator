/**  模板制作：Create by Jim
* Modify.cs
*
* 功 能： [N/A]
* 类 名： Modify.cs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
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
    public partial class Modify : PageBaseAdmin
    {       
        /*权限配置开始*/
        /*请在【功能行为管理】添加相应的数据取得返回的ID，后台配置：【角色】对应->【权限管理】对应->【功能行为管理】*/
        protected new int Act_UpdateData = -1;//修改数据-单个，如：CMS_内容管理_编辑数据
        protected override int Act_PageLoad { get { return Act_UpdateData; } } //页面权限，如：CMS_内容管理_列表页；注意：这里直接使用按钮权限进行配置，请根据实际情况设置
        /*权限配置结束*/

        <$$ModifyAspxCs$$>

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
