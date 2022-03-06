
using System;
using System.Collections.Generic;
using System.Linq;

public class ThinkGoal : CompositeGoal<NPC>
{
    private List<GoalEvaluator> _evaluators;

    public ThinkGoal(NPC owner, List<GoalEvaluator> evaluators) : base(owner)
    {
        _evaluators = evaluators;
    }
    
    public void Arbitrate()
    {
        var best = 0.0;
        GoalEvaluator mostDesirable = null;

        foreach (var evaluator in _evaluators)
        {
            var desirability = evaluator.CalculateDesirability(Owner);

            if (desirability >= best)
            {
                best = desirability;
                mostDesirable = evaluator;
            }
        }

        mostDesirable?.SetGoal(Owner);
    }

    public override void Activate()
    {
        Arbitrate();
        Status = GoalStatus.Active;
    }

    public override GoalStatus Process()
    {
        if (Status == GoalStatus.Inactive) Activate();

        var subgoalStatus = ProcessSubgoals();
        if (subgoalStatus == GoalStatus.Completed || subgoalStatus == GoalStatus.Failed)
            Status = GoalStatus.Inactive;

        return Status;
    }
    
    public bool NotPresent(Type goal)
    {
        if (SubGoals.Any())
            return SubGoals.First().GetType() != goal;
        return true;
    }
    
    public override void Terminate() { }
}