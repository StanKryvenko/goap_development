
using UnityEngine;

public class LookAtPlayerGoalEvaluator : GoalEvaluator
{
    public override double CalculateDesirability(NPC bot)
    {
        var desirability = 0.0;
        if (NPCFeature.GetNearestPlayer(bot, 1f) != null)
            desirability = 0.06;
        desirability *= characterBias;
        
        return desirability;
    }

    public override void SetGoal(NPC owner)
    {
        if (!owner.ThinkingGoal.NotPresent(typeof(LookAtPlayerGoal))) return;
        
        owner.ThinkingGoal.RemoveAllSubgoals();
        owner.ThinkingGoal.AddSubgoal(new LookAtPlayerGoal(owner));
    }

    public LookAtPlayerGoalEvaluator(double characterBias) : base(characterBias) { }
}