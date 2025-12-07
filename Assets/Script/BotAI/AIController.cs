using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("Wander Settings")]
    [SerializeField] private float moveRadius = 10f;
    [SerializeField] private float newPointDelay = 2f;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 12f;
    [SerializeField] private float detectionDistance = 2f;
    [SerializeField] private string targetTag = "Player";

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 15f;

    [SerializeField] private Animator anim;
    
    private NavMeshAgent _agent;
    private float        _timer;

    private Transform _target;

    public Transform Target
    {
        get => _target;
        set => _target = value;
    }

    private bool      _isChasing = false;

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
        if(anim != null)
            anim = GetComponent<Animator>();
        _timer = newPointDelay;
        MoveToRandomPoint();
    }

    private void Update()
    {
        if (_agent == null || !_agent.isOnNavMesh)
            return;

        DetectTarget();

        if (_isChasing)
            ChaseBehavior();
        else
            WanderBehavior();
        
        Attack();
        
    }

    public void Die()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        anim.SetTrigger("isDeath");
    }
    
    public void Hit()
    {
        anim.SetTrigger("isHit");
    }

    public void Attack()
    {
        if (_target != null && Vector3.Distance(transform.position, _target.position) <= detectionDistance)
        {
            anim.SetTrigger("isAttack");
            // 👉 Nếu có target thì xoay về phía target
            
                Vector3 dir = (_target.gameObject.transform.position - transform.position).normalized;
                dir.y = 0; 

                if (dir != Vector3.zero)
                {
                    Quaternion lookRot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 0.7f);
                }
            
        }
    }
    
    public void TakeDamage(float damage)
    {

        if (_target != null && Vector3.Distance(transform.position, _target.position) <= detectionDistance)
        {
            _target.GetComponent<PlayerAttackController>().GetDamage(damage);
            Debug.Log(_target.GetComponent<PlayerAttackController>().heath);
        }
    }
    
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
    
    private void ChaseBehavior()
    {
        if (_target == null)
        {
            _isChasing = false;
            return;
        }

        MoveToTarget();
    }
    
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