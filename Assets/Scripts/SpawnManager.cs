using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [Space]
    [SerializeField] private float spawnDelayStart = 3.0f;
    [SerializeField] private float spawnDelayReduce = 0.5f;
    [SerializeField] private float spawnDelayMin = 0.5f;

    [Space]
    [SerializeField] private int stageLength = 20;

    [Space]
    [SerializeField] private float spawnDistance = 25f;

    private GameManager gameManager;

    void Awake()
    {
        // Get game manager reference via singleton
        gameManager = GameManager.Instance;
    }

    void Start()
    {
        // Start coroutine to spawn enemies
        StartCoroutine(SpawnEnemyWave());
    }

    // Coroutine to spawn enemies with increasing difficulty
    IEnumerator SpawnEnemyWave()
    {
        float spawnDelay = spawnDelayStart;
        int currentStage = 0;

        // Loop to spawn single enemies continuously with a delay in between
        while (true)
        {
            // Spawn a single enemy
            SpawnEnemy();

            // Get the current difficulty stage
            int newStage = (int)gameManager.GetTimeAlive() / stageLength;

            // If stage advances, set new stage and reduce delay between enemy spawns
            if (currentStage < newStage)
            {
                currentStage = newStage;
                spawnDelay -= spawnDelayReduce;
                if (spawnDelay < spawnDelayMin)
                    spawnDelay = spawnDelayMin;
            }

            // Wait out the delay before re-running spawn loop
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
