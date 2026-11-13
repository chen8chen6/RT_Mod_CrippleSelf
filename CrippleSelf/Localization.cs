using HarmonyLib;
using Kingmaker.Localization;
using Kingmaker.Localization.Enums;
using Kingmaker.Localization.Shared;

namespace CrippleSelf
{
    //Patch LocalizationManager.LoadPack(locale)
    [HarmonyPatch(typeof(LocalizationManager), "LoadPack", new Type[]
    {
        typeof(Locale)
    })]
    internal class Localization
    {
        [HarmonyPostfix]
        public static void Postfix(ref LocalizationManager __instance)
        {
            Main.ModEntry.Logger.Log("LoadPack(locale)");
            LoadLanguagePack(ref __instance);
        }

        public static void LoadLanguagePack(ref LocalizationManager __instance)
        {
            var logger = Main.ModEntry.Logger;

            //Check if m_loadedMarkKey set
            if (null == m_loadedMarkKey)
            {
                logger.Critical("LoadedMarkKey NOT set! Please call Localization.SetLoadedMarkKey() before harmony.PatchAll() in Main.Load()");
                return;
            }

            //Check if is loading/loaded
            if (m_isLoadding || IsLoaded(ref __instance))
            {
                logger.Log("Language pack is " + (m_isLoadding ? "Loading" : "Loaded"));
                return;
            }

            //Load language pack
            string packPath = PackPath(ref __instance);
            m_isLoadding = true;
            logger.Log($"Loading language pack: {packPath}");
            LocalizationPack pack = __instance.LoadPack(packPath, __instance.CurrentPack.Locale);
            __instance.CurrentPack.AddStrings(pack);
            m_isLoadding = false;

            //Check if loaded
            if (IsLoaded(ref __instance))
                logger.Log($"Load language pack successed: {packPath}");
            else
                logger.Error($"Load language pack failed: {packPath}");
        }

        public static void SetLoadedMarkKey(string key) { m_loadedMarkKey = key; }

        private static string m_loadedMarkKey = null;   //Used to check if mod language pack is loaded
        private static bool m_isLoadding = false;

        private static bool IsLoaded(ref LocalizationManager __instance)
        {
            //Check if m_loadedMarkKey added in LocalizationManager.CurrentPack
            if (!__instance.CurrentPack.TryGetText(m_loadedMarkKey, out string text))
            {
                Main.ModEntry.Logger.Warning("Language pack NOT loaded: " + text);
                return false;
            }
            return true;
        }

        private static string PackPath(ref LocalizationManager __instance)
        {
            return Path.Combine(Main.ModEntry.Path, "Localization/" + __instance.CurrentPack.Locale.ToString() + ".json");
        }
    }

    //Patch LocalizationManager.LoadPack(string, locale)
    [HarmonyPatch(typeof(LocalizationManager), "LoadPack", new Type[]
    {
        typeof(string),
        typeof(Locale)
    })]
    internal static class LocalizationReloadPatchWithPath
    {
        [HarmonyPostfix]
        private static void Postfix(ref LocalizationManager __instance)
        {
            Main.ModEntry.Logger.Log("LoadPack(string, locale)");
            Localization.LoadLanguagePack(ref __instance);
        }
    }
}

