public abstract class AtomicGoal<T> : Goal<T> where T: NPC
{
    public override void AddSubgoal(Goal<T> goal) => throw new System.NotImplementedException();

    protected AtomicGoal(T owner) : base(owner) { }
}