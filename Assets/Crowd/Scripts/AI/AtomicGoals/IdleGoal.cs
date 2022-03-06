using UnityEngine;

public class IdleGoal : AtomicGoal<NPC>
{
    private float _sitTime;
    
    public override void Activate()
    {
        Status = GoalStatus.Active;

        Owner.GetSteering().StopMovement();
        Owner.State = NPCState.Sitting;
        _sitTime = Time.time;
    }

    public override GoalStatus Process()
    {
        // if status is inactive, call Active() and set status to active
        if (Status == GoalStatus.Inactive) Activate();

        if (NPCFeature.IsAnybodyAround(Owner) || Owner.Adrenaline >= Adrenaline.Bored) 
            Status = GoalStatus.Completed;
        else if (Time.time > _sitTime + 10)
            Owner.Adrenaline = Adrenaline.Bored;

        return Status;
    }

    public override void Terminate()
    {
        Owner.GetSteering().AllowMovement();
        Owner.State = NPCState.None;
    }

    public IdleGoal(NPC owner) : base(owner) { }
}