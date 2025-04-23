using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> asteroidPrefabs;
    [SerializeField] private float maxSpawnRange = 1000;
    [SerializeField] private float maxSpawnCount = 100;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private Transform player;

    private float count = 0;

    void Start()
    {
        Invoke(nameof(SpawnAsteroid), spawnInterval);
    }

    void SpawnAsteroid()
    {
        count++;
        if (maxSpawnCount > count)
        {
            Invoke(nameof(SpawnAsteroid), spawnInterval);
        }

        GameObject prefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)];
        GameObject asteroid = Instantiate(prefab, transform);

        float scale = Random.Range(1.0f, 50.0f);
        asteroid.transform.localScale = scale * Vector3.one;

        Vector3 min = player.position - maxSpawnRange * Vector3.one;
        Vector3 max = player.position + maxSpawnRange * Vector3.one;

        asteroid.transform.position = new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }
}
