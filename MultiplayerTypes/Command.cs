using System;

namespace MultiplayerTypes
{
    [Serializable]
    public class Command
    {
        public CommandType commandType;
        public string target;
        public string source;
        public string message;
        public string serializedCompany;
        public float money;
        public int year;
    }

    [Serializable]
    public enum CommandType
    {
        Login,
        Logout,
        Hack,
        Blame,
        Update
    }

    [Serializable]
    public enum HackType
    {
        Computer,
        Server,
        Progress,
        Capital
    }
}
