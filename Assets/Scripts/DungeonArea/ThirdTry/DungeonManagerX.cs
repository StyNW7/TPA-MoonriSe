using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonManagerX : MonoBehaviour
{
    [Header("Dungeon Settings")]
    public int dungeonWidth = 20;
    public int dungeonHeight = 20;
    public int roomCount = 8; // 1 spawn, 1 exit, 6 lainnya (3 kecil, 2 sedang, 2 besar)

    [Header("Room Prefabs")]
    public GameObject spawnRoomPrefab;
    public GameObject exitRoomPrefab;
    public GameObject smallRoomPrefab;
    public GameObject mediumRoomPrefab;
    public GameObject largeRoomPrefab;
    public GameObject hallwayPrefab;

    [Header("Entities")]
    public GameObject zombiePrefab;
    public GameObject portalPrefab;
    public GameObject playerSpawnPoint;

    private List<Room> rooms = new List<Room>();
    private int[,] dungeonGrid;
    private const int EMPTY = 0, ROOM = 1, HALLWAY = 2;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        dungeonGrid = new int[dungeonWidth, dungeonHeight];
        rooms.Clear();

        // 1. Generate Spawn Room
        Room spawnRoom = PlaceRoom(spawnRoomPrefab, true);
        dungeonGrid[spawnRoom.x, spawnRoom.y] = ROOM;

        // 2. Generate Exit Room (harus jauh dari Spawn Room)
        Room exitRoom;
        do
        {
            exitRoom = PlaceRoom(exitRoomPrefab, false);
        } while (Vector2.Distance(new Vector2(spawnRoom.x, spawnRoom.y), new Vector2(exitRoom.x, exitRoom.y)) < 10);

        dungeonGrid[exitRoom.x, exitRoom.y] = ROOM;

        // 3. Generate Other Rooms (Small, Medium, Large)
        int smallCount = 3, mediumCount = 2, largeCount = 2;

        for (int i = 0; i < roomCount - 2; i++)
        {
            GameObject roomPrefab;
            if (smallCount > 0) { roomPrefab = smallRoomPrefab; smallCount--; }
            else if (mediumCount > 0) { roomPrefab = mediumRoomPrefab; mediumCount--; }
            else { roomPrefab = largeRoomPrefab; largeCount--; }

            Room newRoom = PlaceRoom(roomPrefab, false);
            dungeonGrid[newRoom.x, newRoom.y] = ROOM;
        }

        // 4. Connect Rooms using A* Pathfinding
        ConnectRoomsWithHallways();

        // 5. Spawn Player in Spawn Room
        Instantiate(playerSpawnPoint, new Vector3(spawnRoom.x * 10, 1, spawnRoom.y * 10), Quaternion.identity);

        // 6. Spawn Portal in Exit Room
        Instantiate(portalPrefab, new Vector3(exitRoom.x * 10, 1, exitRoom.y * 10), Quaternion.identity);

        // 7. Spawn Zombies in Small, Medium, and Large Rooms
        SpawnZombies();
    }

    Room PlaceRoom(GameObject prefab, bool isSpawn)
    {
        int x, y;
        do
        {
            x = Random.Range(2, dungeonWidth - 2);
            y = Random.Range(2, dungeonHeight - 2);
        } while (dungeonGrid[x, y] != EMPTY);

        Room room = new Room(x, y, prefab);
        rooms.Add(room);
        Instantiate(prefab, new Vector3(x * 10, 0, y * 10), Quaternion.identity);
        return room;
    }

    void ConnectRoomsWithHallways()
    {
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Room start = rooms[i];
            Room end = rooms[i + 1];
            AStarPathfinding(start, end);
        }
    }

    void AStarPathfinding(Room start, Room end)
    {
        int x = start.x, y = start.y;
        while (x != end.x || y != end.y)
        {
            dungeonGrid[x, y] = HALLWAY;
            Instantiate(hallwayPrefab, new Vector3(x * 10, 0, y * 10), Quaternion.identity);

            if (x < end.x) x++;
            else if (x > end.x) x--;

            if (y < end.y) y++;
            else if (y > end.y) y--;
        }
    }

    void SpawnZombies()
    {
        foreach (Room room in rooms)
        {
            if (room.prefab == spawnRoomPrefab || room.prefab == exitRoomPrefab) continue;

            int zombieCount = room.prefab == smallRoomPrefab ? 1 :
                              room.prefab == mediumRoomPrefab ? 2 : 3;

            for (int i = 0; i < zombieCount; i++)
            {
                Vector3 position = new Vector3(room.x * 10 + Random.Range(-3, 3), 1, room.y * 10 + Random.Range(-3, 3));
                Instantiate(zombiePrefab, position, Quaternion.identity);
            }
        }
    }

    public class Room
    {
        public int x, y;
        public GameObject prefab;

        public Room(int x, int y, GameObject prefab)
        {
            this.x = x;
            this.y = y;
            this.prefab = prefab;
        }
    }


}

