using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PathGenerator : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private LevelScript level;
    int lastColumn;
    int lastRow;
    [Header("Path")]
    public GameObject pathPrefab;
    [SerializeField] private Transform paths;
    [SerializeField] private BuildingPlacer placer;
    [Header("Grid")]
    public int rows;
    public int columns;
    public float cellSize;
    private bool[,] grid;
    private void Start()
    {
        grid = new bool[rows, columns];
        GameObject path = Instantiate(pathPrefab, paths);
        Vector3 position = new Vector3(0f, 0f, Random.Range(0, rows) * cellSize);
        path.transform.position = position;
        level.spawn = new Vector3(-cellSize, 1f, position.z);
        lastColumn = 0;
        lastRow = Mathf.FloorToInt(position.z / cellSize);
        grid[lastRow, lastColumn] = true;
        Generate();
    }

    private void Generate()
    {
        int column = lastColumn;
        int row = lastRow;

        int direction = Random.Range(0, 3);
        switch (direction)
        {
            case 0:
                column++;
                break;
            case 1:
                row--;
                break;
            case 2:
                row++;
                break;
            default:
                Debug.LogError("Direction out of bounds");
                break;
        }

        if (!(column >= columns || row < 0 || row >= rows || grid[row, column]))
        {
            GameObject path = Instantiate(pathPrefab, paths);
            Vector3 position = new Vector3(column * cellSize, 0f, row * cellSize);
            path.transform.position = position;
            lastColumn = column;
            lastRow = row;
            grid[lastRow, lastColumn] = true;
            placer.occupiedPositions.Add(Vector3Int.RoundToInt(position));
        }
        if (lastColumn < columns - 1) Generate();
    }
}