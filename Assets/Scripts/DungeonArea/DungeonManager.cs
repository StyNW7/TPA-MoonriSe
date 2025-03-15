// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DungeonManager : MonoBehaviour
// {

//     public int dungeonWidth = 50;
//     public int dungeonHeight = 50;
//     public int roomCount = 9; // 1 Spawn + 1 Exit + 7 lainnya
//     public GameObject wallPrefab;
//     public GameObject floorPrefab;
//     public GameObject spawnRoomPrefab;
//     public GameObject exitRoomPrefab;
//     public GameObject[] smallRoomPrefabs;
//     public GameObject[] mediumRoomPrefabs;
//     public GameObject[] largeRoomPrefabs;
//     public GameObject portalPrefab;

//     private List<Room> rooms = new List<Room>();

//     void Start()
//     {
//         GenerateDungeon();
//     }

//     void GenerateDungeon()
//     {
//         rooms.Clear();
//         PlaceRooms();
//         ConnectRooms();
//         SpawnEnemies();
//         SpawnExitPortal();
//     }

//     void PlaceRooms()
//     {
//         // Tempatkan Spawn Room
//         Room spawnRoom = new Room(10, 10, 6, 6, RoomType.Spawn);
//         rooms.Add(spawnRoom);
//         Instantiate(spawnRoomPrefab, new Vector3(spawnRoom.x, 0, spawnRoom.y), Quaternion.identity);

//         // Tempatkan Exit Room (harus jauh dari Spawn Room)
//         Room exitRoom;
//         do
//         {
//             exitRoom = new Room(Random.Range(10, dungeonWidth - 10), Random.Range(10, dungeonHeight - 10), 6, 6, RoomType.Exit);
//         } while (Vector2.Distance(new Vector2(spawnRoom.x, spawnRoom.y), new Vector2(exitRoom.x, exitRoom.y)) < 15);
//         rooms.Add(exitRoom);
//         Instantiate(exitRoomPrefab, new Vector3(exitRoom.x, 0, exitRoom.y), Quaternion.identity);

//         // Tempatkan Room lainnya
//         for (int i = 0; i < 3; i++) rooms.Add(new Room(Random.Range(5, dungeonWidth - 5), Random.Range(5, dungeonHeight - 5), 4, 4, RoomType.Small));
//         for (int i = 0; i < 2; i++) rooms.Add(new Room(Random.Range(5, dungeonWidth - 5), Random.Range(5, dungeonHeight - 5), 6, 6, RoomType.Medium));
//         for (int i = 0; i < 2; i++) rooms.Add(new Room(Random.Range(5, dungeonWidth - 5), Random.Range(5, dungeonHeight - 5), 8, 8, RoomType.Large));
//     }

//     void ConnectRooms()
//     {
//         //AStarPathfinding pathfinding = new AStarPathfinding(dungeonWidth, dungeonHeight);
//         //pathfinding.ConnectRooms(rooms);
//     }

//     void SpawnEnemies()
//     {
//         foreach (Room room in rooms)
//         {
//             if (room.roomType == RoomType.Small) SpawnZombies(room, 1);
//             if (room.roomType == RoomType.Medium) SpawnZombies(room, 2);
//             if (room.roomType == RoomType.Large) SpawnZombies(room, 3);
//         }
//     }

//     void SpawnZombies(Room room, int count)
//     {
//         for (int i = 0; i < count; i++)
//         {
//             Vector3 spawnPos = new Vector3(room.x + Random.Range(0, room.width), 0, room.y + Random.Range(0, room.height));
//             Instantiate(Resources.Load("Zombie"), spawnPos, Quaternion.identity);
//         }
//     }

//     void SpawnExitPortal()
//     {
//         Room exitRoom = rooms.Find(r => r.roomType == RoomType.Exit);
//         Vector3 portalPosition = new Vector3(exitRoom.x + exitRoom.width / 2, 0, exitRoom.y + exitRoom.height / 2);
//         Instantiate(portalPrefab, portalPosition, Quaternion.identity);
//     }

// }
