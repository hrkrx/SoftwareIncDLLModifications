using UnityEngine;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

namespace Trainer
{
    //Your mod should have exactly one class that implements the ModMeta interface
    public class Main : ModMeta
    {
        //This function is used to generate the content in the "Mods" section of the options window
        //The behaviors array contains all behaviours that have been spawned for this mod, one for each implementation
        public void ConstructOptionsScreen(RectTransform parent, ModBehaviour[] behaviours)
        {
            //We need a reference to a behavior to read and write from the mod settings file
            var behavior = behaviours.OfType<TrainerBehaviour>().First();
            List<GameObject> objs = new List<GameObject>();

            //Start by spawning a label
            var label = WindowManager.SpawnLabel();
            label.text = "This Mod was created by LtPain";
            WindowManager.AddElementToElement(label.gameObject, parent.gameObject, new Rect(0, 0, 250, 32),
                new Rect(0, 0, 0, 0));



            //Button for some Money
            var buttonMoney = WindowManager.SpawnButton();
            buttonMoney.GetComponentInChildren<UnityEngine.UI.Text> ().text = "Add some Money";
            buttonMoney.onClick.AddListener(() => {
                behavior.IncreaseMoney();
            });
            objs.Add(buttonMoney.gameObject);

            //Button for MaxLand
            var buttonMaxLand = WindowManager.SpawnButton();
            buttonMaxLand.GetComponentInChildren<UnityEngine.UI.Text>().text = "Unlock all space";
            buttonMaxLand.onClick.AddListener(() => {
                behavior.UnlockAllSpace();
            });
            objs.Add(buttonMaxLand.gameObject);

            //Button for AddRep
            var buttonAddRep = WindowManager.SpawnButton();
            buttonAddRep.GetComponentInChildren<UnityEngine.UI.Text>().text = "Add Reputation";
            buttonAddRep.onClick.AddListener(() => {
                behavior.AddRep();
            });
            objs.Add(buttonAddRep.gameObject);

            //Button for UnlockAll
            var buttonUnlockAll = WindowManager.SpawnButton();
            buttonUnlockAll.GetComponentInChildren<UnityEngine.UI.Text>().text = "Unlock all Furniture";
            buttonUnlockAll.onClick.AddListener(() => {
                behavior.UnlockAll();
            });
            objs.Add(buttonUnlockAll.gameObject);

            //Button for EmployeeAge
            var buttonEmployeeAge = WindowManager.SpawnButton();
            buttonEmployeeAge.GetComponentInChildren<UnityEngine.UI.Text>().text = "Reset age of employees";
            buttonEmployeeAge.onClick.AddListener(() => {
                behavior.ResetAgeOfEmployees();
            });
            objs.Add(buttonEmployeeAge.gameObject);

            //Button for MaxSkill
            var buttonMaxSkill = WindowManager.SpawnButton();
            buttonMaxSkill.GetComponentInChildren<UnityEngine.UI.Text>().text = "Max Skill of employees";
            buttonMaxSkill.onClick.AddListener(() => {
                behavior.EmployeesToMax();
            });
            objs.Add(buttonMaxSkill.gameObject);

            //Button for SkipDay
            var buttonSkipDay = WindowManager.SpawnButton();
            buttonSkipDay.GetComponentInChildren<UnityEngine.UI.Text>().text = "Skip Day";
            buttonSkipDay.onClick.AddListener(() => {
                behavior.SkipDay();
            });

            //CheckBox for EmployeeAge
            var lockAge = WindowManager.SpawnCheckbox();
            lockAge.GetComponentInChildren<UnityEngine.UI.Text>().text = "Lock age of employees";
            lockAge.isOn = false;
            lockAge.onValueChanged.AddListener((a) => {
                behavior.LockAgeOfEmployees(a);
            });
            objs.Add(lockAge.gameObject);

            //CheckBox for LockEmployeeStress
            var lockStress = WindowManager.SpawnCheckbox();
            lockStress.GetComponentInChildren<UnityEngine.UI.Text>().text = "Disable Stress";
            lockStress.isOn = false;
            lockStress.onValueChanged.AddListener((a) => {
                behavior.LockStressOfEmployees(a);
            });
            //objs.Add(lockStress.gameObject);

            //Button for UpgradeComputer
            var buttonUpgradeComputer = WindowManager.SpawnButton();
            buttonUpgradeComputer.GetComponentInChildren<UnityEngine.UI.Text>().text = "Upgrade all Computer";
            buttonUpgradeComputer.onClick.AddListener(() => {
                behavior.UpgradeAllComputer();
            });
            objs.Add(buttonUpgradeComputer.gameObject);

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