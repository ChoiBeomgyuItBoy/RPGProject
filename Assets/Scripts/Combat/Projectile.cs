using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float speed = 1f;

        [Tooltip("If projectile will always follow its target")]
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private float lifeAfterHit = 0.2f;
        [SerializeField] private bool isHoming = false;

        private Health target = null;
        private CapsuleCollider targetCapsule;

        private float damage = 0f;

        private void Start()
        {
            targetCapsule = target.GetComponent<CapsuleCollider>();

            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if(target == null) return;
            if(isHoming && !target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(CalculateMovement());
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Health>() != target) return;
            if(target.IsDead) return;

            target.TakeDamage(damage);

            speed = 0f;

            if(hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            Destroy(gameObject, lifeAfterHit);
        }

        public void SetProjectileInfo(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            return target.transform.position + Vector3.up * GetTargetCenter();
        }

        private float GetTargetCenter()
        {
            return (targetCapsule.height / 2);
        }

        private Vector3 CalculateMovement()
        {
            return Vector3.forward * speed * Time.deltaTime;
        }
    }
}