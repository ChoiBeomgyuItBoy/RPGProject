using System;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Effects/Health Effect")]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] float healthChange = 10;

        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach(var target in data.GetTargets())
            {
                Health health = target.GetComponent<Health>();

                if(health != null)
                {
                    if(healthChange < 0)
                    {
                        health.TakeDamage(data.GetUser(), -healthChange);
                    }
                    else
                    {
                        health.Heal(healthChange);
                    }
                }
            }

            finished();
        }

        public override IEnumerable<string> GetEffectInfo()
        {
            if(healthChange > 0)
            {
                yield return $"Restores {healthChange} HP";
            }
            else
            {
                yield return $"Reduces {-healthChange} HP on aquired targets";
            }
        }
    }
}
