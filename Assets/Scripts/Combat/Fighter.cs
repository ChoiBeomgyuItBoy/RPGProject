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

        private Transform target;

        private float timeSinceLastAttack = 0;

        public event Action OnAttack;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;

            if(!GetIsInRange()) 
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else 
            {
                GetComponent<Mover>().Cancel();  
                AttackBehaviour();
            }
        }

        private bool GetIsInRange() => Vector3.Distance(transform.position, target.position) < weaponRange;

        private void AttackBehaviour()
        {
            if(timeSinceLastAttack < timeBetweenAttacks) return;

            OnAttack?.Invoke();

            timeSinceLastAttack = 0f;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);

            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null; 
        }

        // Animation Event
        private void Hit()
        {
            if(target == null) return;

            if(target.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(attackDamage);
            }
        }
    }
}