using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float rand;
    [SerializeField] private float minRate = 3f;
    [SerializeField] private float maxRate = 6f;
    [SerializeField] private bool canSpawn = true;
    [SerializeField] private GameObject[] asteroids;
    [SerializeField] float posX;
    [SerializeField] float posY;
    [SerializeField] float moveSpeed;
    private AsteroidMovement asteroidMovement;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAsteroids());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SpawnAsteroids()
    {

        WaitForSeconds wait;

        while (canSpawn)
        {
            rand = Random.Range(minRate, maxRate);
            wait = new WaitForSeconds(rand);
            yield return wait;
            rand = Random.Range(0, asteroids.Length);
            GameObject asteroidToSpawn = asteroids[(int)rand];
            asteroidMovement = asteroidToSpawn.GetComponent<AsteroidMovement>();
            asteroidMovement.moveVector = new Vector3(posX, posY, 0) * moveSpeed;
            GameObject instantiatedAsteroid = Instantiate(asteroidToSpawn, transform.position, Quaternion.identity);
             instantiatedAsteroid.tag = "asteroid";
        }
    }
}
