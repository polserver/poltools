using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace LoginServer
{
    class Options
    {
        private static string _sqlhost;
        private static int _sqlport;
        private static string _sqluser;
        private static string _sqlpass;
        private static int _loginport;
        private static string _db;

        public Options() 
        { }

        public string MySQL_Host
        {
            get
            {
                return _sqlhost;
            }
        }

        public int MySQL_Port
        {
            get
            {
                return _sqlport;
            }
        }

        public string MySQL_User
        {
            get
            {
                return _sqluser;
            }
        }

        public string MySQL_Pass
        {
            get
            {
                return _sqlpass;
            }
        }

        public int LoginServer_Port
        {
            get
            {
                return _loginport;
            }
        }

        public string My_DB
        {
            get
            {
                return _db;
            }
        }

        public bool Load()
        {
            string optionsFile = Path.Combine(Program.ServerPath, "Options.xml");
            if (!File.Exists(optionsFile))
            {
                Program.running = false;
                return false;
            }

            XmlDocument dom = new XmlDocument();
            dom.Load(optionsFile);
            XmlElement xOptions = dom["Options"];
            XmlElement xMySQL = (XmlElement)xOptions.SelectSingleNode("MySQL");
            XmlElement xLoginServer = (XmlElement)xOptions.SelectSingleNode("LoginServer");

            XmlElement elem = (XmlElement)xMySQL.SelectSingleNode("Host");
            if (elem != null)
                _sqlhost = elem.InnerText;
            else
                _sqlhost = "127.0.0.1";

            elem = (XmlElement)xMySQL.SelectSingleNode("Port");
            if (elem != null)
                int.TryParse(elem.InnerText, out _sqlport);
            else
                _sqlport = 3306;

            elem = (XmlElement)xMySQL.SelectSingleNode("Username");
            if (elem != null)
                _sqluser = elem.InnerText;
            else
                _sqluser = "root";

            elem = (XmlElement)xMySQL.SelectSingleNode("Password");
            if (elem != null)
                _sqlpass = elem.InnerText;
            else
                _sqlpass = "";

            elem = (XmlElement)xMySQL.SelectSingleNode("Database");
            if (elem != null)
                _db = elem.InnerText;
            else
                _db = "pol_loginserver";

            elem = (XmlElement)xLoginServer.SelectSingleNode("Port");
            if (elem != null)
                int.TryParse(elem.InnerText, out _loginport);
            else
                _loginport = 5003;

            return true;
        
        }
    }
}
