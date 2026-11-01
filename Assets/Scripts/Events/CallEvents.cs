using UnityEngine;

public class CallEvents : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Events.OnEvent.Invoke();
    }
}
