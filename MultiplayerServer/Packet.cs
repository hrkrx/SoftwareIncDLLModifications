using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerServer
{
    public class Packet
    {
        public byte[] data;

        public Packet (byte[] d)
        {
            data = d;
        }
        
        public Packet (TcpClient client)
        {
            long s = size(client);
            byte[] d = new byte[s];
            client.GetStream().Read(d, 0, d.Length);
            data = d;
        }

        public void send(TcpClient client)
        {
            byte[] s = size(data);
            client.GetStream().Write(s, 0, s.Length);
            client.GetStream().Write(data, 0, data.Length);
        }

        private long size(TcpClient c)
        {
            byte[] sizearray = new byte[4];
            c.GetStream().Read(sizearray, 0, 4);
            return sizearray[0] + (255) * sizearray[1] + (255 * 255) * sizearray[2] + (255 * 255 * 255) * sizearray[3];
        }

        private byte[] size(byte[] b)
        {
            byte[] res = new byte[4];
            res[3] = (byte)(b.Length / (255 * 255 * 255));
            res[2] = (byte)(b.Length / (255 * 255) - (res[3] * 255 * 255 * 255));
            res[1] = (byte)(b.Length / (255) - (res[2] * 255 * 255) - (res[3] * 255 * 255 * 255));
            res[0] = (byte)(b.Length - (res[1] * 255) - (res[2] * 255 * 255) - (res[3] * 255 * 255 * 255));
            return res;
        }

    }
}
