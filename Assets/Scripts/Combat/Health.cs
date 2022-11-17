using System;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        private bool IsDead => health <= 0;

        public void TakeDamage(float damage)
        {
            if(IsDead) return;

            health = Mathf.Max(0f, health - damage);

            print(health);
        }
    }
}
