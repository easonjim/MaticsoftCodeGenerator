using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

namespace Maticsoft.DBUtility
{
	/// <summary>
    /// Copyright (C) Maticsoft
	/// ���ݷ��ʻ�����(����Oracle)
	/// �����û������޸������Լ���Ŀ����Ҫ��
	/// </summary>
	public abstract class DbHelperOra
	{
        //���ݿ������ַ���(web.config������)�����Զ�̬����connectionString֧�ֶ����ݿ�.		
        public static string connectionString = PubConstant.ConnectionString;     
		public DbHelperOra()
		{			
		}

        #region ���÷���
        
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool Exists(string strSql, params OracleParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

       
        #endregion

		
		#region  ִ�м�SQL���

		/// <summary>
		/// ִ��SQL��䣬����Ӱ��ļ�¼��
		/// </summary>
		/// <param name="SQLString">SQL���</param>
		/// <returns>Ӱ��ļ�¼��</returns>
		public static int ExecuteSql(string SQLString)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{				
				using (OracleCommand cmd = new OracleCommand(SQLString,connection))
				{
					try
					{		
						connection.Open();
						int rows=cmd.ExecuteNonQuery();
						return rows;
					}
					catch(System.Data.OracleClient.OracleException E)
					{					
						connection.Close();
						throw new Exception(E.Message);
					}
				}				
			}
		}
		
