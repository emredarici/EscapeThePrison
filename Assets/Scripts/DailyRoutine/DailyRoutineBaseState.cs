using UnityEngine;

public abstract class DailyRoutineBaseState
{
    public abstract void EnterState(DailyRoutineManager dailyRoutineManager);
    public abstract void UpdateState(DailyRoutineManager dailyRoutineManager);
    public abstract void ExitState(DailyRoutineManager dailyRoutineManager);
}

public class HeadcountState : DailyRoutineBaseState
{
    public override void EnterState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void UpdateState(DailyRoutineManager dailyRoutineManager)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dailyRoutineManager.SwitchState(dailyRoutineManager.bedtimeState);
        }
    }

    public override void ExitState(DailyRoutineManager dailyRoutineManager)
    {
    }
}

public class ChowtimeState : DailyRoutineBaseState
{
    public override void EnterState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void UpdateState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void ExitState(DailyRoutineManager dailyRoutineManager)
    {
    }
}

public class RectimeState : DailyRoutineBaseState
{
    public override void EnterState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void UpdateState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void ExitState(DailyRoutineManager dailyRoutineManager)
    {
    }
}


public class BedtimeState : DailyRoutineBaseState
{
    public override void EnterState(DailyRoutineManager dailyRoutineManager)
    {
    }

    public override void UpdateState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void ExitState(DailyRoutineManager dailyRoutineManager)
    {
    }
}
