using System.Collections;
using UnityEngine;
using Unity.AI.Navigation;

public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;
    public bool MazeFinsihed;
    public NavMeshSurface navMeshSurface;

    private void Start()
    {
        StartCoroutine(BeginGame());
    }

    private IEnumerator BeginGame()
    {
        MazeFinsihed = false;
        mazeInstance = Instantiate(mazePrefab);
        yield return StartCoroutine(mazeInstance.Generate());
        Debug.Log("Maze generation completed.");

        BakeNavMesh();
        Debug.Log("NavMesh baking completed.");
        MazeFinsihed = true;

    }

    private void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
