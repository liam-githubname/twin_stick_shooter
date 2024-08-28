using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DamageSystem : MonoBehaviour
{
  [SerializeField] private GameObject healthPackPrefab; // Assign this in the Inspector
  public Ship_Properties ship;
  [SerializeField] public ScoreManager score;
  // Start is called before the first frame update
  [Header("Damage Settings")]
  [SerializeField] float DamageFactor = 1.0f;
  [SerializeField] float minVelocity = 0.0f;
  [SerializeField] float currVelocity = 0.0f;
  [SerializeField] float colMass = 0.0f;
  [SerializeField] float damage = 0.0f;
  [SerializeField] float health, maxHealth = 0.0f;
  [SerializeField] FloatingHealthBar healthBar;
  [SerializeField] AudioClip[] damageSFX;
  [SerializeField] AudioClip[] deathSFX;

  [SerializeField] private int enemyLayer;
  private float colVel;
  public Animator animinator;
  public GameObject collidedObject;


  void OnCollisionEnter2D(Collision2D col)
  {
    collidedObject = col.gameObject;
    currVelocity = ship.rb.velocity.magnitude;
    colMass = col.rigidbody.mass;
    colVel = col.rigidbody.velocity.magnitude;
    //This sometimes comes out as a negative. Gotta fix it.
    if (minVelocity <= currVelocity)
    {
      damage = (currVelocity + colMass + colVel) * DamageFactor;

      if (gameObject.transform.parent.name == "Player" && !collidedObject.CompareTag("thrown_asteroid") && !collidedObject.CompareTag("grabbed_asteroid"))
      {
        TakeDamage(damage);
        score.updateScoreMultiplier(true);
      }
      if (gameObject.transform.parent.name == "Enemies")
        TakeDamage(damage);
      if (col.gameObject.CompareTag("bullet"))
      {
        Destroy(col.gameObject);
      }
    }
  }

  public void RecoverHealth(float amount)
  {
    health += amount;
    health = Mathf.Min(health, maxHealth); // Ensure health does not exceed maxHealth
    healthBar.UpdateHealthBar(health, maxHealth);
  }

  public void TakeDamage(float damageAmount)
  {
    health -= damageAmount;
    if (health > 0 && gameObject.CompareTag("ship"))
    {
      SFXManager.instance.playRandSFX(damageSFX, transform, 1f);
    }
    healthBar.UpdateHealthBar(health, maxHealth);

    if (health <= 0 && gameObject != null)
    {
      animinator.SetBool("isDead", true);
    }
    else
      animinator.SetBool("isDead", false);
  }
  public float getHealth()
  {
    return health;
  }

  public void ZaWardu()
  {
    Time.timeScale = 0;
  }
  void Die()
  {
    if (collidedObject != null)
    {
      if (collidedObject.tag == "thrown_asteroid")
      {
        score.updateScoreMultiplier(false);
      }
      // Check if the GameObject's layer is the Enemy layer
      if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
        // 30% chance to drop a health pack
        if (Random.value <= 0.3f)
        {
          HealthPackManager.instance.SpawnHealthPack(transform.position);
        }
      }
      Debug.Log("Enemy died");
      Destroy(gameObject);
    }
  }


  void playDeathSFX()
  {
    if (gameObject != null)
      SFXManager.instance.playRandSFX(deathSFX, transform, 1f);
  }
  void Start()
  {
    animinator = GetComponent<Animator>();

    ship = gameObject.GetComponent<Ship_Properties>();

    health = maxHealth;

    healthBar.UpdateHealthBar(health, maxHealth);

    enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

    score = FindObjectOfType<ScoreManager>();
  }

  // Update is called once per frame
  void Update()
  {

  }
}
