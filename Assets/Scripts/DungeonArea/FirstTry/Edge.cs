using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    private Node source;
    private Node destination;

    private float price;
    public Edge(Node source, Node destination)
    {
        this.source = source;
        this.destination = destination;

        this.price = Mathf.Sqrt(Mathf.Pow((source.PosX - destination.PosX), 2) + Mathf.Pow((source.PosY - destination.PosY), 2));
    }

    public Node Source
    {
        get { return source; }
    }

    public Node Destination
    {
        get { return destination; }
    }

    public float Price
    {
        get { return price; }
    }
}