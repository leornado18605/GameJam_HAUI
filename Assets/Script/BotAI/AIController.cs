using UnityEngine;
using UnityEngine.AI;

public partial class AIController : MonoBehaviour
{
    [Header("Wander Settings")]
    [SerializeField] private float moveRadius = 10f;
    [SerializeField] private float newPointDelay = 2f;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 12f;
    [SerializeField] private string targetTag = "Player";

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 15f;

    [SerializeField] private Animator anim;
    
    private NavMeshAgent _agent;
    private float        _timer;

    private Transform _target;
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
    }
}