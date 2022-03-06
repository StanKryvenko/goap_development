public enum GoalStatus
{
    Inactive,  // the goal is waiting to be activated
    Active,    // the goal activated and will be processed
    Completed, // the goal has completed and will be removed
    Failed     // the goal has failed and will replan or be removed
}

public abstract class Goal<T> where T: NPC
{
    protected T Owner;
    protected GoalStatus Status = GoalStatus.Inactive;
    protected int Type = 0;

    protected Goal(T owner)
    {
        Owner = owner;
    }

    public bool IsActive     => Status == GoalStatus.Active;
    public bool IsInactive   => Status == GoalStatus.Inactive;
    public bool IsCompleted  => Status == GoalStatus.Completed;
    public bool HasFailed    => Status == GoalStatus.Failed;
    public int GoalType() => Type;

    public abstract void Activate();
    public abstract GoalStatus Process();
    public abstract void Terminate();

    public abstract void AddSubgoal(Goal<T> goal);
}