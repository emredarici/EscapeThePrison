using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRoutineManager : MonoBehaviour
{
    DailyRoutineBaseState currentState;
    HeadcountState headcountState = new HeadcountState();
    ChowtimeState chowtimeState = new ChowtimeState();
    RectimeState rectimeState = new RectimeState();
    BedtimeState bedtimeState = new BedtimeState();

    void Start()
    {
        currentState = headcountState;
        currentState.EnterStatate(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(DailyRoutineBaseState newState)
    {
        currentState = newState;
        currentState.EnterStatate(this);
    }
}
