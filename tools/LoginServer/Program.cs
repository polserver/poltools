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
using MySql.Data.MySqlClient;

namespace LoginServer
{
    class Program
    {

        static string progversion = "0.001";
        static public string ServerPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        static public bool running = true;
        static public ServerList server_list = new ServerList();
        static public Options options = new Options();

        static void Main(string[] args)
        {
            Console.WriteLine("POL Remote Login Server v" + progversion + " (VS.NET 2008)");
            Console.WriteLine("Copyright (C) 2009 POL Development Team");
            Console.WriteLine();

            try
            {
                if (!options.Load())
                {
                    Console.WriteLine("Unable to load options file! Exiting...");
                    Console.WriteLine();
                    Environment.Exit(1000);
                }
                Console.WriteLine("Options File Loaded...");

                LoginSQL MyConnection = new LoginSQL();
                
                if (!MyConnection.CreateConnection())
                {
                    Console.WriteLine("Unable to connect to the database. Exiting...");
                    Console.WriteLine();
                    Environment.Exit(1001);
                }
                Console.WriteLine("Connected to MySQL Database");
                // MyConnection.LoadAccounts();
                MyConnection.Close();
                
    
                int port = options.LoginServer_Port;

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

        /// <summary>
        /// Program Class Deconstructor
        /// </summary>
        /// <returns></returns>
        ~Program()
        {
            running = false;
        }

        /// <summary>
        /// Builds internal server list from serverlist.xml
        /// </summary>
        /// <returns></returns>
        private static void PopulateList()
        {
            Servers servers = new Servers();
            foreach (ServerInfo newServer in servers)
            {
                server_list.AddServer(newServer);
            }
        }
    }
}
