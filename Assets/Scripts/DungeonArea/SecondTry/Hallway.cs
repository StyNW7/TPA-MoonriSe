// using UnityEngine;

// public class Hallway : MonoBehaviour
// {
//     public Vector2Int position;

//     public GameObject wallNorth;
//     public GameObject wallSouth;
//     public GameObject wallEast;
//     public GameObject wallWest;

//     private void Awake()
//     {
//         // Pastikan referensi dinding tetap ada setelah instantiate
//         if (wallNorth == null) wallNorth = transform.Find("North")?.gameObject;
//         if (wallSouth == null) wallSouth = transform.Find("South")?.gameObject;
//         if (wallEast == null) wallEast = transform.Find("East")?.gameObject;
//         if (wallWest == null) wallWest = transform.Find("West")?.gameObject;

//         if (wallNorth == null) Debug.LogError($"Wall North tidak ditemukan di {gameObject.name}");
//         if (wallSouth == null) Debug.LogError($"Wall South tidak ditemukan di {gameObject.name}");
//         if (wallEast == null) Debug.LogError($"Wall East tidak ditemukan di {gameObject.name}");
//         if (wallWest == null) Debug.LogError($"Wall West tidak ditemukan di {gameObject.name}");
//     }

//     public void RemoveWall(Vector2Int direction, bool isDeadEnd = false)
//     {
//         Debug.Log($"[Hallway: {gameObject.name}] Mencoba menghapus dinding arah {direction}");

//         if (direction == Vector2Int.up && wallNorth != null)
//         {
//             Debug.Log("North Wall terhapus!");
//             Destroy(wallNorth);
//             wallNorth = null;
//         }
//         else if (direction == Vector2Int.down && wallSouth != null)
//         {
//             Debug.Log("South Wall terhapus!");
//             Destroy(wallSouth);
//             wallSouth = null;
//         }
//         else if (direction == Vector2Int.left && wallWest != null)
//         {
//             Debug.Log("West Wall terhapus!");
//             Destroy(wallWest);
//             wallWest = null;
//         }
//         else if (direction == Vector2Int.right && wallEast != null)
//         {
//             Debug.Log("East Wall terhapus!");
//             Destroy(wallEast);
//             wallEast = null;
//         }

//         // Jika hallway berada di ujung, hancurkan dinding tambahan ke kanan atau depan
//         if (isDeadEnd)
//         {
//             if (direction == Vector2Int.right && wallNorth != null)
//             {
//                 Debug.Log("Dead-End Hallway: Hapus Wall Utara!");
//                 Destroy(wallNorth);
//                 wallNorth = null;
//             }
//             if (direction == Vector2Int.up && wallEast != null)
//             {
//                 Debug.Log("Dead-End Hallway: Hapus Wall Timur!");
//                 Destroy(wallEast);
//                 wallEast = null;
//             }
//         }
//     }
// }



using UnityEngine;

public class Hallway : MonoBehaviour
{
    public Vector2Int position;

    public GameObject wallNorth, wallSouth, wallEast, wallWest;

    public void RemoveWall(Vector2Int direction)
    {
        if (direction == Vector2Int.up && wallNorth != null)
        {
            Destroy(wallNorth);
        }
        else if (direction == Vector2Int.down && wallSouth != null)
        {
            Destroy(wallSouth);
        }
        else if (direction == Vector2Int.left && wallWest != null)
        {
            Destroy(wallWest);
        }
        else if (direction == Vector2Int.right && wallEast != null)
        {
            Destroy(wallEast);
        }
    }
}
