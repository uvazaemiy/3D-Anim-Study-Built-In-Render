using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshNPCController : MonoBehaviour, ICharacter
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float stopDistance;
    public int damage { get; set; }
    
    private NavMeshAgent npcAgent;
    private int currentIndex = 0;

    private void Start()
    {
        npcAgent = GetComponent<NavMeshAgent>();
        
        npcAgent.SetDestination(waypoints[currentIndex].position);
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (npcAgent.remainingDistance < stopDistance)
        {
            currentIndex++;

            if (currentIndex >= waypoints.Length)
                currentIndex = 0;
            
            npcAgent.SetDestination(waypoints[currentIndex].position);
        }
    }

    public void Fight()
    {
        Debug.Log("NPC: Почали бійку!");
    }
}