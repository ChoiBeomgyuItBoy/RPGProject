using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover), typeof(ActionScheduler))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;

        private Mover mover;
        private ActionScheduler scheduler;
        private Transform target;

        bool IsInRange => Vector3.Distance(transform.position, target.position) < weaponRange;

        private void Start()
        {
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            if(target == null) return;

            if(!IsInRange) 
            {
                mover.MoveTo(target.position);
            }
            else 
            {
                mover.Cancel();
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            scheduler.StartAction(this);

            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}