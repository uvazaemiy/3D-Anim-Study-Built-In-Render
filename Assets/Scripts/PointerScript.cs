using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerScript : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 offset;

    void FixedUpdate()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
        transform.position = ClampToScreen(screenPos, GetComponent<RectTransform>().rect.width / 2, GetComponent<RectTransform>().rect.height / 2);
    }

    private Vector3 ClampToScreen(Vector3 pos, float elementWidth, float elementHeight)
    {
        float minX = elementWidth;
        float maxX = Screen.width - elementWidth;
        float minY = elementHeight;
        float maxY = Screen.height - elementHeight;

        Vector3 viewportPos = cam.ScreenToViewportPoint(pos);

        viewportPos.x = Mathf.Clamp(viewportPos.x, minX / Screen.width, maxX / Screen.width);
        viewportPos.y = Mathf.Clamp(viewportPos.y, minY / Screen.height, maxY / Screen.height);

        return cam.ViewportToScreenPoint(viewportPos);
    }
}
