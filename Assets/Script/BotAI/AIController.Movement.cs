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
            _agent.speed = walkSpeed;             // tốc độ đi bộ
            _agent.SetDestination(hit.position);

            anim.SetBool("run", false);
            anim.SetBool("walk", true);
        }
    }

    /// <summary>
    /// Di chuyển đến mục tiêu (Player)
    /// </summary>
    protected void MoveToTarget()
    {
        if (_target != null)
        {
            _agent.speed = runSpeed;
            _agent.stoppingDistance = 1.5f;
            
            _agent.SetDestination(_target.position);
            anim.SetBool("run",true);
            anim.SetBool("walk",false);
        }
            
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            anim.SetBool("run", false);
            _agent.isStopped = true;
        }
        else
        {
            _agent.isStopped = false;
        }
        
        
    }
}