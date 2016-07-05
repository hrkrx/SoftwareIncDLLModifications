using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Trainer
{
    public class TrainerBehaviour : ModBehaviour
    {
        public bool ModActive = false;
        public bool LockAge = false;
        public bool LockStress = false;
        public bool LockHunger = false;
        public Text button = WindowManager.SpawnLabel();
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
                foreach (var item in GameSettings.Instance.sActorManager.Actors)
                {
                    if (LockAge)
                    {
                        item.employee.AgeMonth = 20 * 12;
                        item.UpdateAgeLook();
                    }
                    if (LockStress)
                    {
                        item.StressFactor = 0;
                    }
                }
            }
        }

        public void IncreaseMoney()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                GameSettings.Instance.MyCompany.MakeTransaction(999999999999, Company.TransactionCategory.Deals);
            }
        }

        public void ResetAgeOfEmployees()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                foreach (var item in GameSettings.Instance.sActorManager.Actors)
                {
                    item.employee.AddThought("I'm as good as new!", 1);
                    item.employee.AgeMonth = 20 * 12;
                    item.UpdateAgeLook();
                }

            }
        }

        public override void OnActivate()
        {
            ModActive = true;
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                HUD.Instance.AddPopupMessage("Trainer V1 has been activated!", "Cogs", "", 0, 1);
            }
        }

        internal void AddRep()
        {
            if (!((UnityEngine.Object)GameSettings.Instance != (UnityEngine.Object)null))
                return;
            GameSettings.Instance.MyCompany.BusinessReputation = 1f;
            SoftwareType random1 = Utilities.GetRandom<SoftwareType>(Enumerable.Where<SoftwareType>((IEnumerable<SoftwareType>)GameSettings.Instance.SoftwareTypes.Values, (Func<SoftwareType, bool>)(x => !x.OneClient)));
            string random2 = Utilities.GetRandom<string>((IEnumerable<string>)random1.Categories.Keys);
            GameSettings.Instance.MyCompany.AddFans(100000000, random1.Name, random2);
        }

        public override void OnDeactivate()
        {
            ModActive = false;
            if (!ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                HUD.Instance.AddPopupMessage("Trainer V1 has been deactivated!", "Cogs", "", 0, 1);
            }
        }

        internal void LockAgeOfEmployees(bool a)
        {
            LockAge = a;
        }

        internal void EmployeesToFemale()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                foreach (var item in GameSettings.Instance.sActorManager.Actors)
                {
                    item.employee.AddThought("I like this!", 1);
                    item.UpdateAgeLook();
                }
            }
        }

        internal void EmployeesToMax()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                if (!((UnityEngine.Object)SelectorController.Instance != (UnityEngine.Object)null))
                    return;
                foreach (var x in GameSettings.Instance.sActorManager.Actors)
                {
                    for (int index = 0; index < 5; ++index)
                    {
                        x.employee.ChangeSkill((Employee.EmployeeRole)index, 1f, false);
                        foreach (string specialization in GameSettings.Instance.Specializations)
                        {
                            x.employee.AddToSpecialization(Employee.EmployeeRole.Designer, specialization, 1f, 0, true);
                            x.employee.AddToSpecialization(Employee.EmployeeRole.Artist, specialization, 1f, 0, true);
                            x.employee.AddToSpecialization(Employee.EmployeeRole.Programmer, specialization, 1f, 0, true);
                        }
                    }
                }
            }
        }

        internal void LockStressOfEmployees(bool a)
        {
            LockStress = a;
        }

        internal void SkipDay()
        {
            if (!((UnityEngine.Object)GameSettings.Instance != (UnityEngine.Object)null) || GameSettings.ForcePause)
                return;
            int num1 = TimeOfDay.Instance.Hour;
            bool flag = false;
            int num2 = 1;
            for (int index = 0; index < num2; ++index)
            {
                do
                {
                    flag |= TimeOfDay.Instance.AddHour(!flag);
                }
                while (!GameSettings.ForcePause && TimeOfDay.Instance.Hour != num1 && !flag);
            }
        }

        internal void UnlockAllSpace()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                if (!((UnityEngine.Object)GameSettings.Instance != (UnityEngine.Object)null))
                    return;
                GameSettings.Instance.BuildableArea = new Rect(9f, 1f, 246f, 254f);
                GameSettings.Instance.ExpandLand(0, 0);
            }
        }

        internal void UnlockAll()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                Cheats.UnlockFurn = !Cheats.UnlockFurn;
                if (!((UnityEngine.Object)HUD.Instance != (UnityEngine.Object)null))
                    return;
                HUD.Instance.UpdateFurnitureButtons();
            }
        }

        internal void UpgradeAllComputer()
        {
            if (ModActive && GameSettings.Instance != null && HUD.Instance != null)
            {
                Furniture[] furnList = GameSettings.Instance.sRoomManager.AllFurniture.Where(x => x.GetComputer() != null).ToArray();
                HUD.Instance.serverWindow.ServerListUpdateEnabled = false;
                bool flag = false;
                SelectorController.Instance.Selected.Clear();
                foreach (Furniture furn in furnList)
                {
                    if (furn.GetComputer().FurnitureUpgrade != null)
                    {
                        float price = furn.GetComputer().FurnitureUpgrade.gameObject.GetComponent<Furniture>().Cost;

                        if ((Cheats.UnlockFurn || TimeOfDay.Instance.Year + SDateTime.BaseYear >= furn.GetComputer().FurnitureUpgrade.GetComponent<Furniture>().UnlockYear) && (GameSettings.Instance.MyCompany.CanMakeTransaction(-price)))
                        {

                            Furniture furniture = UpgradeFurniture(furn.GetComputer());
                            furniture.gameObject.SetActive(true);
                            SelectorController.Instance.Selected.Add((Selectable)furniture);
                            GameSettings.Instance.MyCompany.MakeTransaction(-price, Company.TransactionCategory.Construction);
                            CostDisplay.Instance.Show(price, furniture.transform.position);
                            CostDisplay.Instance.FloatAway();
                            flag = true;
                        }
                    }
                }

                HUD.Instance.serverWindow.ServerListUpdateEnabled = true;
                if (!flag)
                    return;
                HUD.Instance.PlayFurniturePlace();
                HUD.Instance.PlayKaching();
                SelectorController.Instance.DoPostSelectChecks();
                HUD.Instance.serverWindow.UpdateServerList();
                HUD.Instance.AddPopupMessage(furnList.Length + " computers have been upgraded!", "Cogs", "", 0, 1);
            }
        }


        private Furniture UpgradeFurniture(Furniture furn)
        {
            Vector3 vector3 = furn.OriginalPosition;
            Quaternion rotation = furn.transform.rotation;
            float num = furn.RotationOffset;
            Room room = furn.Parent;
            SnapPoint snapPoint1 = furn.SnappedTo;
            Dictionary<int, Furniture> dictionary = Enumerable.ToDictionary<SnapPoint, int, Furniture>(Enumerable.Where<SnapPoint>((IEnumerable<SnapPoint>)furn.SnapPoints, (Func<SnapPoint, bool>)(sp => (UnityEngine.Object)sp.UsedBy != (UnityEngine.Object)null)), (Func<SnapPoint, int>)(sp => sp.Id), (Func<SnapPoint, Furniture>)(sp => sp.UsedBy));
            for (int index = 0; index < furn.SnapPoints.Length; ++index)
                furn.SnapPoints[index].UsedBy = (Furniture)null;
            TableScript component1 = furn.GetComponent<TableScript>();
            Server component2 = furn.GetComponent<Server>();
            Actor ownedBy = furn.OwnedBy;
            UnityEngine.Object.Destroy((UnityEngine.Object)furn.gameObject);
            //if (SelectorController.Instance.DeleteParent(furn))
            //    UnityEngine.Object.Destroy((UnityEngine.Object)furn.SnappedTo.Parent.gameObject);
            Furniture component3 = UnityEngine.Object.Instantiate<GameObject>(furn.FurnitureUpgrade).GetComponent<Furniture>();
            component3.RotationOffset = num;
            try
            {
                if ((UnityEngine.Object)snapPoint1 != (UnityEngine.Object)null)
                {
                    if (component3.IsSnapping)
                    {
                        component3.SnappedTo = snapPoint1;
                        snapPoint1.UsedBy = component3;
                    }
                    else
                        vector3 = new Vector3(vector3.x, (float)(furn.Parent.Floor * 2), vector3.z);
                }
                component3.OriginalPosition = vector3;
                component3.transform.position = vector3;
                component3.transform.rotation = rotation;
                component3.Parent = room;
                Server component4 = component3.GetComponent<Server>();
                if ((UnityEngine.Object)component2 != (UnityEngine.Object)null && (UnityEngine.Object)component4 != (UnityEngine.Object)null)
                {
                    component4.WireTo(component2.Rep, true);
                    component4.PreWired = true;
                }
                component3.OwnedBy = ownedBy;
                if ((UnityEngine.Object)ownedBy != (UnityEngine.Object)null)
                    ownedBy.Owns.Add(component3);
                if (furn.InteractionPoints.Length == component3.InteractionPoints.Length)
                {
                    for (int index = 0; index < furn.InteractionPoints.Length; ++index)
                    {
                        if ((UnityEngine.Object)furn.InteractionPoints[index].UsedBy != (UnityEngine.Object)null && furn.InteractionPoints[index].Name.Equals(component3.InteractionPoints[index].Name))
                        {
                            Actor usedBy = furn.InteractionPoints[index].UsedBy;
                            usedBy.UsingPoint = component3.InteractionPoints[index];
                            component3.InteractionPoints[index].UsedBy = usedBy;
                        }
                    }
                }
                component3.UpdateFreeNavs(false);
            }
            catch (Exception e)
            {
                HUD.Instance.AddPopupMessage(e.Message + "\n" + e.StackTrace, "Cogs", "", 0, 1);
            }
            return component3;
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
