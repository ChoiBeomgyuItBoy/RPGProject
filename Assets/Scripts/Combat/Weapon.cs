using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons / Create New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedWeaponPrefab = null;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 10f;

        public void Spawn(Transform handTransform, Animator animator)
        {   
            if(equippedWeaponPrefab != null)
            {
                Instantiate(equippedWeaponPrefab, handTransform);
            }

            if(animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public float GetRange() => weaponRange;
        public float GetDamage() => weaponDamage;
    }
}
