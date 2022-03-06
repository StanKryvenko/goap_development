using System.Collections.Generic;
using System.Linq;

public abstract class CompositeGoal<T> : Goal<T> where T: NPC
{
    protected List<Goal<T>> SubGoals = new List<Goal<T>>();

    public void RemoveAllSubgoals()
    {            
        for (var i = SubGoals.Count - 1; i >= 0; i--)
        {
            SubGoals[i].Terminate();
            SubGoals.RemoveAt(i);
        }
        SubGoals.Clear();
    }
    
    public GoalStatus ProcessSubgoals()
    {
        // remove all completed and failed goals from the front of the subgoal list
        for (var i = SubGoals.Count - 1; i >= 0; i--)
        {
            if (!SubGoals[i].IsCompleted && !SubGoals[i].HasFailed) continue;
            SubGoals[i].Terminate();
            SubGoals.RemoveAt(i);
        }
        // if any subgoals remain, process the one at the front of the list
        if (SubGoals.Any())
        {
            // grab the status of the frontmost subgoal
            var statusOfSubGoals = SubGoals.First().Process();
            
            // we have to test for the special case where the frontmost subgoal
            // reports "completed" and the subgoal list contains additional goals.
            // when this is the case, to ensure the parent keeps processing its
            // subgoal list, the "active" status is returned
            if (statusOfSubGoals == GoalStatus.Completed && SubGoals.Count > 1)
                return GoalStatus.Active;

            return statusOfSubGoals;
        }
        // no more subgoals to process - return "completed"

        return GoalStatus.Completed;
    }

    public override void AddSubgoal(Goal<T> goal) => SubGoals.Add(goal);

    protected CompositeGoal(T owner) : base(owner) { }
}