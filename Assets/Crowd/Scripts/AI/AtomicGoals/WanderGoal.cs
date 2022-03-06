public class WanderGoal : AtomicGoal<NPC>
{
    public override void Activate()
    {
        Status = GoalStatus.Active;
        Owner.GetSteering().WanderOn();
    }

    public override GoalStatus Process()
    {
        // if status is inactive, call Active() and set status to active
        if (Status == GoalStatus.Inactive) Activate();

        return Status;
    }

    public override void Terminate() => Owner.GetSteering().WanderOff();

    public WanderGoal(NPC owner) : base(owner) { }
}