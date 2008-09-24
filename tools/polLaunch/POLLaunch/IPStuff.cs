using System.Net;
using System.IO;

namespace POLLaunch
{
    static class IPAddressClass
    {
        static public IPAddress[] GetDetectedIPs()
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            return addr;
        }
        
        static public string GetWanIP()
        {
            WebRequest request = WebRequest.Create("http://www.polserver.com/getip.php");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream data_stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(data_stream);
            string server_response = reader.ReadToEnd();
            reader.Close();
            data_stream.Close();
            response.Close();

            return server_response;
        }
    }
}