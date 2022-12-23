using RPG.Attributes;
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

        private const string weaponName = "Weapon";
        
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
        
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {   
            DestroyOldWeapon(rightHand, leftHand);

            if(equippedWeaponPrefab != null)
            {
                Transform handTransform = GetHandTransfrom(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedWeaponPrefab, handTransform);

                weapon.name = weaponName;
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
        }

        private AnimatorOverrideController GetDefaultController(Animator animator)
        {
            return animator.runtimeAnimatorController as AnimatorOverrideController;
        }

        public void LaunchProjectile(GameObject instigator, Transform rightHand, Transform leftHand, Health target)
        {
            Transform handTransform = GetHandTransfrom(rightHand, leftHand);
            Projectile projectileInstance = Instantiate(projectile, handTransform.position, Quaternion.identity);

            projectileInstance.SetProjectileInfo(target, instigator, weaponDamage);
        }
    }
}
