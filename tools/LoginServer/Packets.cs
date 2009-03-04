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
using System.IO;

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

        static public void WriteBE16(MemoryStream ms, short n)
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

        static public void WriteBE(MemoryStream ms, IPAddress ip)
        {
            ms.Write(ip.GetAddressBytes(), 0, 4);
        }

        static public void LoginPacket(ref NetworkStream ns)
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
                SendServerList(ref ns);
            }
            else
            {
                SendLoginDenied(ref ns, (byte)00);
            }
        }

        public static void ServerSelectPacket(ref NetworkStream ns)
        {
            int index = ReadBE16(ns);
            if (index >= Program.server_list.ServerCount)
                return;
            SendConnectGameServer(ref ns, index); 
        }

        public static void SendConnectGameServer(ref NetworkStream ns, int index)
        {
            MemoryStream ms = new MemoryStream(11);
            ServerInfo server = Program.server_list.Servers[index];

            ms.WriteByte(0x8C);
            WriteBE(ms, server.ip);
            WriteBE16(ms, (short)server.port);
            WriteBE32(ms, 0);

            byte[] tmp = ms.ToArray();
            ns.Write(tmp, 0, tmp.Length);
            Console.WriteLine(HexDump.Dump(tmp));
        }


        public static void SendLoginDenied(ref NetworkStream ns, byte reason)
        {
            byte[] tmp = { 0x82, reason };
            ns.Write(tmp, 0, 2);
        }

        private static void SendServerList(ref NetworkStream ns)
        {
            ServerList sl = Program.server_list;
            MemoryStream ms = new MemoryStream(6 + 40 * sl.ServerCount);

            short server_count = (short)sl.ServerCount;
            
            ms.WriteByte(0xA8); // Game Server List CMD
            WriteBE16(ms, (short)(6 + 40*server_count)); // MsgLen
            ms.WriteByte(0xCC); // System Info Flag (0x64: Spy on client, 0xCC: no spy)
            WriteBE16(ms, server_count); // # of Servers

            short index = 0;
            foreach (ServerInfo server in sl.Servers)
            {
                WriteBE16(ms, index++);
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
