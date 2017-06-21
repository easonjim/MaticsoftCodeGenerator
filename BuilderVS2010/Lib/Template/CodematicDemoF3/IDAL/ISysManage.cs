using System.Data;
using <$$namespace$$>.Model;
namespace <$$namespace$$>.IDAL
{
	/// <summary>
	/// �ӿڲ�-ϵͳ����
	/// </summary>
	public interface ISysManage
	{
		int AddTreeNode(SysNode node);
		void UpdateNode(SysNode node);
		void DelTreeNode(int nodeid);
		DataSet GetTreeList(string strWhere);
		SysNode GetNode(int NodeID);
		void AddLog(string time,string loginfo,string Particular);
		void DeleteLog(int ID);
		void DelOverdueLog(int days);
		void DeleteLog(string strWhere);
		DataSet GetLogs(string strWhere);
		DataRow GetLog(string ID);


	}
}
