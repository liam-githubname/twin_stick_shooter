using UnityEngine;

public class EnemyOne : MonoBehaviour
{
    public GameObject player;
    public Ship_Properties sp;
    public float chaseSpeedFasterTimes = 1.1f;
    public float attackRange = 30f;
    public float attackCooldown = 1f;
    //public float patrolRadius = 200f;
    //public int numberOfWaypoints = 20;
   // public float waypointRadius = 50f;
    public float spreadShotChange = 0.1f;
   // public Vector3 mapCenter = Vector3.zero;
   // public Vector2 mapSize = new Vector2(100, 100);
   // public Transform[] waypoints;
    // private int waypointIndex = 0;
    public GameObject bulletPrefab; // Assign this in the Unity editor
    public Transform shootingPoint; // The point from which bullets are fired
    public float bulletSpeed = 40f;
    private float shootTimer;
    public int spreadShotBullets = 5; // Number of bullets in a spread shot
    public float detectionDistance = 50f;
    private int left = -120;
    private int right = 120;
    private int top = 60;
    private int bottom = -60;
    private Vector3 patrolTarget;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Ship");
        sp = GetComponent<Ship_Properties>();
        // Programmatic waypoint generation
        // patrolTarget = GetRandomPositionInMap();
    }

    private void Update()
    {
        if (player == null) return;
        ObstacleAvoidanceCheck();
        AvoidOtherEnemies();
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {

            if (gameObject.tag == "bulletenemy")
            {
                Vector2 directionToPlayer = player.transform.position - transform.position;
                float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));

                // Check if the enemy is facing the player within a certain angle before shooting
                float angleToPlayer = Vector3.Angle(transform.up, directionToPlayer);
                if (angleToPlayer <= 45)
                {
                    shootTimer -= Time.deltaTime;
                    if (shootTimer <= 0)
                    {
                        if (Random.value <= spreadShotChange)
                        {
                            // Perform spread shot
                            PerformSpreadShot();
                        }
                        else
                        {
                            attackShot();
                        }
                        shootTimer = attackCooldown; // Reset the shooting timer
                    }
                }
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        sp.SetInput(direction, false, false);
        transform.position += direction * sp.moveSpeed * Time.deltaTime * chaseSpeedFasterTimes;
    }

    void Patrol()
{
    if (Vector3.Distance(transform.position, patrolTarget) < 0.5f)
    {
        // Get a new patrol target when close to the current target
        patrolTarget = GetRandomPositionInMap();
    }

    Vector3 directionToTarget = (patrolTarget - transform.position).normalized;
    // Adjust this direction based on the presence of nearby enemies
    Vector3 avoidanceVector = CalculateAvoidanceVector();
    Vector3 effectiveDirection = (directionToTarget + avoidanceVector).normalized;

     
    sp.SetInput(effectiveDirection, false, false);
    transform.position += effectiveDirection * sp.moveSpeed * 0.1f * Time.deltaTime;
}

