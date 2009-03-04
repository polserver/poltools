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
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace LoginServer
{
    class Client
    {
        private Thread _client_thread;
        private TcpClient client_sock;

        public Client(TcpClient sock)
        {
            client_sock = sock;
            _client_thread = new Thread(new ThreadStart(HandleClient));
            _client_thread.IsBackground = true;
            _client_thread.Start();
        }

        void HandleClient()
        {
            NetworkStream ns = client_sock.GetStream();
            int c = -1;

            byte[] seed = new byte[4];
            ns.Read(seed, 0, 4);

            while ((c = ns.ReadByte()) >= 0)
            {
                switch (c)
                {
                    case 0x80:
                        Console.WriteLine("Login Packet!");
                        Packets.LoginPacket(ref ns);
                        break;
                    
                    case 0xA0:
                        Console.WriteLine("Server Select!");
                        Packets.ServerSelectPacket(ref ns);
                        break;

                    default:
                        byte[] tmp = new byte[1024];
                        int len = ns.Read(tmp, 0, 1024);
                        HexDump.DumpUnk((byte)c, tmp, len, SockUtils.GetIP(client_sock));
                        break;
                }
            }
        }
    }
}
