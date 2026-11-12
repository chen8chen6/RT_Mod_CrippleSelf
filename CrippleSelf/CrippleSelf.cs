using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private static readonly string[] ALL_TRAUMAS = {
            AssetID.Trauma.BrokenRibs,
            AssetID.Trauma.CrippledArm_Left,
            AssetID.Trauma.CrippledArm_Right,
            AssetID.Trauma.Concussion,
            AssetID.Trauma.NerveDamage,
            AssetID.Trauma.CrippledLeg,
        };

        private static BlueprintAbility AbilityCrippleSelf()
        {
            AbilityEffectRunAction abilityEffect = new() { Actions = ApplyAllTrauma() };
            BlueprintAbility gun_and_run = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("5e60764f84c94277ae6a78b63a1fd2aa");

            return new BlueprintAbility
            {
                AssetGuid = "5e60764f84c94277ae6a78b63a1fd2ab", //TODO: random guid
                Comment = "test cripppleSelf ability",
                name = "MOD_CrippleSelf_ability",

                Range = AbilityRange.Personal,
                CanTargetSelf = true,
                ShouldTurnToTarget = true,

                CombatStateRestriction = BlueprintAbility.CombatStateRestrictionType.NoRestriction,
                UsingInOverwatchArea = BlueprintAbility.UsingInOverwatchAreaType.WillNotCauseAttack,
                UsingInThreateningArea = BlueprintAbility.UsingInThreateningAreaType.CanUseWithoutAOO,

                ComponentsArray = new BlueprintComponent[4] { abilityEffect, gun_and_run.ComponentsArray[1], gun_and_run.ComponentsArray[2], gun_and_run.ComponentsArray[3] },
                m_Icon = gun_and_run.Icon,
            };

        }

        private static ActionList ApplyAllTrauma()
        {
            var applyAllTrauma = new ActionList { Actions = new GameAction[ALL_TRAUMAS.Length + 1] };

            for (int i = 0; i < ALL_TRAUMAS.Length; ++i)
                applyAllTrauma.Actions[i] = ApplyTrauma(ALL_TRAUMAS[i]);

            //add traumabuff rank 6
            applyAllTrauma.Actions[ALL_TRAUMAS.Length] = RankUpTraumaBuff();

            return applyAllTrauma;
        }

        private static ContextActionApplyBuff ApplyTrauma(string guid)
        {
            return new ContextActionApplyBuff()
            {
                m_Buff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(guid).ToReference<BlueprintBuffReference>(),
                BuffEndCondition = Kingmaker.UnitLogic.Buffs.BuffEndCondition.RemainAfterCombat,
                Permanent = true,
                //name = "name here",
            };
        }

        private static ContextActionApplyBuff RankUpTraumaBuff()
        {
            return new ContextActionApplyBuff()
            {
                m_Buff = ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(AssetID.Trauma.TraumaBuff).ToReference<BlueprintBuffReference>(),
                BuffEndCondition = Kingmaker.UnitLogic.Buffs.BuffEndCondition.RemainAfterCombat,
                Permanent = true,
                Ranks = ALL_TRAUMAS.Length,
            };
        }
    }
}
