using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(menuName = "RPG/Abilities/New Ability")]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
        [SerializeField] float coolDownTime = 0;

        public override void Use(GameObject user)
        {
            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();

            if(cooldownStore.GetTimeRemaining(this) > 0) return;

            AbilityData data = new AbilityData(user);

            targetingStrategy.StartTargeting(data, () => TargetAquired(data));
        }

        private void TargetAquired(AbilityData data)
        {
            CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();

            cooldownStore.StartCooldown(this, coolDownTime);

            foreach(var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }

            foreach (var effectStrategy in effectStrategies)
            {
                effectStrategy.StartEffect(data, EffectFinished);
            }
        }

        private void EffectFinished()
        {

        }
    }   
}
