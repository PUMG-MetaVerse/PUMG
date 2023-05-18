using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject targetPrefab2;
    public GameObject targetPrefab3;

    public Vector3 spawnRange = new Vector3(1700, 500, 1700);

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnTarget();
        }
        for (int i = 0; i < 2; i++)
        {
            SpawnTarget2();
        }
        SpawnTarget3();
    }

    void SpawnTarget()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(0, spawnRange.x),
            Random.Range(0, spawnRange.y),
            Random.Range(0, spawnRange.z)
        );

        GameObject newTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
        newTarget.name = "TargetCube";
    }
    void SpawnTarget2()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(0, spawnRange.x),
            Random.Range(0, spawnRange.y),
            Random.Range(0, spawnRange.z)
        );

        GameObject newTarget = Instantiate(targetPrefab2, spawnPosition, Quaternion.identity);
        newTarget.name = "TargetCube";
    }
    void SpawnTarget3()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(0, spawnRange.x),
            Random.Range(0, spawnRange.y),
            Random.Range(0, spawnRange.z)
        );

        GameObject newTarget = Instantiate(targetPrefab3, spawnPosition, Quaternion.identity);
        newTarget.name = "TargetCube";
    }
}
