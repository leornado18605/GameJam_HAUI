using UnityEngine;

public partial class AIController
{
    private void DetectTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        Transform found = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag(targetTag))
            {
                found = hit.transform;
                break;
            }
        }

        if (found != null)
        {
            _target    = found;
            _isChasing = true;
        }
        else
        {
            _target    = null;
            _isChasing = false;
        }
    }
}