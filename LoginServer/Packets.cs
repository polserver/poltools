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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LoginServer
{
    class Packets
    {
        static public string GetString(byte[] byte_str)
        {
            Encoding enc = Encoding.ASCII;
            return enc.GetString(byte_str);
        }

        static public byte[] GetBytes(string str, int size)
        {
            Encoding enc = Encoding.ASCII;

            int bsize = str.Length;
            if (bsize > size)
                bsize = size;

            byte[] byt_string = enc.GetBytes(str.ToCharArray(), 0, bsize);
            
            MemoryStream tmp = new MemoryStream(size);
            tmp.SetLength(size);

            tmp.Write(byt_string, 0, byt_string.Length);

            return tmp.ToArray();
        }

        static public int ReadBE16(NetworkStream ns)
        {
            return (ns.ReadByte() << 8) | ns.ReadByte();
        }

        static public int ReadInt32(NetworkStream ns)
        {
            return (ns.ReadByte() << 24) 
                | (ns.ReadByte() << 16)
                | (ns.ReadByte() << 8)
                | (ns.ReadByte());
        }

        static public void WriteBE16(MemoryStream ms, short n)
        {
            ms.WriteByte((byte)(n >> 8));
            ms.WriteByte((byte)(n & 0x00FF));
        }

        static public void WriteBE16(MemoryStream ms, ushort n)
        {
            ms.WriteByte((byte)(n >> 8));
            ms.WriteByte((byte)(n & 0x00FF));
        }

        private static void WriteBE32(MemoryStream ms, int p)
        {
            ms.WriteByte((byte)(p >> 0x18));
            ms.WriteByte((byte)(p >> 0x10));
            ms.WriteByte((byte)(p >> 0x08));
            ms.WriteByte((byte)p);
        }

        private static void WriteBE32(MemoryStream ms, uint p)
        {
            ms.WriteByte((byte)(p >> 0x18));
            ms.WriteByte((byte)(p >> 0x10));
            ms.WriteByte((byte)(p >> 0x08));
            ms.WriteByte((byte)p);
        }

        static public void WriteBE(MemoryStream ms, IPAddress ip)
        {
            byte[] address = ip.GetAddressBytes();
            ms.WriteByte(address[3]);
            ms.WriteByte(address[2]);
            ms.WriteByte(address[1]);
            ms.WriteByte(address[0]);
        }

        static public void WriteBEflipped(MemoryStream ms, IPAddress ip)
        {
            byte[] address = ip.GetAddressBytes();
            ms.WriteByte(address[0]);
            ms.WriteByte(address[1]);
            ms.WriteByte(address[2]);
            ms.WriteByte(address[3]);
        }

        static public void SeedPacket(ref NetworkStream ns, ref Client.Version ver)
        {
            int seed = ReadInt32(ns);
            ver.Major = ReadInt32(ns);
            ver.Minor = ReadInt32(ns);
            ver.Revision = ReadInt32(ns);
            ver.Patch = ReadInt32(ns);
            Console.WriteLine("Seed: 0x{0:X}", seed);
            Console.WriteLine("Client Version {0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Revision, ver.Patch);
        }

        static public void LoginPacket(ref NetworkStream ns, Client client)
        {
            string username, password;
            {
                byte[] byte_username = new byte[30];
                byte[] byte_password = new byte[30];

                ns.Read(byte_username, 0, 30);
                ns.Read(byte_password, 0, 30);
                ns.ReadByte(); // next-login-key;

                username = Packets.GetString(byte_username);
                password = Packets.GetString(byte_password);
            }
            if (Auth.CanLogin(username, password))
            {
                Console.WriteLine("Client connected: {0}", username);
                SendServerList(ref ns,client);
            }
            else
            {
                SendLoginDenied(ref ns, (byte)00);
            }
        }

        public static void ServerSelectPacket(ref NetworkStream ns, Client client)
        {
            int index = ReadBE16(ns);
            if (client.CompareVersionInSARange()) //UO:SA Beta hack
                index = 0;
            if (index >= Program.server_list.ServerCount)
                return;
            SendConnectGameServer(ref ns, index, client); 
        }

        public static void SendConnectGameServer(ref NetworkStream ns, int index, Client client)
        {
            MemoryStream ms = new MemoryStream(11);
            ServerInfo server = Program.server_list.Servers[index];

            ms.WriteByte(0x8C);
            WriteBEflipped(ms, server.ip);
            WriteBE16(ms, (short)server.port);
            //Pol like seed
            ms.WriteByte(0xFE);
            ms.WriteByte(0xFE);
            if (client.CompareVersion(Client.VER60142))
                ms.WriteByte(0xFD);
            else if (client.CompareVersionInSARange()) //UO:SA Beta hack (for 0xb9 packet)
                ms.WriteByte(0xFD);
            else
                ms.WriteByte(0xFE);
            //if (client.isUOKR)
            //    ms.WriteByte(0xFC);
            //else
            if (client.CompareVersion(Client.VER6017))
                ms.WriteByte(0xFD);
            else
                ms.WriteByte(0xFE);

            byte[] tmp = ms.ToArray();
            ns.Write(tmp, 0, tmp.Length);
        }


        public static void SendLoginDenied(ref NetworkStream ns, byte reason)
        {
            byte[] tmp = { 0x82, reason };
            ns.Write(tmp, 0, 2);
        }

        private static void SendServerList(ref NetworkStream ns, Client client)
        {
            ServerList sl = Program.server_list;
            MemoryStream ms = new MemoryStream(6 + 40 * sl.ServerCount);

            short server_count = (short)sl.ServerCount;
            
            ms.WriteByte(0xA8); // Game Server List CMD
            WriteBE16(ms, (short)(6 + 40*server_count)); // MsgLen
            ms.WriteByte(0x00); // System Info Flag (0x64: Spy on client, 0xCC: no spy)
            WriteBE16(ms, server_count); // # of Servers

            short index = 0;
            foreach (ServerInfo server in sl.Servers)
            {
                if (client.CompareVersionInSARange()) //UO:SA Beta hack
                    WriteBE16(ms, 0x3D);
                else
                    WriteBE16(ms, index);
                index++;
                byte[] name = GetBytes(server.Name, 32);
                ms.Write(name, 0, 32); // 32 bytes
                ms.WriteByte(0); // percent full
                ms.WriteByte(0); // timezone
                WriteBE(ms, server.ip);
            }

            byte[] tmp = ms.ToArray();
            ns.Write(tmp, 0, tmp.Length);
        }
    }
}
