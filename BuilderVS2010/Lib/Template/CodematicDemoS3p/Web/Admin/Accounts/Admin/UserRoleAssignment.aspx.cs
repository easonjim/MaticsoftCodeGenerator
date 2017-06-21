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
using LTP.Accounts.Bus;

namespace Maticsoft.Web.Accounts.Admin
{
    public partial class UserRoleAssignment : System.Web.UI.Page//Maticsoft.Web.Accounts.MoviePage
    {

        #region ��ʼ��
        Maticsoft.BLL.Accounts_Users bll = new Maticsoft.BLL.Accounts_Users();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {                
                InitialDataBind();
                ddlUser_SelectedIndexChanged(sender, e);
            }
        }
        #endregion

        #region ���ܺ���

        #region ��ʼ�����ݰ�
        private void InitialDataBind()
        {
            DataSet ds;

            #region ���û������б�
            string[] str = new string[3] { "AA", "AG", "PG" };

            User user = new User();

            this.ddlUser.Items.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                ds = new DataSet();
                ds = user.GetUsersByType(str[i].ToString(), "");

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    string userID = ds.Tables[0].Rows[j]["UserID"].ToString();
                    string userName = ds.Tables[0].Rows[j]["UserName"].ToString();

                    ListItem li = new ListItem(userName, userID);
                    this.ddlUser.Items.Add(li);
                }

                ds.Dispose();
            }
            #endregion

            #region �󶨽�ɫ
            int userid = Convert.ToInt32(this.ddlUser.SelectedValue);
            FillSelectedRoleList(userid);
            FillAllRoleList(userid);
            #endregion
        }
        #endregion

        //����û����еĽ�ɫ
        private void FillSelectedRoleList(int userid)
        {
            this.SelectedRoleList.Items.Clear();

            string strWhere = " UserID=" + userid;

            DataSet ds = new DataSet();
            ds = bll.GetRolesByUser(userid);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string roleid = ds.Tables[0].Rows[i]["Roleid"].ToString();
                string description = ds.Tables[0].Rows[i]["Description"].ToString();

                ListItem li = new ListItem(description,roleid);
                this.SelectedRoleList.Items.Add(li);
            }

            ds.Dispose();

        }

        //����û�û�еĽ�ɫ
        private void FillAllRoleList(int userid)
        {
            this.AllRoleList.Items.Clear();
           
            DataSet ds = new DataSet();
            ds = bll.GetRolesByNoUser(userid);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string roleid = ds.Tables[0].Rows[i]["Roleid"].ToString();
                string description = ds.Tables[0].Rows[i]["Description"].ToString();

                ListItem li = new ListItem(description, roleid);
                this.AllRoleList.Items.Add(li);
            }

            ds.Dispose();

        }

        #endregion

        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            int userid = int.Parse(this.ddlUser.SelectedValue);
            FillSelectedRoleList(userid);
            FillAllRoleList(userid);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.doing.Attributes.Add("display", "none");
            int userid = int.Parse(this.ddlUser.SelectedValue);
            string Idlist = "";
            int num = this.AllRoleList.Items.Count;

            for (int i = 0; i < num; i++)
            {
                if (this.AllRoleList.Items[i].Selected)
                {
                    int roleid = int.Parse(this.AllRoleList.Items[i].Value);
                    string description = this.AllRoleList.Items[i].Text;             

                    bll.Add(userid,roleid);
                    Idlist += roleid + ",";
                 
                }
            }


            #region �����־

            //��ȡ��ǰ�û���Ȩ��
            AccountsPrincipal user = new AccountsPrincipal(Context.User.Identity.Name);
            //��ȡ��ǰ�û�
            User currentUser = new LTP.Accounts.Bus.User(user);
            try
            {

                UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "����Ա��  | ϵͳ���� | �û���ɫȨ���������� |  Ҫ�������û�ID�� " + userid + " , �����õĽ�ɫID�� " + Idlist);
            }
            catch
            {
                UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "����Ա��  |  ϵͳ���� | �û���ɫȨ���������� | Ҫ�������û�ID�� " + userid + " , �����õĽ�ɫID " + Idlist + ", �����־ʧ��");
            }


            #endregion

            ddlUser_SelectedIndexChanged(sender, e);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            this.doing.Attributes.Add("display", "none");
            int userid = int.Parse(this.ddlUser.SelectedValue);
            string Idlist = "";
            int num = this.SelectedRoleList.Items.Count;

            for (int i = 0; i < num; i++)
            {
                if (this.SelectedRoleList.Items[i].Selected)
                {
                    int roleid = int.Parse(this.SelectedRoleList.Items[i].Value);
                    string description = this.SelectedRoleList.Items[i].Text;
                    bll.Delete(userid, roleid);
                    Idlist += roleid + ",";

                }
            }


            #region �����־

            //��ȡ��ǰ�û���Ȩ��
            AccountsPrincipal user = new AccountsPrincipal(Context.User.Identity.Name);
            //��ȡ��ǰ�û�
            User currentUser = new LTP.Accounts.Bus.User(user);
            try
            {

                UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "����Ա��  |  ϵͳ���� | �û���ɫȨ���������� | Ҫ�Ƴ����û�ID�� " + userid + " , ���Ƴ��Ľ�ɫID�� " + Idlist);
            }
            catch
            {
                UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "����Ա��  |  ϵͳ���� | �û���ɫȨ���������� |  Ҫ�Ƴ����û�ID�� " + userid + " , ���Ƴ��Ľ�ɫID�� " + Idlist + ", �����־ʧ��");
            }


            #endregion

            ddlUser_SelectedIndexChanged(sender, e);
        }
            
    }
}
