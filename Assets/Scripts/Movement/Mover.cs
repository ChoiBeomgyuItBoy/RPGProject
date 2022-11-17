using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using System;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent), typeof(ActionScheduler))]
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent agent;

        public event Action<float> OnLocomotion;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        
        private void Update()
        {
            OnLocomotion?.Invoke(GetLocalSpeed());
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);

            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
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

