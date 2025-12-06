
using UnityEngine;
public class AIInstaller : MonoBehaviour
{
    [SerializeField] private AgentProvider       provider;
    [SerializeField] private AIController wander;

    private void Awake()
    {
        wander.Initialize(provider);
    }
}