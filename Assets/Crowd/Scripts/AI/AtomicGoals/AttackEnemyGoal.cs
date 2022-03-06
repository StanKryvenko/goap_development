using UnityEngine;

public class AttackEnemyGoal : AtomicGoal<NPC>
{
    private NPC _nearestNpc;
    
    public override void Activate()
    {
        Status = GoalStatus.Active;

        _nearestNpc = NPCFeature.GetNearestEnemy(Owner, 2f);
        Owner.GetSteering().StopMovement();
        Owner.State = NPCState.Attacking;
    }

    public override GoalStatus Process()
    {
        // if status is inactive, call Active() and set status to active
        if (Status == GoalStatus.Inactive) Activate();

        if (_nearestNpc != null)
        {
            Owner.GetSteering().LookAt(_nearestNpc.gameObject);
            if (Random.Range(0, 3) == 0)
                Object.Destroy(_nearestNpc.gameObject);
        }
        else
        {
            Status = GoalStatus.Completed;
            return Status;
        }

        if (Vector3.Distance(_nearestNpc.transform.position, Owner.Pos) > 2)
            Status = GoalStatus.Completed;

        return Status;
    }

    public override void Terminate()
    {
        Owner.GetSteering().AllowMovement();
        Owner.State = NPCState.None;
    }

    public AttackEnemyGoal(NPC owner) : base(owner) { }
}