// avoid gather together when partoling
Vector3 CalculateAvoidanceVector()
{
    float radius = 15f; 
    LayerMask enemyLayer = LayerMask.GetMask("Enemy");
    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

    Vector2 avoidanceVector = Vector2.zero;
    int nAvoid = 0;

    foreach (var hit in hits)
    {
        if (hit != gameObject.GetComponent<Collider2D>()) 
        {
            nAvoid++;
            avoidanceVector += (Vector2)(transform.position - hit.transform.position);
        }
    }

    if (nAvoid > 0)
    {
        avoidanceVector /= nAvoid; 
    }
    return avoidanceVector.normalized * sp.moveSpeed;
}
    void attackShot()
    {
        if (bulletPrefab && shootingPoint)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
            bullet.transform.parent = GameObject.Find("Ammo").transform;
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.freezeRotation = true;

            // Calculate direction towards the player
            Vector2 shootDirection = (player.transform.position - transform.position).normalized;
            bullet.transform.up = shootDirection;
            bulletRb.velocity = shootDirection * bulletSpeed;


            // Optionally, destroy the bullet after some time to prevent it from cluttering the scene 
            Destroy(bullet, 3f);
        }
    }
    void PerformSpreadShot()
    {
        float startAngle = -60f; // Starting angle for spread shot
        float angleIncrement = 120f / (spreadShotBullets - 1); // Angle increment between bullets

        for (int i = 0; i < spreadShotBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
            bullet.transform.parent = GameObject.Find("Ammo").transform;
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.freezeRotation = true;

            // Calculate direction for this bullet in the spread
            float currentAngle = startAngle + (angleIncrement * i);
            Vector2 shootDirection = Quaternion.Euler(0, 0, currentAngle) * transform.up;
            bullet.transform.up = shootDirection;
            bulletRb.velocity = shootDirection * bulletSpeed;

            // Optionally, destroy the bullet after some time
            Destroy(bullet, 5f);
        }
    }

   /* private void GenerateWaypoints()
    {
        waypoints = new Transform[numberOfWaypoints];
        for (int i = 0; i < numberOfWaypoints; i++)
        {
            Vector3 randomPosition = GetRandomPositionInMap();
            GameObject waypointObj = new GameObject("Waypoint" + i);
            waypointObj.transform.position = randomPosition;
            waypoints[i] = waypointObj.transform;
        }
    }*/


    private Vector3 GetRandomPositionInMap()
    {
        float offset = 10f; // Adjust as necessary to prevent enemies from sticking too close to boundaries
        float x = Random.Range(left + offset, right - offset);
        float y = Random.Range(bottom + offset, top - offset);

        return new Vector3(x, y, 1f); // Assuming enemies move in the XY plane
    }
void ObstacleAvoidanceCheck()
{
    Vector2 castDirection = transform.up; 
    float castRadius = 8f;  
    RaycastHit2D hit = Physics2D.CircleCast(transform.position, castRadius, castDirection, detectionDistance);

    
    int asteroidLayer = LayerMask.NameToLayer("Asteroids");

    if (hit.collider != null && hit.collider.gameObject.layer == asteroidLayer)
    {
        AvoidObstacle(hit.point, hit.normal);
    }
}


   void AvoidObstacle(Vector2 hitPoint, Vector2 hitNormal)
{
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    Vector2 avoidDirection = Vector2.Perpendicular(hitNormal).normalized;

    // Ensuring that the avoidance direction is away from the obstacle
    if (Vector2.Dot(avoidDirection, transform.up) < 0)
        avoidDirection = -avoidDirection;

    float avoidSpeed = sp.moveSpeed;
    Vector2 newVelocity = avoidDirection * avoidSpeed;
    rb.velocity = Vector2.Lerp(rb.velocity, newVelocity, Time.deltaTime * 5f);

    // Smoother rotation towards the new direction
    float targetAngle = Mathf.Atan2(avoidDirection.y, avoidDirection.x) * Mathf.Rad2Deg - 90; // Adjusted rotation fixing
    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * 5f);
}

private void AvoidOtherEnemies()
{
    float radius = 3f; // Detection radius
    LayerMask enemyLayer = LayerMask.GetMask("Enemy");
    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

    Vector2 avoidanceVector = Vector2.zero;
    int nAvoid = 0;

    foreach (var hit in hits)
    {
        if (hit != gameObject.GetComponent<Collider2D>()) // Exclude self
        {
            nAvoid++;
            avoidanceVector += (Vector2)(transform.position - hit.transform.position);
        }
    }

    if (nAvoid > 0)
    {
        avoidanceVector /= nAvoid; // Normalize the average vector
        avoidanceVector = avoidanceVector.normalized * sp.moveSpeed;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.Lerp(rb.velocity, avoidanceVector, Time.deltaTime * 5f);
    }
}


}
