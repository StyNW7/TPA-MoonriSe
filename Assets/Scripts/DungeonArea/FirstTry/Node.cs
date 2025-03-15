using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    private int posX;
    private int posY;
    private Node prevNode;

    private float heuristic = -1;
    public Node(int posX, int posY)
    {
        this.posX = posX;
        this.posY = posY;
    }

    public int PosX
    {
        get { return posX; }
        set { posX = value; }
    }

    public int PosY
    {
        get { return posY; }
        set { posY = value; }
    }

    public float Heuristic
    {
        get { return heuristic; }
    }

    public Node PrevNode
    {
        get { return prevNode; }
        set { prevNode = value; }
    }


    public void SetHeuristic(int tarPosX, int tarPosY)
    {
        heuristic = Mathf.Sqrt(Mathf.Pow(posX - tarPosX, 2) + Mathf.Pow(posY - tarPosY, 2));
    }
}