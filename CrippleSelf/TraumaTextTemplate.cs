using Kingmaker.Blueprints;
using Kingmaker.Localization;
using Kingmaker.Localization.Enums;
using Kingmaker.TextTools.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace CrippleSelf
{
    //{mod.crippleself.traumasToApply|, } -> "trauma1, trauma2, ..."
    internal class TraumaTextTemplate : TextTemplate
    {
        public const string TemplateStr = "mod.crippleself.traumasToApply";
        public override string Generate(bool capitalized, List<string> parameters)
        {
            //Use cached result
            int hash = Main.Settings.GetHash();
            Locale locale = LocalizationManager.Instance.CurrentPack.Locale;
            if (hash == m_lastSettingHash
                && locale == m_lastLocale)
            {
                return m_cachedResult;
            }

            var isToApply = Main.Settings.GetIsToApply();
            var traumaList = AssetID.ALL_TRAUMAS;
            string separator = parameters.Count > 0 ? parameters[0] : ", ";
            string res = null;

            //Dynamically generate ability description
            for (int i = 0; i < isToApply.Length; ++i)
            {
                if (isToApply[i])
                {
                    if (null != res)
                        res += separator;
                    string name = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(traumaList[i]).Name;
                    res += $"<b><color=#272F6C><link=\"f:{traumaList[i]}\">{name}</link></color></b>";
                }
            }

            //Cache result
            m_lastSettingHash = hash;
            m_lastLocale = locale;
            m_cachedResult = res;

            return res;
        }

        private int m_lastSettingHash = int.MaxValue;
        private Locale m_lastLocale = Locale.Sound;
        private string m_cachedResult = null;
    }
}
