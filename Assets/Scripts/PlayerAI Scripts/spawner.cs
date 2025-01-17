using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class spawner : MonoBehaviour
{

    public GameObject enemy;
    public GameObject[] spawnpoints;
    public float spawnTime;
    public float spawnDelay;
    public int wave;
    public float navMeshSampleRadius = 2.0f;
    public int wavenum;
    public NavMeshSurface navMeshSurface;
    // Use this for initialization
    void Start()
    {
        spawnpoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Debug.Log("there are "+ spawnpoints.Length+" spawnpoints");


        navMeshSurface.BuildNavMesh();

        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
   
    }

    public void SpawnObject()
    {
        for (int i = 0; i < wavenum; i++)
        {
            int randomPoint = Random.Range(0, spawnpoints.Length);
            Vector3 spawnPosition = spawnpoints[randomPoint].transform.position;

            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(spawnPosition, out hit, navMeshSampleRadius, UnityEngine.AI.NavMesh.AllAreas))
            {
                // Spawn the enemy at the closest valid position on the NavMesh
                Instantiate(enemy, hit.position, spawnpoints[randomPoint].transform.rotation);
            }
            else
            {
                Debug.LogWarning($"Failed to find a valid NavMesh position near {spawnPosition}. Enemy not spawned.");
            }

        }
        wave++;
    }
}
