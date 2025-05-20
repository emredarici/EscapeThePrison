using UnityEngine;
using UnityEngine.AI;

public abstract class NPCState
{
    protected NPCController npc;
    public float StartTime { get; private set; }

    public NPCState(NPCController npc)
    {
        this.npc = npc;
    }

    public virtual void Enter()
    {
        StartTime = Time.time;
    }

    public virtual void Update() { }

    public void RandomState(int a, int b)
    {

        int random = Random.Range(a, b);
        switch (random)
        {
            case 0:
                this.npc.SetState(new SleepingState(npc));
                break;
            case 1:
                this.npc.SetState(new LookingOutsideState(npc));
                break;
            case 2:
                this.npc.SetState(new ToiletState(npc));
                break;
        }
    }

    public virtual void Exit()
    {
        this.npc.animator.SetBool("isSleeping", false);
        this.npc.agent.enabled = true;
    }

    public bool HasWaited(int minTime, int maxTime)
    {
        int randomTime = Random.Range(minTime, maxTime);
        return Time.time - StartTime >= randomTime;
    }
}

public class IdleState : NPCState
{
    public IdleState(NPCController npc) : base(npc) { }

    public override void Enter()
    {
        this.npc.agent.enabled = true;
    }

    public override void Update()
    {
    }
}


public class SleepingState : NPCState
{
    private Transform target;

    public SleepingState(NPCController npc) : base(npc)
    {
        this.target = npc.GetBedPosition();
    }

    public override void Enter()
    {
        Debug.Log("Sleeping");
        base.Enter();
        this.npc.MoveTo(target.position, OnReachedBed);
    }

    private void OnReachedBed()
    {
        SleepRoutine();
    }

    private void SleepRoutine()
    {
        npc.agent.enabled = false;
        npc.animator.SetBool("isSleeping", true);
        if (HasWaited(15, 20))
        {
            if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.bedtimeState)
                RandomState(0, 3);
            else
                return;
        }
        npc.transform.position = npc.myCell.bedPosition.position;
        npc.transform.rotation = npc.myCell.bedPosition.rotation;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        base.Exit();
        npc.agent.enabled = true;
    }
}

public class LookingOutsideState : NPCState
{
    private Transform target;

    public LookingOutsideState(NPCController npc) : base(npc)
    {
        target = npc.GetBarsPosition();
    }

    public override void Enter()
    {
        base.Enter();
        npc.MoveTo(target.position, () => DebugToolKit.Log("LookOutside"));
    }

    public override void Update()
    {
        if (HasWaited(5, 7))
        {
            if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.bedtimeState)
                RandomState(0, 3);
            else
                return;
        }
    }
}

public class ToiletState : NPCState
{
    private Transform target;

    public ToiletState(NPCController npc) : base(npc)
    {
        target = npc.GetToiletPosition();
    }

    public override void Enter()
    {
        base.Enter();
        npc.MoveTo(target.position, () => DebugToolKit.Log("Sit"));
    }

    public override void Update()
    {
        if (HasWaited(7, 10))
        {
            if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.bedtimeState)
                RandomState(0, 3);
            else
                return;
        }
    }
}

