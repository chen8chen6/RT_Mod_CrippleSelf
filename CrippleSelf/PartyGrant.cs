using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Interfaces;
using Kingmaker.PubSubSystem;
using Kingmaker.PubSubSystem.Core;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Progression.Features;

namespace CrippleSelf
{
    internal sealed class PartyGrant : IAreaHandler, /*ISubscriber,*/ ICompanionChangeHandler, ISubscriber<IBaseUnitEntity>, IPartyHandler
    {
        public void OnAreaDidLoad()
        {
            foreach (BaseUnitEntity unit in Game.Instance.Player.AllCharacters)
            {
                PartyGrant.Grant(unit);
            }
        }

        public void OnAreaBeginUnloading()
        {
        }

        public void HandleRecruit()
        {
            PartyGrant.Grant(EventInvokerExtensions.BaseUnitEntity);
        }

        public void HandleUnrecruit()
        {
        }

        public void HandleAddCompanion()
        {
            PartyGrant.Grant(EventInvokerExtensions.BaseUnitEntity);
        }

        public void HandleCompanionActivated()
        {
            PartyGrant.Grant(EventInvokerExtensions.BaseUnitEntity);
        }

        public void HandleCompanionRemoved(bool stayInGame)
        {
        }

        public void HandleCapitalModeChanged()
        {
        }

        private static void Grant(BaseUnitEntity unit)
        {
            BlueprintAbility crippleSelf = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>(AssetID.Ability.MOD_CRIPPLE_SELF);
            if (null == crippleSelf)
            {
                Logger.Error($"Ability NOT found: CrippleSelf[{AssetID.Ability.MOD_CRIPPLE_SELF}]");
                return;
            }

            bool isSuitable = IsSuitable(unit);
            if (isSuitable && !unit.Facts.Contains(crippleSelf))
            {
                Logger.Warning($"Add ability for unit {unit.Name}");
                unit.AddFact(crippleSelf, null);
            }
#if false
            else if (!isSuitable && unit.Facts.Contains(crippleSelf))
            {
                Logger.Warning($"Remove ability for unit {unit.Name}");
                unit.Facts.Remove(crippleSelf);
            }
#endif
            return;
        }

        private static bool IsSuitable(BaseUnitEntity unit)
        {
            do
            {
                //Check unit available
                if (null == unit || unit.IsPet)
                    break;

                //Check has trauma-relative feats
                BlueprintFeature facingTheEnd = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(AssetID.Feature.FACING_THE_END);
                BlueprintFeature woundedBeast = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(AssetID.Feature.WOUNDED_BEAST);
                if (null == facingTheEnd || null == woundedBeast)
                {
                    Logger.Error($"Feature found: FacingTheEnd[{AssetID.Feature.FACING_THE_END}] or WoundedBeast[{AssetID.Feature.WOUNDED_BEAST}]");
                    break;
                }

                return (unit.Facts.Contains(facingTheEnd)
                    || unit.Facts.Contains(woundedBeast));

            } while (false);

            return false;
        }
    }
}
