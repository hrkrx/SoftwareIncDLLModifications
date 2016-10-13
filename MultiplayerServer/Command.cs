using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerServer
{
    [Serializable]
    public class Command
    {
        public CommandType commandType;
        public string target;
        public string source;
        public string message;
        public string serializedCompany;

    }

    public enum CommandType
    {
        Login,
        Logout,
        Hack,
        Blame,
        Update
    }

    public enum HackType
    {
        Computer,
        Server,
        Progress,
        Capital
    }
}
