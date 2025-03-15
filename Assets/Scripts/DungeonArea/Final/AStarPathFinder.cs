using System.Collections.Generic;
using UnityEngine;

public static class AStarPathFinder
{
    public static List<Vector2Int> FindPath(int[,] grid, Vector2Int start, Vector2Int end)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        Node startNode = new Node(start, null, 0, GetHeuristic(start, end));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            openList.Sort((a, b) => (a.F).CompareTo(b.F));
            Node currentNode = openList[0];

            if (currentNode.Position == end)
                return RetracePath(currentNode);

            openList.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (Vector2Int neighbor in GetNeighbors(currentNode.Position, grid))
            {
                if (closedSet.Contains(new Node(neighbor, null, 0, 0)))
                    continue;

                int gCost = currentNode.G + 1;
                Node neighborNode = new Node(neighbor, currentNode, gCost, GetHeuristic(neighbor, end));

                if (!openList.Contains(neighborNode) || gCost < neighborNode.G)
                {
                    openList.Add(neighborNode);
                }
            }
        }

        return new List<Vector2Int>();
    }

    private static int GetHeuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private static List<Vector2Int> RetracePath(Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        while (currentNode != null)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    private static List<Vector2Int> GetNeighbors(Vector2Int position, int[,] grid)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Directions for neighbors (up, down, left, right)
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int newPos = position + dir;

            // Check if the new position is within bounds
            if (newPos.x >= 0 && newPos.x < grid.GetLength(0) && newPos.y >= 0 && newPos.y < grid.GetLength(1))
            {
                if (grid[newPos.x, newPos.y] != 1) // 1 = Room, 2 = Wall
                {
                    neighbors.Add(newPos);
                }
            }
        }

        return neighbors;
    }


     private class Node
    {
        public Vector2Int Position;
        public Node Parent;
        public int G;
        public int H;

        public int F => G + H;

        public Node(Vector2Int position, Node parent, int g, int h)
        {
            Position = position;
            Parent = parent;
            G = g;
            H = h;
        }

        public override bool Equals(object obj)
        {
            if (obj is Node other)
                return Position.Equals(other.Position);
            return false;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }


}