using UnityEngine;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Multiplayer
{
    //Your mod should have exactly one class that implements the ModMeta interface
    public class Main : ModMeta
    {
        string ip = "192.168.2.113";
        //This function is used to generate the content in the "Mods" section of the options window
        //The behaviors array contains all behaviours that have been spawned for this mod, one for each implementation
        public void ConstructOptionsScreen(RectTransform parent, ModBehaviour[] behaviours)
        {
            //We need a reference to a behavior to read and write from the mod settings file
            var behavior = behaviours.OfType<MultiplayerBehaviour>().First();
            List<GameObject> objs = new List<GameObject>();
            List<GameObject> windowsObjs = new List<GameObject>();
            var gameWindow = HUD.Instance;
            TableLayoutWindow ModWindow = null;
            var screenSizeX = Screen.width;
            var screenSizeY = Screen.height;

            //Start by spawning a label
            var label = WindowManager.SpawnLabel();
            label.text = "This Mod was created by LtPain";
            WindowManager.AddElementToElement(label.gameObject, parent.gameObject, new Rect(0, 0, 250, 32),
                new Rect(0, 0, 0, 0));

            #region ModControls
            Button mainButton = WindowManager.SpawnButton();
            mainButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Multiplayer";
            mainButton.onClick.AddListener(() =>
            {
                if (!behavior.loggedin)
                {
                    WindowManager.SpawnInputDialog("Enter IP of the server you want to connect", "Connect", ip, (result) =>
                    {
                        behavior.StartConnect(result);
                    });
                }
                else
                {
                    behavior.CreateMainWindow();
                }
                
            });
            WindowManager.AddElementToElement(mainButton.gameObject, gameWindow.gameObject, new Rect(screenSizeX - 80f, 0f, 80f, 32f), new Rect(0f, 0f, 0f, 0f));


            int counter = 1;
            foreach (var item in objs)
            {
                WindowManager.AddElementToElement(item, parent.gameObject, new Rect(0, counter * 32, 250, 32),
                new Rect(0, 0, 0, 0));
                counter++;
            } 
            #endregion
        }

        public string Name
        {
            //This will be displayed as the header in the Options window
            get { return "Multiplayer V1"; }
        }
    }
}