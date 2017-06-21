using System.Text;
using System.Data;
using <$$namespace$$>.IDAL;
using Maticsoft.DBUtility;
namespace <$$namespace$$>.SQLServerDAL
{
	/// <summary>
	/// ϵͳ�˵������ࡣ(��ͨSQLʵ�ַ�ʽ)
	/// </summary>
    public class SysManage : ISysManage
	{
        //��������Ը������ݿ�,֧�ֶ����ݿ⣬֧�ֲ��ü��ܷ�ʽʵ��
        //DbHelperSQLP DbHelperSQL = new DbHelperSQLP(PubConstant.GetConnectionString("ConnectionString2"));
		public SysManage()
		{            
		}
        
		public int GetMaxId()
		{
			string strsql="select max(NodeID)+1 from S_Tree";
			object obj=DbHelperSQL.GetSingle(strsql);
			if(obj==null)
			{
				return 1;
			}
			else
			{
				return int.Parse(obj.ToString());
			}
		}

        public int AddTreeNode(<$$namespace$$>.Model.SysNode node)
		{
			node.NodeID=GetMaxId();

			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into S_Tree(");
			strSql.Append("NodeID,Text,ParentID,Location,OrderID,comment,Url,PermissionID,ImageUrl)");
			strSql.Append(" values (");
			strSql.Append("'"+node.NodeID+"',");
			strSql.Append("'"+node.Text+"',");
			strSql.Append(""+node.ParentID+",");			
			strSql.Append("'"+node.Location+"',");
			strSql.Append(""+node.OrderID+",");		
			strSql.Append("'"+node.Comment+"',");
			strSql.Append("'"+node.Url+"',");
			strSql.Append(""+node.PermissionID+",");
			strSql.Append("'"+node.ImageUrl+"'");
			strSql.Append(")");						
			DbHelperSQL.ExecuteSql(strSql.ToString());
			return node.NodeID;

		}

        public void UpdateNode(<$$namespace$$>.Model.SysNode node)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update S_Tree set ");
			strSql.Append("Text='"+node.Text+"',");
			strSql.Append("ParentID="+node.ParentID.ToString()+",");
			strSql.Append("Location='"+node.Location+"',");
			strSql.Append("OrderID="+node.OrderID+",");
			strSql.Append("comment='"+node.Comment+"',");
			strSql.Append("Url='"+node.Url+"',");
			strSql.Append("PermissionID="+node.PermissionID+",");
			strSql.Append("ImageUrl='"+node.ImageUrl+"'");
			strSql.Append(" where NodeID="+node.NodeID);
			DbHelperSQL.ExecuteSql(strSql.ToString());

		}

		public void DelTreeNode(int nodeid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete S_Tree ");
			strSql.Append(" where NodeID="+nodeid);					
			DbHelperSQL.ExecuteSql(strSql.ToString());
		}


		public DataSet GetTreeList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * from S_Tree ");	
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by parentid,orderid ");

			return DbHelperSQL.Query(strSql.ToString());
		}


		/// <summary>
		/// �õ��˵��ڵ�
		/// </summary>
		/// <param name="NodeID"></param>
		/// <returns></returns>
        public <$$namespace$$>.Model.SysNode GetNode(int NodeID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * from S_Tree ");	
			strSql.Append(" where NodeID="+NodeID);
            <$$namespace$$>.Model.SysNode node = new <$$namespace$$>.Model.SysNode();
            DataSet ds=DbHelperSQL.Query(strSql.ToString());
			if(ds.Tables[0].Rows.Count>0)
			{
				node.NodeID=int.Parse(ds.Tables[0].Rows[0]["NodeID"].ToString());
				node.Text=ds.Tables[0].Rows[0]["text"].ToString();
				if(ds.Tables[0].Rows[0]["ParentID"].ToString()!="")
				{
					node.ParentID=int.Parse(ds.Tables[0].Rows[0]["ParentID"].ToString());
				}
				node.Location=ds.Tables[0].Rows[0]["Location"].ToString();
				if(ds.Tables[0].Rows[0]["OrderID"].ToString()!="")
				{
					node.OrderID=int.Parse(ds.Tables[0].Rows[0]["OrderID"].ToString());
				}
				node.Comment=ds.Tables[0].Rows[0]["comment"].ToString();
				node.Url=ds.Tables[0].Rows[0]["url"].ToString();
				if(ds.Tables[0].Rows[0]["PermissionID"].ToString()!="")
				{
					node.PermissionID=int.Parse(ds.Tables[0].Rows[0]["PermissionID"].ToString());
				}
				node.ImageUrl=ds.Tables[0].Rows[0]["ImageUrl"].ToString();	
								
				return node;
			}
			else
			{
				return null;
			}			
		}

		#region ��־
		/// <summary>
		/// ������־
		/// </summary>
		/// <param name="time"></param>
		/// <param name="loginfo"></param>
		public void AddLog(string time,string loginfo,string Particular)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into S_Log(");
			strSql.Append("datetime,loginfo,Particular)");
			strSql.Append(" values (");
			strSql.Append("'"+time+"',");
			strSql.Append("'"+loginfo+"',");	
			strSql.Append("'"+Particular+"'");	
			strSql.Append(")");						
			DbHelperSQL.ExecuteSql(strSql.ToString());			
		}
		public void DeleteLog(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete S_Log ");	
			strSql.Append(" where ID= "+ID);
			DbHelperSQL.ExecuteSql(strSql.ToString());
		}
		public void DelOverdueLog(int days)
		{			
			string str=" DATEDIFF(day,[datetime],getdate())>"+days;
			DeleteLog(str);
		}
		public void DeleteLog(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete S_Log ");	
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			DbHelperSQL.ExecuteSql(strSql.ToString());
		}
		public DataSet GetLogs(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * from S_Log ");	
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by ID DESC");
			return DbHelperSQL.Query(strSql.ToString());
		}
		public DataRow GetLog(string ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * from S_Log ");				
			strSql.Append(" where ID= "+ID);
			return DbHelperSQL.Query(strSql.ToString()).Tables[0].Rows[0];
		}
		#endregion
		
	}
}
