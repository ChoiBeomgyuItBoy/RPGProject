using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover), typeof(ActionScheduler))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float attackDamage = 10f;

        private Health target;

        private float timeSinceLastAttack = 0;

        public event Action OnAttack;
        public event Action OnCancel;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;
            if(target.IsDead) return;

            if(!IsInRange()) 
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else 
            {
                GetComponent<Mover>().Cancel();  
                AttackBehaviour();
            }
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        } 

        public bool CanAttack(CombatTarget combatTarget)
        {
            if(combatTarget == null) return false;

            Health currentTarget = combatTarget.GetComponent<Health>();

            return currentTarget != null && !currentTarget.IsDead;
        }

        private void AttackBehaviour()
        {
            FaceTarget();

            if(timeSinceLastAttack < timeBetweenAttacks) return;

            OnAttack?.Invoke();

            timeSinceLastAttack = 0f;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);

            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            OnCancel?.Invoke();

            target = null; 
        }

        private void FaceTarget()
        {
            Vector3 lookPosition = target.transform.position - transform.position;

            lookPosition.y = 0f;

            transform.rotation = Quaternion.LookRotation(lookPosition);
        }

        // Animation Event
        private void Hit()
        {
            if(target == null) return;

            target.TakeDamage(attackDamage);
        }
    }
}