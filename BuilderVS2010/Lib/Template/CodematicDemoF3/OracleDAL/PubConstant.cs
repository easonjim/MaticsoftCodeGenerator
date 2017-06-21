using System;
using System.Configuration;
namespace <$$namespace$$>.OracleDAL
{

    public class PubConstant
    {
        /// <summary>
        /// �õ�web.config������������ݿ������ַ�����
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = ConfigurationManager.AppSettings[configName];
            string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
            if (ConStringEncrypt == "true")
            {
                connectionString = Maticsoft.Common.DEncrypt.DESEncrypt.Decrypt(connectionString);
            }
            return connectionString;
        }

    }
}
