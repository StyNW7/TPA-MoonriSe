// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class RandomTreeSpawnTerrain : MonoBehaviour
// {
//     public GameObject treePrefab;         // Prefab pohon (bisa satu atau array, disesuaikan)
//     public int treeCount = 10;              // Jumlah pohon yang akan di-spawn
//     public Vector2 areaSize = new Vector2(20, 20); // Ukuran area spawn
//     public float minSpacing = 2f;           // Jarak minimum antar pohon
//     public float raycastHeight = 100f;      // Ketinggian awal untuk raycast ke terrain
//     public float greeneryThreshold = 0.5f;  // Threshold untuk menentukan area hijau (0 - 1)

//     // Referensi Terrain (pastikan terrain ada di scene dan di-assign)
//     public Terrain terrain;

//     void Start()
//     {
//         SpawnTrees();
//     }

//     void SpawnTrees()
//     {
//         for (int i = 0; i < treeCount; i++)
//         {
//             Vector3 randomPosition = GetRandomValidPosition();
//             if (randomPosition != Vector3.zero)
//             {
//                 // Spawning dengan rotasi acak di sumbu Y
//                 Instantiate(treePrefab, randomPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
//             }
//         }
//     }

//     Vector3 GetRandomValidPosition()
//     {
//         Vector3 position = Vector3.zero;
//         int maxAttempts = 20; // Mencegah loop tak terbatas

//         while (maxAttempts-- > 0)
//         {
//             // Pilih posisi acak di area spawn
//             float x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
//             float z = Random.Range(-areaSize.y / 2, areaSize.y / 2);
//             Vector3 randomPos = new Vector3(x, raycastHeight, z) + transform.position;

//             // Raycast ke bawah untuk mendapatkan titik di terrain
//             if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
//             {
//                 // Pastikan objek yang terkena raycast adalah terrain
//                 if (hit.collider.gameObject == terrain.gameObject)
//                 {
//                     // Dapatkan tinggi terrain tepat di titik tersebut
//                     float terrainHeight = hit.point.y;
//                     position = new Vector3(randomPos.x, terrainHeight, randomPos.z);

//                     // Lakukan validasi untuk memastikan posisi berada di area greenery
//                     if (IsGreenery(position))
//                     {
//                         // Periksa juga spacing untuk mencegah overlap (opsional)
//                         if (!Physics.CheckSphere(position, minSpacing))
//                         {
//                             return position;
//                         }
//                     }
//                 }
//             }
//         }

//         // Jika tidak ditemukan posisi yang valid, kembalikan Vector3.zero
//         return Vector3.zero;
//     }

//     // Fungsi untuk menentukan apakah posisi berada di area greenery
//     bool IsGreenery(Vector3 position)
//     {
//         // Pastikan terrain dan TerrainData tersedia
//         if (terrain == null) return false;
//         TerrainData terrainData = terrain.terrainData;
//         if (terrainData == null) return false;

//         // Konversi posisi dunia ke koordinat alphamap terrain
//         Vector3 terrainPos = terrain.transform.position;
//         int mapX = Mathf.RoundToInt((position.x - terrainPos.x) / terrainData.size.x * terrainData.alphamapWidth);
//         int mapZ = Mathf.RoundToInt((position.z - terrainPos.z) / terrainData.size.z * terrainData.alphamapHeight);

//         // Ambil splat map 1x1 di posisi tersebut
//         float[,,] alphamaps = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

//         // Asumsikan indeks 0 adalah area hijau dan indeks 1 adalah area pasir
//         float greenValue = alphamaps[0, 0, 0];  // Nilai tekstur hijau
//         // Jika greenValue melebihi threshold, maka posisi dianggap berada di area greenery
//         return greenValue >= greeneryThreshold;
//     }
// }
