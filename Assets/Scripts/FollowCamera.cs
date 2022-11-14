using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 difference;

    private void Start()
    {
        difference = transform.position - target.position;
    }

    private void LateUpdate()
    {
        transform.position = target.position + difference;
    }
}
