using UnityEngine;

public partial class AIController
{
    /// <summary>
    /// State Wander – chỉ chạy khi không Chase
    /// </summary>
    private void WanderBehavior()
    {
        UpdateTimer();
        CheckArrival();
        CheckInterval();
    }

    private void UpdateTimer()
    {
        _timer += Time.deltaTime;
    }

    /// <summary>
    /// Khi tới điểm đích → chọn điểm mới
    /// </summary>
    private void CheckArrival()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            MoveToRandomPoint();
        }
    }

    /// <summary>
    /// Khi quá thời gian delay → đổi điểm ngẫu nhiên
    /// </summary>
    private void CheckInterval()
    {
        if (_timer >= newPointDelay)
        {
            MoveToRandomPoint();
            _timer = 0f;
        }
    }
}