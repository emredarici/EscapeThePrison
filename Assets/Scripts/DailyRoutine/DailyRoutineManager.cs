using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DailyRoutineManager : Singleton<DailyRoutineManager>
{
    public DayManager dayManager;
    public DayTimeManager dayTimeManager;
    public DailyRoutineBaseState currentState;
    public HeadcountState headcountState = new HeadcountState();
    public ChowtimeState chowtimeState = new ChowtimeState();
    public RectimeState rectimeState = new RectimeState();
    public BedtimeState bedtimeState = new BedtimeState();

    public NPCController[] allNpcs => FindObjectsOfType<NPCController>();

    public List<NavMeshAgent> npcs;
    public Transform grabFoodPosition;
    public List<Transform> exitPosition;
    public GameObject cellDoors;
    public GameObject playerCellDoor;
    public GameObject mainDoor;
    public GameObject lockPosition;
    public GameObject policeRoomDoor;

    [Header("Headcount")]
    public GameObject headcountPolice;
    public Transform headcountPosition;

    [Header("MainNpcs")]
    public GameObject twoDayNPC;
    public Transform twoDayVFXPosition;
    public GameObject thirdDayNPC;
    public GameObject thirdDay2NPC;
    public GameObject fourDayNPC;
    public Transform fourDayVFXPosition;
    public Transform thirdDayVFXPosition;
    public Transform thirdDay2VFXPosition;
    [HideInInspector] public bool isThirdDayNPCDialouge = false;
    [HideInInspector] public bool isFourDayNPCDialouge = false;
    public GameObject policeRoomVFX;

    [Header("AudioSources")]
    public AudioSource rectimeSource;
    public AudioSource chowtimeSource;
    public AudioSource bedtimeSource;

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
        MinigameManager.Instance.RegisterMinigame("OpenDoor", DailyRoutineManager.Instance.gameObject.GetComponent<OpenDoorMG>());
        MinigameManager.Instance.RegisterMinigame("BrakeDoor", DailyRoutineManager.Instance.gameObject.GetComponent<BrakeDoorMG>());


    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(DailyRoutineBaseState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        foreach (var npc in allNpcs)
        {
            PopulateNpcList();
            npc.StartState();
        }
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
            Vector3 newPos = grabFoodPosition.position - grabFoodPosition.right * (i * stepDistance);
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
                    firstNpc.GetComponent<NPCController>().targetExitPosition = currentExitPosition;
                    firstNpc.SetDestination(currentExitPosition.position);

                    DebugToolKit.Log("NPC reached exit position.");

                    npcs.RemoveAt(0);
                    exitPosition.RemoveAt(0);
                    for (int i = 0; i < npcs.Count; i++)
                    {
                        Vector3 newPos = grabFoodPosition.position - grabFoodPosition.right * (i * stepDistance);
                        npcs[i].SetDestination(newPos);
                    }
                    yield return new WaitForSeconds(2f);

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
            Vector3 newPos = headcountPosition.position + Vector3.left * (i * stepDistance);
            npcs[i].SetDestination(newPos);
        }
        float markerX = headcountPosition.position.x - (npcs.Count * stepDistance);
        Vector3 markerPos = new Vector3(markerX, headcountPosition.position.y, headcountPosition.position.z);
        VFXManager.Instance.SpawnLocationMarker(markerPos);
        UIManager.Instance.ChangeText(UIManager.Instance.informationText, "Counting is starting, please proceed to the designated area!");
        StartCoroutine(ResetNpcRotationsWhenArrived());
    }

    private IEnumerator ResetNpcRotationsWhenArrived()
    {
        bool allArrived = false;
        while (!allArrived)
        {
            allArrived = true;
            foreach (var npc in npcs)
            {
                if (npc.enabled && npc.isOnNavMesh)
                {
                    if (npc.pathPending || npc.remainingDistance > 0.1f || npc.hasPath)
                    {
                        allArrived = false;
                        break;
                    }
                }
            }
            yield return null;
        }

        foreach (var npc in npcs)
        {
            npc.transform.rotation = Quaternion.identity;
        }
    }
    public void PlayerHeadCount()
    {
        UIManager.Instance.ChangeText(UIManager.Instance.informationText, "Counting is in progress, please stand by.");
        StartCoroutine(PlayerHeadCountCoroutine());
    }

    private IEnumerator PlayerHeadCountCoroutine()
    {
        AudioManager.Instance.PlayAudio(headcountPolice.GetComponent<AudioSource>(), AudioManager.Instance.talkSource);
        yield return new WaitForSeconds(8f);
        UIManager.Instance.ChangeText(UIManager.Instance.informationText, $"Attendance confirmed. Headcount: {npcs.Count + 1}.");
        DebugToolKit.Log("Player headcount confirmed.");
        AudioManager.Instance.StopAudio(headcountPolice.GetComponent<AudioSource>());
        yield return new WaitForSeconds(5f);
        this.SwitchState(chowtimeState);
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
                agent.enabled = true;
                agent.updateRotation = true;
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
            child.localRotation *= Quaternion.Euler(child.transform.rotation.x, child.transform.rotation.y, 90);
        }
    }

    public void CloseAllCellDoors()
    {
        lockPosition.SetActive(false);
        mainDoor.transform.position = new Vector3(-10.145f, 1.77f, 10.033f);
        foreach (Transform child in cellDoors.transform)
        {
            child.localRotation *= Quaternion.Euler(child.transform.rotation.x, child.transform.rotation.y, -90);
        }
    }

    public void OpenDoorMiniGame()
    {
        mainDoor.transform.position = new Vector3(-11.86f, 1.77f, 10.033f);
        playerCellDoor.transform.localRotation *= Quaternion.Euler(playerCellDoor.transform.rotation.x, playerCellDoor.transform.rotation.y, 90);
    }

    public void ResetAllNpcPositions()
    {
        foreach (var npc in allNpcs)
        {
            npc.OnPositionReset();
        }
    }
}
