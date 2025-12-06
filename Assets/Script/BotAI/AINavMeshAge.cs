using UnityEngine;
using UnityEngine.AI;

public class AIWanderNavmeshOnly : MonoBehaviour
{
    [Header("Wander Settings")]
    [SerializeField] private float moveRadius = 10f;
    [SerializeField] private float newPointDelay = 2f;

    private NavMeshAgent _agent;
    private float _timer;

    public void Initialize(IAgentProvider provider)
    {
        _agent = provider.Agent;
    }


    private void Start()
    {
        if (_agent == null)
        {
            enabled = false;
            return;
        }

        _timer = newPointDelay;
        MoveToRandomPoint();
    }


    private void Update()
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return;

        HandleWanderBehavior();
    }


    private void HandleWanderBehavior()
    {
        UpdateTimer();
        CheckArrival();
        CheckInterval();
    }

    private void UpdateTimer()
    {
        _timer += Time.deltaTime;
    }

    private void CheckArrival()
    {
        //Target is completed
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            MoveToRandomPoint();
        }
    }

    private void CheckInterval()
    {
        if (_timer >= newPointDelay)
        {
            MoveToRandomPoint();
            _timer = 0f;
        }
    }

    private void MoveToRandomPoint()
    {
        Vector3 randomPos = Random.insideUnitSphere * moveRadius + transform.position;

        //Find navmesh
        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, moveRadius, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }
}