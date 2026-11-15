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
            AbilityEffectRunAction abilityEffect = new() { Actions = ActionCrippleSelf() };
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
                ComponentsArray = [abilityEffect],  //TODO: UI Component
            };
        }

        private static ActionList ActionCrippleSelf()
        {
            return new ActionList()
            {
                Actions = [
                    RemoveBuff(AssetID.Trauma.TRAUMA_RANK),
                    RankUpTraumaBuff(),     //After this action, caster has unknown num of traumas
                    RemoveBuff(AssetID.Trauma.BROKEN_RIBS),
                    RemoveBuff(AssetID.Trauma.CONCUSSION),
                    RemoveBuff(AssetID.Trauma.CRIPPLED_ARM_LEFT),
                    RemoveBuff(AssetID.Trauma.CRIPPLED_ARM_RIGHT),
                    RemoveBuff(AssetID.Trauma.CRIPPLED_LEG),
                    RemoveBuff(AssetID.Trauma.NEUROGENIC_SHOCK),
                    ApplySelectedTraumas(),
                ]
            };
        }

        private static Mod_CrippleSelf_RankUpTrauma RankUpTraumaBuff()
        {
            BlueprintBuff TraumaCntBuff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(AssetID.Trauma.TRAUMA_RANK);
            if (null == TraumaCntBuff)
                return null;

            return new Mod_CrippleSelf_RankUpTrauma()
            {
                m_Buff = TraumaCntBuff.ToReference<BlueprintBuffReference>(),
                BuffEndCondition = Kingmaker.UnitLogic.Buffs.BuffEndCondition.RemainAfterCombat,
                Permanent = true,
                //Ranks = TraumaCntBuff.MaxRank,    //Ranks val set in RunAction() in runtime
            };
        }

        private static ContextActionRemoveBuff RemoveBuff(string buffGuid)
        {
            BlueprintBuff buff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(buffGuid);
            var buffRef = buff?.ToReference<BlueprintBuffReference>();
            return new ContextActionRemoveBuff() { m_Buff = buffRef };
        }

        private static Mod_CrippleSelf_ApplySelectedTraumas ApplySelectedTraumas()
        {
            return new Mod_CrippleSelf_ApplySelectedTraumas()
            {
                BuffEndCondition = Kingmaker.UnitLogic.Buffs.BuffEndCondition.RemainAfterCombat,
                Permanent = true,
            };
        }
    }
}
