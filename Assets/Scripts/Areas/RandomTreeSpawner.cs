using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTreeSpawner : MonoBehaviour
{
    
    public GameObject treePrefabs; // Array pohon yang akan di-spawn
    public int treeCount = 10; // Jumlah pohon yang ingin di-spawn
    public Vector2 areaSize = new Vector2(20, 20); // Ukuran area spawn
    public float minSpacing = 2f; // Jarak minimum antar pohon

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject treePrefab = treePrefabs;
            // GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            Instantiate(treePrefab, randomPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 position;
        int maxAttempts = 10; // Cegah infinite loop

        do
        {
            float x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
            float z = Random.Range(-areaSize.y / 2, areaSize.y / 2);
            position = new Vector3(x, 0, z) + transform.position;

            maxAttempts--;
            if (maxAttempts <= 0) break; // Hindari loop tak terbatas

        } while (Physics.CheckSphere(position, minSpacing)); // Cek overlap dengan objek lain

        return position;
    }

}
