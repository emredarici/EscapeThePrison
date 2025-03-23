using UnityEngine;

public class DailyRoutineManager : MonoBehaviour
{
    public static DailyRoutineManager Instance { get; private set; }
    public DailyRoutineBaseState currentState;
    HeadcountState headcountState = new HeadcountState();
    ChowtimeState chowtimeState = new ChowtimeState();
    RectimeState rectimeState = new RectimeState();
    public BedtimeState bedtimeState = new BedtimeState();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        currentState = headcountState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(DailyRoutineBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
        Debug.Log("Switched to " + currentState);
    }
}
