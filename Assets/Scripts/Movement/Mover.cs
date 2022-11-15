using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent), typeof(ActionScheduler))]
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent agent;
        private ActionScheduler scheduler;
        
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            scheduler = GetComponent<ActionScheduler>();
        }

        public void StartMoveAction(Vector3 destination)
        {
            scheduler.StartAction(this);

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
    }
}

