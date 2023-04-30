using System;
using UnityEngine;
using RPG.Control;
using RPG.Movement;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Effects/(AI) Teleport Effect")]
    public class TeleportEffect : EffectStrategy
    {
        public override void StartEffect(AbilityData data, Action finished)
        {
            Vector3 currentWaypoint = data.GetUser().GetComponent<AIController>().GetCurrentWaypoint();
            data.GetUser().GetComponent<Mover>().Teleport(currentWaypoint);
            data.GetUser().GetComponent<AIController>().CycleWaypoint();
            finished();
        }
    }
}