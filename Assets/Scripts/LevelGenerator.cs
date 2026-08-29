using UnityEngine;
using UnityEditor;

public class LevelGeneratorWindow : EditorWindow
{
    // Джерело даних рівня
    private Texture2D levelMap;

    // Префаби
    private GameObject wallPrefab;
    private GameObject floorPrefab;
    private GameObject playerPrefab;
    private GameObject trapPrefab;

    // Налаштування сітки
    private float cellSize = 1f;
    private float wallHeight = 2f;
    private Transform levelParent;

    // Кольори-маркери
    private Color wallColor = Color.black;
    private Color floorColor = Color.white;
    private Color playerColor = Color.green;
    private Color trapColor = Color.red;
    private float colorTolerance = 0.1f;
    private bool floorUnderEverything = true;

    // Змінна для скролу у вікні
    private Vector2 scrollPos;
    private bool playerSpawnedThisRun = false;

    [MenuItem("Tools/Level Generator")]
    public static void ShowWindow()
    {
        GetWindow<LevelGeneratorWindow>("Level Generator");
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("Джерело даних рівня", EditorStyles.boldLabel);
        levelMap = (Texture2D)EditorGUILayout.ObjectField("Level Map (Texture2D)", levelMap, typeof(Texture2D), false);
        
        EditorGUILayout.Space();

        GUILayout.Label("Префаби", EditorStyles.boldLabel);
        wallPrefab = (GameObject)EditorGUILayout.ObjectField("Wall Prefab", wallPrefab, typeof(GameObject), false);
        floorPrefab = (GameObject)EditorGUILayout.ObjectField("Floor Prefab", floorPrefab, typeof(GameObject), false);
        playerPrefab = (GameObject)EditorGUILayout.ObjectField("Player Prefab", playerPrefab, typeof(GameObject), false);
        trapPrefab = (GameObject)EditorGUILayout.ObjectField("Trap Prefab", trapPrefab, typeof(GameObject), false);

        EditorGUILayout.Space();

        GUILayout.Label("Налаштування сітки", EditorStyles.boldLabel);
        cellSize = EditorGUILayout.FloatField(new GUIContent("Cell Size", "Розмір однієї клітинки у світових одиницях"), cellSize);
        wallHeight = EditorGUILayout.FloatField(new GUIContent("Wall Height", "Висота стіни відносно підлоги"), wallHeight);
        levelParent = (Transform)EditorGUILayout.ObjectField(new GUIContent("Level Parent", "Батьківський об'єкт"), levelParent, typeof(Transform), true);

        EditorGUILayout.Space();

        GUILayout.Label("Кольори-маркери", EditorStyles.boldLabel);
        wallColor = EditorGUILayout.ColorField("Wall Color", wallColor);
        floorColor = EditorGUILayout.ColorField("Floor Color", floorColor);
        playerColor = EditorGUILayout.ColorField("Player Color", playerColor);
        trapColor = EditorGUILayout.ColorField("Trap Color", trapColor);
        colorTolerance = EditorGUILayout.Slider(new GUIContent("Color Tolerance", "Допустиме відхилення кольору (0 = точний збіг)"), colorTolerance, 0f, 0.5f);

        EditorGUILayout.Space();

        GUILayout.Label("Додатково", EditorStyles.boldLabel);
        floorUnderEverything = EditorGUILayout.Toggle("Floor Under Everything", floorUnderEverything);

        EditorGUILayout.Space();

        // Кнопки дій
        GUILayout.BeginHorizontal();
        
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Generate Level", GUILayout.Height(30)))
        {
            GenerateLevel();
        }

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear Level", GUILayout.Height(30)))
        {
            ClearLevel();
        }
        GUI.backgroundColor = Color.white;

        GUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    private void GenerateLevel()
    {
        if (levelMap == null)
        {
            Debug.LogError("[LevelGenerator] Не призначена текстура рівня (levelMap).");
            return;
        }

        if (!levelMap.isReadable)
        {
            Debug.LogError("[LevelGenerator] Текстура не позначена як Read/Write Enabled у налаштуваннях імпорту.");
            return;
        }

        // Групуємо дії для Ctrl+Z
        Undo.SetCurrentGroupName("Generate Level");
        int group = Undo.GetCurrentGroup();

        ClearLevel();

        if (levelParent == null)
        {
            GameObject parentObj = new GameObject("Level");
            levelParent = parentObj.transform;
            Undo.RegisterCreatedObjectUndo(parentObj, "Create Level Parent");
        }

        playerSpawnedThisRun = false;

        Color[] pixels = levelMap.GetPixels();
        int width = levelMap.width;
        int height = levelMap.height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixel = pixels[y * width + x];

                if (pixel.a < 0.1f) continue;

                Vector3 worldPos = new Vector3(x * cellSize, 0f, y * cellSize);
                ProcessCell(pixel, worldPos, x, y);
            }
        }

