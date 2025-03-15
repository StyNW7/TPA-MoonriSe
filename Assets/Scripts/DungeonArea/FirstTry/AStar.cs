using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    // Start is called before the first frame update
    private int sizeX;
    private int sizeY;

    private int counter;

    private List<Node> openList;
    public AStar(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
    }

    public void insertNode(Node node)
    {
        if (openList.Contains(node))
        {
            return;
        }
        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].Heuristic > node.Heuristic)
            {

                openList.Insert(i, node);
                return;
            }
        }

        openList.Insert(openList.Count, node);
    }

    public char[,] duplicateMap(char[,] map)
    {
        char[,] tempMap = new char[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                tempMap[i, j] = map[i, j];
            }
        }

        return tempMap;
    }

    public char[,] trace(Node start, Node end, char[,] map)
    {

        int[] dirX = { 0, 1, 0, -1 };
        int[] dirY = { 1, 0, -1, 0 };
        char[,] tempMap = duplicateMap(map);

        openList = new List<Node>();

        insertNode(start);
        Node curr = null;

        while (openList.Count > 0)
        {
            Debug.Log("OpenListCount[0]" + openList[0]);
            curr = openList[0];
            openList.RemoveAt(0);
            tempMap[curr.PosX, curr.PosY] = 'X';
            counter++;

            //if (counter > 1000)
            //{
            //    return traceBack(curr, map);
            //}



            if (curr.PosX == end.PosX && curr.PosY == end.PosY)
            {
                Debug.Log("Masuk curr posx posy");
                return traceBack(curr, map);
            }

            for (int i = 0; i < 4; i++)
            {
                if (curr.PosX + dirX[i] <= 0 || curr.PosY + dirY[i] <= 0 || curr.PosX + dirX[i] >= sizeX || curr.PosY + dirY[i] >= sizeY)
                {

                    continue;
                }
                if (tempMap[curr.PosX + dirX[i], curr.PosY + dirY[i]] == '#' || tempMap[curr.PosX + dirX[i], curr.PosY + dirY[i]] == ' ' || tempMap[curr.PosX + dirX[i], curr.PosY + dirY[i]] == 'D')
                {
                    Node newNode = new Node(curr.PosX + dirX[i], curr.PosY + dirY[i]);
                    newNode.SetHeuristic(end.PosX, end.PosY);
                    newNode.PrevNode = curr;
                    Debug.Log("New Node Prev: " + newNode.PrevNode.PosX);
                    insertNode(newNode);
                }


            }


        }

        return traceBack(curr, map);
    }

    public char[,] traceBack(Node node, char[,] map)
    {
        Node curr = node;

        do
        {
            Debug.Log("Current: " + curr.PosX + " " + curr.PosY);
            if (curr.Heuristic == -1)
            {
                Debug.Log("Masuk do heu");
                break;
            }
            if (map[curr.PosX, curr.PosY] != 'D')
            {
                map[curr.PosX, curr.PosY] = ' ';
            }

            curr = curr.PrevNode;

        } while (true);

        return map;
    }
}