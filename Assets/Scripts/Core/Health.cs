using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
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
    }
}
