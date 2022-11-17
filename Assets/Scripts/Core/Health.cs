using System;
using UnityEngine;

namespace RPG.Core
{
    [RequireComponent(typeof(ActionScheduler))]
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        public bool IsDead => health == 0;

        public event Action OnDead;

        public void TakeDamage(float damage)
        {
            if(IsDead) return;

            health = Mathf.Max(0f, health - damage);

            if(IsDead) Die();
        }

        private void Die()
        {
            OnDead?.Invoke();

            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
