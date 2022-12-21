using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons / Create New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedWeaponPrefab = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 10f;
        [SerializeField] private bool isRightHanded = true;
        
        public bool HasProjectile() => projectile != null;
        public float GetRange() => weaponRange;
        public float GetDamage() => weaponDamage;

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
        
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {   
            if(equippedWeaponPrefab != null)
            {
                Transform handTransform = GetHandTransfrom(rightHand, leftHand);

                Instantiate(equippedWeaponPrefab, handTransform);
            }

            if(animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Transform handTransform = GetHandTransfrom(rightHand, leftHand);

            Projectile projectileInstance = Instantiate(projectile, handTransform.position, Quaternion.identity);

            projectileInstance.SetProjectileInfo(target, weaponDamage);
        }
    }
}