		/// <summary>
		/// ִ�ж���SQL��䣬ʵ�����ݿ�����
		/// </summary>
		/// <param name="SQLStringList">����SQL���</param>		
		public static void ExecuteSqlTran(ArrayList SQLStringList)
		{
			using (OracleConnection conn = new OracleConnection(connectionString))
			{
				conn.Open();
				OracleCommand cmd = new OracleCommand();
				cmd.Connection=conn;				
				OracleTransaction tx=conn.BeginTransaction();			
				cmd.Transaction=tx;				
				try
				{   		
					for(int n=0;n<SQLStringList.Count;n++)
					{
						string strsql=SQLStringList[n].ToString();
						if (strsql.Trim().Length>1)
						{
							cmd.CommandText=strsql;
							cmd.ExecuteNonQuery();
						}
					}										
					tx.Commit();					
				}
				catch(System.Data.OracleClient.OracleException E)
				{		
					tx.Rollback();
					throw new Exception(E.Message);
				}
			}
		}
		/// <summary>
		/// ִ�д�һ���洢���̲����ĵ�SQL��䡣
		/// </summary>
		/// <param name="SQLString">SQL���</param>
		/// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
		/// <returns>Ӱ��ļ�¼��</returns>
		public static int ExecuteSql(string SQLString,string content)
		{				
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				OracleCommand cmd = new OracleCommand(SQLString,connection);
                System.Data.OracleClient.OracleParameter myParameter = new System.Data.OracleClient.OracleParameter("@content", OracleType.NVarChar);
				myParameter.Value = content ;
				cmd.Parameters.Add(myParameter);
				try
				{
					connection.Open();
					int rows=cmd.ExecuteNonQuery();
					return rows;
				}
				catch(System.Data.OracleClient.OracleException E)
				{				
					throw new Exception(E.Message);
				}
				finally
				{
					cmd.Dispose();
					connection.Close();
				}	
			}
		}		
		/// <summary>
		/// �����ݿ������ͼ���ʽ���ֶ�(������������Ƶ���һ��ʵ��)
		/// </summary>
		/// <param name="strSQL">SQL���</param>
		/// <param name="fs">ͼ���ֽ�,���ݿ���ֶ�����Ϊimage�����</param>
		/// <returns>Ӱ��ļ�¼��</returns>
		public static int ExecuteSqlInsertImg(string strSQL,byte[] fs)
		{		
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				OracleCommand cmd = new OracleCommand(strSQL,connection);
                System.Data.OracleClient.OracleParameter myParameter = new System.Data.OracleClient.OracleParameter("@fs", OracleType.LongRaw);
				myParameter.Value = fs ;
				cmd.Parameters.Add(myParameter);
				try
				{
					connection.Open();
					int rows=cmd.ExecuteNonQuery();
					return rows;
				}
				catch(System.Data.OracleClient.OracleException E)
				{				
					throw new Exception(E.Message);
				}
				finally
				{
					cmd.Dispose();
					connection.Close();
				}				
			}
		}
		
		/// <summary>
		/// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
		/// </summary>
		/// <param name="SQLString">�����ѯ������</param>
		/// <returns>��ѯ�����object��</returns>
		public static object GetSingle(string SQLString)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				using(OracleCommand cmd = new OracleCommand(SQLString,connection))
				{
					try
					{
						connection.Open();
						object obj = cmd.ExecuteScalar();
						if((Object.Equals(obj,null))||(Object.Equals(obj,System.DBNull.Value)))
						{					
							return null;
						}
						else
						{
							return obj;
						}				
					}
					catch(System.Data.OracleClient.OracleException e)
					{						
						connection.Close();
						throw new Exception(e.Message);
					}	
				}
			}
		}
		/// <summary>
        /// ִ�в�ѯ��䣬����OracleDataReader ( ע�⣺���ø÷�����һ��Ҫ��SqlDataReader����Close )
		/// </summary>
		/// <param name="strSQL">��ѯ���</param>
		/// <returns>OracleDataReader</returns>
		public static OracleDataReader ExecuteReader(string strSQL)
		{
			OracleConnection connection = new OracleConnection(connectionString);			
			OracleCommand cmd = new OracleCommand(strSQL,connection);				
			try
			{
				connection.Open();
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				return myReader;
			}
			catch(System.Data.OracleClient.OracleException e)
			{								
				throw new Exception(e.Message);
			}			
			
		}		
		/// <summary>
		/// ִ�в�ѯ��䣬����DataSet
		/// </summary>
		/// <param name="SQLString">��ѯ���</param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string SQLString)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				DataSet ds = new DataSet();
				try
				{
					connection.Open();
					OracleDataAdapter command = new OracleDataAdapter(SQLString,connection);				
					command.Fill(ds,"ds");
				}
				catch(System.Data.OracleClient.OracleException ex)
				{				
					throw new Exception(ex.Message);
				}			
				return ds;
			}			
		}


		#endregion

		#region ִ�д�������SQL���

		/// <summary>
		/// ִ��SQL��䣬����Ӱ��ļ�¼��
		/// </summary>
		/// <param name="SQLString">SQL���</param>
		/// <returns>Ӱ��ļ�¼��</returns>
		public static int ExecuteSql(string SQLString,params OracleParameter[] cmdParms)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{				
				using (OracleCommand cmd = new OracleCommand())
				{
					try
					{		
						PrepareCommand(cmd, connection, null,SQLString, cmdParms);
						int rows=cmd.ExecuteNonQuery();
						cmd.Parameters.Clear();
						return rows;
					}
					catch(System.Data.OracleClient.OracleException E)
					{				
						throw new Exception(E.Message);
					}
				}				
			}
		}
		
			
		/// <summary>
		/// ִ�ж���SQL��䣬ʵ�����ݿ�����
		/// </summary>
		/// <param name="SQLStringList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����OracleParameter[]��</param>
		public static void ExecuteSqlTran(Hashtable SQLStringList)
		{			
			using (OracleConnection conn = new OracleConnection(connectionString))
			{
				conn.Open();
				using (OracleTransaction trans = conn.BeginTransaction()) 
				{
					OracleCommand cmd = new OracleCommand();
					try 
					{
						//ѭ��
						foreach (DictionaryEntry myDE in SQLStringList)
						{	
							string 	cmdText=myDE.Key.ToString();
							OracleParameter[] cmdParms=(OracleParameter[])myDE.Value;
							PrepareCommand(cmd,conn,trans,cmdText, cmdParms);
							int val = cmd.ExecuteNonQuery();
							cmd.Parameters.Clear();

							trans.Commit();
						}					
					}
					catch 
					{
						trans.Rollback();
						throw;
					}
				}				
			}
		}
	
				
		/// <summary>
		/// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
		/// </summary>
		/// <param name="SQLString">�����ѯ������</param>
		/// <returns>��ѯ�����object��</returns>
		public static object GetSingle(string SQLString,params OracleParameter[] cmdParms)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				using (OracleCommand cmd = new OracleCommand())
				{
					try
					{
						PrepareCommand(cmd, connection, null,SQLString, cmdParms);
						object obj = cmd.ExecuteScalar();
						cmd.Parameters.Clear();
						if((Object.Equals(obj,null))||(Object.Equals(obj,System.DBNull.Value)))
						{					
							return null;
						}
						else
						{
							return obj;
						}				
					}
					catch(System.Data.OracleClient.OracleException e)
					{				
						throw new Exception(e.Message);
					}					
				}
			}
		}
		
		/// <summary>
        /// ִ�в�ѯ��䣬����OracleDataReader ( ע�⣺���ø÷�����һ��Ҫ��SqlDataReader����Close )
		/// </summary>
		/// <param name="strSQL">��ѯ���</param>
		/// <returns>OracleDataReader</returns>
		public static OracleDataReader ExecuteReader(string SQLString,params OracleParameter[] cmdParms)
		{		
			OracleConnection connection = new OracleConnection(connectionString);
			OracleCommand cmd = new OracleCommand();				
			try
			{
				PrepareCommand(cmd, connection, null,SQLString, cmdParms);
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				cmd.Parameters.Clear();
				return myReader;
			}
			catch(System.Data.OracleClient.OracleException e)
			{								
				throw new Exception(e.Message);
			}					
			
		}		
		
		/// <summary>
		/// ִ�в�ѯ��䣬����DataSet
		/// </summary>
		/// <param name="SQLString">��ѯ���</param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string SQLString,params OracleParameter[] cmdParms)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				OracleCommand cmd = new OracleCommand();
				PrepareCommand(cmd, connection, null,SQLString, cmdParms);
				using( OracleDataAdapter da = new OracleDataAdapter(cmd) )
				{
					DataSet ds = new DataSet();	
					try
					{												
						da.Fill(ds,"ds");
						cmd.Parameters.Clear();
					}
					catch(System.Data.OracleClient.OracleException ex)
					{				
						throw new Exception(ex.Message);
					}			
					return ds;
				}				
			}			
		}


		private static void PrepareCommand(OracleCommand cmd,OracleConnection conn,OracleTransaction trans, string cmdText, OracleParameter[] cmdParms) 
		{
			if (conn.State != ConnectionState.Open)
				conn.Open();
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			if (trans != null)
				cmd.Transaction = trans;
			cmd.CommandType = CommandType.Text;//cmdType;
			if (cmdParms != null) 
			{
				foreach (OracleParameter parm in cmdParms)
					cmd.Parameters.Add(parm);
			}
		}

		#endregion

		#region �洢���̲���

		/// <summary>
        /// ִ�д洢���� ����SqlDataReader ( ע�⣺���ø÷�����һ��Ҫ��SqlDataReader����Close )
		/// </summary>
		/// <param name="storedProcName">�洢������</param>
		/// <param name="parameters">�洢���̲���</param>
		/// <returns>OracleDataReader</returns>
		public static OracleDataReader RunProcedure(string storedProcName, IDataParameter[] parameters )
		{
			OracleConnection connection = new OracleConnection(connectionString);
			OracleDataReader returnReader;
			connection.Open();
			OracleCommand command = BuildQueryCommand( connection,storedProcName, parameters );
			command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);				
			return returnReader;			
		}
		
		
		/// <summary>
		/// ִ�д洢����
		/// </summary>
		/// <param name="storedProcName">�洢������</param>
		/// <param name="parameters">�洢���̲���</param>
		/// <param name="tableName">DataSet����еı���</param>
		/// <returns>DataSet</returns>
		public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName )
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				DataSet dataSet = new DataSet();
				connection.Open();
				OracleDataAdapter sqlDA = new OracleDataAdapter();
				sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters );
				sqlDA.Fill( dataSet, tableName );
				connection.Close();
				return dataSet;
			}
		}

		
		/// <summary>
		/// ���� OracleCommand ����(��������һ���������������һ������ֵ)
		/// </summary>
		/// <param name="connection">���ݿ�����</param>
		/// <param name="storedProcName">�洢������</param>
		/// <param name="parameters">�洢���̲���</param>
		/// <returns>OracleCommand</returns>
		private static OracleCommand BuildQueryCommand(OracleConnection connection,string storedProcName, IDataParameter[] parameters)
		{			
			OracleCommand command = new OracleCommand( storedProcName, connection );
			command.CommandType = CommandType.StoredProcedure;
			foreach (OracleParameter parameter in parameters)
			{
				command.Parameters.Add( parameter );
			}
			return command;			
		}
		
		/// <summary>
		/// ִ�д洢���̣�����Ӱ�������		
		/// </summary>
		/// <param name="storedProcName">�洢������</param>
		/// <param name="parameters">�洢���̲���</param>
		/// <param name="rowsAffected">Ӱ�������</param>
		/// <returns></returns>
		public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected )
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				int result;
				connection.Open();
				OracleCommand command = BuildIntCommand(connection,storedProcName, parameters );
				rowsAffected = command.ExecuteNonQuery();
				result = (int)command.Parameters["ReturnValue"].Value;
				//Connection.Close();
				return result;
			}
		}
		
		/// <summary>
		/// ���� OracleCommand ����ʵ��(��������һ������ֵ)	
		/// </summary>
		/// <param name="storedProcName">�洢������</param>
		/// <param name="parameters">�洢���̲���</param>
		/// <returns>OracleCommand ����ʵ��</returns>
		private static OracleCommand BuildIntCommand(OracleConnection connection,string storedProcName, IDataParameter[] parameters)
		{
			OracleCommand command = BuildQueryCommand(connection,storedProcName, parameters );
			command.Parameters.Add( new OracleParameter ( "ReturnValue",
                OracleType.Int32, 4, ParameterDirection.ReturnValue,
				false,0,0,string.Empty,DataRowVersion.Default,null ));
			return command;
		}
		#endregion	

	}
}
