
using UnityEngine;

public class IdlingGoalEvaluator : GoalEvaluator
{
    public override double CalculateDesirability(NPC bot)
    {
        var desirability = 0.06;
        if (NPCFeature.IsAnybodyAround(bot) || bot.Adrenaline >= Adrenaline.Bored)
            desirability = 0;
        desirability *= characterBias;
        
        return desirability;
    }

    public override void SetGoal(NPC owner)
    {
        if (!owner.ThinkingGoal.NotPresent(typeof(IdleGoal))) return;
        
        owner.ThinkingGoal.RemoveAllSubgoals();
        owner.ThinkingGoal.AddSubgoal(new IdleGoal(owner));
    }

    public IdlingGoalEvaluator(double characterBias) : base(characterBias) { }
}