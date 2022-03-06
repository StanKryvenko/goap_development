
using UnityEngine;

public class ExploreGoalEvaluator : GoalEvaluator
{
    public override double CalculateDesirability(NPC bot)
    {
        var desirability = 0.05f;
        
        return desirability;
    }

    public override void SetGoal(NPC owner)
    {
        if (!owner.ThinkingGoal.NotPresent(typeof(ExploreGoal))) return;
        
        owner.ThinkingGoal.RemoveAllSubgoals();
        owner.ThinkingGoal.AddSubgoal(new ExploreGoal(owner));
    }

    public ExploreGoalEvaluator(double characterBias) : base(characterBias) { }
}