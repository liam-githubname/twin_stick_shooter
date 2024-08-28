using System.Collections;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    private ScoreManager playerScore;
    [SerializeField] int timeBetweenWaves = 20;
    [SerializeField] int easyScore = 0;
    [SerializeField] int mediumScore = 5000;
    [SerializeField] int hardScore = 10000;
    private bool spawnEasy;
    private bool spawnMedium;
    private bool spawnHard;
    private GameObject playerObj;
    private GameObject enemiesParent;
    [SerializeField] GameObject[] enemyObjs;
    [SerializeField] GameObject[] spawners;
    private float offset = 17.5f;
    private bool canSpawn;

    // Start is called before the first frame update
    void Start()
    {
        playerScore = FindObjectOfType<ScoreManager>();
        playerObj = GameObject.Find("Ship");
        enemiesParent = GameObject.Find("Enemies");
        StartCoroutine(SpawnWave());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScore.score >= easyScore && playerScore.score < mediumScore)
            spawnEasy = true;
        else if (playerScore.score >= mediumScore && playerScore.score < hardScore)
        {
            spawnEasy = false;
            spawnMedium = true;
        }
        else
        {
            spawnMedium = false;
            spawnHard = true;
        }

        if (enemiesParent.transform.childCount == 0)
            StartCoroutine(SpawnWave());

        if (enemiesParent.transform.childCount > 0)
            canSpawn = false;
        else
            canSpawn = true;
    }

    // get a random spawner from array
    // create function in spawner.cs to handle enemies
    // depending on the position of spawner, change enemy vector
    IEnumerator SpawnWave()
    {
        while (playerObj != null && canSpawn)
        {
            SpawnEnemiesBasedOnDifficulty();
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemiesBasedOnDifficulty()
    {
        WaveConfiguration config = GetWaveConfiguration();
        SpawnEnemies(config);
    }

    WaveConfiguration GetWaveConfiguration()
    {
        if (spawnEasy)
            return new WaveConfiguration(3, 5, 5);
        else if (spawnMedium)
            return new WaveConfiguration(5, 10, 10);
        else if (spawnHard)
            return new WaveConfiguration(7, 15, 15);

        return new WaveConfiguration(0, 0, 0);  // Default case if no difficulty set
    }

    void SpawnEnemies(WaveConfiguration config)
    {
        SpawnEnemyType(0, config.NumOfBombers);  // index 0 of enemyObjs represents Bombers
        SpawnEnemyType(1, config.NumOfShooters); // index 1 of enemyObjs represents Shooters
        SpawnEnemyType(2, config.NumOfSuiciders); // index 2 of enemyObjs represents Suiciders
    }

    void SpawnEnemyType(int enemyIndex, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject randSpawner;

            if (enemyIndex == 0)
                randSpawner = spawners[Random.Range(23, 35)];
            else
                randSpawner = spawners[Random.Range(0, spawners.Length)];

            Vector3 newEnemyPos = randSpawner.transform.position;

            if (newEnemyPos.x > 120)
                newEnemyPos = new Vector3(newEnemyPos.x - offset, newEnemyPos.y, -0.2f);
            else if (newEnemyPos.x < -120)
                newEnemyPos = new Vector3(newEnemyPos.x + offset, newEnemyPos.y, -0.2f);
            else if (newEnemyPos.y > 60)
                newEnemyPos = new Vector3(newEnemyPos.x, newEnemyPos.y - offset, -0.2f);
            else if (newEnemyPos.y < -60)
                newEnemyPos = new Vector3(newEnemyPos.x, newEnemyPos.y + offset, -0.2f);

            GameObject enemyInstance = Instantiate(enemyObjs[enemyIndex], newEnemyPos, Quaternion.identity);

            if (enemyIndex == 0)
                enemyInstance.tag = "bomber";
            else if (enemyIndex == 1)
                enemyInstance.tag = "bulletenemy";
            else if (enemyIndex == 2)
                enemyInstance.tag = "enemy";

            enemyInstance.layer = LayerMask.NameToLayer("Enemy");
            enemyInstance.transform.SetParent(GameObject.Find("Enemies").transform);
        }
    }

    struct WaveConfiguration
    {
        public int NumOfBombers;
        public int NumOfShooters;
        public int NumOfSuiciders;

        public WaveConfiguration(int bombers, int shooters, int suiciders)
        {
            NumOfBombers = bombers;
            NumOfShooters = shooters;
            NumOfSuiciders = suiciders;
        }
    }
}
