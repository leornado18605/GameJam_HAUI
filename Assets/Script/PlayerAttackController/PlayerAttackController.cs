using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Stats")]
    public GameObject hitEffect;
    public float attackRange = 1.5f; 
    public float damage = 10f;
    public float heath = 100f;
    public float attackRate = 1f;   // 1 hit / giây
    
    [SerializeField] private EnemyAttack target;

    public EnemyAttack Target
    {
        get => target;
        set => target = value;
    }

    public LayerMask enemyLayer;
    public LayerMask weaponLayer;

    private void Update()
    {
        target = FindEnemy();
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.GetComponent<PlayerController>().TakeWeapon(GetWeapon());
        }
    }

    

    public EnemyAttack FindEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (hits.Length > 0)
            return hits[0].GetComponent<EnemyAttack>();

        return null;
    }

    public GameObject GetWeapon()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, weaponLayer);

        if (hits.Length > 0)
        {
            Debug.Log(hits[0].gameObject.name);
            return hits[0].gameObject;
        }
            

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage()
    {
        if (target != null)
        {
            target.GetDamage(damage);
            var effect = Instantiate(hitEffect, target.transform.position, Quaternion.identity);
            effect.GetComponent<ParticleSystem>().Play();
            effect.transform.localScale *= 3.0f;
            Destroy(effect,1f);
        }
            
    }
}