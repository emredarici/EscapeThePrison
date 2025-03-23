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

    public bool HasWaited(float time)
    {
        return Time.time - StartTime >= time;
    }

    // 🔹 IDLE STATE
    public class IdleState : NPCState
    {
        private float waitTime;

        public IdleState(NPCController npc) : base(npc) { }

        public override void Enter()
        {
            base.Enter();
            waitTime = Random.Range(2f, 5f);
        }

        public override void Update()
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                if (!npc.HasSlept)
                {
                    npc.SetState(new SleepingState(npc));
                }
                else if (!npc.HasLookedOutside)
                {
                    npc.SetState(new LookingOutsideState(npc));
                }
                else if (!npc.HasUsedToilet)
                {
                    npc.SetState(new ToiletState(npc));
                }
                else
                {
                    npc.BedTimeDailyRoutine();
                }
            }
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
            if (HasWaited(10f))
            {
                npc.HasSlept = true;
                npc.SetState(new IdleState(npc));
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
            if (HasWaited(5f))
            {
                npc.HasLookedOutside = true;
                npc.SetState(new IdleState(npc));
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
            if (HasWaited(7f))
            {
                npc.HasUsedToilet = true;
                npc.SetState(new IdleState(npc));
            }
        }
    }
}
