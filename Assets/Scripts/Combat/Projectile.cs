using System;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float speed = 1f;

        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private float lifeAfterHit = 0.5f;
        
        [Tooltip("If projectile will always follow its target")]
        [SerializeField] private bool isHoming = false;

        private Health target = null;
        private Vector3 targetPoint;
        private GameObject instigator = null;

        private float damage = 0f;

        [SerializeField] private UnityEvent onHit;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if(target != null && isHoming && !target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(CalculateMovement());
        }

        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();

            if (target != null && health != target) return;
            if (health == null || health.IsDead) return;
            if (other.gameObject == instigator) return;

            health.TakeDamage(instigator, damage);

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
            SetProjectileInfo(instigator, damage, target);
        }

        public void SetProjectileInfo(Vector3 targetPoint, GameObject instigator, float damage)
        {
            SetProjectileInfo(instigator, damage, null, targetPoint);
        }

        public void SetProjectileInfo(GameObject instigator, float damage, Health target=null, Vector3 targetPoint=default)
        {
            this.target = target;
            this.targetPoint = targetPoint;
            this.instigator = instigator;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if(target == null)
            {
                return targetPoint;
            }

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