using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float health = 100f;

        private readonly int DeadHash = Animator.StringToHash("die");

        public bool IsDead => health == 0;

        public void TakeDamage(float damage)
        {
            if(IsDead) return;

            health = Mathf.Max(0f, health - damage);

            if(IsDead) Die();
        }

        private void Die()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();

            GetComponent<Animator>().SetTrigger(DeadHash);
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            float currentHealth = (float) state;

            health = currentHealth;

            if(IsDead) Die();
        }
    }
}
