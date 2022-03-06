using UnityEngine;

public class ExploreGoal : CompositeGoal<NPC>
{
    private Vector3 _currentDestination;
    private bool _destinationIsSet;
    private float _explorationStartTime;
    
    public ExploreGoal(NPC owner) : base(owner) { }

    public override void Activate()
    {
        Status = GoalStatus.Active;
        RemoveAllSubgoals();
        
        _explorationStartTime = Time.time;
        if (!_destinationIsSet)
        {
            // grab a random map location
            _currentDestination = Owner.GetSteering().GetRandomLocationAround(Owner.Pos);
            _destinationIsSet = true;
        }
        
        if (Owner.GetSteering().CalculatePathToTarget(_currentDestination).corners.Length == 0)
            Status = GoalStatus.Failed;
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
        
        if (Owner.Adrenaline == Adrenaline.Bored && Time.time > _explorationStartTime + 5)
        {
            Owner.Adrenaline = Adrenaline.Calm;
            Status = GoalStatus.Completed;
        }

        return Status;
    }

    public override void Terminate() { }
}