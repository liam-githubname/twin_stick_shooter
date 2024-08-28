using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public bool canSpawn;
    public bool spawnGolden;
    [SerializeField] private GameObject[] asteroids;
    [SerializeField] public GameObject[] goldenAsteroids;
    private GameObject asteroidsParent;
    public GameObject asteroidInstance;

    // Start is called before the first frame update
    void Start()
    {
        asteroidsParent = GameObject.Find("Asteroids");
        canSpawn = false;
        spawnGolden = false;
    }

    void Update()
    {
        if (canSpawn)
            SpawnAsteroids();
    }

    private void SpawnAsteroids()
    {
        if (!canSpawn)
            return;

        GameObject asteroidPrefab;

        // Select a random asteroid prefab to spawn based on the spawnGolden flag
        if (!spawnGolden)
        {
            asteroidPrefab = asteroids[Random.Range(0, asteroids.Length)];
        }
        else
        {
            asteroidPrefab = goldenAsteroids[Random.Range(0, goldenAsteroids.Length)];
        }

        if (asteroidPrefab == null)
            return;

        // Instantiate the asteroid at the specified position and rotation
        GameObject asteroidInstance = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
        Debug.Log("Spawned asteroid at spawner " + gameObject.name);

        // Set the position of the instantiated asteroid
        asteroidInstance.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);

        // Add the AsteroidProperties component to the instantiated asteroid if it doesn't already have one
        AsteroidProperties asteroidProperties = asteroidInstance.GetComponent<AsteroidProperties>();
        if (asteroidProperties == null)
        {
            asteroidInstance.AddComponent<AsteroidProperties>();
        }

        // Add or configure the BoxCollider2D
        BoxCollider2D collider = asteroidInstance.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = asteroidInstance.AddComponent<BoxCollider2D>();
        }
        Vector2 asteroidBoxColliderSize = compareAsteroidString(asteroidInstance);
        collider.size = asteroidBoxColliderSize;

        // Set additional properties for the instantiated asteroid
        if (!spawnGolden)
            asteroidInstance.tag = "asteroid";
        else
            asteroidInstance.tag = "gold_asteroid";

        asteroidInstance.layer = LayerMask.NameToLayer("Asteroids");
        if (asteroidsParent != null)
            asteroidInstance.transform.SetParent(asteroidsParent.transform);

        // Update spawn control flag
        canSpawn = false;
    }

    Vector2 compareAsteroidString(GameObject asteroid)
    {
        Vector2 result = Vector2.zero;

        if (asteroid.name.Contains("AsterMed") || asteroid.name.Contains("AsterBig"))
            result = new Vector2(2, 2);
        else if (asteroid.name.Contains("AsterHuge"))
            result = new Vector2(3.5f, 3.5f);

        return result;
    }
}
