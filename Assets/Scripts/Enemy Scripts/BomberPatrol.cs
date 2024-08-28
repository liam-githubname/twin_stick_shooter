using UnityEngine;

public class BomberPatrol : MonoBehaviour
{
    public float patrolSpeed = 5f;
    public GameObject bombPrefab;
    public float minDropInterval = 1f; // Minimum time interval between bomb drops
    public float maxDropInterval = 5f; // Maximum time interval between bomb drops

    private float dropCounter;
    private Vector2 screenBounds;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        SetRandomDropTime();
    }

    void Update()
    {
        Patrol();
        DropBomb();
    }

    void Patrol()
    {
        // Move the bomber back and forth across the screen
        if (transform.position.x > screenBounds.x || transform.position.x < -screenBounds.x)
        {
            patrolSpeed = -patrolSpeed; // Change direction
        }
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
    }

    void DropBomb()
    {
        dropCounter -= Time.deltaTime;
        if (dropCounter <= 0)
        {
            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            bomb.GetComponent<Rigidbody2D>().freezeRotation = true;
            bomb.GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezePositionX;
            bomb.transform.parent = GameObject.Find("Ammo").transform;
            Physics2D.IgnoreCollision(bomb.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            SetRandomDropTime();
        }
    }

    void SetRandomDropTime()
    {
        dropCounter = Random.Range(minDropInterval, maxDropInterval);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other != null && other.gameObject.tag != "ship")
            Physics2D.IgnoreCollision(other.collider, gameObject.GetComponent<Collider2D>());
    }
}
