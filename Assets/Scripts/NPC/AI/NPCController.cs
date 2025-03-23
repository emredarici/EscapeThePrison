using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public PrisonCell myCell;
    private NavMeshAgent agent;
    private NPCState currentState;

    public bool HasSlept { get; set; }
    public bool HasLookedOutside { get; set; }
    public bool HasUsedToilet { get; set; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetState(new NPCState.IdleState(this));
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
            currentState?.Update();
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


    public bool HasWaited(float time)
    {
        return Time.time - currentState.StartTime >= time;
    }

    public void BedTimeDailyRoutine()
    {
        HasSlept = false;
        HasLookedOutside = false;
        HasUsedToilet = false;
    }
}
