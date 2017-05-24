using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace MultiplayerServer
{
    public class EfficientLogger
    {
        static UdpClient udp = null;
        static IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, 16000);

        private static void ConnectLogger()
        {
            udp = new UdpClient();
            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }
        public static void Log(string message)
        {
            if (udp == null || !udp.Client.Connected) ConnectLogger();
            message = "[" + DateTime.Now.ToShortTimeString() + "][LOG]" + message;
            byte[] data = getData(message);
            udp.Send(data, data.Length, groupEP);
        }

        public static void Debug(string message)
        {
            if (udp == null || !udp.Client.Connected) ConnectLogger();
            message = "[" + DateTime.Now.ToShortTimeString() + "][DEBUG]" + message;
            byte[] data = getData(message);
            udp.Send(data, data.Length, groupEP);
        }

        public static void Error(string message)
        {
            if (udp == null || !udp.Client.Connected) ConnectLogger();
            message = "[" + DateTime.Now.ToShortTimeString() + "][ERROR]" + message;
            byte[] data = getData(message);
            udp.Send(data, data.Length, groupEP);
        }

        private static byte[] getData(string message)
        {
            byte[] msgData = Encoding.Unicode.GetBytes(message);
            return msgData;
        }

        public static void writeToConsole()
        {
            Console.WriteLine("Begin Logging");
            ThreadPool.QueueUserWorkItem(new WaitCallback(receive), null);
        }

        private static void receive(object o)
        {
            UdpClient receiver = new UdpClient();
            receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            IPEndPoint recieverEP = new IPEndPoint(IPAddress.Any, 16000);
            receiver.Client.Bind(recieverEP);
            Console.WriteLine("ThreadLog");
            while (true)
            {
                try
                {
                    byte[] data = receiver.Receive(ref recieverEP);
                    Console.WriteLine(Encoding.Unicode.GetChars(data));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
               
            }
        }
    }
}
