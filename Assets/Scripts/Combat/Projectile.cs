using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        private Health target = null;
        private CapsuleCollider targetCapsule;

        private float damage = 0f;

        private void Start()
        {
            targetCapsule = target.GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            ProjectileBehaviour();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Health>() != target) return;
            
            target.TakeDamage(damage);
            Destroy(gameObject);
        }

        public void SetProjectileInfo(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private void ProjectileBehaviour()
        {
            transform.LookAt(GetAimLocation());
            transform.Translate(CalculateMovement());
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