using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter), typeof(Mover), typeof(ActionScheduler))]
    [RequireComponent(typeof(Health))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private float waypointTolerance = 2f;

        private GameObject player;

        private Fighter fighter;
        private Mover mover;
        private ActionScheduler scheduler;
        private Health health;

        private Vector3 guardPosition;
        private Vector3 currentWaypoint;

        private float timeSinceLastSawPlayer = Mathf.Infinity;

        private int currentWaypointIndex = 0;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            if(health.IsDead) return;

            if(IsInChaseRange() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if(timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else 
            {
                PatrolBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private void SuspicionBehaviour()
        {
            scheduler.CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if(patrolPath != null)
            {
                if(AtWaypoint())
                {
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            mover.StartMoveAction(nextPosition);
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

        private bool IsInChaseRange()
        {
            return IsInRange(transform.position, player.transform.position, chaseDistance);
        }

        private bool IsInRange(Vector3 init, Vector3 target, float range)
        {
            float targetDistanceSqr  = (init - target).sqrMagnitude;
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