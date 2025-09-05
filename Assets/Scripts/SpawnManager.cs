using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnDelay = 2.0f;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            Vector3 playerPosition = PlayerControl.Instance.transform.position;
            Vector3 randomDirection = playerPosition + new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f));
            Vector3 spawnPosition = randomDirection.normalized * 20f;

            Instantiate(enemyPrefab, spawnPosition, enemyPrefab.transform.rotation);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
