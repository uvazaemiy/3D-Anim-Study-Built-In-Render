using UnityEngine;
using UnityEditor;

public class LevelEditorTool : EditorWindow
{
    private GameObject objectToPlace;
    
    // Перейменував змінну, щоб не плутати ручний вимикач із кулдауном
    private bool isToolEnabled = true; 
    
    private float placementCooldown = 1f; // затримка в секундах
    private double lastPlacementTime = 0; // час останнього спавну

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorTool>("Level Editor");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Editor Tool", EditorStyles.boldLabel);

        objectToPlace = (GameObject)EditorGUILayout.ObjectField("Object to Place:", objectToPlace, typeof(GameObject), true);
        isToolEnabled = EditorGUILayout.Toggle("Enable Tool:", isToolEnabled);
        placementCooldown = EditorGUILayout.FloatField("Cooldown (sec):", placementCooldown);

        GUILayout.Space(10); // Відступ для краси

        // Розраховуємо час
        double timePassed = EditorApplication.timeSinceStartup - lastPlacementTime;
        float timeRemaining = Mathf.Max(0f, placementCooldown - (float)timePassed);

        // Кольоровий вивід статусу таймера
        if (timeRemaining > 0)
        {
            GUI.color = Color.yellow;
            EditorGUILayout.LabelField("Status:", $"Wait... {timeRemaining:F2} sec");
        }
        else
        {
            GUI.color = Color.green;
            EditorGUILayout.LabelField("Status:", "Ready to place!");
        }
        GUI.color = Color.white; // Повертаємо стандартний колір

        GUILayout.Space(10);

        if (GUILayout.Button("Reset Cooldown"))
        {
            lastPlacementTime = 0; // Миттєво скидає таймер
        }
    }

    private void Update()
    {
        // Оптимізація: перемальовуємо вікно ТІЛЬКИ тоді, коли йде кулдаун
        if (EditorApplication.timeSinceStartup - lastPlacementTime < placementCooldown)
        {
            Repaint();
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        // Якщо інструмент вимкнено або об'єкт не вибрано - нічого не робимо
        if (!isToolEnabled || objectToPlace == null) return;

        Event e = Event.current;

        // Перевіряємо клік лівою кнопкою миші
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            // Якщо кулдаун ще не пройшов - блокуємо спавн
            if (EditorApplication.timeSinceStartup - lastPlacementTime < placementCooldown)
            {
                return; 
            }

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Створюємо об'єкт (з підтримкою префабів)
                GameObject spawnedObj = PrefabUtility.InstantiatePrefab(objectToPlace) as GameObject;
                if (spawnedObj == null) spawnedObj = Instantiate(objectToPlace);

                spawnedObj.transform.position = hit.point;
                Undo.RegisterCreatedObjectUndo(spawnedObj, "Place Object");

                // Записуємо точний час створення
                lastPlacementTime = EditorApplication.timeSinceStartup;
                
                // Перехоплюємо клік, щоб не виділялися інші об'єкти на сцені
                e.Use(); 
            }
        }
    }
}