using System;
using GameDevTV.Inventories;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float speed = 1f;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] float lifeAfterHit = 0.5f;
        [SerializeField] bool isHoming = false;
        [SerializeField] string displayName = "";
        Health target = null;
        Vector3 targetPoint;
        GameObject instigator = null;
        float damage = 0f;
        [SerializeField] UnityEvent onHit;
        [SerializeField] InventoryItem projectileItem;
        [SerializeField] int itemsToConsume = 1;

        public string GetDisplayName()
        {
            return displayName;
        }

        public bool SetProjectileInfo(Health target, GameObject instigator, float damage)
        {
            return SetProjectileInfo(instigator, damage, target);
        }

        public bool SetProjectileInfo(Vector3 targetPoint, GameObject instigator, float damage)
        {
            return SetProjectileInfo(instigator, damage, null, targetPoint);
        }

        public bool SetProjectileInfo(GameObject instigator, float damage, Health target=null, Vector3 targetPoint=default)
        {
            Inventory inventory = instigator.GetComponent<Inventory>();

            if(inventory != null)
            {
                if(projectileItem != null)
                {
                    if(!inventory.HasItem(projectileItem)) 
                    {
                        Destroy(gameObject);
                        return false;
                    }

                    int slot = inventory.FindSlot(projectileItem);
                    int number = inventory.GetNumberInSlot(slot);

                    if(number < itemsToConsume)
                    {
                        Destroy(gameObject);
                        return false;
                    }

                    inventory.RemoveFromSlot(slot, itemsToConsume);
                }
            }

            this.target = target;
            this.targetPoint = targetPoint;
            this.instigator = instigator;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);

            return true;
        }

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