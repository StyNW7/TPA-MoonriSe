using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public float cellSize = 1f; // Ukuran grid
    public Vector2Int gridSize = new Vector2Int(10, 10); // Ukuran total grid

    public static GridSystem Instance;
    private void Awake()
    {
        Instance = this;
    }

    
    public Vector3 GetNearestGridPosition(Vector3 position)
    {
        float x = Mathf.Round(position.x / cellSize) * cellSize;
        float z = Mathf.Round(position.z / cellSize) * cellSize;
        return new Vector3(x, position.y, z);
    }

    public Vector3 GetSnappedPosition(Vector3 rawPosition)
    {
        float x = Mathf.Round(rawPosition.x / cellSize) * cellSize;
        float z = Mathf.Round(rawPosition.z / cellSize) * cellSize;
        return new Vector3(x, rawPosition.y, z);
    }

    public bool IsPositionInsideGrid(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int z = Mathf.FloorToInt(position.z / cellSize);
        return x >= 0 && x < gridSize.x && z >= 0 && z < gridSize.y;
    }
}
