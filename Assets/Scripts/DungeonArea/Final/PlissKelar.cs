using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    
    [Header("Dungeon Settings")]
    public int dungeonWidth = 50;
    public int dungeonHeight = 50;
    public float tileSize = 3f;
    private const int ROOM_SIZE = 3;

    [Header("Serialized Grid & Tilemap")]
    [SerializeField] private Grid dungeonGrid;
    [SerializeField] private Tilemap tilemap;

    [Header("Special Room Prefabs")]
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject finishRoomPrefab;

    [Header("Room Size Prefabs")]
    [SerializeField] private GameObject smallRoomPrefab;
    [SerializeField] private GameObject mediumRoomPrefab;
    [SerializeField] private GameObject largeRoomPrefab;

    [SerializeField] private GameObject hallwayPrefab;
    [SerializeField] private GameObject wallPrefab;

    [Header("Room Count Settings")]
    public int smallRoomCount = 3;
    public int mediumRoomCount = 2;
    public int largeRoomCount = 2;


    [Header("Player Reference")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject endArc;

    private List<Room> rooms = new List<Room>();
    private int[,] dungeonGridArray;

    public static Vector3 ExitRoomPosition;

    private void Start()
    {
        dungeonGridArray = new int[dungeonWidth, dungeonHeight];
        Debug.Log($"Grid Origin: {transform.position}");
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        PlaceRoom();

        ConnectRooms();
        RenderDungeon();

        // if (startRoomPrefab != null)
        // {
        //     PlacePlayerAtStartRoom();
        // }
    }

    // private void PlacePlayerAtStartRoom()
    // {
    //     if (playerPrefab != null && startRoomPrefab != null)
    //     {
    //         Vector3 playerPosition = GetWorldPosition(startRoomPrefab.x + startRoomPrefab.Width / 2, startRoomPrefab.y + startRoomPrefab.Height / 2);
    //         GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
    //         Debug.Log($"Player placed at: {playerPosition}");
    //     }
    //     else
    //     {
    //         Debug.LogError("Player prefab or start room is missing.");
    //     }
    // }

    private void PlaceRoom()
    {
        int attempts = 0;

        // Place the specified number of small rooms
        for (int i = 0; i < smallRoomCount; i++)
        {
            bool isPlaced = false;
            while (attempts < 100)
            {
                if (isPlaced)
                {
                    isPlaced = false;
                    break;
                }
                int x = Random.Range(1, dungeonWidth - ROOM_SIZE - 1);
                int y = Random.Range(2, dungeonHeight - ROOM_SIZE - 1);

                Room newRoom = new Room(x, y, RoomSize.Small);

                if (!IsRoomOverlapping(newRoom) && IsAreaClear(newRoom))
                {
                    rooms.Add(newRoom);
                    MarkRoomOnGrid(newRoom);
                    isPlaced = true;
                }

                attempts++;
            }
        }

        attempts = 0;

        // Place the specified number of medium rooms
        for (int i = 0; i < mediumRoomCount; i++)
        {
            bool isPlaced = false;
            while (attempts < 100)
            {
                if (isPlaced)
                {
                    isPlaced = false;
                    break;
                }
                int x = Random.Range(1, dungeonWidth - ROOM_SIZE - 1);
                int y = Random.Range(2, dungeonHeight - ROOM_SIZE - 1);

                Room newRoom = new Room(x, y, RoomSize.Medium);

                if (!IsRoomOverlapping(newRoom) && IsAreaClear(newRoom))
                {
                    rooms.Add(newRoom);
                    MarkRoomOnGrid(newRoom);
                    isPlaced = true;
                }

                attempts++;
            }
        }

        // Reset for large rooms
        attempts = 0;

        // Place the specified number of large rooms
        for (int i = 0; i < largeRoomCount + 2; i++)
        {
            bool isPlaced = false;
            while (attempts < 100)
            {
                if (isPlaced)
                {
                    isPlaced = false;
                    break;
                }
                int x = Random.Range(1, dungeonWidth - ROOM_SIZE - 1);
                int y = Random.Range(2, dungeonHeight - ROOM_SIZE - 1);

                Room newRoom = new Room(x, y, RoomSize.Large);

                if (!IsRoomOverlapping(newRoom) && IsAreaClear(newRoom))
                {
                    rooms.Add(newRoom);
                    MarkRoomOnGrid(newRoom);
                    isPlaced = true;
                }

                attempts++;
            }
        }
    }

    private bool IsRoomOverlapping(Room room)
    {
        foreach (Room other in rooms)
        {
            if (room.Intersects(other))
                return true;
        }
        return false;
    }

    private bool IsAreaClear(Room room)
    {
        for (int x = room.x - 1; x <= room.x + ROOM_SIZE + 1; x++)
        {
            for (int y = room.y - 1; y <= room.y + ROOM_SIZE + 1; y++)
            {
                if (x >= 0 && x < dungeonWidth && y >= 0 && y < dungeonHeight)
                {
                    if (dungeonGridArray[x, y] != 0)
                        return false;
                }
            }
        }
        return true;
    }

    private void MarkRoomOnGrid(Room room)
    {
        // Mark the room area as occupied based on its size
        for (int x = room.x; x < room.x + room.Width; x++)
        {
            for (int y = room.y; y < room.y + room.Height; y++)
            {
                dungeonGridArray[x, y] = 1;
            }
        }

        // Set the access point below the room
        Vector2Int accessPoint = room.AccessPoint;
        if (accessPoint.y >= 0)
        {
            dungeonGridArray[accessPoint.x, accessPoint.y] = 3;
        }
    }

    private void ConnectRooms()
    {
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector2Int start = rooms[i].AccessPoint;
            Vector2Int end = rooms[i + 1].AccessPoint;

            if (start.y > 0 && end.y > 0)
            {
                List<Vector2Int> path = AStarPathFinder.FindPath(dungeonGridArray, start, end);

                foreach (Vector2Int pos in path)
                {
                    if (dungeonGridArray[pos.x, pos.y] == 0)
                    {
                        dungeonGridArray[pos.x, pos.y] = 2;
                    }
                }
            }
        }
    }

    private void RenderDungeon()
    {
        bool startRoomPlaced = false;
        bool endRoomPlaced = false;
        foreach (Room room in rooms)
        {
            Vector3 position = Vector3.zero;
            if (room.roomSize == RoomSize.Small)
            {
                position = GetWorldPosition((int)room.Center.x, (int)room.Center.y) + new Vector3(0.1f, 0, 1.8f);
            }
            else
            {
                position = GetWorldPosition((int)room.Center.x, (int)room.Center.y) + new Vector3(2.5f, 0, 3f);
            }


            if (room.roomSize == RoomSize.Large)
            {
                if (!startRoomPlaced)
                {
                    GameObject roomPrefabToUse = startRoomPrefab;
                    Instantiate(roomPrefabToUse, position, Quaternion.identity).layer = LayerMask.NameToLayer("Ground");
                    startRoomPlaced = true;

                    if (playerPrefab != null)
                    {
                        playerPrefab.transform.position = position;
                        Debug.Log($"Player ditempatkan di SpawnRoom pada posisi: {position}");
                    }

                    continue;
                }
                else if (!endRoomPlaced)
                {
                    GameObject roomPrefabToUse = finishRoomPrefab;
                    Instantiate(roomPrefabToUse, position, Quaternion.identity).layer = LayerMask.NameToLayer("Ground");
                    endRoomPlaced = true;

                    ExitRoomPosition = position;

                    // Geser portal ke diagonal kiri bawah
                    portal.transform.position = position + new Vector3(-3f, 1.5f, -3f);

                    // Geser endArc ke diagonal kiri bawah dan naikkan posisinya
                    endArc.transform.position = position + new Vector3(-3f, 0f, -3f); 

                    continue;
                }
            }

            GameObject roomPrefabToUseSpawn = GetRoomPrefab(room.roomSize);
            Instantiate(roomPrefabToUseSpawn, position, Quaternion.identity).layer = LayerMask.NameToLayer("Ground");
        }

        // Place hallways and walls
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                Vector3 position = GetWorldPosition(x, y);

                if (dungeonGridArray[x, y] == 2 || dungeonGridArray[x, y] == 3)
                {
                    Instantiate(hallwayPrefab, position + new Vector3(2f, 0, 2f), Quaternion.identity).layer = LayerMask.NameToLayer("Ground");
                    PlaceHallwayWalls(position);
                }
            }
        }
    }


    // Factory Design Pattern


    private GameObject GetRoomPrefab(RoomSize roomSize)
    {
        switch (roomSize)
        {
            case RoomSize.Small:
                return smallRoomPrefab;
            case RoomSize.Medium:
                return mediumRoomPrefab;
            case RoomSize.Large:
                return largeRoomPrefab;
            default:
                return smallRoomPrefab;
        }
    }

    private void PlaceHallwayWalls(Vector3 floorPosition)
    {
        Vector3Int cell = dungeonGrid.WorldToCell(floorPosition);

        if (!HasAdjacentFloor(cell.x + 1, cell.y) && dungeonGridArray[cell.x + 1, cell.y] == 0)
        {
            Vector3 rightWallPos = dungeonGrid.GetCellCenterWorld(new Vector3Int(cell.x + 1, cell.y, cell.z)) + new Vector3(-1f, 0, -1f);
            GameObject rightWall = Instantiate(wallPrefab, rightWallPos, Quaternion.identity);
            rightWall.transform.SetLocalPositionAndRotation(
                rightWall.transform.position,
                new Quaternion(0, -0.707106829f, 0, 0.707106829f)
            );
            rightWall.name = "Right Wall";
            rightWall.layer = LayerMask.NameToLayer("Ground");
        }

        if (!HasAdjacentFloor(cell.x - 1, cell.y) && dungeonGridArray[cell.x - 1, cell.y] == 0)
        {
            Vector3 leftWallPos = dungeonGrid.GetCellCenterWorld(new Vector3Int(cell.x - 1, cell.y, cell.z)) + new Vector3(2f, 0f, 2f);
            GameObject leftWall = Instantiate(wallPrefab, leftWallPos, Quaternion.identity);
            leftWall.transform.SetLocalPositionAndRotation(
                leftWall.transform.position,
                new Quaternion(0, 0.707106829f, 0, 0.707106829f)
            );
            leftWall.name = "Left Wall";
            leftWall.layer = LayerMask.NameToLayer("Ground");
        }

        if (!HasAdjacentFloor(cell.x, cell.y + 1) && dungeonGridArray[cell.x, cell.y + 1] == 0)
        {
            Vector3 topWallPos = dungeonGrid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y + 1, cell.z)) + new Vector3(-1f, 0f, -1f);
            GameObject topWall = Instantiate(wallPrefab, topWallPos, Quaternion.identity);
            topWall.name = "Top Name";
            topWall.layer = LayerMask.NameToLayer("Ground");
        }

        if (!HasAdjacentFloor(cell.x, cell.y - 1) && dungeonGridArray[cell.x, cell.y - 1] == 0)
        {
            Vector3 bottomWallPos = dungeonGrid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y - 1, cell.z)) + new Vector3(-1f, 0f, 2f);
            GameObject bottomWall = Instantiate(wallPrefab, bottomWallPos, Quaternion.identity);
            bottomWall.name = "Bottom Name";
            bottomWall.layer = LayerMask.NameToLayer("Ground");
        }
    }

    private bool HasAdjacentFloor(int x, int y)
    {
        if (x >= 0 && x < dungeonWidth && y >= 0 && y < dungeonHeight)
        {
            return dungeonGridArray[x, y] == 1 || dungeonGridArray[x, y] == 2; // 1 = Room, 2 = Hallway
        }
        return false;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        Vector3Int cellPosition = new Vector3Int(x, y, 0);
        return dungeonGrid.GetCellCenterWorld(cellPosition);
    }


    // private void OnDrawGizmos()
    // {
    //     if (dungeonGridArray == null) return;

    //     for (int x = 0; x < dungeonWidth; x++)
    //     {
    //         for (int y = 0; y < dungeonHeight; y++)
    //         {
    //             Vector3 position = GetWorldPosition(x, y) + new Vector3(0.5f, 0f, 0.5f);

    //             if (dungeonGridArray[x, y] == 1) // Room Tile
    //             {
    //                 Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f); // Green (Transparent)
    //             }
    //             else if (dungeonGridArray[x, y] == 2) // Hallway Tile
    //             {
    //                 Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 0.5f); // Blue (Transparent)
    //             }
    //             else if (dungeonGridArray[x, y] == 3) // Access Point
    //             {
    //                 Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 1.0f); // Yellow (Opaque)
    //             }
    //             else
    //             {
    //                 continue; // Skip walls and empty spaces
    //             }

    //             Gizmos.DrawCube(position, new Vector3(tileSize, 0.1f, tileSize));
    //         }
    //     }
    // }


    // Langsung aja buat Class di dalam sini

    public enum RoomSize
    {
        Small,    // 2x2 room
        Medium,   // 3x2 room
        Large     // 3x3 room
    }

    public class Room
    {
        public int x, y;
        public RoomSize roomSize;

        public Room(int x, int y, RoomSize roomSize)
        {
            this.x = x;
            this.y = y;
            this.roomSize = roomSize;
        }

        public int Width
        {
            get
            {
                switch (roomSize)
                {
                    case RoomSize.Small:
                        return 2;
                    case RoomSize.Medium:
                        return 3;
                    case RoomSize.Large:
                        return 3;
                    default:
                        return 2;
                }
            }
        }

        public int Height
        {
            get
            {
                switch (roomSize)
                {
                    case RoomSize.Small:
                        return 2;
                    case RoomSize.Medium:
                        return 2;
                    case RoomSize.Large:
                        return 3;
                    default:
                        return 2;
                }
            }
        }

        public Vector2Int AccessPoint => new Vector2Int(x + Width / 2, y - 1);
        public Vector3 Center
        {
            get
            { 
                float centerX = x + Width / 2f;
                float centerY = y + Height / 2f;
                return new Vector3(centerX, centerY, 0);
            }
        }

        public bool Intersects(Room other)
        {
            return x < other.x + other.Width && x + Width > other.x &&
                y < other.y + other.Height && y + Height > other.y;
        }
        
    }


}