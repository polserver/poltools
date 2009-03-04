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
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace LoginServer
{
    class Listener
    {
        private TcpListener _listen;
        private Thread _listen_thread;
        private int port = 0;
        private List<Client> _clients = new List<Client>();

        const int backlog = 10;

        public Listener(int port)
        {
            this.port = port;
            _listen = new TcpListener(IPAddress.Any, port);
            start();
        }
        private void start()
        {
            _listen.Start(backlog);

            _listen_thread = new Thread(new ParameterizedThreadStart(ListenForClients));
            _listen_thread.IsBackground = true;
            _listen_thread.Start(_listen);

        }
        private void ListenForClients(object listener)
        {
            TcpListener listen = listener as TcpListener;
            if (listen == null)
                return;

            while (Program.running)
            {
                TcpClient client = listen.AcceptTcpClient();
                Console.WriteLine("New client connected: {0}", SockUtils.GetIP(client));
                _clients.Add(new Client(client));
            }

            listen.Stop();
        }
    }
}
