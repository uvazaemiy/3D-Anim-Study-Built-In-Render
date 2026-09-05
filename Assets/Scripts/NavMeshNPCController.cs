using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshNPCController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float stopDistance;
    private NavMeshAgent npcAgent;
    
    private List<int> infinityList = new List<int>();

    private void Start()
    {
        npcAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

    }
}