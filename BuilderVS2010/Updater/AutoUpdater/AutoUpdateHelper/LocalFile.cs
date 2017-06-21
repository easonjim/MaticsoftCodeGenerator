﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Autoupdater
{
    public class LocalFile
    {
        #region The private fields
        private string path = "";
        private string lastver = "";
        private int size = 0;
        private string version = "";
        private string tryTimes = "";
        #endregion

        #region The public property
        [XmlAttribute("path")]
        public string Path { get { return path; } set { path = value; } }
        [XmlAttribute("lastver")]
        public string LastVer { get { return lastver; } set { lastver = value; } }
        [XmlAttribute("size")]
        public int Size { get { return size; } set { size = value; } }
        [XmlAttribute("version")]
        public string Version { get { return version; } set { version = value; } }
        [XmlAttribute("tryTimes")]
        public string TryTimes { get { return tryTimes; } set { tryTimes = value; } }
        #endregion

        #region The constructor of LocalFile
        public LocalFile(string path, string ver, int size, string versionid)
        {
            this.path = path;
            this.lastver = ver;
            this.size = size;
            this.version = versionid;
        }

        public LocalFile()
        {
        }
        #endregion

    }
}
