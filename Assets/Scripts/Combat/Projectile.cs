using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        [Tooltip("If projectile will always follow its target")]
        [SerializeField] private bool isHoming = false;

        [SerializeField] private GameObject hitEffect = null;

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

            if(hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            Destroy(gameObject);
        }

        public void SetProjectileInfo(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
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