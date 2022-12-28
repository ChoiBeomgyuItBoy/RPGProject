using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float speed = 1f;

        [Tooltip("If projectile will always follow its target")]
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private float lifeAfterHit = 0.5f;
        [SerializeField] private bool isHoming = false;

        private Health target = null;
        private GameObject instigator = null;

        private float damage = 0f;

        [SerializeField] private UnityEvent onHit;

        private void Start()
        {
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

            target.TakeDamage(instigator, damage);

            speed = 0f;

            onHit.Invoke();

            if(hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            Destroy(gameObject, lifeAfterHit);
        }

        public void SetProjectileInfo(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.instigator = instigator;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            return target.transform.position + Vector3.up * GetTargetCenter();
        }

        private float GetTargetCenter()
        {
            return (target.GetComponent<CapsuleCollider>().height / 2);
        }

        private Vector3 CalculateMovement()
        {
            return Vector3.forward * speed * Time.deltaTime;
        }
    }
}