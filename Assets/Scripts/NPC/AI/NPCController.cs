using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public PrisonCell myCell;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform targetExitPosition;
    public NPCState currentState;
    public Animator animator;
    public AudioSource audioSource;

    public bool hasRandomStateSet = false;
    private GameObject npcTray;

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        npcTray = this.transform.GetChild(1).gameObject;
        StopCarryingTray();
    }

    public void SetState(NPCState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    public void OnPositionReset()
    {
        agent.Warp(myCell.barsPosition.transform.position);
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
    public Transform GetRectimePosition() => myCell.rectimePosition;

    public void MoveTo(Vector3 destination, System.Action onArrived)
    {
        agent.SetDestination(destination);
        StartCoroutine(WaitUntilArrived(onArrived));
    }

    private IEnumerator WaitUntilArrived(System.Action onArrived)
    {
        while (agent.enabled && agent.isOnNavMesh && (agent.pathPending || agent.remainingDistance > 0.1f))
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
        animator.SetBool("isSitting", false);
    }

    public void RectimeStateNPC()
    {
        MoveTo(this.myCell.rectimePosition.position, () =>
        {
            Debug.Log("NPC is at rectime position");
        });
    }

    public void CarriyngTray()
    {
        this.animator.SetBool("isTray", true);
        this.npcTray.SetActive(true);
    }

    public void StopCarryingTray()
    {
        this.animator.SetBool("isTray", false);
        this.npcTray.SetActive(false);
    }
}
