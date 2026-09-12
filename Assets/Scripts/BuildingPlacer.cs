using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class BuildingPlacer : MonoBehaviour
{
    public bool needToBuild;
    public GameObject buildingPrefab;
    public LevelScript level;
    public int price;
    [SerializeField] private int gridSize;
    [SerializeField] private GameObject placerPrefab;
    private Transform placer;

    public List<Vector3Int> occupiedPositions = new List<Vector3Int>();

    private void Update()
    {
        if (!needToBuild)
        {
            return;
        }
        if (Input.GetMouseButtonDown(1)) needToBuild = false;
        if (!placer) placer = Instantiate(placerPrefab).transform;
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3Int position = Vector3Int.RoundToInt(hit.point);
                position.x = Mathf.RoundToInt(position.x / gridSize) * gridSize;
                position.y = 0;
                position.z = Mathf.RoundToInt(position.z / gridSize) * gridSize;
                placer.position = position;
                if (Input.GetKeyDown(KeyCode.R)) placer.Rotate(0, 90, 0);
                if (!occupiedPositions.Contains(position))
                {
                    placer.GetComponent<MeshRenderer>().material.color = new Color32(130, 130, 130, 125);
                    if (Input.GetMouseButtonDown(0)) PlaceBuiding(position);
                }
                else placer.GetComponent<MeshRenderer>().material.color = new Color32(255, 0, 0, 125);
            }
        }
    }

    private void PlaceBuiding(Vector3Int position)
    {
        Instantiate(buildingPrefab, position, placer.rotation);
        Destroy(placer.gameObject);
        occupiedPositions.Add(position);
        level.FillScoreText(price);
        needToBuild = false;
    }
}
