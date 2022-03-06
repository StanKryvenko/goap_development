
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Steering : MonoBehaviour
{
    private const float WalkingSpeed = 2;
    private const float RunningSpeed = 4;
    
    public Vector3 Velocity => _agent.velocity;
    public Vector3 NextPoint => _agent.steeringTarget;

    public float TimeToFinish()
    {
        var timeToFinish = 0f;
        var startedAnalysis = false;
        for (var cornerId = 0; cornerId < _agent.path.corners.Length; cornerId++)
        {
            if (!startedAnalysis && _agent.path.corners[cornerId] == NextPoint)
                startedAnalysis = true;
            if (!startedAnalysis) continue;
            
            var prevPoint = cornerId == 0 ? _agent.transform.position : _agent.path.corners[cornerId - 1];
            timeToFinish += Vector3.Distance(prevPoint, NextPoint) / _agent.speed;
        }

        return timeToFinish;
    }
    public Vector3 SteeringTarget { get; set; }
    
    public double WanderRadius { get; set; }
    public double WanderDistance { get; set; }
    public double WanderJitter { get; set; }

    public Vector3 WanderTarget { get; private set; }
    private Transform _relativeTransform;

    private NavMeshAgent _agent;
    private bool _isWandering;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        var randomLocation = GetRandomLocationAround(transform.position, 5);
        if (double.IsPositiveInfinity(randomLocation.x))
            randomLocation = GetRandomLocationAround(transform.position);
        transform.position = randomLocation;
    }

    private Vector3 Wander()
    {
        // add a small random vector to the target's position (RandomClamped
        // returns a value between -1 and 1
        WanderTarget += new Vector3(
            (float)(Random.Range(-1.0f, 1.0f) * WanderJitter),
            0,
            (float)(Random.Range(-1.0f, 1.0f) * WanderJitter));
        // reproject this new vector back onto a unit circle
        WanderTarget = WanderTarget.normalized;
        // increase the length of the vector to the same as the radius
        // of the wander circle
        WanderTarget *= (float) WanderRadius;
        // move the target into a position WanderDist in front of the agent
        var targetLocal = WanderTarget + new Vector3(0, 0, (float) WanderDistance);
        // project the target into world space
        var targetWorld = Quaternion.LookRotation(_relativeTransform.forward, _relativeTransform.up) * targetLocal;
        // and steer toward it
        return _relativeTransform.position + targetWorld;
    }

    public void Init(Transform relativeTransform, int distance, int radius, int jitter)
    {
        _relativeTransform = relativeTransform;
        WanderDistance     = distance;
        WanderRadius       = radius;
        WanderJitter       = jitter;
        
    }

    public void WanderOn() => _isWandering = true;
    public void WanderOff() => _isWandering = false;

    public void SeekAndArrive(Vector3 position)
    {
        WanderTarget = position;
        _agent.SetDestination(position);
    }

    public void SeekOff() => _agent.SetDestination(_agent.transform.position);
    
    private void Update()
    {
        if (_isWandering)
        {
            var wanderTarget = Wander();
            if (NavMesh.SamplePosition(wanderTarget, out var navHit, 2, -1))
                SteeringTarget = navHit.position;
            else
                SteeringTarget *= -1;
            
            if (Vector3.Distance(SteeringTarget, _agent.destination) > _agent.speed)
                _agent.SetDestination(SteeringTarget);
        }
    }

    public NavMeshPath CalculatePathToTarget(Vector3 destination)
    {
        var targetPath = new NavMeshPath();
        _agent.CalculatePath(destination, targetPath);
        return targetPath;
    }

    public Vector3 GetRandomLocationAround(Vector3 origin, float walkRadius = 100)
    {
        var randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += origin;

        NavMesh.SamplePosition(randomDirection, out var hit, walkRadius, 1);
        var finalPosition = hit.position;
        return finalPosition;
    }

    public void SetWalkingSpeed() => _agent.speed = WalkingSpeed;
    public void SetRunningSpeed() => _agent.speed = RunningSpeed;

    public void StopMovement() => _agent.isStopped = true;
    public void AllowMovement() => _agent.isStopped = false;

    public void LookAt(GameObject nearestNpc)
    {
        var lookPos = nearestNpc.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2f);
    }
}
