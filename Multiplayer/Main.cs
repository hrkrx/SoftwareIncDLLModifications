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

            //TextBox for IP
            var textBoxIP = WindowManager.SpawnInputbox();
            textBoxIP.GetComponentInChildren<UnityEngine.UI.Text>().text = "Add some Money";
            objs.Add(textBoxIP.gameObject);

            //Button for some Money
            var buttonConnect = WindowManager.SpawnButton();
            buttonConnect.GetComponentInChildren<UnityEngine.UI.Text>().text = "Add some Money";
            buttonConnect.onClick.AddListener(() =>
            {
                behavior.Connect(textBoxIP.text);
            });
            objs.Add(buttonConnect.gameObject);

            int counter = 1;
            foreach (var item in objs)
            {
                WindowManager.AddElementToElement(item, parent.gameObject, new Rect(0, counter * 32, 250, 32),
                new Rect(0, 0, 0, 0));
                counter++;
            }


            // Button for GenderChange
            // var buttonGenderChange = WindowManager.SpawnButton();
            //buttonGenderChange.GetComponentInChildren<UnityEngine.UI.Text>().text = "Change Employees to Female";
            //buttonGenderChange.onClick.AddListener(() => {
            //    behavior.EmployeesToFemale();
            //});
            //WindowManager.AddElementToElement(buttonGenderChange.gameObject, parent.gameObject, new Rect(0, 128, 250, 32),
            //    new Rect(0, 0, 0, 0));


        }

        public string Name
        {
            //This will be displayed as the header in the Options window
            get { return "Trainer V1"; }
        }
    }
}