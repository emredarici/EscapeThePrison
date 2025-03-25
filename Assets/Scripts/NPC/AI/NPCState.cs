using UnityEngine;

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
                npc.SetState(new SleepingState(npc));
                break;
            case 1:
                npc.SetState(new LookingOutsideState(npc));
                break;
            case 2:
                npc.SetState(new ToiletState(npc));
                break;
        }
    }

    public bool HasWaited(int minTime, int maxTime)
    {
        int randomTime = Random.Range(minTime, maxTime);
        return Time.time - StartTime >= randomTime;
    }
}


public class SleepingState : NPCState
{
    private Transform target;

    public SleepingState(NPCController npc) : base(npc)
    {
        target = npc.GetBedPosition();
    }

    public override void Enter()
    {
        base.Enter();
        npc.MoveTo(target.position, () => Debug.Log("Sleeping"));
    }

    public override void Update()
    {
        if (HasWaited(5, 10))
        {
            RandomState(0, 3);
        }
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
        npc.MoveTo(target.position, () => Debug.Log("LookOutside"));
    }

    public override void Update()
    {
        if (HasWaited(5, 7))
        {
            RandomState(0, 3);
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
        npc.MoveTo(target.position, () => Debug.Log("Sit"));
    }

    public override void Update()
    {
        if (HasWaited(7, 10))
        {
            RandomState(0, 3);
        }
    }
}

