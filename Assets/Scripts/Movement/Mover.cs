using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using System;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent), typeof(ActionScheduler), typeof(Health))]
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent agent;
        private Health health;

        public event Action<float> OnLocomotion;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        
        private void Update()
        {
            agent.enabled = !health.IsDead;

            OnLocomotion?.Invoke(GetLocalSpeed());
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);

            MoveTo(destination);
        }

        private void MoveTo(Vector3 destination)
        {
            agent.destination = destination;

            agent.isStopped = false;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        private float GetLocalSpeed()
        {
            Vector3 globalVelocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(globalVelocity);

            return localVelocity.z;
        }
    }
}

