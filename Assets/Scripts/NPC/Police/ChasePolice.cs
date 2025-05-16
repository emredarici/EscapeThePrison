using UnityEngine;
using UnityEngine.AI;

public class ChasePolice : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Player Detection")]
    public Transform playerTransform;
    [Tooltip("Yapay zekanın görüş açısı (derece cinsinden)")]
    public float fovAngle = 45f;
    [Tooltip("Yapay zekanın görüş mesafesi")]
    public float viewDistance = 10f;

    [Header("Movement Parameters")]
    [Tooltip("Yapay zekanın takip ederkenki hızı")]
    public float chaseSpeed = 5f;
    [Tooltip("Yapay zekanın devriye gezerkenki hızı")]
    public float patrolSpeed = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform currentTarget;
    private AIState currentState;

    private enum AIState
    {
        Patrol,
        Chase
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentTarget = pointA;
        currentState = AIState.Patrol;
    }

    private void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                PatrolState();
                break;

            case AIState.Chase:
                ChaseState();
                break;
        }
    }

    private void PatrolState()
    {
        if (CanSeePlayer())
        {
            StartChasing();
            currentState = AIState.Chase;
            return;
        }
        animator.SetFloat("Speed", 0.5f);
        Patrol();
    }

    private void ChaseState()
    {
        if (!CanSeePlayer())
        {
            StopChasing();
            currentState = AIState.Patrol;
            return;
        }

        animator.SetFloat("Speed", 1f);
        ChasePlayer();
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > viewDistance)
            return false;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > fovAngle / 2f)
            return false;

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, viewDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void StartChasing()
    {
        agent.speed = chaseSpeed;
    }

    private void StopChasing()
    {
        agent.speed = patrolSpeed;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
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
