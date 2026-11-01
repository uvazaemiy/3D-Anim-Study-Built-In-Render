using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAction;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Активуємо подію");
            OnAction.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Підписуємо до події нову функцію");
            OnAction.AddListener(NewCodeAction);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Відписуємо від події якусь функцію");
            OnAction.RemoveListener(NewCodeAction);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            int count = OnAction.GetPersistentEventCount();
            Debug.Log("Кількість функцій на події У ІНСПЕКТОРІ: " + count);
        }
    }

    private void NewCodeAction()
    {
        Debug.Log("Подія у Коді");
    }

    public void NewInspectorAction()
    {
        Debug.Log("Подія у Інспекторі");
    }
}
