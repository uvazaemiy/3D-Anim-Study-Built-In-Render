using UnityEngine;

public class RaycastPlayer : MonoBehaviour
{
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("Попали в: " + hit.transform.name);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log(transform.name + " був натиснутий!");
    }
}
