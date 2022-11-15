using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover))]
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float weaponRange = 2f;

        private Mover mover;
        private Transform target;

        bool IsInRange => Vector3.Distance(transform.position, target.position) < weaponRange;

        private void Start()
        {
            mover = GetComponent<Mover>();
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
                mover.Stop();
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}