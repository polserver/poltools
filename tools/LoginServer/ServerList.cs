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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

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
}
