using UnityModManagerNet;

namespace CrippleSelf
{
    internal class Logger
    {
        public static void SetLogger(UnityModManager.ModEntry.ModLogger logger) { m_logger = logger; }
        public static void Info(string str) { m_logger?.Log(str); }
        public static void Warning(string str) { m_logger?.Warning(str); }
        public static void Critical(string str) { m_logger?.Critical(str); }
        public static void Error(string str) { m_logger?.Error(str); }

        private static UnityModManager.ModEntry.ModLogger m_logger = null;
    }
}
