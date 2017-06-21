﻿/**  模板制作：Create by Jim
* Add.cs
*
* 功 能： [N/A]
* 类 名： Add.cs
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
using JSoft.Common;
using JSoft.Accounts.Bus;

namespace JSoft.Web.SA.Tree
{
    public partial class Add : PageBaseAdmin
    {
        /*权限配置开始*/
        /*请在【功能行为管理】添加相应的数据取得返回的ID，后台配置：【角色】对应->【权限管理】对应->【功能行为管理】*/
        protected new int Act_AddData = -1;//添加数据，如：CMS_内容管理_添加数据
        protected override int Act_PageLoad { get { return Act_AddData; } } //页面权限，如：CMS_内容管理_列表页；注意：这里直接使用按钮权限进行配置，请根据实际情况设置
        /*权限配置结束*/

        protected void Page_Load(object sender, EventArgs e)
        {
                       
        }

        		protected void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(this.txtTreeText.Text.Trim().Length==0)
			{
				strErr+="TreeText不能为空！\\n";	
			}
			if(!PageValidate.IsNumber(txtParentID.Text))
			{
				strErr+="ParentID格式错误！\\n";	
			}
			if(this.txtParentPath.Text.Trim().Length==0)
			{
				strErr+="ParentPath不能为空！\\n";	
			}
			if(this.txtLocation.Text.Trim().Length==0)
			{
				strErr+="Location不能为空！\\n";	
			}
			if(!PageValidate.IsNumber(txtOrderID.Text))
			{
				strErr+="OrderID格式错误！\\n";	
			}
			if(this.txtComment.Text.Trim().Length==0)
			{
				strErr+="Comment不能为空！\\n";	
			}
			if(this.txtUrl.Text.Trim().Length==0)
			{
				strErr+="Url不能为空！\\n";	
			}
			if(!PageValidate.IsNumber(txtPermissionID.Text))
			{
				strErr+="PermissionID格式错误！\\n";	
			}
			if(this.txtImageUrl.Text.Trim().Length==0)
			{
				strErr+="ImageUrl不能为空！\\n";	
			}
			if(!PageValidate.IsNumber(txtModuleID.Text))
			{
				strErr+="ModuleID格式错误！\\n";	
			}
			if(!PageValidate.IsNumber(txtKeShiDM.Text))
			{
				strErr+="KeShiDM格式错误！\\n";	
			}
			if(this.txtKeshiPublic.Text.Trim().Length==0)
			{
				strErr+="KeshiPublic不能为空！\\n";	
			}
			if(!PageValidate.IsNumber(txtTreeType.Text))
			{
				strErr+="TreeType格式错误！\\n";	
			}

			if(strErr!="")
			{
				MessageBox.ShowFailTip(this,strErr);
				return;
			}
			string TreeText=this.txtTreeText.Text;
			int ParentID=int.Parse(this.txtParentID.Text);
			string ParentPath=this.txtParentPath.Text;
			string Location=this.txtLocation.Text;
			int OrderID=int.Parse(this.txtOrderID.Text);
			string Comment=this.txtComment.Text;
			string Url=this.txtUrl.Text;
			int PermissionID=int.Parse(this.txtPermissionID.Text);
			string ImageUrl=this.txtImageUrl.Text;
			int ModuleID=int.Parse(this.txtModuleID.Text);
			int KeShiDM=int.Parse(this.txtKeShiDM.Text);
			string KeshiPublic=this.txtKeshiPublic.Text;
			int TreeType=int.Parse(this.txtTreeType.Text);
			bool Enabled=this.chkEnabled.Checked;

			JSoft.Model.SA.Tree model=new JSoft.Model.SA.Tree();
			model.TreeText=TreeText;
			model.ParentID=ParentID;
			model.ParentPath=ParentPath;
			model.Location=Location;
			model.OrderID=OrderID;
			model.Comment=Comment;
			model.Url=Url;
			model.PermissionID=PermissionID;
			model.ImageUrl=ImageUrl;
			model.ModuleID=ModuleID;
			model.KeShiDM=KeShiDM;
			model.KeshiPublic=KeshiPublic;
			model.TreeType=TreeType;
			model.Enabled=Enabled;

			JSoft.BLL.SA.Tree bll=new JSoft.BLL.SA.Tree();
			bll.Add(model);
			JSoft.Common.MessageBox.ShowSuccessTip(this,"保存成功！","add.aspx");
			/*增加日志*/LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, string.Format("Add：【{0}】", new JavaScriptSerializer().Serialize(model)), this);

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
