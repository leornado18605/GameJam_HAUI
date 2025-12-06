using UnityEngine;
using UnityEngine.AI;

//
public class AgentProvider : MonoBehaviour, IAgentProvider
{
    [SerializeField] private NavMeshAgent agent;

    public NavMeshAgent Agent => agent;
}