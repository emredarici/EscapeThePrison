using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PatrolPolice : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;


    [Tooltip("Yapay zekanın devriye gezerkenki hızı")]
    public float patrolSpeed = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    public Transform currentTarget;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", 0.5f);
        currentTarget = pointA;
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) < 1f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }

        agent.SetDestination(currentTarget.position);
    }
}
