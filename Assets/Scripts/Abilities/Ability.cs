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

        public override void Use(GameObject user)
        {
            AbilityData data = new AbilityData(user);

            targetingStrategy.StartTargeting(data, () => TargetAquired(data));
        }

        private void TargetAquired(AbilityData data)
        {
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
