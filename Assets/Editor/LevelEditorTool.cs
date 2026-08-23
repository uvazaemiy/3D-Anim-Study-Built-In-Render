using UnityEngine;
using UnityEditor;

public class LevelEditorTool : EditorWindow
{
    private GameObject objectToPlace;
    private bool canPlaceObject = true;
    private float placementCooldown = 1f; // час відновлення в секундах
    private float timeSinceLastPlacement = 0f;

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorTool>("Level Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Editor Tool", EditorStyles.boldLabel);

        objectToPlace = (GameObject)EditorGUILayout.ObjectField("Object to Place:", objectToPlace, typeof(GameObject), false);

        if (GUILayout.Button("Toggle Placement"))
        {
            canPlaceObject = !canPlaceObject;
        }

        if (GUILayout.Button("Reset Placement"))
        {
            canPlaceObject = true;
        }

        if (objectToPlace)
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
        else
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
    }


    private void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && canPlaceObject)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(objectToPlace, hit.point, Quaternion.identity);
                canPlaceObject = false;
                timeSinceLastPlacement = 0f;
                EditorApplication.update += UpdatePlacementCooldown;
            }

            sceneView.Repaint();
        }
    }

    private void UpdatePlacementCooldown()
    {
        timeSinceLastPlacement += Time.deltaTime;

        if (timeSinceLastPlacement >= placementCooldown)
        {
            canPlaceObject = true;
            EditorApplication.update -= UpdatePlacementCooldown;
        }
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        EditorApplication.update -= UpdatePlacementCooldown;
    }
}