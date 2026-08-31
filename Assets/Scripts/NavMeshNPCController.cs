using UnityEngine;
using UnityEngine.AI;

public class NavMeshNPCController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float stopDistance;
    private NavMeshAgent npcAgent;

    private void Start()
    {
        npcAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        /*for (int i = 0; i < waypoints.Length; i++)
        {
            npcAgent.SetDestination(waypoints[i].position);

            if (Vector3.Distance(npcAgent.transform.position, waypoints[i].position) < stopDistance)
                i++;

            if (i >= waypoints.Length)
                i = 0;
        }*/

        
        //TODO Домашнє завдання:
        
        int i = 0;

        while (true)
        {
            do
            {
                npcAgent.SetDestination(waypoints[i].position);
            } 
            while (Vector3.Distance(npcAgent.transform.position, waypoints[i].position) < stopDistance);

            i++;
            
            if (i >= waypoints.Length)
                i = 0;
        }
    }
}
