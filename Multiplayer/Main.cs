using UnityEngine;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

namespace Multiplayer
{
    //Your mod should have exactly one class that implements the ModMeta interface
    public class Main : ModMeta
    {
        string ip = "79.143.188.80";
        //This function is used to generate the content in the "Mods" section of the options window
        //The behaviors array contains all behaviours that have been spawned for this mod, one for each implementation
        public void ConstructOptionsScreen(RectTransform parent, ModBehaviour[] behaviours)
        {
            //We need a reference to a behavior to read and write from the mod settings file
            var behavior = behaviours.OfType<MultiplayerBehaviour>().First();
            List<GameObject> objs = new List<GameObject>();

            //Start by spawning a label
            var label = WindowManager.SpawnLabel();
            label.text = "This Mod was created by LtPain";
            WindowManager.AddElementToElement(label.gameObject, parent.gameObject, new Rect(0, 0, 250, 32),
                new Rect(0, 0, 0, 0));


            var connectButton = WindowManager.SpawnButton();
            connectButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Connect";
            connectButton.onClick.AddListener(() => {
                WindowManager.SpawnInputDialog("Enter IP of the server you want to connect", "Connect", ip, (result) => {
                    behavior.StartConnect(result);
                });
            });
            objs.Add(connectButton.gameObject);

            var infoButton = WindowManager.SpawnButton();


            int counter = 1;
            foreach (var item in objs)
            {
                WindowManager.AddElementToElement(item, parent.gameObject, new Rect(0, counter * 32, 250, 32),
                new Rect(0, 0, 0, 0));
                counter++;
            }
        }

        public string Name
        {
            //This will be displayed as the header in the Options window
            get { return "Multiplayer V1"; }
        }
    }
}