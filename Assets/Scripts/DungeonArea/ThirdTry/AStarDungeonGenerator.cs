// using System.Collections.Generic;
// using UnityEngine;

// public class AStarDungeonGenerator : MonoBehaviour
// {
//     [Header("Prefabs")]
//     public GameObject floorPrefab;
//     public GameObject wallPrefab;

//     [Header("Dungeon Settings")]
//     public int mapWidth = 30;
//     public int mapHeight = 30;
//     public int roomCount = 5;
//     public Vector2Int roomMinSize = new Vector2Int(4, 4);
//     public Vector2Int roomMaxSize = new Vector2Int(8, 8);

//     private List<RectInt> rooms = new List<RectInt>();
//     private HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
//     private Dictionary<Vector2Int, GameObject> spawnedObjects = new Dictionary<Vector2Int, GameObject>();

//     void Start()
//     {
//         GenerateDungeon();
//     }

//     void GenerateDungeon()
//     {
//         ClearDungeon();
//         GenerateRooms();
//         ConnectRoomsWithHallways();
//         GenerateWalls();
//     }

//     void ClearDungeon()
//     {
//         foreach (var obj in spawnedObjects.Values)
//         {
//             Destroy(obj);
//         }
//         spawnedObjects.Clear();
//         rooms.Clear();
//         floorPositions.Clear();
//     }

//     void GenerateRooms()
//     {
//         for (int i = 0; i < roomCount; i++)
//         {
//             Vector2Int roomSize = new Vector2Int(
//                 Random.Range(roomMinSize.x, roomMaxSize.x),
//                 Random.Range(roomMinSize.y, roomMaxSize.y));

//             Vector2Int roomPosition = new Vector2Int(
//                 Random.Range(1, mapWidth - roomSize.x - 1),
//                 Random.Range(1, mapHeight - roomSize.y - 1));

//             RectInt newRoom = new RectInt(roomPosition, roomSize);

//             // Ensure no overlapping rooms
//             bool overlaps = false;
//             foreach (var room in rooms)
//             {
//                 if (newRoom.Overlaps(room))
//                 {
//                     overlaps = true;
//                     break;
//                 }
//             }

//             if (!overlaps)
//             {
//                 rooms.Add(newRoom);
//                 CarveRoom(newRoom);

//                 // Pastikan Room Pertama = Spawn Room, Room Terakhir = Exit Room
//                 if (i == 0) Debug.Log("Spawn Room dibuat di: " + roomPosition);
//                 if (i == roomCount - 1) Debug.Log("Exit Room dibuat di: " + roomPosition);
//             }
//         }
//     }

//     void CarveRoom(RectInt room)
//     {
//         for (int x = room.xMin; x < room.xMax; x++)
//         {
//             for (int y = room.yMin; y < room.yMax; y++)
//             {
//                 Vector2Int pos = new Vector2Int(x, y);
//                 if (!floorPositions.Contains(pos))
//                 {
//                     SpawnPrefab(floorPrefab, pos);
//                     floorPositions.Add(pos);
//                 }
//             }
//         }
//     }

//     void ConnectRoomsWithHallways()
//     {
//         for (int i = 0; i < rooms.Count - 1; i++)
//         {
//             Vector2Int start = GetRandomPointInRoom(rooms[i]);
//             Vector2Int end = GetRandomPointInRoom(rooms[i + 1]);

//             List<Vector2Int> path = AStarPathfind(start, end);

//             foreach (var pos in path)
//             {
//                 if (!floorPositions.Contains(pos))
//                 {
//                     SpawnPrefab(floorPrefab, pos);
//                     floorPositions.Add(pos);
//                 }
//             }
//         }
//     }

//     void GenerateWalls()
//     {
//         HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

//         foreach (var pos in floorPositions)
//         {
//             foreach (var neighbor in GetNeighbors(pos))
//             {
//                 if (!floorPositions.Contains(neighbor) && !wallPositions.Contains(neighbor))
//                 {
//                     SpawnPrefab(wallPrefab, neighbor);
//                     wallPositions.Add(neighbor);
//                 }
//             }
//         }
//     }

//     void SpawnPrefab(GameObject prefab, Vector2Int position)
//     {
//         if (!spawnedObjects.ContainsKey(position))
//         {
//             GameObject obj = Instantiate(prefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
//             spawnedObjects[position] = obj;
//         }
//     }

//     List<Vector2Int> GetNeighbors(Vector2Int position)
//     {
//         return new List<Vector2Int>
//         {
//             position + Vector2Int.up,
//             position + Vector2Int.down,
//             position + Vector2Int.left,
//             position + Vector2Int.right
//         };
//     }
// }
