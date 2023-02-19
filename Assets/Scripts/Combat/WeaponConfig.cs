using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(menuName = "RPG/Inventory/New Weapon")]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedWeaponPrefab = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 10f;
        [SerializeField] private float percentageBonus = 0f;
        [SerializeField] private bool isRightHanded = true;

        private const string weaponName = "Weapon";
        
        public bool HasProjectile() => projectile != null;
        public float GetRange() => weaponRange;
        public float GetDamage() => weaponDamage;
        public float GetPercentageBonus() => percentageBonus;

        private Transform GetHandTransfrom(Transform rightHand, Transform leftHand)
        {
            if(isRightHanded) 
            {
                return rightHand;
            }
            else 
            {
                return leftHand;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {   
            Transform oldWeapon = rightHand.Find(weaponName);

            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }

            if(oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
        
        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {   
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            if(equippedWeaponPrefab != null)
            {
                Transform handTransform = GetHandTransfrom(rightHand, leftHand);
                weapon = Instantiate(equippedWeaponPrefab, handTransform);

                weapon.gameObject.name = weaponName;
            }

            var defaultController = GetDefaultController(animator);

            if(animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if(defaultController != null)
            {
                animator.runtimeAnimatorController = defaultController.runtimeAnimatorController;
            }

            return weapon;
        }

        private AnimatorOverrideController GetDefaultController(Animator animator)
        {
            return animator.runtimeAnimatorController as AnimatorOverrideController;
        }

        public void LaunchProjectile(GameObject instigator, Transform rightHand, Transform leftHand, Health target, float calculatedDamage)
        {
            Transform handTransform = GetHandTransfrom(rightHand, leftHand);
            Projectile projectileInstance = Instantiate(projectile, handTransform.position, Quaternion.identity);

            projectileInstance.SetProjectileInfo(target, instigator, calculatedDamage);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return weaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return percentageBonus;
            }
        }
    }
}
