using UnityEngine;

public class FollowLeaderGoal : CompositeGoal<NPC>
{
    private Vector3 _currentDestination;
    
    public FollowLeaderGoal(NPC owner) : base(owner) { }

    public override void Activate()
    {
        Status = GoalStatus.Active;
        RemoveAllSubgoals();

        var leadersTarget = Owner.Home.GroupLeader.GetSteering().WanderTarget;
        if (leadersTarget == Vector3.zero)
        {
            Status = GoalStatus.Completed;
            return;
        }

        _currentDestination = Owner.GetSteering().GetRandomLocationAround(leadersTarget, 5);

        if (Owner.GetSteering().CalculatePathToTarget(_currentDestination).corners.Length == 0)
            Status = GoalStatus.Completed;
        else
        {
            RemoveAllSubgoals();
            AddSubgoal(new SeekToPositionGoal(Owner, _currentDestination));
        }
    }

    public override GoalStatus Process()
    {
        if (Status == GoalStatus.Inactive) Activate();

        Status = ProcessSubgoals();
        
        return Status;
    }

    public override void Terminate() { }
}