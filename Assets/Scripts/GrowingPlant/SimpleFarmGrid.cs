using System.Collections.Generic;
using UnityEngine;

public class SimpleFarmGrid : MonoBehaviour
{

    public static SimpleFarmGrid Instance;
    private void Awake() { Instance = this; }

    public List<DirtTile> allDirtTiles = new List<DirtTile>();

    public void RegisterDirt(DirtTile tile)
    {
        if (!allDirtTiles.Contains(tile))
            allDirtTiles.Add(tile);
    }

    public DirtTile GetNearestDirt(Vector3 playerPosition, float maxDistance)
    {
        DirtTile nearest = null;
        float minDist = maxDistance;
        
        foreach (var tile in allDirtTiles)
        {
            float dist = Vector3.Distance(playerPosition, tile.transform.position);
            if (dist < minDist && tile.IsEmpty())
            {
                minDist = dist;
                nearest = tile;
            }
        }
        return nearest;
    }
    
}
