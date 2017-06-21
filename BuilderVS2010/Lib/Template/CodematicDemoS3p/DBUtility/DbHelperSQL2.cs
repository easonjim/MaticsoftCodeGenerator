using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Maticsoft.DBUtility
{
	/// <summary>
    /// Enterprise Library 2.0 ���ݷ��ʽ�һ����װ��
    /// Copyright (C) Maticsoft
	/// All rights reserved	
	/// </summary>
	public abstract class DbHelperSQL2
	{        
		public DbHelperSQL2()
		{
        }

        #region ���÷���

        public static int GetMaxID(string FieldName,string TableName)
        {
            string strSql = "select max(" + FieldName + ")+1 from " + TableName;
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);           
            object obj = db.ExecuteScalar(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
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
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            object obj = db.ExecuteScalar(dbCommand);
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
        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            BuildDBParameter(db, dbCommand, cmdParms);            
            object obj = db.ExecuteScalar(dbCommand);           
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
        
        /// <summary>
        /// ���ز���
        /// </summary>
        public static void BuildDBParameter(Database db, DbCommand dbCommand, params SqlParameter[] cmdParms)
        {
            foreach (SqlParameter sp in cmdParms)
            {
                db.AddInParameter(dbCommand, sp.ParameterName, sp.DbType,sp.Value);
            }
        }
        #endregion

        #region  ִ�м�SQL���

        /// <summary>
		/// ִ��SQL��䣬����Ӱ��ļ�¼��
		/// </summary>
		/// <param name="strSql">SQL���</param>
		/// <returns>Ӱ��ļ�¼��</returns>
        public static int ExecuteSql(string strSql)
		{    
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(dbCommand);
		}

		public static int ExecuteSqlByTime(string strSql,int Times)
		{           
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            dbCommand.CommandTimeout = Times;
            return db.ExecuteNonQuery(dbCommand);
		}
		
		/// <summary>
		/// ִ�ж���SQL��䣬ʵ�����ݿ�����
		/// </summary>
		/// <param name="SQLStringList">����SQL���</param>		
		public static void ExecuteSqlTran(ArrayList SQLStringList)
		{

            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection dbconn = db.CreateConnection())
            { 
                dbconn.Open();
                DbTransaction dbtran = dbconn.BeginTransaction();
                try
                {
                    //ִ�����
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            DbCommand dbCommand = db.GetSqlStringCommand(strsql);
                            db.ExecuteNonQuery(dbCommand);
                        }
                    }
                    //ִ�д洢����
                    //db.ExecuteNonQuery(CommandType.StoredProcedure, "InserOrders");
                    //db.ExecuteDataSet(CommandType.StoredProcedure, "UpdateProducts");
                    dbtran.Commit();
                }
                catch
                {
                    dbtran.Rollback();
                }
                finally
                {
                    dbconn.Close();
                }
            }
        }

        #region ִ��һ�� �����ֶδ����������
        /// <summary>
		/// ִ�д�һ���洢���̲����ĵ�SQL��䡣
		/// </summary>
		/// <param name="strSql">SQL���</param>
		/// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
		/// <returns>Ӱ��ļ�¼��</returns>
		public static int ExecuteSql(string strSql,string content)
		{	
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            db.AddInParameter(dbCommand, "@content", DbType.String, content);
            return db.ExecuteNonQuery(dbCommand);
		}		
		
        /// <summary>
		/// ִ�д�һ���洢���̲����ĵ�SQL��䡣
		/// </summary>
		/// <param name="strSql">SQL���</param>
		/// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
		/// <returns>���������Ĳ�ѯ���</returns>
		public static object ExecuteSqlGet(string strSql,string content)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            db.AddInParameter(dbCommand, "@content", DbType.String, content);
            object obj = db.ExecuteNonQuery(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return null;
            }
            else
            {
                return obj;
            }		
		}		
		
        /// <summary>
		/// �����ݿ������ͼ���ʽ���ֶ�(������������Ƶ���һ��ʵ��)
		/// </summary>
		/// <param name="strSql">SQL���</param>
		/// <param name="fs">ͼ���ֽ�,���ݿ���ֶ�����Ϊimage�����</param>
		/// <returns>Ӱ��ļ�¼��</returns>
		public static int ExecuteSqlInsertImg(string strSql,byte[] fs)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            db.AddInParameter(dbCommand, "@fs", DbType.Byte, fs);
            return db.ExecuteNonQuery(dbCommand);			
        }
        #endregion

        /// <summary>
		/// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
		/// </summary>
		/// <param name="strSql">�����ѯ������</param>
		/// <returns>��ѯ�����object��</returns>
		public static object GetSingle(string strSql)
		{            
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            object obj = db.ExecuteScalar(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return null;
            }
            else
            {
                return obj;
            }      
		}
		
        /// <summary>
        /// ִ�в�ѯ��䣬����SqlDataReader ( ע�⣺ʹ�ú�һ��Ҫ��SqlDataReader����Close )
		/// </summary>
		/// <param name="strSql">��ѯ���</param>
		/// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSql)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            SqlDataReader dr = (SqlDataReader)db.ExecuteReader(dbCommand);
            return dr;      
			
		}		
		
        /// <summary>
		/// ִ�в�ѯ��䣬����DataSet
		/// </summary>
		/// <param name="strSql">��ѯ���</param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string strSql)
		{            
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            return db.ExecuteDataSet(dbCommand);
            
		}
		public static DataSet Query(string strSql,int Times)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            dbCommand.CommandTimeout = Times;
            return db.ExecuteDataSet(dbCommand);
		}

		#endregion

		#region ִ�д�������SQL���

		/// <summary>
		/// ִ��SQL��䣬����Ӱ��ļ�¼��
		/// </summary>
		/// <param name="strSql">SQL���</param>
		/// <returns>Ӱ��ļ�¼��</returns>
		public static int ExecuteSql(string strSql,params SqlParameter[] cmdParms)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            BuildDBParameter(db, dbCommand, cmdParms);    
            return db.ExecuteNonQuery(dbCommand);
		}
		
			
		/// <summary>
		/// ִ�ж���SQL��䣬ʵ�����ݿ�����
		/// </summary>
		/// <param name="SQLStringList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����SqlParameter[]��</param>
		public static void ExecuteSqlTran(Hashtable SQLStringList)
		{
            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                DbTransaction dbtran = dbconn.BeginTransaction();
                try
                {
                    //ִ�����
                    foreach (DictionaryEntry myDE in SQLStringList)
                    {
                        string strsql = myDE.Key.ToString();
                        SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;                        
                        if (strsql.Trim().Length > 1)
                        {
                            DbCommand dbCommand = db.GetSqlStringCommand(strsql);
                            BuildDBParameter(db, dbCommand, cmdParms);    
                            db.ExecuteNonQuery(dbCommand);
                        }
                    }
                    dbtran.Commit();
                }
                catch
                {
                    dbtran.Rollback();
                }
                finally
                {
                    dbconn.Close();
                }
            }
		}
	
				
		/// <summary>
		/// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
		/// </summary>
		/// <param name="strSql">�����ѯ������</param>
		/// <returns>��ѯ�����object��</returns>
		public static object GetSingle(string strSql,params SqlParameter[] cmdParms)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            BuildDBParameter(db, dbCommand, cmdParms);    
            object obj = db.ExecuteScalar(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return null;
            }
            else
            {
                return obj;
            }      
		}
		
		/// <summary>
        /// ִ�в�ѯ��䣬����SqlDataReader ( ע�⣺ʹ�ú�һ��Ҫ��SqlDataReader����Close )
		/// </summary>
		/// <param name="strSql">��ѯ���</param>
		/// <returns>SqlDataReader</returns>
		public static SqlDataReader ExecuteReader(string strSql,params SqlParameter[] cmdParms)
		{		
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            BuildDBParameter(db, dbCommand, cmdParms);
            SqlDataReader dr = (SqlDataReader)db.ExecuteReader(dbCommand);
            return dr;
			
		}		
		
		/// <summary>
		/// ִ�в�ѯ��䣬����DataSet
		/// </summary>
		/// <param name="strSql">��ѯ���</param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string strSql,params SqlParameter[] cmdParms)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            BuildDBParameter(db, dbCommand, cmdParms);   
            return db.ExecuteDataSet(dbCommand);
		}


		private static void PrepareCommand(SqlCommand cmd,SqlConnection conn,SqlTransaction trans, string cmdText, SqlParameter[] cmdParms) 
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
                foreach (SqlParameter parameter in cmdParms)
				{
					if ( ( parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input ) && 
						(parameter.Value == null))
					{
						parameter.Value = DBNull.Value;
					}
					cmd.Parameters.Add(parameter);
				}
			}
		}

		#endregion

		#region �洢���̲���

        /// <summary>
        /// ִ�д洢���̣�����Ӱ�������		
        /// </summary>       
        public static int RunProcedure(string storedProcName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName);
            return db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// ִ�д洢���̣��������������ֵ��Ӱ�������		
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <param name="OutParameter">�����������</param>
        /// <param name="rowsAffected">Ӱ�������</param>
        /// <returns></returns>
        public static object RunProcedure(string storedProcName, IDataParameter[] InParameters, SqlParameter OutParameter, int rowsAffected)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName);
            BuildDBParameter(db, dbCommand, (SqlParameter[])InParameters);
            db.AddOutParameter(dbCommand, OutParameter.ParameterName, OutParameter.DbType, OutParameter.Size);
            rowsAffected = db.ExecuteNonQuery(dbCommand);
            return db.GetParameterValue(dbCommand,"@" + OutParameter.ParameterName);  //�õ����������ֵ
        }

		/// <summary>
        /// ִ�д洢���̣�����SqlDataReader ( ע�⣺ʹ�ú�һ��Ҫ��SqlDataReader����Close )
		/// </summary>
		/// <param name="storedProcName">�洢������</param>
		/// <param name="parameters">�洢���̲���</param>
		/// <returns>SqlDataReader</returns>
		public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters )
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName, parameters);            
            //BuildDBParameter(db, dbCommand, parameters);
            return (SqlDataReader)db.ExecuteReader(dbCommand);           
		}
				
		/// <summary>
        /// ִ�д洢���̣�����DataSet
		/// </summary>
		/// <param name="storedProcName">�洢������</param>
		/// <param name="parameters">�洢���̲���</param>
		/// <param name="tableName">DataSet����еı���</param>
		/// <returns>DataSet</returns>
		public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName )
		{           
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName, parameters);
            //BuildDBParameter(db, dbCommand, parameters);
            return db.ExecuteDataSet(dbCommand); 
		}
        /// <summary>
        /// ִ�д洢���̣�����DataSet(�趨�ȴ�ʱ��)
        /// </summary>
		public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName ,int Times)
		{           
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName, parameters);
            dbCommand.CommandTimeout = Times;
            //BuildDBParameter(db, dbCommand, parameters);
            return db.ExecuteDataSet(dbCommand); 
		}

		
		/// <summary>
		/// ���� SqlCommand ����(��������һ���������������һ������ֵ)
		/// </summary>
		/// <param name="connection">���ݿ�����</param>
		/// <param name="storedProcName">�洢������</param>
		/// <param name="parameters">�洢���̲���</param>
		/// <returns>SqlCommand</returns>
		private static SqlCommand BuildQueryCommand(SqlConnection connection,string storedProcName, IDataParameter[] parameters)
		{			
			SqlCommand command = new SqlCommand( storedProcName, connection );
			command.CommandType = CommandType.StoredProcedure;
			foreach (SqlParameter parameter in parameters)
			{
				if( parameter != null )
				{
					// ���δ����ֵ���������,���������DBNull.Value.
					if ( ( parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input ) && 
						(parameter.Value == null))
					{
						parameter.Value = DBNull.Value;
					}
					command.Parameters.Add(parameter);
				}
			}			
			return command;			
		}		
		/// <summary>
		/// ���� SqlCommand ����ʵ��(��������һ������ֵ)	
		/// </summary>
		/// <param name="storedProcName">�洢������</param>
		/// <param name="parameters">�洢���̲���</param>
		/// <returns>SqlCommand ����ʵ��</returns>
		private static SqlCommand BuildIntCommand(SqlConnection connection,string storedProcName, IDataParameter[] parameters)
		{
			SqlCommand command = BuildQueryCommand(connection,storedProcName, parameters );
			command.Parameters.Add( new SqlParameter ( "ReturnValue",
				SqlDbType.Int,4,ParameterDirection.ReturnValue,
				false,0,0,string.Empty,DataRowVersion.Default,null ));
			return command;
		}
		#endregion	

	}

}
