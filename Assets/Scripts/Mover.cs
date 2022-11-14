using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(inputReader.IsClicking) { MoveToCursor(); }
    }

    private void MoveToCursor()
    {
        Ray ray = GetCameraRay();
        RaycastHit hit = GetRaycastHit(ray);

        if(hit.transform != null)
        {
            agent.destination = hit.point;
        }
    }

    private RaycastHit GetRaycastHit(Ray ray)
    {
        RaycastHit hit;

        Physics.Raycast(ray, out hit);

        return hit;
    }

    private Ray GetCameraRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
