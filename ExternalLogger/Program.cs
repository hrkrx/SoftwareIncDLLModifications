using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            EfficientLogger.writeToConsole();
            while (true) ;
        }
    }
}
