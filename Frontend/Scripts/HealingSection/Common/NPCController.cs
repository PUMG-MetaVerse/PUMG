using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform[] destinations;
    private NavMeshAgent agent;
    private int currentDestinationIndex = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNextDestination();
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetNextDestination();
        }
    }

    private void SetNextDestination()
    {
        if (destinations.Length == 0)
        {
            return;
        }
        currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Length;
        agent.SetDestination(destinations[currentDestinationIndex].position);
    }
}
