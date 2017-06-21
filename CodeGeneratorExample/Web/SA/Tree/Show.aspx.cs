/**  模板制作：Create by Jim
* Show.cs
*
* 功 能： [N/A]
* 类 名： Show.cs
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

namespace JSoft.Web.SA.Tree
{
    public partial class Show : PageBaseAdmin
    {        
        /*权限配置开始*/
        /*请在【功能行为管理】添加相应的数据取得返回的ID，后台配置：【角色】对应->【权限管理】对应->【功能行为管理】*/
        protected override int Act_PageLoad { get { return -1; } } //页面权限，如：CMS_内容管理_列表页；注意：这里的ID和List页面ID一样，请根据实际情况配置
        /*权限配置结束*/

        		public string strid=""; 
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
				{
					strid = Request.Params["id"];
					int NodeID=(Convert.ToInt32(strid));
					ShowInfo(NodeID);
				}
			}
		}
		
	private void ShowInfo(int NodeID)
	{
		JSoft.BLL.SA.Tree bll=new JSoft.BLL.SA.Tree();
		JSoft.Model.SA.Tree model=bll.GetModel(NodeID);
		this.lblNodeID.Text=model.NodeID.ToString();
		this.lblTreeText.Text=model.TreeText;
		this.lblParentID.Text=model.ParentID.ToString();
		this.lblParentPath.Text=model.ParentPath;
		this.lblLocation.Text=model.Location;
		this.lblOrderID.Text=model.OrderID.ToString();
		this.lblComment.Text=model.Comment;
		this.lblUrl.Text=model.Url;
		this.lblPermissionID.Text=model.PermissionID.ToString();
		this.lblImageUrl.Text=model.ImageUrl;
		this.lblModuleID.Text=model.ModuleID.ToString();
		this.lblKeShiDM.Text=model.KeShiDM.ToString();
		this.lblKeshiPublic.Text=model.KeshiPublic;
		this.lblTreeType.Text=model.TreeType.ToString();
		this.lblEnabled.Text=model.Enabled?"是":"否";

	}


        	
	    public void btnCancle_Click(object sender, EventArgs e)
	    {
	        Response.Redirect("list.aspx");
	    }
    }

}
