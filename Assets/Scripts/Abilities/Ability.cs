using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Core;
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
        [SerializeField] float manaCost = 0;

        public override void Use(GameObject user)
        {
            Mana mana = user.GetComponent<Mana>();

            if(mana.GetMana() < manaCost) 
            {
                return;
            }

            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();

            if(cooldownStore.GetTimeRemaining(this) > 0) 
            {
                return;
            }

            AbilityData data = new AbilityData(user);

            user.GetComponent<ActionScheduler>().StartAction(data);

            targetingStrategy.StartTargeting(data, () => TargetAquired(data));
        }

        private void TargetAquired(AbilityData data)
        {
            if(data.IsCancelled()) return;

            Mana mana = data.GetUser().GetComponent<Mana>();

            if(!mana.UseMana(manaCost)) 
            {
                return;
            }

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
