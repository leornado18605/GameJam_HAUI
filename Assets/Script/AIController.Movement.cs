using UnityEngine;
using UnityEngine.AI;

public partial class AIController
{
    /// <summary>
    /// Di chuyển đến điểm ngẫu nhiên trong NavMesh
    /// </summary>
    protected void MoveToRandomPoint()
    {
        Vector3 randomPos = Random.insideUnitSphere * moveRadius + transform.position;

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, moveRadius, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }

    /// <summary>
    /// Di chuyển đến mục tiêu (Player)
    /// </summary>
    protected void MoveToTarget()
    {
        if (_target != null)
            _agent.SetDestination(_target.position);
    }
}