using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Interfaces;
using Kingmaker.PubSubSystem;
using Kingmaker.PubSubSystem.Core;
using Kingmaker.UnitLogic.Abilities.Blueprints;

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
            var logger = Main.ModEntry.Logger;
            BlueprintAbility crippleSelf = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>(AssetID.Ability.MOD_CRIPPLE_SELF);
            if (null == crippleSelf)
            {
                logger.Critical($"Ability NOT found: CrippleSelf[{AssetID.Ability.MOD_CRIPPLE_SELF}]");
                return;
            }

            if (null != unit && !unit.IsPet && !unit.Facts.Contains(crippleSelf))
            {
                logger.Warning($"Add ability for unit {unit.Name}");
                unit.AddFact(crippleSelf, null);
            }

            return;
        }

    }
}
