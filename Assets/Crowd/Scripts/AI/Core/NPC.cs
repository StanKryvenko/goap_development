using System.Collections.Generic;
using UnityEngine;

public enum NPCState
{
    None,
    Sitting,
    Yelling,
    Attacking,
    Dead
}

public enum Adrenaline
{
    Calm = 0,
    Bored = 1,
    Active = 2,
    Survive = 3,
    Panic = 4
}

public class NPC : MonoBehaviour
{
    // the number of times a second a bot 'thinks' about changing strategy
    public int GoalAppraisalUpdateFreq = 4;
    private float _lastThinkTime = 0;
    
    public Vector3 Pos => transform.position;

    public ThinkGoal ThinkingGoal;
    
    public NPCState State { get; set; }
    public Adrenaline Adrenaline { get; set; } = Adrenaline.Calm;
    public NPCSpawner Home { get; set; }

    private Steering _steering;

    public void Init()
    {
        _steering = gameObject.AddComponent<Steering>();
        _steering.Init(transform, 3, 1, 1);

        var evaluators = new List<GoalEvaluator>
        {
            new VoteForLeaderGoalEvaluator(1),
            new AttackNearestPlayerGoalEvaluator(1),
            new FollowLeaderPathGoalEvaluator(1),
            new ExploreGoalEvaluator(1),
            new IdlingGoalEvaluator(1),
            new LookAtPlayerGoalEvaluator(1)
        };
        ThinkingGoal = new ThinkGoal(this, evaluators);
    }

    public Steering GetSteering() => _steering;

    public void Tick()
    {
        if (State == NPCState.Dead)
        {
            _steering.StopMovement();
            foreach (var col in GetComponentsInChildren<Collider>())
                col.enabled = false;
            return;
        }
        
        ThinkingGoal.Process();

        if (Time.time > _lastThinkTime + 1f / GoalAppraisalUpdateFreq)
        {
            ThinkingGoal.Arbitrate();
        }
    }
}
