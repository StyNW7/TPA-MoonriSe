using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject water;
    public GameObject flowerPrefab;

    void Start()
    {
        GenerateIsland();
        DecorateTerrain();
    }

    void GenerateIsland()
    {
        GameObject sea = Instantiate(water, new Vector3(100, 0, 100), Quaternion.identity);
        sea.transform.localScale = new Vector3(50, 1, 50); // Ukuran air
    }

    void DecorateTerrain()
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 position = new Vector3(Random.Range(200, 220), 0, Random.Range(200, 220));
            Instantiate(flowerPrefab, position, Quaternion.identity);
        }
    }
}
