using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [Header("За кем следить")]
    [SerializeField] private Transform target; // Сюда перетащи car (1)

    [Header("Настройки дистанции")]
    [SerializeField] private float distance = 6.0f;     // Насколько далеко камера от машины
    [SerializeField] private float heightOffset = 1.0f; // Смещение высоты цели (чтобы смотреть чуть выше центра машины)

    [Header("Чувствительность мыши")]
    [SerializeField] private float sensitivityX = 3.0f; // Скорость вращения по горизонтали
    [SerializeField] private float sensitivityY = 2.0f; // Скорость вращения по вертикали

    [Header("Ограничения по высоте")]
    [SerializeField] private float minimumY = -10f;     // Чтобы камера не уходила сквозь землю под машину
    [SerializeField] private float maximumY = 60f;      // Максимальный угол наклона сверху

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // Инициализируем начальный поворот камеры на основе текущего положения
        Vector3 angles = transform.eulerAngles;
        rotationX = angles.y;
        rotationY = angles.x;
    }

    void LateUpdate()
    {
        if (!target) return;

        // 1. Считываем движение мыши
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;

        // 2. Ограничиваем наклон по вертикали, чтобы камера не перевернулась вверх тормашками
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        // 3. Вычисляем поворот и позицию
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        
        // Точка, на которую направлена камера (центр машины + смещение по высоте)
        Vector3 targetPosition = target.position + Vector3.up * heightOffset;

        // Отодвигаем камеру назад на расстояние distance с учетом полученного вращения
        Vector3 position = targetPosition - (rotation * Vector3.forward * distance);

        // 4. Применяем координаты и поворот к камере
        transform.position = position;
        transform.rotation = rotation;
    }
}