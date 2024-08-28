using Unity.VisualScripting;
using UnityEngine;

public class SpaceshipInteraction : MonoBehaviour
{
  private Rigidbody2D rb;
  private Vector2 aim;
  public float grabDistance;
  public bool isTrigger;
  public bool isMining;
  public bool hasAsteroid = false;
  private float aimDeadzone = 0.05f;
  [SerializeField] public int goldScoreValue;

  private int asteroidLayerMask;
  [SerializeField] public float holdDistance = 3.5f;
  [SerializeField] public float throwSpeed = 20f;
  [SerializeField] public float maxThrowMultiplier = 5f;
  [SerializeField] public float shotPower = 0f;
  private Rigidbody2D asteroid;
  private float triggerHoldDuration;
  private Vector2 direction = Vector2.zero;
  [SerializeField] public GameObject MinedPrefab;
  [SerializeField] public ScoreManager score;


  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    asteroidLayerMask = 1 << LayerMask.NameToLayer("Asteroids");
    score = FindObjectOfType<ScoreManager>();
  }
  public void SetInput(Vector2 aim, bool isTrigger, bool isMining)
  {
    this.aim = aim;
    this.isTrigger = isTrigger;
    this.isMining = isMining;
  }
  public Vector2 GetAim()
  {
    return aim;
  }
  void FixedUpdate()
  {

    //Looking to grab the asteroid
    if (isTrigger && !hasAsteroid)
    {
      RaycastHit2D hit = AsteroidInRange(aim);
      Rigidbody2D grabbedAsteroid = GrabAsteroid(hit);
      if (grabbedAsteroid != null)
      {
        asteroid = grabbedAsteroid;
        hasAsteroid = true;
        asteroid.velocity = Vector2.zero;
        asteroid.tag = "grabbed_asteroid";
        triggerHoldDuration = 0;
      }
    }
    //if the player has the asteroid and is still holding the trigger
    else if (isTrigger && hasAsteroid)
    {

      if (direction == Vector2.zero)
      {
        direction = asteroid.position.normalized;
      }

      AimAsteroid(asteroid);
      triggerHoldDuration += Time.deltaTime;

      if (triggerHoldDuration < 1.25f)
      {
        shotPower = (float)(triggerHoldDuration / 1.25) * throwSpeed;
      }
      else
      {
        shotPower = maxThrowMultiplier * throwSpeed;
      }

      if (isMining)
      {
        if (asteroid.gameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("Gold") || asteroid.gameObject.tag == "gold_asteroid")
        {
          score.updateScore(100);
          GameObject.Find("Asteroids").GetComponent<SpawnerAsteroids>().goldenAsteroidsOnMap.Remove(asteroid.gameObject);
          Destroy(asteroid.gameObject);
          hasAsteroid = false;
          asteroid = null;
          triggerHoldDuration = 0;
          shotPower = 0;
        }
        else
          ShotgunBlast(asteroid);
      }
    }

    else if (!isTrigger && hasAsteroid)
    {
      ThrowAsteroid(asteroid, false);
      hasAsteroid = false;
      asteroid = null;
      triggerHoldDuration = 0;
      shotPower = 0;
    }
  }



  //Player Functions
  RaycastHit2D AsteroidInRange(Vector2 aim)
  {
    Vector2 rayOrigin = rb.transform.position;
    Vector2 rayDirection = aim.normalized;
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, grabDistance, asteroidLayerMask);
    return hit;
  }

  void AimAsteroid(Rigidbody2D asteroid)
  {
    if ((aim.x > aimDeadzone || aim.x < -aimDeadzone) && (aim.y > aimDeadzone || aim.y < -aimDeadzone))
      direction = aim.normalized;

    asteroid.transform.position = (Vector2)rb.transform.position + direction * holdDistance;

  }


  Rigidbody2D GrabAsteroid(RaycastHit2D hit)
  {
    return (hit.collider != null) ? hit.collider.attachedRigidbody : null;
  }


  void ThrowAsteroid(Rigidbody2D asteroid, bool needsRandom)
  {
    asteroid.gameObject.tag = "thrown_asteroid";
    if (!needsRandom)
    {
      asteroid.velocity = new Vector2(direction.x * shotPower, direction.y * shotPower);
    }
    else
    {
      asteroid.velocity = new Vector2((direction.x + Random.Range(-0.1f, 0.1f)) * shotPower, (direction.y + Random.Range(-0.1f, 0.1f)) * shotPower);
    }
  }

  void ShotgunBlast(Rigidbody2D asteroid)
  {
    GameObject MinedAsteroid = Instantiate(this.MinedPrefab, asteroid.transform.position, Quaternion.identity);
    MinedAsteroid.transform.parent = GameObject.Find("ShotgunBlast").transform;
    GameObject.Find("Asteroids").GetComponent<SpawnerAsteroids>().asteroidsOnMap.Remove(asteroid.gameObject);
    Destroy(asteroid.gameObject);
    hasAsteroid = false;
    Rigidbody2D[] childrenAsteroids = MinedAsteroid.GetComponentsInChildren<Rigidbody2D>();
    foreach (Rigidbody2D child in childrenAsteroids)
    {
      ThrowAsteroid(child, true);
      if (child.GetComponent<DestroyAfterDelay>() == null)
        child.AddComponent<DestroyAfterDelay>();
    }

    if (MinedAsteroid.GetComponent<DestroyAfterDelay>() == null)
      MinedAsteroid.AddComponent<DestroyAfterDelay>();
  }
}
