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
    class Program
    {
        static public bool running = true;
        static public ServerList server_list = new ServerList();
        
        static void Main(string[] args)
        {
            int port = 5003;

            Console.WriteLine("LoginServer v0.001 -- POL Team");
            Console.WriteLine("==============================");

            try
            {
                Console.WriteLine("Populating server list...");
                Console.WriteLine();

                PopulateList();
                server_list.ReportServers();              

                Console.WriteLine("Starting listener on port {0}.", port);
                new Listener(port);

                Console.WriteLine("Login Server loaded.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }

            Console.ReadLine();
        }

        private static void PopulateList()
        {
            server_list.AddServer("POL Distro", "poldistro.dyndns.org", 5003);
            server_list.AddServer("POL Distro: OMG", "127.0.0.1", 5003);
        }
        ~Program()
        {
            running = false;
        }
    }
}
