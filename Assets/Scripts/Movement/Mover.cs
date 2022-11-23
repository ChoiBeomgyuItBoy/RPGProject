using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private float maxSpeed = 6f;

        private readonly int ForwardSpeedHash = Animator.StringToHash("forwardSpeed");

        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        
        private void Update()
        {
            agent.enabled = !GetComponent<Health>().IsDead;

            GetComponent<Animator>().SetFloat(ForwardSpeedHash, GetLocalSpeed());
        }

        private void DisableAgent()
        {
            agent.enabled = !GetComponent<Health>().IsDead;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);

            MoveTo(destination, speedFraction);
        }

        private void MoveTo(Vector3 destination, float speedFraction)
        {
            agent.destination = destination;
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
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

