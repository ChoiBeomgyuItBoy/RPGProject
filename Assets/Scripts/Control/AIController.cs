using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;
using System;

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

        Fighter fighter;
        Mover mover;
        ActionScheduler scheduler;
        Health health;
        GameObject player;

        LazyValue<Vector3> guardPosition;
        Vector3 currentWaypoint;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;

        int currentWaypointIndex = 0;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");

            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();

            guardPosition = new LazyValue<Vector3>(GetInitialPosition);
        }

        private Vector3 GetInitialPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
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
                PatrolBehaviour();
            }
            
            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0f;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach(RaycastHit hit in hits)
            {
                if(!hit.transform.TryGetComponent<AIController>(out AIController controller)) continue;

                controller.Aggrevate();
            }
        }

        private void SuspicionBehaviour()
        {
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