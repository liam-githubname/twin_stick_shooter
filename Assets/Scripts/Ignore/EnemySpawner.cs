using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public GameObject bulletEnemyPrefab;
    [SerializeField] public GameObject suiciderEnemyPrefab;
    [SerializeField] public GameObject bomberEnemyPrefab;
    public Transform[] spawnPoints;
    public string enemyTag1 = "enemy"; // Tag used to identify enemy objects
    public string enemyTag2 = "bulletenemy";
    [SerializeField] public int waveNumber = 0;
    [SerializeField] public float timeBetweenWaves = 5f;
    private float waveCountdown;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {

        if (waveCountdown <= 0)
        {
            int totalEnemies = GetEnemyCount1() + GetEnemyCount2();
            if (totalEnemies <= 1) // Only spawn new waves if 1 or fewer total enemies are present
            {
                StartCoroutine(SpawnWave());
            }
            waveCountdown = timeBetweenWaves;
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }


    IEnumerator SpawnWave()
    {
        waveNumber++;
        for (int i = 0; i < waveNumber; i++)
        {
            SpawnEnemy(bulletEnemyPrefab);
            if (waveNumber > 1) // Start adding suicider enemies from the second wave
            {
                SpawnEnemy(suiciderEnemyPrefab);
            }
            else if (waveNumber == 3) // Start adding bomer enemies from the second wave
            {
                SpawnEnemy(bomberEnemyPrefab);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
    }

    int GetEnemyCount1()
    {
        return GameObject.FindGameObjectsWithTag(enemyTag1).Length;
    }
    int GetEnemyCount2()
    {
        return GameObject.FindGameObjectsWithTag(enemyTag2).Length;
    }
}
