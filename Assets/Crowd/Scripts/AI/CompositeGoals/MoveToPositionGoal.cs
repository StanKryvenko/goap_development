
using UnityEngine;

public class MoveToPositionGoal : CompositeGoal<NPC>
{
    private Vector3 _destination;

    public MoveToPositionGoal(NPC owner, Vector3 destination) : base(owner) 
    {
        _destination = destination;
    }
    
    public override void Activate()
    {
        Status = GoalStatus.Active;
        
        RemoveAllSubgoals();
        
        if (Owner.GetSteering().CalculatePathToTarget(_destination).corners.Length == 0)
            Status = GoalStatus.Failed;
        else
        {
            RemoveAllSubgoals();
            AddSubgoal(new SeekToPositionGoal(Owner, _destination));
        }
    }

    public override GoalStatus Process()
    {
        if (Status == GoalStatus.Inactive) Activate();
        
        Status = ProcessSubgoals();
        
        if (Status == GoalStatus.Failed) Status = GoalStatus.Inactive;
        return Status;
    }

    public override void Terminate() { }
}