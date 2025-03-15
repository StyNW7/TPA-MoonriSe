using UnityEngine;
using System.Collections.Generic;

public class DungeonManagerDoor : MonoBehaviour
{
    public GameObject spawnRoomPrefab;
    public GameObject exitRoomPrefab;
    public GameObject smallRoomPrefab;
    public GameObject mediumRoomPrefab;
    public GameObject largeRoomPrefab;
    public GameObject hallwayPrefab;

    public int dungeonWidth = 10;
    public int dungeonHeight = 10;

    private List<Room> rooms = new List<Room>();
    private Dictionary<Vector2Int, Room> roomDictionary = new Dictionary<Vector2Int, Room>();
    private Dictionary<Vector2Int, Hallway> hallwayDictionary = new Dictionary<Vector2Int, Hallway>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        roomDictionary.Clear();
        rooms.Clear();
        hallwayDictionary.Clear();

        // 1. Generate Spawn Room
        Vector2Int spawnPosition = new Vector2Int(dungeonWidth / 2, dungeonHeight / 2);
        Room spawnRoom = CreateRoom(spawnRoomPrefab, spawnPosition, "SpawnRoom");

        // 2. Generate Exit Room
        Vector2Int exitPosition = GetFarPosition(spawnPosition, 6);
        Vector2Int exitHallwayEnd = GenerateHallwayToRoom(spawnPosition, exitPosition);
        Room exitRoom = CreateRoom(exitRoomPrefab, exitHallwayEnd, "ExitRoom");

        // 3. Generate Other Rooms
        GenerateAndConnectRooms(smallRoomPrefab, 3, "SmallRoom");
        GenerateAndConnectRooms(mediumRoomPrefab, 2, "MediumRoom");
        GenerateAndConnectRooms(largeRoomPrefab, 2, "LargeRoom");

        Debug.Log("Dungeon berhasil dibuat dengan " + rooms.Count + " ruangan dan " + hallwayDictionary.Count + " hallway.");
    }

    void GenerateAndConnectRooms(GameObject roomPrefab, int count, string roomType)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2Int roomPosition = GetValidPosition();
            Vector2Int hallwayEnd = GenerateHallwayToRoom(rooms[0].position, roomPosition);
            CreateRoom(roomPrefab, hallwayEnd, roomType);
        }
    }

    Room CreateRoom(GameObject roomPrefab, Vector2Int position, string roomType)
    {
        if (roomDictionary.ContainsKey(position))
        {
            return null;
        }

        GameObject newRoomObj = Instantiate(roomPrefab, new Vector3(position.x * 10, 0, position.y * 10), Quaternion.identity);
        Room newRoom = newRoomObj.GetComponent<Room>();

        if (newRoom == null)
        {
            Debug.LogError($"Prefab {roomPrefab.name} tidak memiliki script Room.cs!");
            return null;
        }

        newRoom.position = position;
        newRoom.type = roomType;
        newRoom.InitializeDoor(); // Ambil pintu room (south wall dihancurkan otomatis)

        roomDictionary[position] = newRoom;
        rooms.Add(newRoom);
        return newRoom;
    }

    Vector2Int GenerateHallwayToRoom(Vector2Int start, Vector2Int end)
    {
        AStarPathfinder pathfinder = new AStarPathfinder(dungeonWidth, dungeonHeight);
        List<Vector2Int> path = pathfinder.FindPath(start, end);

        if (path.Count < 3) // Minimum 3 langkah agar hallway berhenti sebelum masuk room
        {
            Debug.LogError($"Tidak dapat menemukan jalur valid antara {start} dan {end}");
            return start;
        }

        Vector2Int lastHallwayPos = path[path.Count - 2]; // Berhenti sebelum masuk room
        Vector2Int directionToRoom = end - lastHallwayPos;

        for (int j = 0; j < path.Count - 2; j++) // Loop hanya sampai sebelum room
        {
            Vector2Int current = path[j];
            Vector2Int next = path[j + 1];
            Vector2Int direction = next - current;

            // Jika next sudah ada di hallway, lanjutkan tanpa membuat hallway baru
            if (hallwayDictionary.ContainsKey(next))
            {
                hallwayDictionary[next].RemoveWall(-direction);
                continue;
            }

            // Buat hallway baru jika belum ada hallway di posisi ini
            if (!roomDictionary.ContainsKey(next))
            {
                GameObject newHallwayObj = Instantiate(hallwayPrefab, new Vector3(next.x * 10, 0, next.y * 10), Quaternion.identity);
                Hallway newHallway = newHallwayObj.GetComponent<Hallway>();

                if (newHallway != null)
                {
                    newHallway.position = next;
                    newHallway.RemoveWall(-direction);
                    newHallway.RemoveWall(direction);
                    hallwayDictionary[next] = newHallway;
                }
            }
        }

        return lastHallwayPos; // Return posisi hallway terakhir sebelum masuk room
    }

    Vector2Int GetValidPosition()
    {
        Vector2Int newPos;
        do
        {
            newPos = new Vector2Int(Random.Range(0, dungeonWidth), Random.Range(0, dungeonHeight));
        } while (roomDictionary.ContainsKey(newPos) || hallwayDictionary.ContainsKey(newPos));

        return newPos;
    }

    Vector2Int GetFarPosition(Vector2Int from, int minDistance)
    {
        Vector2Int pos;
        do
        {
            pos = GetValidPosition();
        } while (Vector2Int.Distance(pos, from) < minDistance);
        return pos;
    }
}
