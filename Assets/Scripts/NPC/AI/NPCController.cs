using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public PrisonCell myCell;
    private NavMeshAgent agent;
    private NPCState currentState;

    public bool hasRandomStateSet = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetState(NPCState newState)
    {
        currentState = newState;
        currentState.Enter();
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
        else
        {
            hasRandomStateSet = false;
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
        onArrived?.Invoke();
    }
}
