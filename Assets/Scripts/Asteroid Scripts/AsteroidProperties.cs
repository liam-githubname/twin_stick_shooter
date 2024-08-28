using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidProperties : MonoBehaviour
{
    public Vector2 direction = Vector2.zero;
    private int left = -120;
    private int right = 120;
    private int top = 60;
    private int bottom = -60;
    private Vector2 currentVector;
    private Vector2 targetVector;
    private int mapOffset = 5;
    private Rigidbody2D rb;
    private readonly int[] dirs = { 1, -1 };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (gameObject.transform.position.x < right && gameObject.transform.position.x > left && gameObject.transform.position.y < top && gameObject.transform.position.y > bottom)
            direction = new Vector2(dirs[Random.Range(0, 2)], dirs[Random.Range(0, 2)]);
        else
            direction = GetVectorDirection();

        direction.Normalize();
        direction *= 1.25f;
        rb.velocity = direction;
        currentVector = direction;
        float randomAngle = Random.Range(0f, 360f);
        rb.rotation = randomAngle;
        rb.angularDrag = 0;
        rb.angularVelocity = 45f * dirs[Random.Range(0, 2)];
    }

    // Update is called once per frame
    void Update()
    {
        DestroyAsteroid();
        // Normalize the current vector a bit more towards the normalized target vector each frame
        if (currentVector != Vector2.zero) // Check to avoid division by zero
        {
            targetVector = rb.velocity.normalized * 1.25f; // Maintain the same speed, just normalize direction
                                                           // Now Lerp towards this new targetVector
            currentVector = Vector2.Lerp(rb.velocity, targetVector, Time.deltaTime);
            rb.velocity = currentVector; // Apply the lerped velocity back to the Rigidbody
        }
        if (rb.angularVelocity > 45f || rb.angularVelocity < -45f)
            rb.angularDrag = 0.1f;
        else
            rb.angularDrag = 0f;
    }

    Vector2 GetVectorDirection()
    {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;

        if (x < left && x > left - mapOffset)
            direction = new Vector2(1, 0);
        else if (x > right && x < right + mapOffset)
            direction = new Vector2(-1, 0);
        else if (y < bottom && y > bottom - mapOffset)
            direction = new Vector2(0, 1);
        else if (y > top && y < top + mapOffset)
            direction = new Vector2(0, -1);

        return direction;
    }

    void DestroyAsteroid()
    {
        GameObject currObj = gameObject;
        if (currObj.transform.position.x > right + mapOffset || currObj.transform.position.x < left - mapOffset || currObj.transform.position.y > top + mapOffset || currObj.transform.position.y < bottom - mapOffset)
        {
            if (currObj.CompareTag("gold_asteroid"))
                GameObject.Find("Asteroids").GetComponent<SpawnerAsteroids>().goldenAsteroidsOnMap.Remove(currObj);
            else if (currObj.CompareTag("asteroid") || currObj.CompareTag("thrown_asteroid"))
                GameObject.Find("Asteroids").GetComponent<SpawnerAsteroids>().asteroidsOnMap.Remove(currObj);

            Destroy(currObj);
        }
    }
}
