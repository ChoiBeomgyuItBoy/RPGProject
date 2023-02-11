using System;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Effects/Spawn Projectile Effect")]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] Projectile projectilePrefab;
        [SerializeField] float damage = 10;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] bool useTargetPoint = true;


        public override void StartEffect(AbilityData data, Action finished)
        {
            Fighter fighter = data.GetUser().GetComponent<Fighter>();
            Vector3 spawnPosition = fighter.GetHandTransform(isRightHanded).position;

            if(useTargetPoint)
            {
                SpawnProjectileForTargetPoint(data, spawnPosition);
            }
            else
            {
                SpawnProjectilesForTargets(data, spawnPosition);
            }

            finished();
        }

        private void SpawnProjectileForTargetPoint(AbilityData data, Vector3 spawnPosition)
        {
            Projectile projectile = Instantiate(projectilePrefab);
            projectile.transform.position = spawnPosition;
            projectile.SetProjectileInfo(data.GetTargetedPoint(), data.GetUser(), damage);
        }

        private void SpawnProjectilesForTargets(AbilityData data, Vector3 spawnPosition)
        {
            foreach (var target in data.GetTargets())
            {
                Health targetHealth = target.GetComponent<Health>();

                if (targetHealth != null)
                {
                    Projectile projectile = Instantiate(projectilePrefab);
                    projectile.transform.position = spawnPosition;
                    projectile.SetProjectileInfo(targetHealth, data.GetUser(), damage);
                }
            }
        }
    }
}
