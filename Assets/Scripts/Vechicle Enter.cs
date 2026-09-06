
using UnityEngine;

public class VehicleEntry : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    [SerializeField] private GameObject player;          // Твой персонаж
    [SerializeField] private MonoBehaviour carController; // Скрипт CarController на машине
    [SerializeField] private GameObject playerCamera;   // Камера персонажа
    [SerializeField] private GameObject carCamera;      // Камера машины (кинематографичная или сзади)
    [SerializeField] private Transform exitPoint;       // Пустой объект рядом с дверью, откуда выйдет игрок

    [Header("Клавиша действия")]
    [SerializeField] private KeyCode actionKey = KeyCode.E;

    private bool isPlayerNear = false;
    public bool inCar = false;

    void Start()
    {
        // На старте игры мы ходим пешком
        carController.enabled = false;
        if (carCamera != null) carCamera.SetActive(false);
        
    }

    void Update()
    {

        // Если стоим рядом с машиной, пешком, и нажали E -> Садимся
        if (isPlayerNear && !inCar && Input.GetKeyDown(actionKey))
        {
            EnterCar();
        }
        // Если уже едем в машине и нажали E -> Выходим
        else if (inCar && Input.GetKeyDown(actionKey))
        {
            ExitCar();
        }
    }

    private void EnterCar()
    {
        inCar = true;
        

        // 1. Прячем игрока и выключаем его камеру
        player.SetActive(false);
        if (playerCamera != null) playerCamera.SetActive(false);

        // 2. Включаем управление тачкой и камеру тачки
        carController.enabled = true;
        if (carCamera != null) carCamera.SetActive(true);
    }

    private void ExitCar()
    {
        inCar = false;

        // 1. Выключаем управление машиной
        carController.enabled = false;
        if (carCamera != null) carCamera.SetActive(false);

        // 2. Останавливаем Rigidbody МАШИНЫ (не VehicleEntry!)
        // Rigidbody находится на том же объекте, что и carController
        Rigidbody rb = carController.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            Debug.LogError("Rigidbody не найден на машине!");
        }

        // 3. Возвращаем игрока
        player.transform.position = exitPoint.position;
        player.SetActive(true);
        if (playerCamera != null) playerCamera.SetActive(true);
    }

    // Проверяем, вошел ли игрок в зону триггера двери
    private void OnTriggerEnter(Collider other)
    {
        // Убедись, что у твоего персонажа в инспекторе стоит Tag "Player"
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    // Игрок отошел от двери
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}