using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float attackDamage = 10f;

        private readonly int AttackHash = Animator.StringToHash("attack");
        private readonly int CancelAttackHash = Animator.StringToHash("cancelAttack");

        private Health target;

        private float timeSinceLastAttack = Mathf.Infinity;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;
            if(target.IsDead) return;

            if(!IsInRange()) 
            {
                GetComponent<Mover>().StartMoveAction(target.transform.position,1f);
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

        // Animation Event
        private void Hit()
        {
            if(target == null) return;

            target.TakeDamage(attackDamage);
        }
    }
}