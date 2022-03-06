
using UnityEngine;

public class FollowLeaderPathGoalEvaluator : GoalEvaluator
{
    public override double CalculateDesirability(NPC bot)
    {
        return bot.Home.GroupLeader == null || bot.Home.GroupLeader == bot ? 0 : 0.9;
    }

    public override void SetGoal(NPC owner)
    {
        if (!owner.ThinkingGoal.NotPresent(typeof(FollowLeaderGoal))) return;
        
        owner.ThinkingGoal.RemoveAllSubgoals();
        owner.ThinkingGoal.AddSubgoal(new FollowLeaderGoal(owner));
    }

    public FollowLeaderPathGoalEvaluator(double characterBias) : base(characterBias) { }
}