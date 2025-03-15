using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;
    public LayerMask groundLayer;

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        for (int i = 0; i < 10; i++) // Spawn 10 pohon
        {
            Vector3 randomPosition = new Vector3(Random.Range(200, 220), 20, Random.Range(200, 220));
            RaycastHit hit;

            // Raycast ke bawah untuk mencari posisi tanah
            if (Physics.Raycast(randomPosition, Vector3.down, out hit, 20f, groundLayer))
            {
                // Spawn pohon di titik yang terkena raycast
                Instantiate(treePrefab, hit.point, Quaternion.identity);
            }
        }
    }
}
