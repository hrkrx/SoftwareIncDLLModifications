using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MultiplayerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("Starting SoftwareInc Multiplayerserver " + version.ToString());
            EfficientLogger.writeToConsole();
            
            ConnectionController.NewConnectionEvent e = null;
            ConnectionController c = new ConnectionController(e);
            c.StartListening();

        }
    }
}
