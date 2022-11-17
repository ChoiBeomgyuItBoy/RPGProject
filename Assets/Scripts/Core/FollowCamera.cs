using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private Vector3 targetDistance;

        private void Start()
        {
            targetDistance = transform.position - target.position;
        }

        private void LateUpdate()
        {
            transform.position = target.position + targetDistance;
        }
    }
}

