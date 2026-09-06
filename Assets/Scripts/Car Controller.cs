using UnityEngine;

public class CarController : MonoBehaviour
{
    [System.Serializable]
    public struct WheelData
    {
        public WheelCollider collider; // Коллайдер из WheelsCol
        public Transform mesh;         // 3D-моделька из Wheels
        public Vector3 rotationOffset; // ЛОКАЛЬНЫЙ ПОВОРОТ ДЛЯ ЭТОГО КОЛЕСА
    }

    [SerializeField] private VehicleEntry ve;
    [Header("Колеса")]
    [SerializeField] private WheelData frontLeft;
    [SerializeField] private WheelData frontRight;
    [SerializeField] private WheelData backLeft;
    [SerializeField] private WheelData backRight;

    [Header("Настройки")]
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float brakeForce = 3000f;
  //  [SerializeField] private Vector3 customCenterOfMass = new Vector3(0, -0.5f, 0);
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody не найден на машине!");
            enabled = false;
            return;
        }
    }
    private void Update()
    {
        if (rb == null) return; // ← ДОБАВИТЬ!
    
        if (ve.inCar == false)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    
    private void FixedUpdate()
    {
        float motor = motorForce * Input.GetAxis("Vertical");
        float steering = maxSteerAngle * Input.GetAxis("Horizontal");
        float brake = Input.GetKey(KeyCode.Space) ? brakeForce : 0f;

        // Движение и тормоза
        ApplyPhysics(frontLeft.collider, motor, steering, brake);
        ApplyPhysics(frontRight.collider, motor, steering, brake);
        ApplyPhysics(backLeft.collider, motor, 0f, brake); // Задние не поворачивают
        ApplyPhysics(backRight.collider, motor, 0f, brake);

        // Обновляем визуал колес
        UpdateWheelVisual(frontLeft);
        UpdateWheelVisual(frontRight);
        UpdateWheelVisual(backLeft);
        UpdateWheelVisual(backRight);

    }

    private void ApplyPhysics(WheelCollider col, float motor, float steer, float brake)
    {
        if (col == null) return;
        col.motorTorque = motor;
        col.steerAngle = steer;
        col.brakeTorque = brake;
    }

    private void UpdateWheelVisual(WheelData wheel)
    {
        if (wheel.collider == null || wheel.mesh == null) return;

        Vector3 pos;
        Quaternion rot;
        wheel.collider.GetWorldPose(out pos, out rot);
        
        wheel.mesh.position = pos;
        // Применяем индивидуальный поворот для конкретного колеса
        wheel.mesh.rotation = rot * Quaternion.Euler(wheel.rotationOffset);
    }
 //   void OnDrawGizmosSelected()
 //   {
    //    if (rb == null) rb = GetComponent<Rigidbody>();
        
   //     Gizmos.color = Color.red;
        // Показываем точку с учетом текущей позиции объекта
  //      Vector3 worldCoM = rb.centerOfMass;
     //   Gizmos.DrawSphere(worldCoM, 1f);
   // }
}

