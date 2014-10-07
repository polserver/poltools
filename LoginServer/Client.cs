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
using System.Net.Sockets;
using System.Threading;

namespace LoginServer
{
    class Client
    {
        private Thread _client_thread;
        private TcpClient client_sock;
        private bool _disconnected;
        private Version client_version = new Version() { Major = 0, Minor = 0, Revision = 0, Patch = 0 };

        public static readonly Version VER6017 = new Version() { Major = 6, Minor = 0, Revision = 1, Patch = 7 };
        public static readonly Version VER60142 = new Version() { Major = 6, Minor = 0, Revision = 14, Patch = 2 };

        public static readonly Version VER601324 = new Version() { Major = 6, Minor = 0, Revision = 13, Patch = 24 };
        public static readonly Version VER60141 = new Version() { Major = 6, Minor = 0, Revision = 14, Patch = 1 };

        public Version ClientVersion
        {
            get
            {
                return client_version;
            }
            set
            {
                client_version = value;
            }
        }

        public struct Version
        {
            public int Major;
            public int Minor;
            public int Revision;
            public int Patch;
        }

        public bool Disconnected
        {
            get { return _disconnected; }
        }

        public Client(TcpClient sock)
        {
            client_sock = sock;
            _disconnected = false;
            _client_thread = new Thread(new ThreadStart(HandleClient));
            _client_thread.IsBackground = true;
            _client_thread.Start();
        }

        void HandleClient()
        {
            NetworkStream ns = client_sock.GetStream();
            int c = -1;

            // Seed Start
            c = ns.ReadByte();
            if (c == 0xEF)
            {
                Packets.SeedPacket(ref ns, ref client_version);
            }
            else
            {
                byte[] seed = new byte[3];
                ns.Read(seed, 0, 3);
            }
            // Seed End
            
            while ((c = ns.ReadByte()) >= 0)
            {
                switch (c)
                {
                    case 0x80:
                        Console.WriteLine("Login Packet!");
                        Packets.LoginPacket(ref ns, this);
                        break;
                    
                    case 0xA0:
                        Console.WriteLine("Server Select!");
                        Packets.ServerSelectPacket(ref ns, this);
                        break;

                    default:
                        byte[] tmp = new byte[1024];
                        int len = ns.Read(tmp, 0, 1024);
                        HexDump.DumpUnk((byte)c, tmp, len, SockUtils.GetIP(client_sock));
                        break;
                }
            }
            Console.WriteLine("Client Disconnected!");
            _disconnected = true;
        }

        public bool CompareVersion(Client.Version compver)
        {
            if (client_version.Major > compver.Major)
                return true;
            else if (client_version.Major < compver.Major)
                return false;
            else if (client_version.Minor > compver.Minor)
                return true;
            else if (client_version.Minor < compver.Minor)
                return false;
            else if (client_version.Revision > compver.Revision)
                return true;
            else if (client_version.Revision < compver.Revision)
                return false;
            else if (client_version.Patch > compver.Patch)
                return true;
            else if (client_version.Patch < compver.Patch)
                return false;

            return true;
        }

        public bool CompareVersionEqual(Client.Version compver)
        {
            if ((client_version.Major == compver.Major)
                && (client_version.Minor == compver.Minor)
                && (client_version.Revision == compver.Revision)
                && (client_version.Patch == compver.Patch))
                return true;

            return false;
        }

        public bool CompareVersionInSARange()
        {
            if ((CompareVersion(VER601324)) && (!CompareVersion(VER60141)))
                return true;
            return false;
        }
    }
}
