
using UnityEngine;

public class SeekToPositionGoal : AtomicGoal<NPC>
{
    private Vector3 _target;
    private double _timeExpected; // estimated time the bot should take to traverse the edge
    private double _startTime; // this records the time this goal was activated
    private Adrenaline _steerMode;
    
    public SeekToPositionGoal(NPC owner, Vector3 target) : base(owner)
    {
        _target = target;
    }
    
    private bool IsStuck() => Time.time > _startTime + _timeExpected;
    
    public override void Activate()
    {
        Status = GoalStatus.Active;

        // the edge behavior flag may specify a type of movement that necessitates a
        // change in the bot's behavior as it follows this edge
        /*switch (_edge.GetBehaviorFlag())
        {
            case NavGraphEdge.Crawl:
                Owner.SetMaxSpeed(maxSpeed);
            
        }*/
        _startTime = Time.time;
        _steerMode = Owner.Adrenaline;
        
        if (Owner.Adrenaline < Adrenaline.Survive)
            Owner.GetSteering().SetWalkingSpeed();
        else
            Owner.GetSteering().SetRunningSpeed();
        
        // calculate the expected time required to reach this waypoint
        _timeExpected = Owner.GetSteering().TimeToFinish();
        
        // factor in a margin of error for any reactive behavior. 2 seconds should be plenty
        const float marginOfError = 2.0f;
        _timeExpected += marginOfError;
        
        // set the steering target
        Owner.GetSteering().SeekAndArrive(_target);
    }

    public override GoalStatus Process()
    {
        if (Status == GoalStatus.Inactive) Activate();

        if (IsStuck())
            Status = GoalStatus.Failed;
        else
        {
            if (_steerMode != Owner.Adrenaline) Status = GoalStatus.Completed;
            if (Vector3.Distance(Owner.Pos, Owner.GetSteering().SteeringTarget) < 2)
                Status = GoalStatus.Completed;
        }

        return Status;
    }

    public override void Terminate()
    {
        Owner.GetSteering().SeekOff();
    }
}