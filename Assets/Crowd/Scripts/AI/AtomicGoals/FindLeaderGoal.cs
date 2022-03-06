using System.Linq;
using UnityEngine;

public class FindLeaderGoal : AtomicGoal<NPC>
{
    public override void Activate()
    {
        Status = GoalStatus.Active;
    }

    public override GoalStatus Process()
    {
        // if status is inactive, call Active() and set status to active
        if (Status == GoalStatus.Inactive) Activate();

        var npcAround = NPCFeature.GetNearestNPCs(Owner, 10).ToList();
        if (npcAround.Count == 0)
        {
            Status = GoalStatus.Completed;
            return Status;
        }

        // Find and vote for a random leader
        var rndNpc = Random.Range(0, npcAround.Count);
        if (Owner.Home.VoteBook.ContainsKey(npcAround[rndNpc]))
            Owner.Home.VoteBook[npcAround[rndNpc]] ++;
        else 
            Owner.Home.VoteBook[npcAround[rndNpc]] = 1;

        return Status;
    }

    public override void Terminate() { }

    public FindLeaderGoal(NPC owner) : base(owner) { }
}