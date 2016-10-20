using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using MultiplayerTypes;

namespace Multiplayer
{
    public class MultiplayerBehaviour : ModBehaviour
    {
        public string ip = "79.143.188.80";  
        public bool ModActive = false;
        public bool loggedin = false;
        int updatefrequency = 10;
        int updatecounter = 0;


        ~MultiplayerBehaviour()
        {
            Logout();
        }

        void Start()
        {
            //All ModBehaviours has a function to load settings from the mod's settings file
            //Note that everything is saved in strings
            //This function uses the default string converter for the generic type argument
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {

            }
        }

        void Update()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                if (updatecounter == updatefrequency)
                {
                    updatecounter = 0;
                    Update();
                } else
                {
                    updatecounter++;
                }
            }
        }

        public override void OnActivate()
        {
            ModActive = true;
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                HUD.Instance.AddPopupMessage("Multiplayer V1 has been activated!", "Cogs", "", 0, 1);
                Connect(ip);
                Login();
            }
        }

        public override void OnDeactivate()
        {
            ModActive = false;
            if (!ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                HUD.Instance.AddPopupMessage("Multiplayer V1 has been deactivated!", "Cogs", "", 0, 1);
                Logout();
            }
        }

        internal void Connect(string text)
        {
            IPAddress ip = IPAddress.Parse(text);
            IPEndPoint endPoint = new IPEndPoint(ip, 9999);                                                                                                                       
            TcpClient connection = new TcpClient();
            connection.Connect(endPoint);
            MultiplayerGlobalCache.MPCache.Add("connection", connection);
            HUD.Instance.AddPopupMessage("Connected successfully to multiplayerserver!", "Cogs", "", 0, 1);
        }

        internal void Login()
        {
            Command c = new Command();
            c.commandType = CommandType.Login;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, c);
            StreamReader sr = new StreamReader(ms);
            c.serializedCompany = sr.ReadToEnd();
            c.source = GameSettings.Instance.MyCompany.Name;
            ms = new MemoryStream();
            bf.Serialize(ms, c);
            Packet p = new Packet(ms.ToArray());
            p.send((TcpClient)MultiplayerGlobalCache.MPCache["connection"]);
            loggedin = true;
        }

        internal void Logout()
        {
            Command c = new Command();
            c.commandType = CommandType.Logout;
            c.source = GameSettings.Instance.MyCompany.Name;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, c);
            Packet p = new Packet(ms.ToArray());
            p.send((TcpClient)MultiplayerGlobalCache.MPCache["connection"]);
            ((TcpClient)MultiplayerGlobalCache.MPCache["connection"]).Close();
            MultiplayerGlobalCache.MPCache.Remove("connection");
        }

        internal void UpdateCompany()
        {
            Command c = new Command();
            c.commandType = CommandType.Update;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, c);
            StreamReader sr = new StreamReader(ms);
            c.serializedCompany = sr.ReadToEnd();
            c.source = GameSettings.Instance.MyCompany.Name;
            ms = new MemoryStream();
            bf.Serialize(ms, c);
            Packet p = new Packet(ms.ToArray());
            p.send((TcpClient)MultiplayerGlobalCache.MPCache["connection"]);
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Sets a _private_ Property Value from a given Object. Uses Reflection.
        /// Throws a ArgumentOutOfRangeException if the Property is not found.
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <param name="obj">Object from where the Property Value is set</param>
        /// <param name="propName">Propertyname as string.</param>
        /// <param name="val">Value to set.</param>
        /// <returns>PropertyValue</returns>
        public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
        {
            Type t = obj.GetType();
            t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
        }
    }
}
