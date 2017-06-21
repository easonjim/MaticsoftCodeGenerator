/**  模板制作：Create by Jim
* List.cs
*
* 功 能： [N/A]
* 类 名： List.cs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;
using <$$namespace$$>.Common;
using System.Drawing;
using <$$namespace$$>.Accounts.Bus;

namespace <$$namespace$$>.Web.Demo
{
    public partial class List : PageBaseAdmin
    {
        /*权限配置开始*/
        /*请在【功能行为管理】添加相应的数据取得返回的ID，后台配置：【角色】对应->【权限管理】对应->【功能行为管理】*/
        protected override int Act_PageLoad { get { return -1; } } //页面权限，如：CMS_内容管理_列表页
        protected new int Act_DeleteList    = -1;//批量删除按钮，如：CMS_内容管理_批量删除数据
        protected new int Act_AddData       = -1;//添加数据，如：CMS_内容管理_添加数据
        protected new int Act_UpdateData    = -1;//修改数据-单个，如：CMS_内容管理_编辑数据
        protected new int Act_DelData       = -1;//删除数据-单个，如：CMS_内容管理_删除数据
        /*权限配置结束*/

        private bool isAll = false;
        <$$ListAspxCs$$>

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnDelete.Attributes.Add("onclick", "return confirm(\"" + Resources.Site.TooltipDelConfirm + "\")");
                /*权限配置开始*/
                /*进行页面显示配置*/
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DeleteList)) && GetPermidByActID(Act_DeleteList) != -1)//批量删除按钮
                {
                    btnDelete.Visible = false;
                    btnDelete2.Visible = false;
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)//添加数据
                {
                    btnAdd.Visible = false;
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateData)) && GetPermidByActID(Act_UpdateData) != -1)//修改数据-单个
                {
                    gridView.Columns[gridView.Columns.Count - 2].Visible = false;//固定是倒数第二个，按实际情况设置
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)//删除数据-单个
                {
                    gridView.Columns[gridView.Columns.Count - 1].Visible = false;//固定是倒数第一个，按实际情况设置
                }
                /*权限配置结束*/
                

                gridView.BorderColor = ColorTranslator.FromHtml(Application[Session["Style"].ToString() + "xtable_bordercolorlight"].ToString());
                gridView.HeaderStyle.BackColor = ColorTranslator.FromHtml(Application[Session["Style"].ToString() + "xtable_titlebgcolor"].ToString());

                //gridView.OnBind();
            }
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }
        
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            /*权限配置开始*/
            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DeleteList)) && GetPermidByActID(Act_DeleteList) != -1)//批量删除按钮
            {
                return;
            }
            /*权限配置结束*/
            string idlist = GetSelIDlist();
            if(idlist.Trim().Length == 0) return;
            /*增加日志*/foreach (var id in idlist.Split(',')) LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, string.Format("Delete：【{0}】", new JavaScriptSerializer().Serialize(bll.GetModel(Common.Globals.SafeInt(id, 0)))), this);
            bll.DeleteList(idlist);
            <$$namespace$$>.Common.MessageBox.Show(this, Resources.Site.TooltipDelOK);
            gridView.OnBind();
        }
        
        #region gridView
                        
        public void BindData()
        {
            #region
            //if (!Context.User.Identity.IsAuthenticated)
            //{
            //    return;
            //}
            //AccountsPrincipal user = new AccountsPrincipal(Context.User.Identity.Name);
            //if (user.HasPermissionID(PermId_Modify))
            //{
            //    gridView.Columns[6].Visible = true;
            //}
            //if (user.HasPermissionID(PermId_Delete))
            //{
            //    gridView.Columns[7].Visible = true;
            //}
            #endregion

            DataSet ds = new DataSet();
            StringBuilder strWhere = new StringBuilder();
            if (txtKeyword.Text.Trim() != "")
            {      
                #warning 代码生成警告：请修改 keywordField 为需要匹配查询的真实字段名称
                #warning 注意：此处的参数要使用SQL防注入：InjectionFilter
                strWhere.AppendFormat("keywordField like '%{0}%'", txtKeyword.Text.Trim());
            }            
            #warning 代码生成警告：请根据实际业务换成数据库分页
            gridView.ToalCount = bll.GetRecordCount(strWhere.ToString());
            //ds = bll.GetList(-1, strWhere.ToString(), "CreatedDate desc");
            if (isAll)
            {
                ds = bll.GetList( strWhere.ToString());
            }
            else
            {
                ds = bll.GetListByPage(strWhere.ToString(), string.IsNullOrEmpty(gridView.SortExpressionStr) ? "" : gridView.SortExpressionStr + " " + gridView.SortDirectionStr, (gridView.PageIndex) * gridView.PageSize+1, (gridView.PageIndex+1) * gridView.PageSize);
            }
            //ds = bll.GetList(strWhere.ToString(), UserPrincipal.PermissionsID, UserPrincipal.PermissionsID.Contains(GetPermidByActID(Act_ShowInvalid)));
            gridView.DataSetSource = ds;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("style", "background:#FFF");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton linkbtnDel = (LinkButton)e.Row.FindControl("LinkButton1");
                linkbtnDel.Attributes.Add("onclick", "return confirm(\"" + Resources.Site.TooltipDelConfirm + "\")");
                
                //object obj1 = DataBinder.Eval(e.Row.DataItem, "Levels");
                //if ((obj1 != null) && ((obj1.ToString() != "")))
                //{
                //    e.Row.Cells[4].Text = obj1.ToString() == "0" ? "Private" : "Shared";
                //}
               
            }
        }
        
        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            /*权限配置开始*/
            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)//删除数据-单个
            {
                gridView.OnBind();
                return;
            }
            /*权限配置结束*/
            #warning 代码生成警告：请检查确认真实主键的名称和类型是否正确
            int ID = (int)gridView.DataKeys[e.RowIndex].Value;
            /*增加日志*/LogHelp.AddUserLog(CurrentUser.UserName, CurrentUser.UserType, string.Format("Delete：【{0}】", new JavaScriptSerializer().Serialize(bll.GetModel(ID))), this);
            bll.Delete(ID);
            gridView.OnBind();
        }

        private string GetSelIDlist()
        {
            string idlist = "";
            bool BxsChkd = false;
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                CheckBox ChkBxItem = (CheckBox)gridView.Rows[i].FindControl(gridView.CheckBoxID);
                if (ChkBxItem != null && ChkBxItem.Checked)
                {
                    BxsChkd = true;
                    #warning 代码生成警告：请检查确认Cells的列索引是否正确
                    if (gridView.DataKeys[i].Value != null)
                    {
                        //idlist += gridView.Rows[i].Cells[1].Text + ",";
                        idlist += gridView.DataKeys[i].Value.ToString() + ",";
                    }
                }
            }
            if (BxsChkd)
            {
                idlist = idlist.Substring(0, idlist.LastIndexOf(","));
            }
            return idlist;
        }

        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ExportToExcel")
            {
                this.isAll = true;
            }
            if (e.CommandName == "ExportToWord")
            {
                this.isAll = true;
            }
        }
        
        #endregion

        //返回按钮
        public void btnReturn_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("");
        }


    }
}
