using UnityEngine;

public class SpaceshipInteraction : MonoBehaviour
{
  private Rigidbody2D rb;
  private Vector2 aim;
  public float grabDistance;
  public bool isTrigger;
  public bool hasAsteroid = false;
  public LayerMask Asteroids;
  private int asteroidLayerMask;
  public float holdDistance;
  public float throwSpeed;
  public Rigidbody2D asteroid;
  private Vector2 prevAim;
  private float triggerHoldDuration;

  public void setInput(Vector2 aim, bool isTrigger) {
    this.aim = aim;
    this.isTrigger = isTrigger;
  }

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    asteroidLayerMask = 1 << LayerMask.NameToLayer("Asteroids");
  }

  RaycastHit2D asteroidInRange(Vector2 aim) {
    Vector2 rayOrigin = rb.transform.position;
    Vector2 rayDirection = aim.normalized;
    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, grabDistance, asteroidLayerMask);
    Debug.DrawRay(rayOrigin, rayDirection*grabDistance, Color.white);
    return hit;
  }

  void Update()
  {
    if (isTrigger && hasAsteroid == false) {
      RaycastHit2D hit = asteroidInRange(aim);
      asteroid = GrabAsteroid(hit);
      hasAsteroid = true;
      triggerHoldDuration = 0;
    } else if (isTrigger && hasAsteroid == true) {
      aimAsteroid(asteroid);
      triggerHoldDuration += Time.deltaTime;
    } else if (!isTrigger && hasAsteroid == true) {
      ThrowAsteroid(asteroid, aim.normalized);
      hasAsteroid = false;
      triggerHoldDuration = 0;
    }
  }

  void aimAsteroid(Rigidbody2D asteroid) {
    Vector2 direction = aim.normalized;
    if (aim.magnitude != 0) {
      prevAim = direction;
    }
    Vector2 newPosition = (Vector2)rb.transform.position + prevAim * holdDistance;
    asteroid.transform.position = newPosition;
  }

  Rigidbody2D GrabAsteroid(RaycastHit2D hit) {
    return hit.collider.attachedRigidbody;
  }

  void ThrowAsteroid(Rigidbody2D asteroid, Vector2 direction) {
    asteroid.velocity = new Vector2(direction.x * throwSpeed * triggerHoldDuration * 2f + rb.velocity.x, direction.y * throwSpeed * triggerHoldDuration * 2f + rb.velocity.y);
  }
}
