using HarmonyLib;
using Kingmaker.PubSubSystem.Core;
using System.Reflection;
using UnityModManagerNet;

namespace CrippleSelf
{
    static class Main
    {
        public static bool Enabled;
        public static UnityModManager.ModEntry ModEntry { get; private set; }

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            Main.ModEntry = modEntry;
            modEntry.OnToggle = OnToggle;
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
    }
}
