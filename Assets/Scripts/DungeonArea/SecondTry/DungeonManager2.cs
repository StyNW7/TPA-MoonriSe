using UnityEngine;
using System.Collections.Generic;

public class DungeonManager : MonoBehaviour
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
    private HashSet<Vector2Int> hallwayPositions = new HashSet<Vector2Int>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        roomDictionary.Clear();
        rooms.Clear();
        hallwayPositions.Clear();

        // Generate Spawn Room
        Vector2Int spawnPosition = new Vector2Int(dungeonWidth / 2, dungeonHeight / 2);
        Room spawnRoom = CreateRoom(spawnRoomPrefab, spawnPosition, "SpawnRoom");

        // Generate Exit Room
        Vector2Int exitPosition = GetFarPosition(spawnPosition, 6);
        Room exitRoom = CreateRoom(exitRoomPrefab, exitPosition, "ExitRoom");

        // Generate Other Rooms
        PlaceRandomRooms(smallRoomPrefab, 3, "SmallRoom");
        PlaceRandomRooms(mediumRoomPrefab, 2, "MediumRoom");
        PlaceRandomRooms(largeRoomPrefab, 2, "LargeRoom");

        // Connect Rooms with A* Pathfinding
        GenerateHallways();

        Debug.Log("Dungeon berhasil dibuat dengan " + rooms.Count + " ruangan dan " + hallwayPositions.Count + " hallway.");
    }

    Room CreateRoom(GameObject roomPrefab, Vector2Int position, string roomType)
    {
        if (roomDictionary.ContainsKey(position))
        {
            return null; // Hindari duplikasi room
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

        newRoom.wallNorth = newRoom.transform.Find("North")?.gameObject;
        newRoom.wallSouth = newRoom.transform.Find("South")?.gameObject;
        newRoom.wallEast = newRoom.transform.Find("East")?.gameObject;
        newRoom.wallWest = newRoom.transform.Find("West")?.gameObject;

        roomDictionary[position] = newRoom;
        rooms.Add(newRoom);
        return newRoom;
    }

    void PlaceRandomRooms(GameObject roomPrefab, int count, string type)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2Int pos = GetValidPosition();
            CreateRoom(roomPrefab, pos, type);
        }
    }

    Vector2Int GetValidPosition()
    {
        Vector2Int newPos;
        int attempts = 0;

        do
        {
            newPos = new Vector2Int(Random.Range(0, dungeonWidth), Random.Range(0, dungeonHeight));
            attempts++;

            if (attempts > 100)
            {
                Debug.LogWarning("Tidak dapat menemukan posisi room yang valid!");
                break;
            }

        } while (roomDictionary.ContainsKey(newPos) || hallwayPositions.Contains(newPos));

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

    void GenerateHallways()
    {
        AStarPathfinder pathfinder = new AStarPathfinder(dungeonWidth, dungeonHeight);

        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector2Int start = rooms[i].position;
            Vector2Int end = rooms[i + 1].position;

            List<Vector2Int> path = pathfinder.FindPath(start, end);

            if (path.Count == 0)
            {
                Debug.LogError($"Tidak dapat menemukan jalur valid antara {start} dan {end}");
                continue;
            }

            for (int j = 0; j < path.Count - 1; j++)
            {
                Vector2Int current = path[j];
                Vector2Int next = path[j + 1];
                Vector2Int direction = next - current;

                if (hallwayPositions.Contains(next)) continue;

                // Jika next adalah room dan merupakan akhir hallway, buat pintu masuk
                if (roomDictionary.ContainsKey(next) && j == path.Count - 2)
                {
                    roomDictionary[next].RemoveWall(-direction);
                    continue;
                }

                if (!roomDictionary.ContainsKey(next))
                {
                    GameObject newHallwayObj = Instantiate(hallwayPrefab, new Vector3(next.x * 10, 0, next.y * 10), Quaternion.identity);
                    Hallway newHallway = newHallwayObj.GetComponent<Hallway>();

                    if (newHallway != null)
                    {
                        newHallway.position = next;
                        newHallway.RemoveWall(-direction);
                        newHallway.RemoveWall(direction);
                        hallwayPositions.Add(next);
                    }
                }

                // Hapus dinding di room yang mengarah ke hallway
                if (roomDictionary.ContainsKey(current))
                {
                    roomDictionary[current].RemoveWall(direction);
                }
            }
        }
    }

    List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        return new List<Vector2Int>
        {
            node + Vector2Int.up,
            node + Vector2Int.down,
            node + Vector2Int.left,
            node + Vector2Int.right
        };
    }
}
