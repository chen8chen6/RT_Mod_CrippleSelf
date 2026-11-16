using Kingmaker.Localization.Enums;
using Kingmaker.Localization.Shared;
using Kingmaker.Utility.DotNetExtensions;

namespace CrippleSelf
{
    internal class Localization
    {
        public bool Started { get; private set; }

        public void Start(ILocalizationProvider localizationProvider, string modPath)
        {
            Logger.Info($"Start({localizationProvider})");
            if (this.Started) return;

            this.Started = true;
            this.m_LocalizationProvider = localizationProvider;
            this.m_modPath = modPath;

            try
            {
                this.ApplyLocalization(localizationProvider.CurrentLocale);
                this.m_LocalizationProvider.LocaleChanged += this.ApplyLocalization;
            }
            catch (Exception e)
            {
                Logger.Error($"Localization.Start({localizationProvider}) failed: {e}");
            }
        }

        public void ApplyLocalization(Locale loc)
        {
            Logger.Info($"ApplyLocalization({loc})");
            if (null == m_LocalizationProvider.CurrentPack) return;

            //LoadLocalizationPack
            LocalizationPack pack = LoadLocalizationPack(this.m_LocalizationProvider.CurrentLocale);
            if (null == pack) return;

            this.m_LocalizationProvider.CurrentPack?.AddStrings(pack);
            Logger.Info($"ApplyLocalization() successed. Pack Size: {this.m_LocalizationProvider.CurrentPack?.m_Strings.Count}");
        }

        public LocalizationPack LoadLocalizationPack(Locale locale)
        {
            LocalizationPack localizationPack = this.m_LocalizationPacks.FirstItem((ValueTuple<Locale, LocalizationPack> i) => i.Item1 == locale).Item2;
            if (localizationPack == null)
            {
                string packPath = PackPath(locale);
                if (this.m_LocalizationProvider != null)
                {
                    localizationPack = this.m_LocalizationProvider.LoadPack(packPath, locale);
                    this.m_LocalizationPacks.Add(new ValueTuple<Locale, LocalizationPack>(locale, localizationPack));
                }
            }
            return localizationPack;
        }

        private string m_modPath = null;
        private ILocalizationProvider m_LocalizationProvider = null;
        private readonly List<ValueTuple<Locale, LocalizationPack>> m_LocalizationPacks = new List<ValueTuple<Locale, LocalizationPack>>();

        private string? PackPath(Locale loc)
        {
            if (null == this.m_modPath)
            {
                Logger.Critical("ModPath NOT set! Please call Localization.SetModPath() before harmony.PatchAll() in Main.Load()");
                return null;
            }
            return Path.Combine(this.m_modPath, "Localization/" + loc.ToString() + ".json");
        }
    }
}
