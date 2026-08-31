using UnityEngine;
using UnityEngine.AI;

public class NavMeshPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float stopDistance;
    private NavMeshAgent agent;

    private GameObject newEffect;
    private Vector3 targetPoint;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (newEffect != null)
                Destroy(newEffect);
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
                agent.SetDestination(targetPoint);
                newEffect = Instantiate(effectPrefab, hit.point, Quaternion.identity);
            }
        }
        
        if (Vector3.Distance(transform.position, targetPoint) < stopDistance)
            Destroy(newEffect);
    }
}
