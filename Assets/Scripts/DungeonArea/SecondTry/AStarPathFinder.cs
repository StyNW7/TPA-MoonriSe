using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder
{
    private int width, height;
    private HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
    private List<Vector2Int> openSet = new List<Vector2Int>();

    public AStarPathfinder(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        openSet.Clear();
        closedSet.Clear();
        openSet.Add(start);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> gScore = new Dictionary<Vector2Int, int>
        {
            [start] = 0
        };
        Dictionary<Vector2Int, int> fScore = new Dictionary<Vector2Int, int>
        {
            [start] = Heuristic(start, goal)
        };

        while (openSet.Count > 0)
        {
            Vector2Int current = GetLowestFScore(openSet, fScore);
            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                int tentativeGScore = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<Vector2Int>();
    }

    private Vector2Int GetLowestFScore(List<Vector2Int> openSet, Dictionary<Vector2Int, int> fScore)
    {
        Vector2Int best = openSet[0];
        int bestScore = fScore[best];

        foreach (var node in openSet)
        {
            if (fScore[node] < bestScore)
            {
                best = node;
                bestScore = fScore[node];
            }
        }

        return best;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            node + Vector2Int.up,
            node + Vector2Int.down,
            node + Vector2Int.left,
            node + Vector2Int.right
        };

        return neighbors;
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
