//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ZombieSpawner : MonoBehaviour
//{

//    public GameObject zombiePrefab;

//    private Dictionary<Room.RoomType, int> zombieCounts = new Dictionary<Room.RoomType, int>
//    {
//        { Room.RoomType.Small, 1 },
//        { Room.RoomType.Medium, 2 },
//        { Room.RoomType.Large, 3 },
//        { Room.RoomType.Spawn, 0 },
//        { Room.RoomType.Exit, 0 }
//    };

//    public void SpawnZombies(Room room)
//    {
//        if (!zombieCounts.ContainsKey(room.roomType) || zombieCounts[room.roomType] == 0)
//            return;

//        int zombieCount = zombieCounts[room.roomType];
//        List<Vector2> spawnPoints = GetAvailableSpawnPoints(room, zombieCount);

//        foreach (var point in spawnPoints)
//        {
//            Instantiate(zombiePrefab, new Vector3(point.x, 0, point.y), Quaternion.identity);
//        }
//    }

//    private List<Vector2> GetAvailableSpawnPoints(Room room, int count)
//    {
//        List<Vector2> spawnPoints = new List<Vector2>();
//        HashSet<Vector2> usedPoints = new HashSet<Vector2>();

//        for (int i = 0; i < count; i++)
//        {
//            Vector2 randomPoint;
//            do
//            {
//                randomPoint = new Vector2(
//                    Random.Range(room.position.x + 1, room.position.x + room.width - 1),
//                    Random.Range(room.position.y + 1, room.position.y + room.height - 1)
//                );
//            } while (usedPoints.Contains(randomPoint));

//            usedPoints.Add(randomPoint);
//            spawnPoints.Add(randomPoint);
//        }

//        return spawnPoints;
//    }

//}
