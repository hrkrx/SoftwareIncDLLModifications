using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MultiplayerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionController.NewConnectionEvent e = null;
            ConnectionController c = new ConnectionController(e);
            c.StartListening();

        }
    }
}
