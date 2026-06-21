using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool isGround = true;

    private Animator animator;
    

    
    
    
    

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        isGround = true;
        animator.SetBool("IsGround", isGround);
    }

    private void OnTriggerExit(Collider other)
    {
        isGround = false;
        animator.SetBool("IsGround", isGround);
    }
}
