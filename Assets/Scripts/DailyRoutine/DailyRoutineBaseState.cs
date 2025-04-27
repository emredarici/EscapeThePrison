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
        dailyRoutineManager.NpcHeadCount();
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
        DebugToolKit.Log("Exiting Headcount State");
        UIManager.Instance.ChangeText(UIManager.Instance.informationText, "");
        VFXManager.Instance.DestroyMarker();
    }
}

public class ChowtimeState : DailyRoutineBaseState
{
    public override void EnterState(DailyRoutineManager dailyRoutineManager)
    {
        dailyRoutineManager.ArrangeQueue();

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
        if (dailyRoutineManager.dayManager.IsDay(Day.Day1))
        {
            UIManager.Instance.movementTrailer.SetActive(true);
            dailyRoutineManager.StartCoroutine(dailyRoutineManager.CountdownSwitchState(10f, dailyRoutineManager.chowtimeState));
        }
    }

    public override void UpdateState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void ExitState(DailyRoutineManager dailyRoutineManager)
    {
        if (dailyRoutineManager.dayManager.IsDay(Day.Day1))
        {
            Object.Destroy(UIManager.Instance.movementTrailer);
        }
    }


}
