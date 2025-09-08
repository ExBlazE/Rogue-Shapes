using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnDelay = 2.0f;
    [SerializeField] private float spawnDistance = 25f;

    void Start()
    {
        StartCoroutine(SpawnEnemyWave());
    }

    // Coroutine to continuously spawn enemies after a certain interval
    IEnumerator SpawnEnemyWave()
    {
        while (true)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Method to spawn enemy
    void SpawnEnemy()
    {
        // Get player position via singleton
        Vector3 playerPosition = PlayerControl.Instance.transform.position;

        // Get random direction from player and set vector magnitude to 1
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        randomDirection.Normalize();

        // Set spawn position at exactly a certain distance from player
        Vector3 spawnPosition = playerPosition + (randomDirection * spawnDistance);

        // Spawn the enemy
        Instantiate(enemyPrefab, spawnPosition, enemyPrefab.transform.rotation);
    }
}
