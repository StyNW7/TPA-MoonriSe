using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshUpdater : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

}
