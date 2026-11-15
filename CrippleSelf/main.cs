using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem.Core;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace CrippleSelf
{
    static class Main
    {
        public static Settings Settings;
        public static bool Enabled;
        public static UnityModManager.ModEntry ModEntry { get; private set; }

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            Main.Settings = Settings.Load<Settings>(modEntry);
            Main.ModEntry = modEntry;

            //TODO: microPatch disables localization of this mod, why?
            Localization.SetLoadedMarkKey(AssetID.LocalizedStringKey.CRIPPLE_SELF_NAME);  //Used to check if mod language pack is loaded

            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            EventBus.Subscribe(new PartyGrant());   //Add CrippleSelf ability on savedata loaded
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Traumas to apply:", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            DrawTraumaOption(AssetID.Trauma.BROKEN_RIBS,        ref Settings.IsToApply_BrokenRibs);
            DrawTraumaOption(AssetID.Trauma.CONCUSSION,         ref Settings.IsToApply_Concussion);
            DrawTraumaOption(AssetID.Trauma.CRIPPLED_ARM_LEFT,  ref Settings.IsToApply_CrippledArm_Left);
            DrawTraumaOption(AssetID.Trauma.CRIPPLED_ARM_RIGHT, ref Settings.IsToApply_CrippledArm_Right);
            DrawTraumaOption(AssetID.Trauma.CRIPPLED_LEG,       ref Settings.IsToApply_CrippledLeg);
            DrawTraumaOption(AssetID.Trauma.NEUROGENIC_SHOCK,   ref Settings.IsToApply_NeurogenicShock);
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }

        private static void DrawTraumaOption(string id, ref bool option)
        {
            BlueprintBuff trauma= ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(id);

            string name = (null == trauma) ? "Unknown" : trauma.m_DisplayName.GetText();
            string desc = (null == trauma) ? "Unknown" : trauma.m_Description.GetText();

            //Add "Left"/"Right" to Crippele Arm
            BlueprintComponent[] componentsArray = trauma.ComponentsArray;//base.ComponentsArray;
            for (int i = 0; i < componentsArray.Length; i++)
            {
                AddStringToFactName addStringToFactName = componentsArray[i] as AddStringToFactName;
                if (addStringToFactName != null)
                {
                    name = addStringToFactName.NewString(name);
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            
            //name
            option = GUILayout.Toggle(option, " " + name, GUILayout.Width(180));
            GUILayout.Space(20);

            //desc, trun brown text to green
            GUILayout.Label(desc.Replace("color=#6D3200", "color=#00FF00"), GUILayout.ExpandWidth(false));

            //AlignLeft
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
