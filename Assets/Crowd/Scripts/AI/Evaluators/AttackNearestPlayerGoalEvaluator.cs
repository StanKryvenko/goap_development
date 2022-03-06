
using UnityEngine;

public class AttackNearestPlayerGoalEvaluator : GoalEvaluator
{
    public override double CalculateDesirability(NPC bot)
    {
        var desirability = 0.0;
        if (NPCFeature.GetNearestEnemy(bot, 1f) != null)
            desirability = 1;
        
        return desirability;
    }

    public override void SetGoal(NPC owner)
    {
        if (!owner.ThinkingGoal.NotPresent(typeof(AttackEnemyGoal))) return;
        
        owner.ThinkingGoal.RemoveAllSubgoals();
        owner.ThinkingGoal.AddSubgoal(new AttackEnemyGoal(owner));
    }

    public AttackNearestPlayerGoalEvaluator(double characterBias) : base(characterBias) { }
}