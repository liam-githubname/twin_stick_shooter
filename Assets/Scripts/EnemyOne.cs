using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : MonoBehaviour
{
  // public bool playerInSight, playerInRange;
  public GameObject player;
  public Ship_Properties sp;
  public float randomAngle;
  [SerializeField] float health, maxHealth = 3f;
  [SerializeField] FloatingHealthBar healthBar;
  private void Start()
  {
    health = maxHealth;
    // healthBar.UpdateHealthBar(health,maxHealth);
   }
  private void Awake()
  {
    player = GameObject.Find("Ship");
    sp = GetComponent<Ship_Properties>();
    healthBar = GetComponentInChildren<FloatingHealthBar>();
  }
  public void TakeDamage(float damageAmount)
  {
    health -=damageAmount;
    healthBar.UpdateHealthBar(health,maxHealth);
    if(health <= 0)
    {
      Die();
    }
  }
  void Die()
  {
    Destroy(gameObject);
  }

  private void Update()
  {
     if (player != null)
    {
        Vector2 direction = player.transform.position - transform.position;
        sp.setInput(direction, false, false);
    }
  }
}
