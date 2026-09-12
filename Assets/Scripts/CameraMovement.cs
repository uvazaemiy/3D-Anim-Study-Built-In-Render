using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed, rotationSpeed, maxHeight, minHeight;
    [SerializeField] private Camera cam;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0 && transform.position.y < minHeight) scroll = 0;
        else if (scroll < 0 && transform.position.y > maxHeight) scroll = 0;
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * Time.deltaTime;
        if (moveX != 0f || moveY != 0f || scroll != 0f)
        {
            rb.AddForce(transform.forward * moveY * movementSpeed + transform.right * moveX * movementSpeed + cam.transform.forward * scroll * movementSpeed);
        }

        float camX = Input.GetAxis("Tactical Horizontal") * Time.deltaTime;
        transform.Rotate(0, camX * rotationSpeed, 0);
    }
}