using System.Collections.Generic;
using UnityEngine;

public class SpawnerAsteroids : MonoBehaviour
{
    [SerializeField] int asteroidLimit = 55;
    [SerializeField] int goldenAsteroidLimit = 15;
    [SerializeField] public List<GameObject> asteroidsOnMap;
    [SerializeField] GameObject[] spawnersArr;
    public Vector2 spawnPos = Vector2.zero;
    [SerializeField] public List<GameObject> goldenAsteroidsOnMap;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (asteroidsOnMap.Count < asteroidLimit)
            instantiateAsteroid();
        if (goldenAsteroidsOnMap.Count < goldenAsteroidLimit)
            instantiateGoldenAsteroid();
    }

    private void instantiateAsteroid()
    {
        GameObject spawnerChosen = spawnersArr[Random.Range(0, spawnersArr.Length)];
        spawnerChosen.GetComponent<Spawner>().canSpawn = true;
        spawnerChosen.GetComponent<Spawner>().spawnGolden = false;
        asteroidsOnMap.Add(spawnerChosen.GetComponent<Spawner>().asteroidInstance);
    }

    private void instantiateGoldenAsteroid()
    {
        GameObject spawnerChosen = spawnersArr[Random.Range(0, spawnersArr.Length)];
        spawnerChosen.GetComponent<Spawner>().canSpawn = true;
        spawnerChosen.GetComponent<Spawner>().spawnGolden = true;
        goldenAsteroidsOnMap.Add(spawnerChosen.GetComponent<Spawner>().asteroidInstance);
    }
}
