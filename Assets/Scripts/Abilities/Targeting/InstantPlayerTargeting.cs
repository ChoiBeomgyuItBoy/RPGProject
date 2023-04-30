using System;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Targeting/Instant Player Targeting")]
    public class InstantPlayerTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            GameObject player = GameObject.FindWithTag("Player");
            data.SetTargets(new GameObject[] { player } );
            data.SetTargetedPoint(player.transform.position);
            finished();
        }
    }
}