        Undo.CollapseUndoOperations(group);
        Debug.Log($"[LevelGenerator] Рівень згенеровано: {width}x{height} клітинок.");
    }

    private void ProcessCell(Color pixel, Vector3 worldPos, int x, int y)
    {
        if (ColorsMatch(pixel, wallColor))
        {
            if (floorUnderEverything) SpawnObject(floorPrefab, worldPos, $"Floor_{x}_{y}");
            
            Vector3 wallPos = worldPos + Vector3.up * (wallHeight * 0.5f);
            GameObject wall = SpawnObject(wallPrefab, wallPos, $"Wall_{x}_{y}");
            if (wall != null)
                wall.transform.localScale = new Vector3(wall.transform.localScale.x, wallHeight, wall.transform.localScale.z);
        }
        else if (ColorsMatch(pixel, floorColor))
        {
            SpawnObject(floorPrefab, worldPos, $"Floor_{x}_{y}");
        }
        else if (ColorsMatch(pixel, playerColor))
        {
            if (floorUnderEverything) SpawnObject(floorPrefab, worldPos, $"Floor_{x}_{y}");
            
            if (!playerSpawnedThisRun)
            {
                Vector3 spawnPos = worldPos + Vector3.up * 0.5f;
                SpawnObject(playerPrefab, spawnPos, "Player");
                playerSpawnedThisRun = true;
            }
            else
            {
                Debug.LogWarning("[LevelGenerator] На карті знайдено декілька точок спавну гравця. Використано першу.");
            }
        }
        else if (ColorsMatch(pixel, trapColor))
        {
            if (floorUnderEverything) SpawnObject(floorPrefab, worldPos, $"Floor_{x}_{y}");
            
            Vector3 trapPos = worldPos + Vector3.up * 0.05f;
            SpawnObject(trapPrefab, trapPos, $"Trap_{x}_{y}");
        }
    }

    private bool ColorsMatch(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) <= colorTolerance &&
               Mathf.Abs(a.g - b.g) <= colorTolerance &&
               Mathf.Abs(a.b - b.b) <= colorTolerance;
    }

    // Універсальний метод спавну для Editor
    private GameObject SpawnObject(GameObject prefab, Vector3 pos, string objName)
    {
        if (prefab == null) return null;

        GameObject obj;
        // Перевіряємо, чи є об'єкт префабом. Якщо так, спавнимо через PrefabUtility для збереження зв'язку
        if (PrefabUtility.IsPartOfPrefabAsset(prefab))
        {
            obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, levelParent);
            obj.transform.position = pos;
        }
        else
        {
            obj = Instantiate(prefab, pos, Quaternion.identity, levelParent);
        }

        obj.name = objName;
        Undo.RegisterCreatedObjectUndo(obj, "Spawn Object");
        return obj;
    }

    private void ClearLevel()
    {
        if (levelParent == null) return;

        Undo.SetCurrentGroupName("Clear Level");
        int group = Undo.GetCurrentGroup();

        // Видаляємо об'єкти з кінця до початку
        for (int i = levelParent.childCount - 1; i >= 0; i--)
        {
            Undo.DestroyObjectImmediate(levelParent.GetChild(i).gameObject);
        }
        
        Undo.CollapseUndoOperations(group);
    }
}