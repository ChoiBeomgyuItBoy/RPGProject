using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;
using System;
using UnityEngine.Events;
using GameDevTV.Inventories;
using RPG.Abilities;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroCooldownTime = 4f;
        [SerializeField] float waypointTolerance = 2f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] [Range(0f,1f)] float patrolSpeedFraction = 0.2f;
        [SerializeField] Ability[] abilities;
        [SerializeField] public UnityEvent onAggrevated;
        [SerializeField] public UnityEvent onPacified;

        Fighter fighter;
        Mover mover;
        ActionScheduler scheduler;
        Health health;
        ActionStore actionStore;
        GameObject player;
        LazyValue<Vector3> guardPosition;
        Vector3 currentWaypoint;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;

        int currentWaypointIndex = 0;

        public void Aggrevate()
        {
            onAggrevated?.Invoke();
            timeSinceAggrevated = 0f;
        }

        public void Reset()
        {
            mover.Teleport(guardPosition.value);
            ResetState();
        }

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");

            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
            actionStore = GetComponent<ActionStore>();
            guardPosition = new LazyValue<Vector3>(() => transform.position);
            guardPosition.ForceInit();
        }

        private void Start()
        {
            if(actionStore == null) return;

            for (int i = 0; i < abilities.Length; i++)
            {
                actionStore.AddAction(abilities[i], i, 1);
            }
        }

        private void Update()
        {
            if (health.IsDead) return;

            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                onPacified?.Invoke();
                PatrolBehaviour();
            }
            
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void ResetState()
        {
            timeSinceLastSawPlayer = Mathf.Infinity;
            timeSinceArrivedWaypoint = Mathf.Infinity;
            timeSinceAggrevated = Mathf.Infinity;
            currentWaypointIndex = 0;
        }

        private void AbilitiesBehaviour()
        {
            actionStore.Use(0, gameObject);
        }

        private void AttackBehaviour()
        {
            if(!GetComponent<Fighter>().enabled) return;

            if(actionStore != null && actionStore.Use(0, gameObject)) 
            {
                onAggrevated?.Invoke();
            }
            else
            {
                fighter.Attack(player);
            }
            
            timeSinceLastSawPlayer = 0f;
            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach(RaycastHit hit in hits)
            {
                AIController controller = hit.transform.GetComponent<AIController>();

                if(controller != null && !controller.GetComponent<Health>().IsDead)
                {
                    controller.Aggrevate();
                }
            }
        }

        private void SuspicionBehaviour()
        {
            if(!GetComponent<Fighter>().enabled) return;
            scheduler.CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if(patrolPath != null)
            {
                if(AtWaypoint())
                {
                    timeSinceArrivedWaypoint = 0f;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if(timeSinceArrivedWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }
        
        private bool AtWaypoint()
        {
            return IsInRange(transform.position, GetCurrentWaypoint(), waypointTolerance);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool IsAggrevated()
        {
            return IsInRange(transform.position, player.transform.position, chaseDistance) || timeSinceAggrevated < aggroCooldownTime;
        }

        private bool IsInRange(Vector3 from, Vector3 to, float range)
        {
            float targetDistanceSqr  = (from - to).sqrMagnitude;
            float rangeSqr = range * range;

            return targetDistanceSqr < rangeSqr;
        }

        // Called in Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}