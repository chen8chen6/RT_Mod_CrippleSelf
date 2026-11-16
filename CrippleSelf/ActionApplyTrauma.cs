using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CrippleSelf
{
    internal class Mod_CrippleSelf_RankUpTrauma : ContextActionApplyBuff
    {
        public override void RunAction()
        {
            bool[] isToApply = Main.Settings.GetIsToApply();

            int traumaCnt = 0;
            for (int i = 0; i < isToApply.Length; ++i)
            {
                if (isToApply[i])
                    ++traumaCnt;
            }

            if (traumaCnt > 0)
            {
                this.Ranks = traumaCnt;
                base.RunAction();
            }
        }
    }

    internal class Mod_CrippleSelf_ApplySelectedTraumas : ContextActionApplyBuff
    {
        public override void RunAction()
        {
            bool[] isToApply = Main.Settings.GetIsToApply();

            string[] traumasToApply =
            {
                AssetID.Trauma.BROKEN_RIBS,
                AssetID.Trauma.CONCUSSION,
                AssetID.Trauma.CRIPPLED_ARM_LEFT,
                AssetID.Trauma.CRIPPLED_ARM_RIGHT,
                AssetID.Trauma.CRIPPLED_LEG,
                AssetID.Trauma.NEUROGENIC_SHOCK,
            };

            for (int i = 0; i < traumasToApply.Length; ++i)
            {
                if (!isToApply[i]) continue;

                BlueprintBuff TraumaBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(traumasToApply[i]);
                if (null == TraumaBuff)
                    continue;

                m_Buff = TraumaBuff.ToReference<BlueprintBuffReference>();
                base.RunAction();
            }
        }
    }
}
