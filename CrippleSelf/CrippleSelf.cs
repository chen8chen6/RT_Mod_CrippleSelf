using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CrippleSelf
{
    internal class CrippleSelf
    {
        [HarmonyPatch(typeof(BlueprintsCache))]
        static class BlueprintsCache_Patches
        {
            public static bool loaded = false;

            [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
            public static void Postfix(ref BlueprintsCache __instance)
            {
                if (loaded) return;
                loaded = true;

                //Register Ability CrippleSelf
                BlueprintAbility CrippleSelf = AbilityCrippleSelf();
                __instance.AddCachedBlueprint(CrippleSelf.AssetGuid, CrippleSelf);
            }
        }

        private static BlueprintAbility AbilityCrippleSelf()
        {
            AbilityEffectRunAction abilityEffect = new() { Actions = ApplyAllTrauma() };
            BlueprintBuff brokenRibs = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(AssetID.Trauma.BROKEN_RIBS);

            return new BlueprintAbility
            {
                AssetGuid = AssetID.Ability.MOD_CRIPPLE_SELF,
                Comment = "Apply all traumas on self",
                name = "MOD_CrippleSelf_ability",

                //Display options
                m_DisplayName = new LocalizedString() { m_Key = AssetID.LocalizedStringKey.CRIPPLE_SELF_NAME },
                m_Description = new LocalizedString() { m_Key = AssetID.LocalizedStringKey.CRIPPLE_SELF_DESC },
                m_Icon = brokenRibs?.Icon,

                //Cast on self
                Range = AbilityRange.Personal,
                CanTargetSelf = true,
                ShouldTurnToTarget = true,
                Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self,

                //Can be casted outsite combat
                CombatStateRestriction = BlueprintAbility.CombatStateRestrictionType.NoRestriction,

                //Will not cause attack
                UsingInOverwatchArea = BlueprintAbility.UsingInOverwatchAreaType.WillNotCauseAttack,
                UsingInThreateningArea = BlueprintAbility.UsingInThreateningAreaType.CanUseWithoutAOO,

                //Ability effects
                ActionPointCost = 0,
                ComponentsArray = [abilityEffect],
            };
        }

        private static ActionList ApplyAllTrauma()
        {
            //Rank up traumabuff to maxRank.
            //This buff works as a counter. Each time the counter ranks up, a random trauma is applyed on target
            return new ActionList { Actions = [RankUpTraumaBuff()] }; ;
        }

        private static ContextActionApplyBuff RankUpTraumaBuff()
        {
            BlueprintBuff TraumaCntBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(AssetID.Trauma.TRAUMA_BUFF);
            if (null == TraumaCntBuff)
                return null;

            return new ContextActionApplyBuff()
            {
                m_Buff = TraumaCntBuff.ToReference<BlueprintBuffReference>(),
                BuffEndCondition = Kingmaker.UnitLogic.Buffs.BuffEndCondition.RemainAfterCombat,
                Permanent = true,
                Ranks = TraumaCntBuff.MaxRank,
            };
        }
    }
}
