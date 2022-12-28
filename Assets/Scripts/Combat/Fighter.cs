using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";

        private readonly int AttackHash = Animator.StringToHash("attack");
        private readonly int CancelAttackHash = Animator.StringToHash("cancelAttack");

        private Health target;
        LazyValue<Weapon> currentWeapon;

        private float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(SetDefaultWeapon);
        }

        private Weapon SetDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;
            if(target.IsDead) return;

            if(!IsInRange()) 
            {
                GetComponent<Mover>().StartMoveAction(target.transform.position, 1f);
            }
            else 
            {
                GetComponent<Mover>().Cancel();  
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(currentWeapon.value);
        }

        private void AttachWeapon(Weapon weapon)
        {
            weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetRange();
        } 

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) return false;

            Health currentTarget = combatTarget.GetComponent<Health>();

            return currentTarget != null && !currentTarget.IsDead;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);

            target = combatTarget.GetComponent<Health>();
        }

        private void AttackBehaviour()
        {
            FaceTarget();

            if(timeSinceLastAttack < timeBetweenAttacks) return;

            timeSinceLastAttack = 0f;

            GetComponent<Animator>().SetTrigger(AttackHash);
            GetComponent<Animator>().ResetTrigger(CancelAttackHash);
        }

        public void Cancel()
        {
            target = null; 

            GetComponent<Mover>().Cancel();

            GetComponent<Animator>().ResetTrigger(AttackHash);
            GetComponent<Animator>().SetTrigger(CancelAttackHash);
        }

        private void FaceTarget()
        {
            Vector3 lookPosition = target.transform.position - transform.position;

            lookPosition.y = 0f;

            transform.rotation = Quaternion.LookRotation(lookPosition);
        }

        public Health GetTarget()
        {
            return target;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }
        
        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string loadedWeaponName = (string) state;
            Weapon loadedWeapon = Resources.Load<Weapon>(loadedWeaponName);

            EquipWeapon(loadedWeapon);
        }

        // Animation Event
        private void Hit()
        {
            if(target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if(currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LaunchProjectile(gameObject, rightHandTransform, leftHandTransform, target, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }
    }
}