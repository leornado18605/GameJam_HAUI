using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Stats")]
    public float attackRange = 1.5f; 
    public float damage = 10f;
    public float attackRate = 1f;   // 1 hit / giây
    
    [SerializeField] private EnemyAttack target;

    public EnemyAttack Target
    {
        get => target;
        set => target = value;
    }

    public LayerMask enemyLayer;

    private void Update()
    {
        target = FindEnemy();
    }

    

    public EnemyAttack FindEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (hits.Length > 0)
            return hits[0].GetComponent<EnemyAttack>();

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage()
    {
        if(target != null)
            target.TakeDamage(damage);
    }
}