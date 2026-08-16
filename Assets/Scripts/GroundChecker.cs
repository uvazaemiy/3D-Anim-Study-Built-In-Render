using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float radius = 0.3f;
    [SerializeField] private Vector3 offset; // Зміщення до ніг
    [SerializeField] private LayerMask layerMask;
    
    public bool isGround = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Перевіряємо зону: центр сфери, радіус, шар землі
        isGround = Physics.CheckSphere(transform.position + offset, radius, layerMask);
        animator.SetBool("IsGround", isGround);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGround ? Color.green : Color.red;
        
        // Візуал тепер на 100% збігається з фізичною перевіркою
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}