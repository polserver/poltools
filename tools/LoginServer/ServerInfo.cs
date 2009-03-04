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
    class ServerInfo
    {
        string servername;
        double percent_full;
        TimeZoneInfo timezone;
        IPEndPoint server_endpoint;

        public string Name
        {
            get
            {
                return servername;
            }
        }
        public double Full
        {
            get
            {
                return percent_full;
            }
        }
        public TimeZoneInfo tz
        {
            get
            {
                return timezone;
            }
        }
        public IPAddress ip
        {
            get
            {
                return server_endpoint.Address;
            }
        }
        public int port
        {
            get
            {
                return server_endpoint.Port;
            }
        }

        public ServerInfo(string servername, IPAddress ip, int port) : this(servername, new IPEndPoint(ip, port)) 
        { }
        ServerInfo(string servername, IPEndPoint server_endpoint) : this(servername, server_endpoint, 0, TimeZoneInfo.Local) 
        { }
        
        ServerInfo(string servername, IPEndPoint server_endpoint, double percent_full, TimeZoneInfo timezone)
        {
            this.servername = servername;
            this.server_endpoint = server_endpoint;
            this.percent_full = percent_full;
            this.timezone = timezone;
        }
    }
}
