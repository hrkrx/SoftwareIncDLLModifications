using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;

namespace MultiplayerServer
{
    public class ConnectionController
    {
        public delegate void NewConnectionEvent();
        public NewConnectionEvent newConnectionEventHandler;
        public ConcurrentQueue<Command> commands;
        public ConcurrentDictionary<string, SoftwareIncClient> clients;

        public ConnectionController(NewConnectionEvent eventHandler)
        {
            newConnectionEventHandler = eventHandler;
            commands = new ConcurrentQueue<Command>();
            clients = new ConcurrentDictionary<string, SoftwareIncClient>();
        }

        public void StartListening()
        {
            IPAddress ip = IPAddress.Any;
            IPEndPoint endpoint = new IPEndPoint(ip, 9999);
            TcpListener listener = new TcpListener(endpoint);
            
            while(true)
            {
                var newClient = listener.AcceptTcpClient();
                
                ThreadPool.QueueUserWorkItem(new WaitCallback(ConnectionHandler), newClient);
            }

        }

        public void ConnectionHandler(object tcp)
        {
            var client = (TcpClient)tcp;
            SoftwareIncClient s = null;
            bool closing = false;
            while (!closing)
            {
                if (client.Connected)
                {
                    Packet p = new Packet(client);
                    Command cmd = commandFromPacket(p);
                    switch (cmd.commandType)
                    {
                        case CommandType.Login:
                            s = new SoftwareIncClient(client, cmd.source);
                            SoftwareIncClient.generateId(s);
                            clients.TryAdd(s.id, s);
                            break;
                        case CommandType.Logout:
                            clients.TryRemove(s.id, out s);
                            client.Close();
                            closing = true;
                            break;
                        case CommandType.Hack:
                            break;
                        case CommandType.Blame:
                            break;
                        case CommandType.Update:
                            break;
                        default:
                            break;
                    }
                }
            }

        }
        
        public Command commandFromPacket(Packet p)
        {
            Command res;
            XmlSerializer ser = new XmlSerializer(typeof(Command));
            res = (Command)ser.Deserialize(new MemoryStream(p.data));
            return res;
        }

        public Packet packetFromCommand(Command c)
        {
            Packet res;
            XmlSerializer ser = new XmlSerializer(typeof(Command));
            MemoryStream ms = new MemoryStream();
            ser.Serialize(ms, c);
            res = new Packet(ms.ToArray());
            return res;
        }
    }
}
