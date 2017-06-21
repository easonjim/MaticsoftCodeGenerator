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
using Microsoft.Web.UI.WebControls;
namespace Maticsoft.Web.SysManage
{
    public partial class IndexTree : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindTreeView();
                if (this.TreeView1.Nodes.Count == 0)
                {
                    lblTip.Visible = true;
                }
            }
        }


        //����ڵ�
        public void BindTreeView()
        {
            Maticsoft.BLL.SysManage bll = new Maticsoft.BLL.SysManage();
            DataTable dt = bll.GetTreeList("").Tables[0];
            DataRow[] drs = dt.Select("ParentID= " + 0);//ѡ�������ӽڵ�	

            //�˵�״̬           
            bool menuExpand = false;
            TreeView1.Nodes.Clear(); // �����
            foreach (DataRow r in drs)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["Text"].ToString();
                string parentid = r["ParentID"].ToString();
                string location = r["Location"].ToString();
                string url = r["Url"].ToString();
                string imageurl = r["ImageUrl"].ToString();
                int permissionid = int.Parse(r["PermissionID"].ToString().Trim());

                //treeview set
                this.TreeView1.Font.Name = "����";
                this.TreeView1.Font.Size = FontUnit.Parse("9");

                Microsoft.Web.UI.WebControls.TreeNode rootnode = new Microsoft.Web.UI.WebControls.TreeNode();
                rootnode.Text = text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"modify.aspx?id=" + nodeid + "\">�޸�</a> "+
                    "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick=\"if (!window.confirm('�����Ҫɾ��������¼��')){return false;}\" href=\"delete.aspx?id=" + nodeid + "\">ɾ��</a>"+
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"add.aspx?nodeid=" + nodeid + "\">���ӽڵ�</a>";
                rootnode.NodeData = nodeid;
                //rootnode.NavigateUrl = url;
                //rootnode.Target = framename;
                rootnode.Expanded = menuExpand;
                rootnode.ImageUrl = "../" + imageurl;

                TreeView1.Nodes.Add(rootnode);

                int sonparentid = int.Parse(nodeid);// or =location
                CreateNode(sonparentid, rootnode, dt);

            }

        }

        //�����ڵ�
        public void CreateNode(int parentid, Microsoft.Web.UI.WebControls.TreeNode parentnode, DataTable dt)
        {

            DataRow[] drs = dt.Select("ParentID= " + parentid);//ѡ�������ӽڵ�			
            foreach (DataRow r in drs)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["Text"].ToString();
                string location = r["Location"].ToString();
                string url = r["Url"].ToString();
                string imageurl = r["ImageUrl"].ToString();
                int permissionid = int.Parse(r["PermissionID"].ToString().Trim());


                Microsoft.Web.UI.WebControls.TreeNode node = new Microsoft.Web.UI.WebControls.TreeNode();
                node.Text = text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"modify.aspx?id=" + nodeid + "\">�޸�</a> " +
                    "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick=\"if (!window.confirm('�����Ҫɾ��������¼��')){return false;}\" href=\"delete.aspx?id=" + nodeid + "\">ɾ��</a>" +
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"add.aspx?nodeid="+nodeid+"\">���ӽڵ�</a>";
                node.NodeData = nodeid;
                //node.NavigateUrl = url;
                //node.Target = TargetFrame;
                node.ImageUrl = "../" + imageurl;
                node.Expanded = false;
                int sonparentid = int.Parse(nodeid);// or =location

                if (parentnode == null)
                {
                    TreeView1.Nodes.Clear();
                    parentnode = new Microsoft.Web.UI.WebControls.TreeNode();
                    TreeView1.Nodes.Add(parentnode);
                }
                parentnode.Nodes.Add(node);
                CreateNode(sonparentid, node, dt);


            }//endforeach		

        }

    }
}
