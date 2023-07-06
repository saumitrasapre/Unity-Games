using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private List<GameObject> spawnPoints = null;
    [SerializeField] private int count = 20;
    [SerializeField] private float minDelay = 0.8f;
    [SerializeField] private float maxDelay = 1.5f;

    IEnumerator SpawnCoroutine()
    {
        while (count > 0)
        {
            count--;
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Vector2 randomOffset = Random.insideUnitCircle;
            Vector2 spawnPoint = spawnPoints[randomIndex].transform.position + (Vector3)randomOffset;

            SpawnEnemy(spawnPoint);

            float randomTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomTime);
        }
    }

    private void SpawnEnemy(Vector2 spawnPoint)
    {
        Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
    }

    private void Start()
    {
        if (spawnPoints.Count > 0)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                SpawnEnemy(spawnPoint.transform.position);
            }
        }

        StartCoroutine(SpawnCoroutine());
    }
}
