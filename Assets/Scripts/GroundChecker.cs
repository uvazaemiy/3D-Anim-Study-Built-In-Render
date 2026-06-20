using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool isGround = true;

    
    
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        isGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGround = false;
    }
}
