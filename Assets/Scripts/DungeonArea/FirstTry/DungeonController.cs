using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonController : MonoBehaviour
{

    [SerializeField] private int sizeX;
    [SerializeField] private int sizeY;
    [SerializeField] private GameObject corridorM;
    [SerializeField] private GameObject wallBottom;
    [SerializeField] private GameObject wallTop;
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallRight;
    [SerializeField] private GameObject wallRoomBottom;
    [SerializeField] private GameObject wallRoomTop;
    [SerializeField] private GameObject wallRoomLeft;
    [SerializeField] private GameObject wallRoomRight;
    [SerializeField] private GameObject wallRoomBottonDoor;
    [SerializeField] private GameObject wallRoomTopDoor;
    [SerializeField] private GameObject wallRoomLeftDoor;
    [SerializeField] private GameObject wallRoomRightDoor;
    [SerializeField] private GameObject spawnRoom;
    [SerializeField] private GameObject normalRoom1;
    [SerializeField] private GameObject normalRoom2;
    [SerializeField] private GameObject itemRoom1;
    [SerializeField] private GameObject itemRoom2;
    [SerializeField] private GameObject enemyRoom;
    [SerializeField] private GameObject bossRoom;
    [SerializeField] private AudioSource audioMaze;


    private List<Node> openedList;
    private int generateCount;

    private List<GameObject> walls;
    private List<GameObject> wallsRoom;
    private List<GameObject> wallsRoomDoor;
    private char[,] map;

    private List<int> sidesX;
    private List<int> sidesY;

    private System.Random rng = new System.Random();
    private List<Edge> edgeList;
    private int normalCount;
    private int[] ancestor;

    public static Vector3 mazeCenter;

    private void instantiateMaze()
    {

        walls = new List<GameObject>() { wallBottom, wallRight, wallTop, wallLeft };
        wallsRoom = new List<GameObject>() { wallRoomBottom, wallRoomRight, wallRoomTop, wallRoomLeft };
        wallsRoomDoor = new List<GameObject>() { wallRoomBottonDoor, wallRoomRightDoor, wallRoomTopDoor, wallRoomLeftDoor };

        for (int i = 1; i < sizeX - 1; i++)
        {
            for (int j = 1; j < sizeY - 1; j++)
            {
                if (map[i, j] == ' ' || map[i, j] == 'D')
                {
                    Instantiate(corridorM, new Vector3(i * 6, 0f, j * 6), Quaternion.identity);

                    for (int k = 0; k < 4; k++)
                    {
                        if (map[i + sidesX[k], j + sidesY[k]] == '#')
                        {
                            Instantiate(walls[k], new Vector3(i * 6, 0f, j * 6), walls[k].transform.rotation);
                        }
                    }
                }
                if (map[i, j] == 'S')
                {
                    Instantiate(spawnRoom, new Vector3(i * 6, 0f, j * 6), spawnRoom.transform.rotation);
                }
                else if (map[i, j] == 'N')
                {
                    instantiateNormalRoom(i, j);
                }
                else if (map[i, j] == 'I')
                {
                    instantiateItemRoom(i, j);
                }
                else if (map[i, j] == 'Q')
                {
                    instantiateItemRoom2(i, j);
                }
                else if (map[i, j] == 'E')
                {
                    instantiateEnemyRoom(i, j);
                }
                else if (map[i, j] == 'B')
                {
                    instantiateBossRoom(i, j);
                }
            }
        }
    }

    private void instantiateNormalRoom(int i, int j)
    {
        int[] roomX = { 0, 1, 1, 0 };
        int[] roomY = { 0, 0, 1, 1 };

        if (normalCount < 2)
        {
            Instantiate(normalRoom1, new Vector3(i * 6, 0f, j * 6), Quaternion.identity);
        }
        else
        {
            Instantiate(normalRoom2, new Vector3(i * 6, 0f, j * 6), Quaternion.identity);
        }


        for (int k = 0; k < 4; k++)
        {
            for (int l = 0; l < 4; l++)
            {
                if (map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] == 'D')
                {
                    Instantiate(wallsRoomDoor[l], new Vector3(i * 6 + roomX[k] * 6 + sidesX[l] * 6, 0f, j * 6 + roomY[k] * 6 + sidesY[l] * 6), Quaternion.identity);
                }
                else if (map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] != 'N' && map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] != 'n')
                {
                    Instantiate(wallsRoom[l], new Vector3(i * 6 + roomX[k] * 6 + sidesX[l] * 6, 0f, j * 6 + roomY[k] * 6 + sidesY[l] * 6), Quaternion.identity);
                }
            }
        }
        normalCount++;
    }

    private void generateSpawnRoom()
    {
        int xRand = 0;
        int yRand = 0;

        int roomRand;
        int dirRand;

        int[] roomX = { 0, 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 4, 4 };
        int[] roomY = { 0, 1, 2, 3, 4, 0, 4, 0, 4, 0, 4, 0, 1, 2, 3, 4 };


        bool detect;
        int count2 = 0;
        do
        {
            count2++;
            detect = false;
            xRand = rng.Next(4, sizeX - 6);
            yRand = rng.Next(4, sizeY - 6);

            for (int j = -1; j < 6; j++)
            {
                for (int k = -1; k < 6; k++)
                {
                    if (map[xRand + j, yRand + k] != '#')
                    {
                        detect = true;
                    }
                }
            }
        } while (detect && count2 < 200);
        Debug.Log(count2);

        for (int j = 0; j < 5; j++)
        {
            for (int k = 0; k < 5; k++)
            {
                map[xRand + j, yRand + k] = 's';
            }
        }
        map[xRand, yRand] = 'S';
        map[xRand - 1, yRand] = 'D';
        map[xRand + 5, yRand] = 'D';
    }

    private void generateNormalRoom()
    {
        int xRand = 0;
        int yRand = 0;

        int roomRand;
        int dirRand;

        int[] roomX = { 0, 1, 1, 0 };
        int[] roomY = { 0, 0, 1, 1 };

        bool detect;
        for (int i = 0; i < 5; i++)
        {
            int count2 = 0;
            do
            {
                count2++;
                detect = false;
                xRand = rng.Next(2, sizeX - 3);
                yRand = rng.Next(2, sizeY - 3);

                for (int j = -1; j < 3; j++)
                {
                    for (int k = -1; k < 3; k++)
                    {
                        if (map[xRand + j, yRand + k] != '#')
                        {
                            detect = true;
                        }
                    }
                }
            } while (detect && count2 < 200);
            Debug.Log(count2);

            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    map[xRand + j, yRand + k] = 'n';
                }
            }
            map[xRand, yRand] = 'N';

            int count = 0;
            do
            {
                count++;
                roomRand = rng.Next(4);
                dirRand = rng.Next(4);

                if (map[xRand + roomX[roomRand] + sidesX[dirRand], yRand + roomY[roomRand] + sidesY[dirRand]] == '#')
                {
                    map[xRand + roomX[roomRand] + sidesX[dirRand], yRand + roomY[roomRand] + sidesY[dirRand]] = 'D';
                    break;
                }

            } while (true && count < 250);
            Debug.Log(count);
        }
    }

    
    public char[,] mazeGenerator()
    {
        generateCount = 0;
        map = new char[sizeX + 1, sizeY + 1];
        int[] directionX = { 0, 2, 0, -2 };
        int[] directionY = { -2, 0, 2, 0 };

        sidesX = new List<int> { 1, 0, -1, 0 };
        sidesY = new List<int> { 0, 1, 0, -1 };

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                map[i, j] = '#';
            }
        }


        generateSpawnRoom();
        generateNormalRoom();
        //generateItemRoom();
        //generateEnemyRoom();
        //generateBossRoom();


        List<Node> nodeList = new List<Node>();
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (map[i, j] == 'D')
                {
                    Debug.Log("Door exists" + i + " " + j);
                    nodeList.Add(new Node(i, j));
                }
            }
        }

        edgeList = new List<Edge>();
        for (int i = 0; i < nodeList.Count; i++)
        {
            for (int j = 1 + i; j < nodeList.Count; j++)
            {
                Debug.Log("Nodelist i : " + nodeList[i].PosX + " " + nodeList[i].PosY + i);
                Debug.Log("Nodelist j : " + nodeList[j].PosX + " " + nodeList[j].PosY + j);
                if (nodeList[i].PosX != nodeList[j].PosX && nodeList[i].PosY != nodeList[j].PosY)
                {
                    insert(new Edge(nodeList[i], nodeList[j]));
                    Debug.Log("Masuk sini");
                }

            }
        }

        ancestor = new int[nodeList.Count];

        for (int i = 0; i < nodeList.Count; i++)
        {
            ancestor[i] = i;
        }

        List<Node> connected = new List<Node>();

        List<Edge> graphRes = new List<Edge>();

        bool first = true;

        foreach (Edge edge in edgeList)
        {
            Debug.Log("Masuk foreach");
            if (first)
            {
                connected.Add(edge.Destination);
                connected.Add(edge.Source);
                graphRes.Add(edge);

                first = false;
                union(nodeList.IndexOf(edge.Destination), nodeList.IndexOf(edge.Source));
            }
            else if ((connected.Contains(edge.Destination) && !connected.Contains(edge.Source)))
            {
                connected.Add(edge.Source);
                graphRes.Add(edge);
                union(nodeList.IndexOf(edge.Destination), nodeList.IndexOf(edge.Source));
            }
            else if ((!connected.Contains(edge.Destination) && connected.Contains(edge.Source)))
            {
                connected.Add(edge.Destination);
                graphRes.Add(edge);
                union(nodeList.IndexOf(edge.Destination), nodeList.IndexOf(edge.Source));
            }
            else if (!connected.Contains(edge.Destination) && !connected.Contains(edge.Source))
            {
                connected.Add(edge.Destination);
                connected.Add(edge.Source);
                graphRes.Add(edge);
                union(nodeList.IndexOf(edge.Destination), nodeList.IndexOf(edge.Source));
            }
            else if (connected.Contains(edge.Destination) && connected.Contains(edge.Source) && (findAncestor(nodeList.IndexOf(edge.Destination)) != findAncestor(nodeList.IndexOf(edge.Source))))
            {
                graphRes.Add(edge);
                union(nodeList.IndexOf(edge.Destination), nodeList.IndexOf(edge.Source));
            }


        }

        AStar astar = new AStar(sizeX, sizeY);

        int count = 0;
        foreach (Edge edge in graphRes)
        {
            Debug.Log("Edge : " + edge.Source.PosX + " " + edge.Source.PosY + " " + edge.Destination.PosX + " " + edge.Destination.PosY);
            map = astar.trace(edge.Source, edge.Destination, map);
            count++;
        }
        return map;
    }

    private void insert(Edge edge)
    {
        for (int i = 0; i < edgeList.Count; i++)
        {
            if (edgeList[i].Price >= edge.Price)
            {
                edgeList.Insert(i, edge);
                return;
            }
        }
        edgeList.Insert(edgeList.Count, edge);
    }

    private void union(int index1, int index2)
    {
        int ancestor1 = findAncestor(index1);
        int ancestor2 = findAncestor(index2);
        ancestor[ancestor1] = ancestor2;
    }

    private int findAncestor(int index)
    {
        if (ancestor[index] != index)
        {
            return findAncestor(ancestor[index]);
        }
        return index;
    }

    void Start()
    {
        mazeGenerator();
        instantiateMaze();
        audioMaze.Play();
        Debug.Log("Titik: " + transform.position);
    }


    // Archieved


    private void instantiateItemRoom(int i, int j)
    {
        int[] roomX = { 0, 1, 2, 0, 2, 0, 1, 2 };
        int[] roomY = { 0, 0, 0, 1, 1, 2, 2, 2 };

        Instantiate(itemRoom1, new Vector3(i * 6, 0f, j * 6), Quaternion.identity);

        for (int k = 0; k < 8; k++)
        {
            for (int l = 0; l < 4; l++)
            {

                if (map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] == 'D')
                {
                    Instantiate(wallsRoomDoor[l], new Vector3(i * 6 + roomX[k] * 6 + sidesX[l] * 6, 0f, j * 6 + roomY[k] * 6 + sidesY[l] * 6), Quaternion.identity);
                }
                else if (map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] != 'I' && map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] != 'i')
                {
                    Instantiate(wallsRoom[l], new Vector3(i * 6 + roomX[k] * 6 + sidesX[l] * 6, 0f, j * 6 + roomY[k] * 6 + sidesY[l] * 6), Quaternion.identity);
                }
            }
        }
    }

    private void instantiateItemRoom2(int i, int j)
    {
        int[] roomX = { 0, 1, 2, 0, 2, 0, 1, 2 };
        int[] roomY = { 0, 0, 0, 1, 1, 2, 2, 2 };

        Instantiate(itemRoom2, new Vector3(i * 6, 0f, j * 6), Quaternion.identity);

        for (int k = 0; k < 8; k++)
        {
            for (int l = 0; l < 4; l++)
            {

                if (map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] == 'D')
                {
                    Instantiate(wallsRoomDoor[l], new Vector3(i * 6 + roomX[k] * 6 + sidesX[l] * 6, 0f, j * 6 + roomY[k] * 6 + sidesY[l] * 6), Quaternion.identity);
                }
                else if (map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] != 'Q' && map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] != 'q')
                {
                    Instantiate(wallsRoom[l], new Vector3(i * 6 + roomX[k] * 6 + sidesX[l] * 6, 0f, j * 6 + roomY[k] * 6 + sidesY[l] * 6), Quaternion.identity);
                }
            }
        }
    }

    private void instantiateEnemyRoom(int i, int j)
    {
        int[] roomX = { 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 3, 3 };
        int[] roomY = { 0, 1, 2, 3, 0, 3, 0, 3, 0, 1, 2, 3 };

        Instantiate(enemyRoom, new Vector3(i * 6, 0f, j * 6), Quaternion.identity);

        for (int k = 0; k < 12; k++)
        {
            for (int l = 0; l < 4; l++)
            {
                if (map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] == 'D')
                {
                    Instantiate(wallsRoomDoor[l], new Vector3(i * 6 + roomX[k] * 6 + sidesX[l] * 6, 0f, j * 6 + roomY[k] * 6 + sidesY[l] * 6), Quaternion.identity);
                }
                else if (map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] != 'E' && map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] != 'e')
                {
                    Instantiate(wallsRoom[l], new Vector3(i * 6 + roomX[k] * 6 + sidesX[l] * 6, 0f, j * 6 + roomY[k] * 6 + sidesY[l] * 6), Quaternion.identity);
                }
            }
        }
    }

    private void instantiateBossRoom(int i, int j)
    {
        int[] roomX = { 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 3, 3 };
        int[] roomY = { 0, 1, 2, 3, 0, 3, 0, 3, 0, 1, 2, 3 };

        Instantiate(bossRoom, new Vector3(i * 6, 0f, j * 6), Quaternion.identity);

        for (int k = 0; k < 12; k++)
        {
            for (int l = 0; l < 4; l++)
            {
                if (map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] == 'D')
                {
                    Instantiate(wallsRoomDoor[l], new Vector3(i * 6 + roomX[k] * 6 + sidesX[l] * 6, 0f, j * 6 + roomY[k] * 6 + sidesY[l] * 6), Quaternion.identity);
                }
                else if (map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] != 'B' && map[i + roomX[k] + sidesX[l], j + roomY[k] + sidesY[l]] != 'b')
                {
                    Instantiate(wallsRoom[l], new Vector3(i * 6 + roomX[k] * 6 + sidesX[l] * 6, 0f, j * 6 + roomY[k] * 6 + sidesY[l] * 6), Quaternion.identity);
                }
            }
        }
    }


    //private void generateItemRoom()
    //{
    //    int xRand = 0;
    //    int yRand = 0;

    //    int[] roomX = { 0, 1, 2, 0, 2, 0, 1, 2 };
    //    int[] roomY = { 0, 0, 0, 1, 1, 2, 2, 2 };

    //    int roomRand;
    //    int dirRand;

    //    bool detect;
    //    for (int i = 0; i < 2; i++)
    //    {
    //        do
    //        {
    //            detect = false;
    //            xRand = rng.Next(2, sizeX - 5);
    //            if (xRand % 2 == 1) xRand++;
    //            yRand = rng.Next(2, sizeY - 5);
    //            if (yRand % 2 == 1) yRand++;

    //            for (int j = -1; j < 4; j++)
    //            {
    //                for (int k = -1; k < 4; k++)
    //                {
    //                    if (map[xRand + j, yRand + k] != '#')
    //                    {
    //                        detect = true;
    //                    }
    //                }
    //            }
    //        } while (detect);

    //        for (int j = 0; j < 3; j++)
    //        {
    //            for (int k = 0; k < 3; k++)
    //            {
    //                map[xRand + j, yRand + k] = 'i';
    //            }
    //        }
    //        map[xRand, yRand] = 'I';

    //        do
    //        {
    //            roomRand = rng.Next(8);
    //            dirRand = rng.Next(4);

    //            if (map[xRand + roomX[roomRand] + sidesX[dirRand], yRand + roomY[roomRand] + sidesY[dirRand]] == '#')
    //            {
    //                map[xRand + roomX[roomRand] + sidesX[dirRand], yRand + roomY[roomRand] + sidesY[dirRand]] = 'D';
    //                break;
    //            }

    //        } while (true);
    //    }

    //    for (int i = 0; i < 2; i++)
    //    {
    //        do
    //        {
    //            detect = false;
    //            xRand = rng.Next(2, sizeX - 6);
    //            if (xRand % 2 == 1) xRand++;
    //            yRand = rng.Next(2, sizeY - 6);
    //            if (yRand % 2 == 1) yRand++;

    //            for (int j = -1; j < 4; j++)
    //            {
    //                for (int k = -1; k < 4; k++)
    //                {
    //                    if (map[xRand + j, yRand + k] != '#')
    //                    {
    //                        detect = true;
    //                    }
    //                }
    //            }
    //        } while (detect);

    //        for (int j = 0; j < 3; j++)
    //        {
    //            for (int k = 0; k < 3; k++)
    //            {
    //                map[xRand + j, yRand + k] = 'q';
    //            }
    //        }
    //        map[xRand, yRand] = 'Q';

    //        do
    //        {
    //            roomRand = rng.Next(8);
    //            dirRand = rng.Next(4);

    //            if (map[xRand + roomX[roomRand] + sidesX[dirRand], yRand + roomY[roomRand] + sidesY[dirRand]] == '#')
    //            {
    //                map[xRand + roomX[roomRand] + sidesX[dirRand], yRand + roomY[roomRand] + sidesY[dirRand]] = 'D';
    //                break;
    //            }

    //        } while (true);
    //    }

    //}

    private void generateItemRoom()
    {
        int[] roomX = { 0, 1, 2, 0, 2, 0, 1, 2 };
        int[] roomY = { 0, 0, 0, 1, 1, 2, 2, 2 };

        for (int i = 0; i < 2; i++)
        {
            TryPlaceRoom('i', 'I', roomX, roomY);
            TryPlaceRoom('q', 'Q', roomX, roomY);
        }
    }

    private void TryPlaceRoom(char fillChar, char centerChar, int[] roomX, int[] roomY)
    {
        int xRand, yRand;
        bool validPosition;

        do
        {
            xRand = GetEvenRandom(2, sizeX - 6);
            yRand = GetEvenRandom(2, sizeY - 6);
            validPosition = IsAreaEmpty(xRand, yRand, 3, 3);
        } while (!validPosition);

        FillRoom(xRand, yRand, fillChar, centerChar);
        ConnectRoom(xRand, yRand, roomX, roomY);
    }

    private int GetEvenRandom(int min, int max)
    {
        int value = rng.Next(min, max);
        return (value % 2 == 1) ? value + 1 : value;
    }

    private bool IsAreaEmpty(int x, int y, int width, int height)
    {
        for (int j = -1; j < width; j++)
        {
            for (int k = -1; k < height; k++)
            {
                if (map[x + j, y + k] != '#')
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void FillRoom(int x, int y, char fillChar, char centerChar)
    {
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                map[x + j, y + k] = fillChar;
            }
        }
        map[x, y] = centerChar;
    }

    private void ConnectRoom(int x, int y, int[] roomX, int[] roomY)
    {
        int roomRand, dirRand;
        do
        {
            roomRand = rng.Next(8);
            dirRand = rng.Next(4);
            int newX = x + roomX[roomRand] + sidesX[dirRand];
            int newY = y + roomY[roomRand] + sidesY[dirRand];

            if (map[newX, newY] == '#')
            {
                map[newX, newY] = 'D';
                break;
            }
        } while (true);
    }

    private void generateEnemyRoom()
    {
        int xRand = 0;
        int yRand = 0;

        int[] roomX = { 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 3, 3 };
        int[] roomY = { 0, 1, 2, 3, 0, 3, 0, 3, 0, 1, 2, 3 };

        int roomRand;
        int dirRand;


        bool detect;
        for (int i = 0; i < 4; i++)
        {
            do
            {
                detect = false;
                xRand = rng.Next(2, sizeX - 5);
                yRand = rng.Next(2, sizeY - 5);

                for (int j = -1; j < 5; j++)
                {
                    for (int k = -1; k < 5; k++)
                    {
                        if (map[xRand + j, yRand + k] != '#')
                        {
                            detect = true;
                        }
                    }
                }
            } while (detect);

            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    map[xRand + j, yRand + k] = 'e';
                }
            }
            map[xRand, yRand] = 'E';

            do
            {
                roomRand = rng.Next(8);
                dirRand = rng.Next(4);

                if (map[xRand + roomX[roomRand] + sidesX[dirRand], yRand + roomY[roomRand] + sidesY[dirRand]] == '#')
                {
                    map[xRand + roomX[roomRand] + sidesX[dirRand], yRand + roomY[roomRand] + sidesY[dirRand]] = 'D';
                    break;
                }

            } while (true);
        }
    }

    private void generateBossRoom()
    {
        int xRand = 0;
        int yRand = 0;

        int[] roomX = { 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 3, 3 };
        int[] roomY = { 0, 1, 2, 3, 0, 3, 0, 3, 0, 1, 2, 3 };

        int roomRand;
        int dirRand;

        bool detect;
        do
        {
            detect = false;
            xRand = rng.Next(2, sizeX - 5);
            yRand = rng.Next(2, sizeY - 5);

            for (int j = -1; j < 5; j++)
            {
                for (int k = -1; k < 5; k++)
                {
                    if (map[xRand + j, yRand + k] != '#')
                    {
                        detect = true;
                    }
                }
            }
        } while (detect);


        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                map[xRand + i, yRand + j] = 'b';
            }
        }
        map[xRand, yRand] = 'B';

        do
        {
            roomRand = rng.Next(8);
            dirRand = rng.Next(4);

            if (map[xRand + roomX[roomRand] + sidesX[dirRand], yRand + roomY[roomRand] + sidesY[dirRand]] == '#')
            {
                map[xRand + roomX[roomRand] + sidesX[dirRand], yRand + roomY[roomRand] + sidesY[dirRand]] = 'D';
                break;
            }

        } while (true);
    }


}