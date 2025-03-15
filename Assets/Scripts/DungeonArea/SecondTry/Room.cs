// using UnityEngine;

// public class Room : MonoBehaviour
// {
//     public Vector2Int position;
//     public string type;

//     // Referensi ke dinding
//     public GameObject wallNorth;
//     public GameObject wallSouth;
//     public GameObject wallEast;
//     public GameObject wallWest;

//     // private void Awake()
//     // {
//     //     // Jika referensi dinding null, cari ulang berdasarkan nama anak
//     //     if (wallNorth == null) wallNorth = transform.Find("North")?.gameObject;
//     //     if (wallSouth == null) wallSouth = transform.Find("South")?.gameObject;
//     //     if (wallEast == null) wallEast = transform.Find("East")?.gameObject;
//     //     if (wallWest == null) wallWest = transform.Find("West")?.gameObject;

//     //     if (wallNorth == null) Debug.LogError($"Wall North tidak ditemukan di {gameObject.name}");
//     //     if (wallSouth == null) Debug.LogError($"Wall South tidak ditemukan di {gameObject.name}");
//     //     if (wallEast == null) Debug.LogError($"Wall East tidak ditemukan di {gameObject.name}");
//     //     if (wallWest == null) Debug.LogError($"Wall West tidak ditemukan di {gameObject.name}");
//     // }

//     public void RemoveWall(Vector2Int direction)
//     {
//         Debug.Log($"[Room: {gameObject.name}] Mencoba menghapus dinding arah {direction}");

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
//         else
//         {
//             Debug.Log($"[Room: {gameObject.name}] Gagal menghapus dinding arah {direction}, mungkin tidak ada dinding di posisi itu.");
//         }
//     }

// }


using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int position;
    public string type;

    public GameObject wallNorth, wallSouth, wallEast, wallWest;
    public GameObject door; // Pintu masuk & keluar

    public void InitializeDoor()
    {
        // Jika south wall ada, hancurkan untuk jadi pintu
        if (wallSouth != null)
        {
            door = wallSouth;
            Debug.Log($"Door dibuat di {position}");
        }
    }

    public void RemoveWall(Vector2Int direction)
    {
        if (direction == Vector2Int.up && wallNorth != null)
        {
            Destroy(wallNorth);
        }
        else if (direction == Vector2Int.down && door != null)
        {
            Debug.Log("Tidak menghancurkan pintu yang sudah ada.");
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
