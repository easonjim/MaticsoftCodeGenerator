using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Autoupdater
{
    public class Config
    {
        #region The private fields
        private bool enabled = true;
        private string serverUrl = string.Empty;
        private string userName = string.Empty;
        private string passWord = string.Empty;
        private string tryTimes = string.Empty;
        private UpdateFileList updateFileList = new UpdateFileList();
        #endregion

        #region The public property
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        public string ServerUrl
        {
            get { return serverUrl; }
            set { serverUrl = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }
        public string TryTimes
        {
            get { return string.IsNullOrEmpty(tryTimes) ? "1":tryTimes; }
            set { tryTimes = value; }
        }

        public UpdateFileList UpdateFileList
        {
            get { return updateFileList; }
            set { updateFileList = value; }
        }
        #endregion

        #region The public method
        public static Config LoadConfig(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Config));
            StreamReader sr = new StreamReader(file);
            Config config = xs.Deserialize(sr) as Config;
            sr.Close();

            return config;
        }

        public void SaveConfig(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Config));
            StreamWriter sw = new StreamWriter(file);
            xs.Serialize(sw, this);
            sw.Close();
        }
        #endregion
    }

}
