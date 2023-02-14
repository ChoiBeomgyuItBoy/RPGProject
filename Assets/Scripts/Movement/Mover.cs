using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using GameDevTV.Saving;
using RPG.Attributes;
using System;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 40f;

        private readonly int ForwardSpeedHash = Animator.StringToHash("forwardSpeed");

        private NavMeshAgent agent;
        private Health health;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        
        private void Update()
        {
            agent.enabled = !GetComponent<Health>().IsDead;

            GetComponent<Animator>().SetFloat(ForwardSpeedHash, GetLocalSpeed());
        }

        [System.Serializable]
        private struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        private void DisableAgent()
        {
            agent.enabled = !health.IsDead;
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

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if(!hasPath) return false;

            if(path.status != NavMeshPathStatus.PathComplete) return false;

            if(GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0f;
            float length = path.corners.Length;

            if(length < 2f) return total;

            for (int i = 0; i < length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
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

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();

            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);

            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;

            agent.enabled = false;

            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();

            agent.enabled = true;

            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public void Teleport(Vector3 destination)
        {
            agent.Warp(destination);
        }
    }
}

