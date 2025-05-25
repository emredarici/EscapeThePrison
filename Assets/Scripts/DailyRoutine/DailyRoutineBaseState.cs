using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
        dailyRoutineManager.dayTimeManager.SetTimeOfDay(DayTimeManager.TimeOfDay.Morning);
        dailyRoutineManager.headcountPolice.SetActive(true);
        dailyRoutineManager.ResetAllNpcPositions();
        dailyRoutineManager.PopulateNpcList();
        foreach (var npc in dailyRoutineManager.allNpcs)
        {
            if (npc.GetComponent<NavMeshAgent>().enabled == false)
                npc.GetComponent<NavMeshAgent>().enabled = true;
        }
        dailyRoutineManager.NpcHeadCount();
    }

    public override void UpdateState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void ExitState(DailyRoutineManager dailyRoutineManager)
    {
        dailyRoutineManager.StartCoroutine(SetActivepolice(5f, dailyRoutineManager.headcountPolice));
        DebugToolKit.Log("Exiting Headcount State");
        UIManager.Instance.DeleteText(UIManager.Instance.informationText);
        VFXManager.Instance.DestroyMarker();
    }

    private IEnumerator SetActivepolice(float time, GameObject police)
    {
        yield return new WaitForSeconds(time);
        police.SetActive(false);
    }
}

public class ChowtimeState : DailyRoutineBaseState
{
    public override void EnterState(DailyRoutineManager dailyRoutineManager)
    {
        dailyRoutineManager.dayTimeManager.SetTimeOfDay(DayTimeManager.TimeOfDay.Afternoon);
        dailyRoutineManager.ArrangeQueue();
        UIManager.Instance.ChangeText(UIManager.Instance.informationText, "Time to eat, all prisoners to the cafeteria!");

        if (dailyRoutineManager.dayManager.IsDay(Day.Day3))
        {
            VFXManager.Instance.SpawnLocationMarker(dailyRoutineManager.thirdDayVFXPosition.position);
            dailyRoutineManager.thirdDayNPC.SetActive(true);
            dailyRoutineManager.isThirdDayNPCDialouge = false;
        }

    }

    public override void UpdateState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void ExitState(DailyRoutineManager dailyRoutineManager)
    {
        UIManager.Instance.DeleteText(UIManager.Instance.informationText);
    }


}

public class RectimeState : DailyRoutineBaseState
{
    public override void EnterState(DailyRoutineManager dailyRoutineManager)
    {
        foreach (var npc in dailyRoutineManager.allNpcs)
        {
            npc.RectimeStateNPC();
        }
        if (dailyRoutineManager.dayManager.IsDay(Day.Day1))
        {
            dailyRoutineManager.StartCoroutine(dailyRoutineManager.CountdownSwitchState(30, dailyRoutineManager.bedtimeState));
        }
        if (dailyRoutineManager.dayManager.IsDay(Day.Day2))
        {
            dailyRoutineManager.twoDayNPC.SetActive(true);
            VFXManager.Instance.SpawnLocationMarker(dailyRoutineManager.twoDayVFXPosition.position);
        }
        if (dailyRoutineManager.dayManager.IsDay(Day.Day3))
        {
            dailyRoutineManager.thirdDay2NPC.SetActive(true);
            VFXManager.Instance.SpawnLocationMarker(dailyRoutineManager.thirdDay2VFXPosition.position);
        }
        dailyRoutineManager.dayTimeManager.SetTimeOfDay(DayTimeManager.TimeOfDay.Evening);
    }

    public override void UpdateState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void ExitState(DailyRoutineManager dailyRoutineManager)
    {
        UIManager.Instance.ChangeText(UIManager.Instance.informationText, "Time to sleep, all prisoners to your cells!");
        dailyRoutineManager.lockPosition.SetActive(true);
        VFXManager.Instance.SpawnLocationMarker(dailyRoutineManager.lockPosition.transform.position);
        if (dailyRoutineManager.dayManager.IsDay(Day.Day2))
        {
            dailyRoutineManager.twoDayNPC.SetActive(false);
        }
        if (dailyRoutineManager.dayManager.IsDay(Day.Day3))
        {
            dailyRoutineManager.thirdDay2NPC.SetActive(false);
        }
    }
}

public class BedtimeState : DailyRoutineBaseState
{
    public override void EnterState(DailyRoutineManager dailyRoutineManager)
    {
        dailyRoutineManager.dayTimeManager.SetTimeOfDay(DayTimeManager.TimeOfDay.Night);
        if (dailyRoutineManager.dayManager.IsDay(Day.Day1))
        {
            if (UIManager.Instance.movementTrailer == null)
            {
                return;
            }
            else
            {
                dailyRoutineManager.dayTimeManager.SetTimeOfDay(DayTimeManager.TimeOfDay.Morning);
            }
            UIManager.Instance.movementTrailer.SetActive(true);
            dailyRoutineManager.StartCoroutine(dailyRoutineManager.CountdownSwitchState(10f, dailyRoutineManager.headcountState));
        }

    }

    public override void UpdateState(DailyRoutineManager dailyRoutineManager)
    {

    }

    public override void ExitState(DailyRoutineManager dailyRoutineManager)
    {
        dailyRoutineManager.lockPosition.SetActive(false);
        if (dailyRoutineManager.dayManager.IsDay(Day.Day1))
        {
            Object.Destroy(UIManager.Instance.movementTrailer);
        }
        dailyRoutineManager.OpenAllCellDoors();
        UIManager.Instance.DeleteText(UIManager.Instance.informationText);
    }
}
