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
using System.Net.Sockets;

namespace LoginServer
{
    static class SockUtils
    {
        static public string HexDump(byte c)
        {
            return String.Format("{0:X2} {1}", c, (c > 20 && c < 127) ? (char)c : '.');
        }
        static public string GetIP(TcpClient client)
        {
            return GetIP((IPEndPoint)client.Client.RemoteEndPoint);
        }
        static public string GetIP (IPEndPoint addr) {
            return addr.Address.ToString();
        }
    }
}
