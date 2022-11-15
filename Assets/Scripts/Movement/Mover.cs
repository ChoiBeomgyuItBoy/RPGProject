using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Fighter fighter;
        
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();
        }

        public void StartMoveAction(Vector3 destination)
        {
            fighter.Cancel();

            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
            agent.isStopped = false;
        }

        public void Stop()
        {
            agent.isStopped = true;
        }
    }
}

