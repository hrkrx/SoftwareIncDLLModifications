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
using System.Timers;

namespace Multiplayer
{
    public class MultiplayerBehaviour : ModBehaviour
    {
        #region Member

        public bool ModActive = false;
        public bool loggedin = false;
        int updatefrequency = 5000;
        Timer t = new Timer();
        List<Player> Player = new List<Player>();
        int placementPosition = 20;
        TableLayoutWindow modWindow;
        #endregion

        #region Unity Calls

        ~MultiplayerBehaviour()
        {
            Logout();
        }

        void Start()
        {
            //All ModBehaviours has a function to load settings from the mod's settings file
            //Note that everything is saved in strings
            //This function uses the default string converter for the generic type argument
            SetStrings();
            t.Elapsed += T_Elapsed;

            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                
            }
        }

        void Update()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                
            }
        }

        public override void OnActivate()
        {
            ModActive = true;
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                HUD.Instance.AddPopupMessage("Multiplayer V1 has been activated!", "Cogs", "", 0, 1);
                StartUpdateListener();
            }
        }

        public override void OnDeactivate()
        {
            ModActive = false;
            if (!ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                t.Stop();
                HUD.Instance.AddPopupMessage("Multiplayer V1 has been deactivated!", "Cogs", "", 0, 1);
                Logout();
            }
        }

        internal void CreateMainWindow()
        {
            if (modWindow != null)
            {
                modWindow.Close();
            }
            else
            {
                modWindow = new TableLayoutWindow();
            }
            modWindow.Show();
        }
        #endregion

        #region Recurring Events
        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                UpdateCompany();
            }
        }

        private void StartUpdatingUI()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                
            }
        }

        #endregion

        #region Network
        internal bool StartConnect(string result)
        {
            bool dialogResult = false;
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null && !loggedin)
            {
                Connect(result);
                Login();
                t.Interval = updatefrequency;
                t.Start();
            }
            return dialogResult;
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

        internal void StartUpdateListener()
        {
            while (ModActive)
            {
                Packet p = new Packet((TcpClient)MultiplayerGlobalCache.MPCache["connection"]);
                
                Command cmd;
                BinaryFormatter bf = new BinaryFormatter();
                cmd = (Command)bf.Deserialize(new MemoryStream(p.data));
                switch (cmd.commandType)
                {
                    case CommandType.Login:
                        break;
                    case CommandType.Logout:
                        break;
                    case CommandType.Hack:
                        break;
                    case CommandType.Blame:
                        break;
                    case CommandType.Update:
                        modWindow.Update(cmd);
                        break;
                    default:
                        break;
                }
            }
        }
        
        #endregion

        #region MultiplayerInteraction
        internal void Login()
        {
            Command c = new Command();
            c.commandType = CommandType.Login;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, GameSettings.Instance.MyCompany);
            c.serializedCompany = Convert.ToBase64String(ms.ToArray());
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
            loggedin = false;
        }

        internal void UpdateCompany()
        {
            Command c = new Command();
            c.commandType = CommandType.Update;
            c.money = GameSettings.Instance.MyCompany.Money;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, GameSettings.Instance.MyCompany);
            c.serializedCompany = Convert.ToBase64String(ms.ToArray());
            c.source = GameSettings.Instance.MyCompany.Name;
            ms = new MemoryStream();
            bf.Serialize(ms, c);
            Packet p = new Packet(ms.ToArray());
            p.send((TcpClient)MultiplayerGlobalCache.MPCache["connection"]);

        }

        public Company GetCompanyFromCommand(Command cmd)
        {
            var bf = new BinaryFormatter();
            var ms = new MemoryStream(Convert.FromBase64String(cmd.serializedCompany));
            var company = (Company)bf.Deserialize(ms);
            return company;
        }
        #endregion

        #region UI

        public void AddButtonToUI(UnityEngine.UI.Button btn)
        {

            WindowManager.AddElementToElement(btn.gameObject, HUD.Instance.gameObject, new UnityEngine.Rect(0, placementPosition, 100, 20), new UnityEngine.Rect(0, 0, 0, 0));
            placementPosition += int.Parse(btn.gameObject.GetComponent<Renderer>().bounds.size.y.ToString()) + 2;
        }

        public void SpawnWindowOnHUD(GUIWindow wnd)
        {
            WindowManager.AddElementToElement(wnd.gameObject, HUD.Instance.gameObject, new Rect(0, 40, 300, 150), new Rect(0, 0, 0, 0));
        }

        public GUIWindow CreateWindow(string label, List<GameObject> content)
        {
            var wnd = WindowManager.SpawnWindow();
            wnd.Title = "";
            wnd.StartHidden = true;
            wnd.OnlyHide = true;

            var serverLabel = WindowManager.SpawnLabel();
            serverLabel.text = label;
            WindowManager.AddElementToElement(serverLabel.gameObject, wnd.gameObject, new Rect(10, 80, 280, 500), new Rect(0, 0, 0, 0));

            return wnd;
        }

        private void SetString(string key, string translatedKey)
        {
            Localization.Translation translation = Localization.Translations[Localization.CurrentTranslation];
            if (!translation.UI.ContainsKey(key))
            {
                translation.UI.Add(key, translatedKey);
            }
            else
            {
                translation.UI[key] = translatedKey;
            }
        }

        public void SetStrings()
        {
            SetString("Multiplayer", "Multiplayer");
        }

        #endregion
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
