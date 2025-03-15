using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public class Node
    {
        public Vector2Int position;
        public int gCost;
        public int hCost;
        public int fCost => gCost + hCost;
        public Node parent;

        public Node(Vector2Int pos)
        {
            position = pos;
        }
    }

    private Dictionary<Vector2Int, Node> nodes = new Dictionary<Vector2Int, Node>();
    private List<Node> openList = new List<Node>();
    private HashSet<Node> closedList = new HashSet<Node>();

    public List<Vector2Int> FindPath(Vector2Int startPos, Vector2Int endPos, HashSet<Vector2Int> walkableTiles)
    {
        nodes.Clear();
        openList.Clear();
        closedList.Clear();

        foreach (var tile in walkableTiles)
        {
            nodes[tile] = new Node(tile);
        }

        if (!nodes.ContainsKey(startPos) || !nodes.ContainsKey(endPos))
        {
            Debug.LogError("Start or End position is not walkable!");
            return null;
        }

        Node startNode = nodes[startPos];
        Node endNode = nodes[endPos];

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = currentNode.position + direction;
                if (!nodes.ContainsKey(neighborPos) || closedList.Contains(nodes[neighborPos]))
                    continue;

                Node neighborNode = nodes[neighborPos];
                int newMovementCost = currentNode.gCost + 10;

                if (newMovementCost < neighborNode.gCost || !openList.Contains(neighborNode))
                {
                    neighborNode.gCost = newMovementCost;
                    neighborNode.hCost = CalculateHCost(neighborNode.position, endNode.position);
                    neighborNode.parent = currentNode;

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        return null;
    }

    private Node GetLowestFCostNode(List<Node> nodeList)
    {
        Node lowestFCostNode = nodeList[0];
        foreach (var node in nodeList)
        {
            if (node.fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = node;
            }
        }
        return lowestFCostNode;
    }

    private List<Vector2Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private int CalculateHCost(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) * 10;
    }
}
