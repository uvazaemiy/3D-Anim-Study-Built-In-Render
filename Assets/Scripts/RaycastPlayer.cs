using UnityEngine;

public class RaycastPlayer : MonoBehaviour
{
    private Ray rayFromCamera;
    private Ray rayForward;

    private void OnDrawGizmos()
    {
        rayFromCamera = Camera.main.ScreenPointToRay(Input.mousePosition);
        rayForward = new  Ray(transform.position, transform.forward);
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayFromCamera);
        Gizmos.DrawRay(rayForward);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.15f, 0.3f);
    }
}
