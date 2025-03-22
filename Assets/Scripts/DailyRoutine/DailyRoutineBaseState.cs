using UnityEngine;

public abstract class DailyRoutineBaseState
{
    public abstract void EnterStatate(DailyRoutineManager dailyRoutineManager);
    public abstract void UpdateState(DailyRoutineManager dailyRoutineManager);
    public abstract void ExitState(DailyRoutineManager dailyRoutineManager);
}
