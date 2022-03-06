
using System;
using UnityEngine;

public class VoteForLeaderGoalEvaluator : GoalEvaluator
{
    private Guid _lastVoted;
    
    public override double CalculateDesirability(NPC bot)
    {
        if (bot.Home.VotingId == _lastVoted)
            return 0;
        
        _lastVoted = bot.Home.VotingId;
        return 1;
    }

    public override void SetGoal(NPC owner)
    {
        if (!owner.ThinkingGoal.NotPresent(typeof(FindLeaderGoal))) return;
        
        owner.ThinkingGoal.RemoveAllSubgoals();
        owner.ThinkingGoal.AddSubgoal(new FindLeaderGoal(owner));
    }

    public VoteForLeaderGoalEvaluator(double characterBias) : base(characterBias) { }
}