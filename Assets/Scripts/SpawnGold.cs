using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGold : MonoBehaviour
{
    [SerializeField] public GameObject[] goldenAsteroids;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator spawn()
    {
        while (true)
        {
            GameObject asteroidPrefab = goldenAsteroids[Random.Range(0, goldenAsteroids.Length)];
            GameObject asteroidInstance = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);

            asteroidInstance.transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(-1f, 1f), 1f);
            AsteroidProperties asteroidProperties = asteroidInstance.GetComponent<AsteroidProperties>();
            if (asteroidProperties == null)
            {
                asteroidInstance.AddComponent<AsteroidProperties>();
            }

            // Set additional properties for the instantiated asteroid
            asteroidInstance.tag = "gold_asteroid";
            asteroidInstance.layer = LayerMask.NameToLayer("Asteroids");
            asteroidInstance.transform.SetParent(GameObject.Find("Asteroids").transform);

            yield return new WaitForSeconds(7);
        }
    }
}
