using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DailyRoutineManager : Singleton<DailyRoutineManager>
{
    public DayManager dayManager;
    public DailyRoutineBaseState currentState;
    public HeadcountState headcountState = new HeadcountState();
    public ChowtimeState chowtimeState = new ChowtimeState();
    public RectimeState rectimeState = new RectimeState();
    public BedtimeState bedtimeState = new BedtimeState();

    public List<NavMeshAgent> npcs;
    public Transform grabFoodPosition;
    public Transform headcountPosition;
    public List<Transform> exitPosition;
    public GameObject cellDoors;
    public GameObject mainDoor;
    public GameObject lockPosition;
    private float stepDistance = 2f;

    [HideInInspector] public bool isMoving = false;

    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        dayManager = FindObjectOfType<DayManager>();
    }
    void Start()
    {
        currentState = bedtimeState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(DailyRoutineBaseState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        NPCController.Instance.StartState();
        currentState.EnterState(this);
        DebugToolKit.Log("Switched to " + currentState);
    }
    public void ArrangeQueue()
    {
        PopulateNpcList();
        CafetariaTableList();
        StartCoroutine(ArrangeQueueCoroutine());
    }

    private IEnumerator ArrangeQueueCoroutine()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            Vector3 newPos = grabFoodPosition.position - grabFoodPosition.forward * (i * stepDistance);
            npcs[i].SetDestination(newPos);
        }

        float waitTime = 15f;
        float elapsedTime = 0f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        DebugToolKit.Log("Yemek hazır! NPC'ler sırayla hareket ediyor.");
        StartCoroutine(MoveQueue());
    }

    private IEnumerator MoveQueue()
    {
        DebugToolKit.Log("MoveQueue started.");
        while (npcs.Count > 0)
        {
            if (!isMoving)
            {
                isMoving = true;

                NavMeshAgent firstNpc = npcs[0];

                if (Mathf.Abs(firstNpc.transform.position.x - grabFoodPosition.position.x) <= 0.1f)
                {
                    Transform currentExitPosition = exitPosition[0];
                    firstNpc.SetDestination(currentExitPosition.position);

                    DebugToolKit.Log("NPC reached exit position.");

                    npcs.RemoveAt(0);
                    exitPosition.RemoveAt(0);
                    for (int i = 0; i < npcs.Count; i++)
                    {
                        Vector3 newPos = grabFoodPosition.position - grabFoodPosition.forward * (i * stepDistance);
                        npcs[i].SetDestination(newPos);
                    }
                    yield return new WaitForSeconds(5f);

                }

                isMoving = false;
            }

            yield return null;
        }
        VFXManager.Instance.SpawnLocationMarker(grabFoodPosition.transform.position);
        UIManager.Instance.ChangeText(UIManager.Instance.informationText, "Your food's ready, grab it and sit at the table.");

        DebugToolKit.Log("MoveQueue completed.");

    }

    public Transform PlayerRandomTablePosition()
    {
        int randomIndex = Random.Range(0, exitPosition.Count);
        Transform randomTable = exitPosition[randomIndex];
        exitPosition.RemoveAt(randomIndex);
        return randomTable;
    }


    public void NpcHeadCount()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            Vector3 newPos = headcountPosition.position - (headcountPosition.forward * -1) * (i * stepDistance);
            npcs[i].SetDestination(newPos);
        }
        VFXManager.Instance.SpawnLocationMarker(headcountPosition.position - (headcountPosition.forward * -1) * (npcs.Count + 1 * stepDistance));
        UIManager.Instance.ChangeText(UIManager.Instance.informationText, "Counting is starting, please proceed to the designated area!");
    }

    public void PlayerHeadCount()
    {
        UIManager.Instance.ChangeText(UIManager.Instance.informationText, "Counting is in progress, please stand by.");
        StartCoroutine(PlayerHeadCountCoroutine());
    }

    private IEnumerator PlayerHeadCountCoroutine()
    {
        yield return new WaitForSeconds(5f);
        UIManager.Instance.ChangeText(UIManager.Instance.informationText, $"Attendance confirmed. Headcount: {npcs.Count + 1}.");
        DebugToolKit.Log("Player headcount confirmed.");
        yield return new WaitForSeconds(3f);
        this.SwitchState(rectimeState);
    }

    public IEnumerator CountdownSwitchState(float time, DailyRoutineBaseState nextState)
    {
        yield return new WaitForSeconds(time);
        this.SwitchState(nextState);
    }

    public void PopulateNpcList()
    {
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");

        npcs.Clear();

        foreach (GameObject npcObject in npcObjects)
        {
            NavMeshAgent agent = npcObject.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                npcs.Add(agent);
            }
        }
    }

    public void CafetariaTableList()
    {
        GameObject[] tableObjects = GameObject.FindGameObjectsWithTag("CafetariaTable");

        exitPosition.Clear();

        foreach (GameObject tableObject in tableObjects)
        {
            Transform tableTransform = tableObject.transform;
            if (tableTransform != null)
            {
                exitPosition.Add(tableTransform);
            }
        }

    }

    public void OpenAllCellDoors()
    {
        mainDoor.transform.position = new Vector3(-11.86f, 1.77f, 10.033f);
        foreach (Transform child in cellDoors.transform)
        {
            child.localRotation *= Quaternion.Euler(child.transform.rotation.x, child.transform.rotation.y, 150f);
        }
    }

    public void CloseAllCellDoors()
    {
        lockPosition.SetActive(false);
        mainDoor.transform.position = new Vector3(-10.145f, 1.77f, 10.033f);
        foreach (Transform child in cellDoors.transform)
        {
            child.localRotation *= Quaternion.Euler(child.transform.rotation.x, child.transform.rotation.y, -150f);
        }
    }
}
