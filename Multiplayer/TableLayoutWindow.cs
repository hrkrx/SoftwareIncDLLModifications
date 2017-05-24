using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiplayerTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Multiplayer
{
    public class TableLayoutWindow
    {
        GUIWindow mainWindow = WindowManager.SpawnWindow();
        List<UnityEngine.UI.Text> playerTextList = new List<UnityEngine.UI.Text>();
        List<Player> playerList = new List<Player>();
        public TableLayoutWindow()
        {
            mainWindow.InitialTitle = "Multiplayer";
            mainWindow.Title = "Multiplayer";
            mainWindow.TitleText.text = "Multiplayer";
            mainWindow.StartHidden = true;
            mainWindow.OnlyHide = true;
        }

        private int ComparePlayer(object x, object y)
        {
            Player px = (Player)x;
            Player py = (Player)y;
            return px.lvl.CompareTo(py);
        }

        private string UpdateContent(object arg)
        {
            Player p = (Player)arg;
            return p.companyName + " | " + p.lvl;
        }

        public void Show()
        {
            mainWindow.Show();
        }

        public void Close()
        {
            mainWindow.Close();
            
        }
        

        internal void Update(Command cmd)
        {
            foreach (var item in playerTextList)
            {
                UnityEngine.Object.DestroyImmediate(item);
            }
            playerTextList.Clear();

            Player p = new Player();
            BinaryFormatter bf = new BinaryFormatter();
            p.companyName = cmd.source;
            p.lvl = cmd.year * (cmd.money / 100000);

            foreach (var item in playerList)
            {
                UnityEngine.UI.Text label = WindowManager.SpawnLabel();
                label.text = item.companyName + " | " + item.lvl;
                playerTextList.Add(label);
            }
            for (int i = 0; i < playerList.Count; i++)
            {
                WindowManager.AddElementToWindow(playerTextList[i].gameObject, mainWindow, new UnityEngine.Rect(0, 32f * i, 250f, 32f), new UnityEngine.Rect(0, 0, 0, 0));
            }

            throw new NotImplementedException();
        }
    }
}
