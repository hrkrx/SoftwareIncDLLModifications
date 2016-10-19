using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerServer
{
    public class SoftwareIncClient
    {
        TcpClient client;
        string name;
        public string id;

        public SoftwareIncClient (TcpClient c, string name)
        {
            client = c;
            this.name = name;
        }

        public static void generateId(SoftwareIncClient s)
        {
            IPEndPoint remoteIpEndPoint = s.client.Client.RemoteEndPoint as IPEndPoint;
            if (remoteIpEndPoint != null)
            {
                s.id = "";
                Random r = new Random(remoteIpEndPoint.Address.GetHashCode());
                for (int i = 0; i < 20; i++)
                {
                    s.id += (char)r.Next(48, 90);
                }
            } else
            {
                throw new Exception("Invalid IP-Address for ID-generation");
            }
        }
    }
}
