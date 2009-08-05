/***************************************************************************
 *
 * $Author: Nando_k - nando@polserver.com
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

namespace LoginServer
{
    class ServerList
    {
        List<ServerInfo> _list = new List<ServerInfo>();

        public void AddServer(string name, string host, int port)
        {
            IPAddress tmp_addr;
            if (!IPAddress.TryParse(host, out tmp_addr))
            {
                Console.WriteLine("{0}: Not an ip, trying DNS lookup.", host);
                tmp_addr = Dns.GetHostAddresses(host)[0];
            }
            _list.Add(new ServerInfo(name, tmp_addr, port));
        }

        public void ReportServers()
        {
            int index = 0;
            Console.WriteLine("Existing servers:");
            foreach (ServerInfo server in _list)
            {
                Console.WriteLine("{0}: {1} \t {2}", index++, server.Name, server.ip);
            }
        }

        public void AddServer(ServerInfo server)
        {
            _list.Add(server);
        }
        public List<ServerInfo> Servers 
        {
            get
            {
                return _list;
            }
        }
        public int ServerCount
        {
            get
            {
                return _list.Count;
            }
        }
    }

    /// <summary>
    /// Enumerator accessor for ServerList XML Reading
    /// </summary>
    /// <returns>New Servers Enumerable Object</returns>
    public class Servers : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return new ServerEnumerator();
        }
    }

    /// <summary>
    /// Exposes the server list details from the XML File.
    /// </summary>
    public class ServerEnumerator : IEnumerator
    {
        private readonly string fileName = Path.Combine(Path.GetDirectoryName(Program.ServerPath), "Servers.xml");
        private XmlTextReader reader;

        /// <summary>
        /// Called when the enumerator needs to be reinitialized.
        /// </summary>
        /// <returns></returns>
        public void Reset()
        {
            if (this.reader != null)
                this.reader.Close();
            System.Diagnostics.Debug.Assert(File.Exists(this.fileName),
             "Servers file does not exist!");
            StreamReader stream = new StreamReader(this.fileName);
            this.reader = new XmlTextReader(stream);
        }

        /// <summary>
        /// Called just prior to Current being invoked.  
        /// If true is returned then the foreach loop will try to get another value from Current. 
        /// If false is returned then the foreach loop terminates.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (this.reader == null)
                this.Reset();

            if (this.FindNextTextElement())
                return true;

            this.reader.Close();
            return false;
        }

        /// <summary>
        /// Invoked every time MoveNext() returns true. 
        /// This extracts the values for the "current" customer from the XML file and returns that
        /// data packaged up as a Customer object.
        /// </summary>
        /// <returns></returns>
        public object Current
        {
            get
            {
                string serverName = this.reader.Value;
                string val = "";
                if (this.FindNextTextElement())
                    val = this.reader.Value;

                string serverHostString = val;
                IPAddress tmp_addr;
                if (!IPAddress.TryParse(serverHostString, out tmp_addr))
                {
                    tmp_addr = Dns.GetHostAddresses(serverHostString)[0];
                }

                val = "0";
                if (this.FindNextTextElement())
                    val = this.reader.Value;
                int serverPort;

                try { serverPort = Int32.Parse(val); }
                catch { serverPort = 5003; }

                return new ServerInfo(serverName, tmp_addr, serverPort);
            }
        }

        /// <summary>
        /// Advances the XmlTextReader to the next Text element in the XML stream.
        /// Returns true if there is more data to be read from the stream, else false.
        /// </summary>
        private bool FindNextTextElement()
        {
            bool readOn = this.reader.Read();
            bool prevTagWasElement = false;
            while (readOn && this.reader.NodeType != XmlNodeType.Text)
            {
                if (prevTagWasElement && this.reader.NodeType == XmlNodeType.EndElement)
                    readOn = false;
                prevTagWasElement = this.reader.NodeType == XmlNodeType.Element;
                readOn = readOn && this.reader.Read();
            }
            return readOn;
        }
    }
}
