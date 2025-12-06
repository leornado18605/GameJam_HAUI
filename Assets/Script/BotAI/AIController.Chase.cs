using UnityEngine;

public partial class AIController
{
    private void ChaseBehavior()
    {
        if (_target == null)
        {
            _isChasing = false;
            return;
        }

        MoveToTarget();
    }
}