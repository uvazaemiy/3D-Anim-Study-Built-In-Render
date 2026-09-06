using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = System.Object;

public class resetRotation : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private VehicleEntry ve;
    private void Start()
    {
        // Автоматически находим Rigidbody на машине
        rb = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && ve.inCar == true)
        {
           
            car.transform.position = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
            car.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}