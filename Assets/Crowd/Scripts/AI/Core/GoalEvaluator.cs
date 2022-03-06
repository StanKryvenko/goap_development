
public abstract class GoalEvaluator
{
    /// <summary>
    /// When desirability score for a goal has been evaluated it is multiplied by this value.
    /// It can be used to create bots with preferences based upon their personality
    /// </summary>
    protected double characterBias;

    public GoalEvaluator(double characterBias)
    {
        this.characterBias = characterBias;
    }
    
    public abstract double CalculateDesirability(NPC bot);

    public abstract void SetGoal(NPC owner);
}