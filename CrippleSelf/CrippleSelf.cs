using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Progression.Features;

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

                //Add CrippleSelf ability when "Facing the end" or "Wounded beast" feature added
                AddAbilityForFeature(ref CrippleSelf, AssetID.Feature.FACING_THE_END);
                AddAbilityForFeature(ref CrippleSelf, AssetID.Feature.WOUNDED_BEAST);
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

                //Diaplay name and description
                m_DisplayName = new LocalizedString() { m_Key = AssetID.LocalizedStringKey.CRIPPLE_SELF_NAME },
                m_Description = new LocalizedString() { m_Key = AssetID.LocalizedStringKey.CRIPPLE_SELF_DESC },

                //Cast on self
                Range = AbilityRange.Personal,
                CanTargetSelf = true,
                ShouldTurnToTarget = true,

                //Can be casted outsite combat
                CombatStateRestriction = BlueprintAbility.CombatStateRestrictionType.NoRestriction,

                //Will not cause attack
                UsingInOverwatchArea = BlueprintAbility.UsingInOverwatchAreaType.WillNotCauseAttack,
                UsingInThreateningArea = BlueprintAbility.UsingInThreateningAreaType.CanUseWithoutAOO,

                ActionPointCost = 0,
                ComponentsArray = [abilityEffect],
                m_Icon = brokenRibs?.Icon,
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

        private static void AddAbilityForFeature(ref BlueprintAbility ability, string featureGuid)
        {
            var feature = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(featureGuid);
            if (null == feature)
                return;

            //TODO: check if addFacts exists in componentArray
            AddFacts fact = new() { m_Facts = [ability.ToReference<BlueprintUnitFactReference>()] };
            feature.ComponentsArray = feature.ComponentsArray.AddToArray(fact);
        }
    }
}
