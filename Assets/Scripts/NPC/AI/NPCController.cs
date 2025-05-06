using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : Singleton<NPCController>
{
    public PrisonCell myCell;
    [HideInInspector] public NavMeshAgent agent;
    public NPCState currentState;
    public Animator animator;

    public bool hasRandomStateSet = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    public void SetState(NPCState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(); // Mevcut durumdan çık
        }

        currentState = newState;
        currentState.Enter(); // Yeni duruma gir
    }

    private void Update()
    {

        if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.bedtimeState)
        {
            if (!hasRandomStateSet)
            {
                NPCState[] states = { new SleepingState(this), new LookingOutsideState(this), new ToiletState(this) };
                SetState(states[Random.Range(0, states.Length)]);
                hasRandomStateSet = true;
            }
            currentState?.Update();
        }
        if (animator != null)
        {
            animator.SetBool("Walking", agent.velocity.magnitude > 0.1f);
        }
    }

    public Transform GetBedPosition() => myCell.bedPosition;
    public Transform GetBarsPosition() => myCell.barsPosition;
    public Transform GetToiletPosition() => myCell.toiletPosition;

    public void MoveTo(Vector3 destination, System.Action onArrived)
    {
        agent.SetDestination(destination);
        StartCoroutine(WaitUntilArrived(onArrived));
    }

    private IEnumerator WaitUntilArrived(System.Action onArrived)
    {
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        if (animator != null)
        {
            animator.SetBool("Walking", false);
        }

        onArrived?.Invoke();
    }

    public void StartState()
    {
        if (DailyRoutineManager.Instance.currentState != DailyRoutineManager.Instance.bedtimeState)
        {
            hasRandomStateSet = false;
            SetState(new IdleState(this));
        }

    }
}
