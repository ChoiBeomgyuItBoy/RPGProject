using System;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Effects/Trigger Animation Effect")]
    public class TriggerAnimationEffect : EffectStrategy
    {
        [SerializeField] string animationTrigger = "";

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.GetUser().GetComponent<Animator>().SetTrigger(animationTrigger);
            finished();
        }
    }
}
