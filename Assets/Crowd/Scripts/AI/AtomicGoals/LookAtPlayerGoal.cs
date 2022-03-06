using UnityEngine;

public class LookAtPlayerGoal : AtomicGoal<NPC>
{
    private GameObject _nearestPlayer;
    
    public override void Activate()
    {
        Status = GoalStatus.Active;

        _nearestPlayer = NPCFeature.GetNearestPlayer(Owner, 2f);
        Owner.GetSteering().StopMovement();
        Owner.State = NPCState.Yelling;
    }

    public override GoalStatus Process()
    {
        // if status is inactive, call Active() and set status to active
        if (Status == GoalStatus.Inactive) Activate();

        if (_nearestPlayer != null)
            Owner.GetSteering().LookAt(_nearestPlayer);
        else
        {
            Status = GoalStatus.Completed;
            return Status;
        }

        if (Vector3.Distance(_nearestPlayer.transform.position, Owner.Pos) > 2)
            Status = GoalStatus.Completed;

        return Status;
    }

    public override void Terminate()
    {
        Owner.GetSteering().AllowMovement();
        Owner.State = NPCState.None;
    }

    public LookAtPlayerGoal(NPC owner) : base(owner) { }
}