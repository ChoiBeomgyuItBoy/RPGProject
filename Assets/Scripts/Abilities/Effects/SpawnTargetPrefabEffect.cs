using System;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Effects/Spawn Target Prefab Effect")]
    public class SpawnTargetPrefabEffect : EffectStrategy
    {
        [SerializeField] GameObject effectPrefab;
        [SerializeField] float destroyDelay  = 1f;

        public override void StartEffect(AbilityData data, Action finished)
        {
            GameObject effectInstance = Instantiate(effectPrefab, data.GetTargetedPoint(), Quaternion.identity);

            Destroy(effectInstance, destroyDelay);

            finished();
        }
    }
}
