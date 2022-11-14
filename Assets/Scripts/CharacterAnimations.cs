using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class CharacterAnimations : MonoBehaviour
{
    private readonly int ForwardSpeedHash = Animator.StringToHash("ForwardSpeed");

    private Animator animator;
    private NavMeshAgent agent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetFloat(ForwardSpeedHash, GetLocalSpeed());
    }

    private float GetLocalSpeed()
    {
        Vector3 agentVelocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(agentVelocity);

        return localVelocity.z;
    }
}
